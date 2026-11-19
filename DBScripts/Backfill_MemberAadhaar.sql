/*
    Backfill_MemberAadhaar.sql

    The Aadhaar duplicate check now reads m_MemberMaster only. Members who finished
    Address KYC (IsAddrssVerified = 'Y') before that change often have no Aadhaar
    on the member record, so this copies their verified number into it, in the
    joining layout: AadharNo / AadharNo2 / AAdharNo3, four digits each.

    Sources, in order of preference, per FormNo:
      1. KycVerify.IdProofNo, when it holds a plain 12-digit number
      2. KycApiLog rows with KycType = 'AADHAAR' and FinalResult = 'VERIFIED' whose
         RefNo is a plain 12-digit number AND whose SHA-256 equals RefNoHash
         (so a mistyped or masked RefNo can never be written)

    Safety:
      - Only members with IsAddrssVerified = 'Y'.
      - Never overwrites: a member whose record already holds a valid 12-digit
        Aadhaar is skipped (and listed if it differs from the KYC number).
      - A FormNo with more than one distinct verified number is skipped and listed.
      - @Apply = 0 (default) only previews. Set @Apply = 1 to write, in one transaction.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @Apply BIT = 0;   -- 0 = preview only, 1 = update m_MemberMaster

IF OBJECT_ID('tempdb..#Src') IS NOT NULL DROP TABLE #Src;
IF OBJECT_ID('tempdb..#Cand') IS NOT NULL DROP TABLE #Cand;

-- ---------- 1. Every verified, plain 12-digit Aadhaar per member ----------
SELECT x.FormNo, x.Aadhaar, x.Source
INTO #Src
FROM (
    SELECT k.Formno AS FormNo,
           LTRIM(RTRIM(CAST(k.IdProofNo AS VARCHAR(20)))) AS Aadhaar,
           'KycVerify' AS Source
    FROM KycVerify k
    WHERE k.IsAddrssVerified = 'Y'

    UNION ALL

    SELECT l.FormNo,
           LTRIM(RTRIM(CAST(l.RefNo AS VARCHAR(20)))),
           'KycApiLog'
    FROM KycApiLog l
    JOIN KycVerify k ON k.Formno = l.FormNo AND k.IsAddrssVerified = 'Y'
    WHERE l.KycType = 'AADHAAR'
      AND l.FinalResult = 'VERIFIED'
      AND l.FormNo <> 0
      AND ISNULL(l.RefNoHash, '') <> ''
      AND LOWER(CONVERT(VARCHAR(64),
              HASHBYTES('SHA2_256', LTRIM(RTRIM(CAST(l.RefNo AS VARCHAR(20))))), 2)) = LOWER(l.RefNoHash)
) x
WHERE LEN(x.Aadhaar) = 12
  AND PATINDEX('%[^0-9]%', x.Aadhaar) = 0
  AND LEFT(x.Aadhaar, 1) BETWEEN '2' AND '9';   -- Aadhaar never starts with 0 or 1

-- ---------- 2. One number per member, plus what the member record holds now ----------
SELECT s.FormNo,
       MIN(s.Aadhaar) AS Aadhaar,
       COUNT(DISTINCT s.Aadhaar) AS DistinctNumbers,
       MAX(CASE WHEN s.Source = 'KycVerify' THEN 1 ELSE 0 END) AS FromKycVerify,
       CAST(NULL AS VARCHAR(40)) AS Current_,
       CAST(NULL AS VARCHAR(30)) AS Action_
INTO #Cand
FROM #Src s
GROUP BY s.FormNo;

UPDATE c SET
    Current_ = CASE
        WHEN LEN(LTRIM(RTRIM(ISNULL(m.AadharNo, '')))) = 12 THEN LTRIM(RTRIM(m.AadharNo))
        ELSE LTRIM(RTRIM(ISNULL(m.AadharNo, ''))) + LTRIM(RTRIM(ISNULL(m.AadharNo2, ''))) + LTRIM(RTRIM(ISNULL(m.AAdharNo3, '')))
    END
FROM #Cand c
JOIN m_MemberMaster m ON m.Formno = c.FormNo;

UPDATE #Cand SET Action_ = CASE
    WHEN Current_ IS NULL                                   THEN 'SKIP: member row missing'
    WHEN DistinctNumbers > 1                                THEN 'SKIP: multiple verified numbers'
    WHEN Current_ = Aadhaar                                 THEN 'SKIP: already present'
    WHEN LEN(Current_) = 12 AND PATINDEX('%[^0-9]%', Current_) = 0
                                                            THEN 'SKIP: differs from member record'
    ELSE 'UPDATE'
END;

-- ---------- 3. Preview ----------
SELECT Action_, COUNT(*) AS Members FROM #Cand GROUP BY Action_ ORDER BY Action_;

SELECT FormNo, 'XXXXXXXX' + RIGHT(Aadhaar, 4) AS AadhaarMasked, DistinctNumbers,
       'XXXXXXXX' + RIGHT(Current_, 4) AS CurrentMasked, Action_
FROM #Cand
WHERE Action_ <> 'SKIP: already present'
ORDER BY Action_, FormNo;

-- Same Aadhaar verified under more than one member: these were real duplicates
-- before the check changed. Backfill still writes each member's own number;
-- decide separately which ID keeps it.
SELECT 'XXXXXXXX' + RIGHT(c.Aadhaar, 4) AS AadhaarMasked,
       COUNT(*) AS Members,
       STUFF((SELECT ', ' + CAST(c2.FormNo AS VARCHAR(20))
              FROM #Cand c2 WHERE c2.Aadhaar = c.Aadhaar AND c2.DistinctNumbers = 1
              FOR XML PATH('')), 1, 2, '') AS FormNos
FROM #Cand c
WHERE c.DistinctNumbers = 1
GROUP BY c.Aadhaar
HAVING COUNT(*) > 1;

-- ---------- 4. Apply ----------
IF @Apply = 1
BEGIN
    BEGIN TRAN;

    UPDATE m SET
        m.AadharNo  = SUBSTRING(c.Aadhaar, 1, 4),
        m.AadharNo2 = SUBSTRING(c.Aadhaar, 5, 4),
        m.AAdharNo3 = SUBSTRING(c.Aadhaar, 9, 4)
    FROM m_MemberMaster m
    JOIN #Cand c ON c.FormNo = m.Formno
    WHERE c.Action_ = 'UPDATE';

    PRINT CAST(@@ROWCOUNT AS VARCHAR(20)) + ' member row(s) updated.';

    COMMIT;
END
ELSE
    PRINT 'Preview only. Set @Apply = 1 to write.';

DROP TABLE #Src;
DROP TABLE #Cand;

/* =====================================================================
   ePay Mobile App API  -  Database Setup Script
   ---------------------------------------------------------------------
   Database : epayind   (web.config -> connectionStrings -> "constr")
   Run order: poori script ek baar chalao (har object ke pehle IF EXISTS
              check hai, isliye dobara chalane par bhi safe hai; tables
              dobara create nahi hongi, data safe rahega).

   Naye objects:
     Tables : Tbl_AppApiLog, Tbl_AppApiExtCallLog, Tbl_AppApiToken
     SPs    : Sp_AppApiLog_Insert, Sp_AppApiLog_Update, Sp_AppApiExtLog_Insert,
              Sp_AppApiLog_Search, Sp_AppApi_DuplicateGuard,
              Sp_AppApi_TokenCreate, Sp_AppApi_TokenValidate, Sp_AppApi_TokenRevoke,
              Sp_App_GetMemberProfile, Sp_App_GetHomeData,
              Sp_App_GetSubscriptionPackages, Sp_App_GetPaymentStatus

   Pehle se bane hue SPs jo API reuse karti hai (inko chhedna nahi hai):
     sp_Login1, Sp_GetAllPageckeDetail, Sp_GetKitAmount, Sp_GetKitDisDetailsNew,
     Sp_GetPachageCondtion, Sp_MMVoucherPurchase, Sp_GetBillNoWiseDetail,
     Sp_GetMonthlyDetails, sp_SetMonthlyActivation, Sp_GetMemberName,
     CheckDeleteAccount, SA_UpdateDeleteAccount, dbo.ufnGetBalance
   ===================================================================== */

USE [epayind]
GO

/* =====================================================================
   1. LOG TABLE  -  har API call ki ek row
   ===================================================================== */
IF OBJECT_ID('dbo.Tbl_AppApiLog', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tbl_AppApiLog
    (
        LogId         BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Tbl_AppApiLog PRIMARY KEY,
        ReqID         VARCHAR(30)    NOT NULL,              -- response mein "reqid" yahi jata hai
        ReqType       VARCHAR(50)    NULL,
        FormNo        INT            NULL,
        IdNo          VARCHAR(50)    NULL,
        HttpMethod    VARCHAR(10)    NULL,
        IPAddress     VARCHAR(50)    NULL,
        UserAgent     NVARCHAR(500)  NULL,
        DeviceId      NVARCHAR(200)  NULL,
        AppVersion    VARCHAR(30)    NULL,
        RequestJson   NVARCHAR(MAX)  NULL,                  -- password/token masked
        ResponseJson  NVARCHAR(MAX)  NULL,
        Status        VARCHAR(20)    NOT NULL CONSTRAINT DF_Tbl_AppApiLog_Status DEFAULT ('STARTED'), -- STARTED / OK / FAILED / ERROR
        ResponseCode  INT            NULL,                  -- 200 / 400 / 401 / 409 / 500
        RefNo         VARCHAR(100)   NULL,                  -- OrderId / BillNo / Debit RefNo (reconciliation ke liye)
        ErrorMsg      NVARCHAR(4000) NULL,
        StackTrace    NVARCHAR(MAX)  NULL,
        DurationMs    INT            NULL,
        ReqTime       DATETIME       NOT NULL CONSTRAINT DF_Tbl_AppApiLog_ReqTime DEFAULT (GETDATE()),
        ResTime       DATETIME       NULL
    );

    CREATE INDEX IX_Tbl_AppApiLog_ReqTime ON dbo.Tbl_AppApiLog (ReqTime);
    CREATE INDEX IX_Tbl_AppApiLog_FormNo  ON dbo.Tbl_AppApiLog (FormNo, ReqTime);
    CREATE INDEX IX_Tbl_AppApiLog_ReqType ON dbo.Tbl_AppApiLog (ReqType, ReqTime);
    CREATE INDEX IX_Tbl_AppApiLog_ReqID   ON dbo.Tbl_AppApiLog (ReqID);
    CREATE INDEX IX_Tbl_AppApiLog_RefNo   ON dbo.Tbl_AppApiLog (RefNo);
END
GO

/* =====================================================================
   2. EXTERNAL CALL LOG  -  API ke andar se bahar gayi calls
      (allupi payment gateway, master wallet balance/debit)
   ===================================================================== */
IF OBJECT_ID('dbo.Tbl_AppApiExtCallLog', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tbl_AppApiExtCallLog
    (
        ExtLogId      BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Tbl_AppApiExtCallLog PRIMARY KEY,
        LogId         BIGINT         NULL,                  -- Tbl_AppApiLog.LogId
        ReqID         VARCHAR(30)    NULL,
        CallName      VARCHAR(50)    NOT NULL,              -- ALLUPI_LOGIN / ALLUPI_INITIATE / MASTER_BALANCE / MASTER_DEBIT
        Url           NVARCHAR(500)  NULL,
        RequestBody   NVARCHAR(MAX)  NULL,                  -- keys masked
        ResponseBody  NVARCHAR(MAX)  NULL,
        HttpStatus    INT            NULL,
        ErrorMsg      NVARCHAR(4000) NULL,
        DurationMs    INT            NULL,
        RecTime       DATETIME       NOT NULL CONSTRAINT DF_Tbl_AppApiExtCallLog_RecTime DEFAULT (GETDATE())
    );

    CREATE INDEX IX_Tbl_AppApiExtCallLog_LogId   ON dbo.Tbl_AppApiExtCallLog (LogId);
    CREATE INDEX IX_Tbl_AppApiExtCallLog_RecTime ON dbo.Tbl_AppApiExtCallLog (RecTime);
END
GO

/* =====================================================================
   3. TOKEN TABLE  -  sirf AppWebBridge.aspx ke liye (home ke web links
      WebView mein bina login khulein). API khud har call par userid /
      passwd se check hoti hai. "home" call par 1 din ka token banta hai
      (DeviceId = 'WEBBRIDGE', member ka ek hi active).
      Token DB mein plain nahi, SHA-256 hash ke roop mein save hota hai.
   ===================================================================== */
IF OBJECT_ID('dbo.Tbl_AppApiToken', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tbl_AppApiToken
    (
        TokenId     BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Tbl_AppApiToken PRIMARY KEY,
        FormNo      INT            NOT NULL,
        IdNo        VARCHAR(50)    NOT NULL,
        TokenHash   CHAR(64)       NOT NULL,
        DeviceId    NVARCHAR(200)  NULL,
        IPAddress   VARCHAR(50)    NULL,
        CreatedOn   DATETIME       NOT NULL CONSTRAINT DF_Tbl_AppApiToken_CreatedOn DEFAULT (GETDATE()),
        ExpiresOn   DATETIME       NOT NULL,
        LastUsedOn  DATETIME       NULL,
        IsActive    BIT            NOT NULL CONSTRAINT DF_Tbl_AppApiToken_IsActive DEFAULT (1),
        RevokedOn   DATETIME       NULL
    );

    CREATE UNIQUE INDEX UX_Tbl_AppApiToken_TokenHash ON dbo.Tbl_AppApiToken (TokenHash);
    CREATE INDEX IX_Tbl_AppApiToken_FormNo ON dbo.Tbl_AppApiToken (FormNo, IsActive);
END
GO

/* =====================================================================
   4. LOG SPs
   ===================================================================== */
IF OBJECT_ID('dbo.Sp_AppApiLog_Insert', 'P') IS NOT NULL DROP PROCEDURE dbo.Sp_AppApiLog_Insert;
GO
CREATE PROCEDURE dbo.Sp_AppApiLog_Insert
    @ReqID       VARCHAR(30),
    @ReqType     VARCHAR(50)   = NULL,
    @HttpMethod  VARCHAR(10)   = NULL,
    @IPAddress   VARCHAR(50)   = NULL,
    @UserAgent   NVARCHAR(500) = NULL,
    @DeviceId    NVARCHAR(200) = NULL,
    @AppVersion  VARCHAR(30)   = NULL,
    @RequestJson NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Tbl_AppApiLog (ReqID, ReqType, HttpMethod, IPAddress, UserAgent, DeviceId, AppVersion, RequestJson)
    VALUES (@ReqID, @ReqType, @HttpMethod, @IPAddress, LEFT(@UserAgent, 500), LEFT(@DeviceId, 200), LEFT(@AppVersion, 30), @RequestJson);

    SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS LogId;
END
GO

IF OBJECT_ID('dbo.Sp_AppApiLog_Update', 'P') IS NOT NULL DROP PROCEDURE dbo.Sp_AppApiLog_Update;
GO
CREATE PROCEDURE dbo.Sp_AppApiLog_Update
    @LogId        BIGINT,
    @ReqType      VARCHAR(50)    = NULL,
    @FormNo       INT            = NULL,
    @IdNo         VARCHAR(50)    = NULL,
    @Status       VARCHAR(20),
    @ResponseCode INT            = NULL,
    @ResponseJson NVARCHAR(MAX)  = NULL,
    @RefNo        VARCHAR(100)   = NULL,
    @ErrorMsg     NVARCHAR(4000) = NULL,
    @StackTrace   NVARCHAR(MAX)  = NULL,
    @DurationMs   INT            = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Tbl_AppApiLog
       SET ReqType      = ISNULL(@ReqType, ReqType),
           FormNo       = ISNULL(@FormNo, FormNo),
           IdNo         = ISNULL(@IdNo, IdNo),
           Status       = @Status,
           ResponseCode = @ResponseCode,
           ResponseJson = @ResponseJson,
           RefNo        = ISNULL(@RefNo, RefNo),
           ErrorMsg     = LEFT(@ErrorMsg, 4000),
           StackTrace   = @StackTrace,
           DurationMs   = @DurationMs,
           ResTime      = GETDATE()
     WHERE LogId = @LogId;
END
GO

IF OBJECT_ID('dbo.Sp_AppApiExtLog_Insert', 'P') IS NOT NULL DROP PROCEDURE dbo.Sp_AppApiExtLog_Insert;
GO
CREATE PROCEDURE dbo.Sp_AppApiExtLog_Insert
    @LogId        BIGINT         = NULL,
    @ReqID        VARCHAR(30)    = NULL,
    @CallName     VARCHAR(50),
    @Url          NVARCHAR(500)  = NULL,
    @RequestBody  NVARCHAR(MAX)  = NULL,
    @ResponseBody NVARCHAR(MAX)  = NULL,
    @HttpStatus   INT            = NULL,
    @ErrorMsg     NVARCHAR(4000) = NULL,
    @DurationMs   INT            = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Tbl_AppApiExtCallLog (LogId, ReqID, CallName, Url, RequestBody, ResponseBody, HttpStatus, ErrorMsg, DurationMs)
    VALUES (@LogId, @ReqID, @CallName, @Url, @RequestBody, @ResponseBody, @HttpStatus, LEFT(@ErrorMsg, 4000), @DurationMs);
END
GO

/* Double tap / retry guard (couponpurchase, subscriptionpay, monthlyactivate, deleteaccount).
   Apni log row mein FormNo bharta hai, phir dekhta hai ki same member ki same reqtype
   pichhle @Seconds mein chal rahi hai (STARTED) ya ho chuki hai (OK).
   Pending > 0 = nayi request rok do. sp_getapplock se ek member ki requests ek-ek karke check hoti hain. */
IF OBJECT_ID('dbo.Sp_AppApi_DuplicateGuard', 'P') IS NOT NULL DROP PROCEDURE dbo.Sp_AppApi_DuplicateGuard;
GO
CREATE PROCEDURE dbo.Sp_AppApi_DuplicateGuard
    @LogId   BIGINT,
    @FormNo  INT,
    @IdNo    VARCHAR(50),
    @ReqType VARCHAR(50),
    @Seconds INT = 20
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Pending INT, @Res VARCHAR(100) = 'AppApiGuard_' + CAST(@FormNo AS VARCHAR(20)) + '_' + @ReqType;

    BEGIN TRAN;
        EXEC sp_getapplock @Resource = @Res, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 5000;

        UPDATE dbo.Tbl_AppApiLog SET FormNo = @FormNo, IdNo = @IdNo WHERE LogId = @LogId;

        SELECT @Pending = COUNT(1)
          FROM dbo.Tbl_AppApiLog
         WHERE FormNo  = @FormNo
           AND ReqType = @ReqType
           AND LogId  <> @LogId
           AND ReqTime > DATEADD(SECOND, -@Seconds, GETDATE())
           AND Status IN ('STARTED', 'OK');
    COMMIT TRAN;

    SELECT @Pending AS Pending;
END
GO

/* Admin ke liye log search.
   Example: EXEC Sp_AppApiLog_Search '2026-09-01', '2026-09-30', @Status = 'ERROR'  */
IF OBJECT_ID('dbo.Sp_AppApiLog_Search', 'P') IS NOT NULL DROP PROCEDURE dbo.Sp_AppApiLog_Search;
GO
CREATE PROCEDURE dbo.Sp_AppApiLog_Search
    @FromDate DATE,
    @ToDate   DATE,
    @FormNo   INT         = NULL,
    @IdNo     VARCHAR(50) = NULL,
    @ReqType  VARCHAR(50) = NULL,
    @Status   VARCHAR(20) = NULL,
    @RefNo    VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT L.LogId, L.ReqID, L.ReqType, L.FormNo, L.IdNo, L.Status, L.ResponseCode, L.RefNo,
           L.ErrorMsg, L.DurationMs, L.IPAddress, L.DeviceId, L.AppVersion,
           L.ReqTime, L.ResTime, L.RequestJson, L.ResponseJson,
           (SELECT COUNT(1) FROM dbo.Tbl_AppApiExtCallLog E WHERE E.LogId = L.LogId) AS ExtCalls
      FROM dbo.Tbl_AppApiLog L
     WHERE L.ReqTime >= @FromDate
       AND L.ReqTime <  DATEADD(DAY, 1, @ToDate)
       AND (@FormNo  IS NULL OR L.FormNo  = @FormNo)
       AND (@IdNo    IS NULL OR L.IdNo    = @IdNo)
       AND (@ReqType IS NULL OR L.ReqType = @ReqType)
       AND (@Status  IS NULL OR L.Status  = @Status)
       AND (@RefNo   IS NULL OR L.RefNo   = @RefNo)
     ORDER BY L.LogId DESC;
END
GO

/* =====================================================================
   5. TOKEN SPs
   ===================================================================== */
IF OBJECT_ID('dbo.Sp_AppApi_TokenCreate', 'P') IS NOT NULL DROP PROCEDURE dbo.Sp_AppApi_TokenCreate;
GO
CREATE PROCEDURE dbo.Sp_AppApi_TokenCreate
    @FormNo    INT,
    @IdNo      VARCHAR(50),
    @TokenHash CHAR(64),
    @DeviceId  NVARCHAR(200) = NULL,
    @IPAddress VARCHAR(50)   = NULL,
    @ValidDays INT           = 30
AS
BEGIN
    SET NOCOUNT ON;

    -- Same device se dobara login -> us device ka purana token band
    IF ISNULL(@DeviceId, '') <> ''
        UPDATE dbo.Tbl_AppApiToken
           SET IsActive = 0, RevokedOn = GETDATE()
         WHERE FormNo = @FormNo AND DeviceId = @DeviceId AND IsActive = 1;

    INSERT INTO dbo.Tbl_AppApiToken (FormNo, IdNo, TokenHash, DeviceId, IPAddress, ExpiresOn)
    VALUES (@FormNo, @IdNo, @TokenHash, @DeviceId, @IPAddress, DATEADD(DAY, @ValidDays, GETDATE()));

    SELECT CONVERT(VARCHAR(20), DATEADD(DAY, @ValidDays, GETDATE()), 120) AS ExpiresOn;
END
GO

IF OBJECT_ID('dbo.Sp_AppApi_TokenValidate', 'P') IS NOT NULL DROP PROCEDURE dbo.Sp_AppApi_TokenValidate;
GO
CREATE PROCEDURE dbo.Sp_AppApi_TokenValidate
    @TokenHash CHAR(64)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TokenId BIGINT;

    SELECT @TokenId = TokenId
      FROM dbo.Tbl_AppApiToken
     WHERE TokenHash = @TokenHash AND IsActive = 1 AND ExpiresOn > GETDATE();

    IF @TokenId IS NULL
        RETURN;   -- koi row nahi = token invalid / expired

    UPDATE dbo.Tbl_AppApiToken SET LastUsedOn = GETDATE() WHERE TokenId = @TokenId;

    SELECT T.FormNo, M.IDNo, M.MemFirstName, M.MemLastName, M.Mobl, M.Email,
           M.Passw, M.IsBlock, M.ActiveStatus
      FROM dbo.Tbl_AppApiToken T
     INNER JOIN dbo.M_MemberMaster M ON M.FormNo = T.FormNo
     WHERE T.TokenId = @TokenId;
END
GO

IF OBJECT_ID('dbo.Sp_AppApi_TokenRevoke', 'P') IS NOT NULL DROP PROCEDURE dbo.Sp_AppApi_TokenRevoke;
GO
CREATE PROCEDURE dbo.Sp_AppApi_TokenRevoke
    @FormNo INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Logout: member ke saare active token band
    UPDATE dbo.Tbl_AppApiToken
       SET IsActive = 0, RevokedOn = GETDATE()
     WHERE FormNo = @FormNo AND IsActive = 1;

    SELECT @@ROWCOUNT AS Revoked;
END
GO

/* =====================================================================
   6. PAGE SPs
   ===================================================================== */

/* AppMaster.master  -  drawer ka naam / ID / photo */
IF OBJECT_ID('dbo.Sp_App_GetMemberProfile', 'P') IS NOT NULL DROP PROCEDURE dbo.Sp_App_GetMemberProfile;
GO
CREATE PROCEDURE dbo.Sp_App_GetMemberProfile
    @FormNo INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT M.FormNo, M.IDNo, M.MemFirstName, M.MemLastName, M.Mobl, M.Email,
           M.ProfilePic, M.ActiveStatus, M.IsBlock, M.KitId, K.KitName,
           REPLACE(CONVERT(VARCHAR(11), M.Doj, 106), ' ', '-')            AS Doj,              -- 12-Jan-2026
           REPLACE(CONVERT(VARCHAR(11), M.ActivationDate, 106), ' ', '-') AS ActivationDate
      FROM dbo.M_MemberMaster M
      LEFT JOIN dbo.M_KitMaster K ON K.KitId = M.KitId
     WHERE M.FormNo = @FormNo;
END
GO

/* WebApp.aspx  -  home screen (5 result sets, same order as page) */
IF OBJECT_ID('dbo.Sp_App_GetHomeData', 'P') IS NOT NULL DROP PROCEDURE dbo.Sp_App_GetHomeData;
GO
CREATE PROCEDURE dbo.Sp_App_GetHomeData
AS
BEGIN
    SET NOCOUNT ON;

    -- 0. Banner slider
    SELECT bg_class, tag, title, subtitle, cta, cta_icon, cta_dark, url, [target], deco
      FROM dbo.BannerSlider WHERE active = 1 ORDER BY sort_order;

    -- 1. All services
    SELECT name, icon, color, url, [target]
      FROM dbo.AllServices WHERE is_active = 1 ORDER BY sort_order;

    -- 2. Best selling
    SELECT [rank], name, icon, icon_bg, rank_bg, users, url, [target]
      FROM dbo.BestSelling WHERE is_active = 1 ORDER BY [rank];

    -- 3. Trust statistics
    SELECT value, label, icon, icon_color, icon_bg
      FROM dbo.TrustStatistics WHERE is_active = 1 ORDER BY sort_order;

    -- 4. Promo banner (bottom)
    SELECT bg_class, deco, tag, title, subtitle, cta, btn_inv, url, [target]
      FROM dbo.PromoBanner WHERE is_active = 1 AND position = 'bottom';
END
GO

/* Appsubscription-now.aspx  -  Check_IdNo() + fillkit() + BindPackages() ka logic
   @FormNo = member ka FormNo (IDNo nahi)
   Result 0 : MemberOk (0 = ID block hai ya FormNo mila hi nahi)
   Result 1 : packages (4,6,7,11) + IsAllowed flag
   Web jaisa: member ka kit M_KitMaster (RowStatus='Y') mein na mile to koi shart nahi,
   saare subscription (ForType 'S') package allowed.                      */
IF OBJECT_ID('dbo.Sp_App_GetSubscriptionPackages', 'P') IS NOT NULL DROP PROCEDURE dbo.Sp_App_GetSubscriptionPackages;
GO
CREATE PROCEDURE dbo.Sp_App_GetSubscriptionPackages
    @FormNo INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @KitId INT, @ActiveStatus VARCHAR(5), @MemberOk BIT = 0, @KitFound BIT = 0, @MinSeq INT = NULL;

    -- Member hai aur block nahi hai
    IF EXISTS (SELECT 1 FROM dbo.M_MemberMaster WHERE FormNo = @FormNo AND ISNULL(IsBlock, 'N') <> 'Y')
        SET @MemberOk = 1;

    -- Web ka Check_IdNo(): kit mila tabhi shart lagti hai
    SELECT TOP 1 @KitId = A.KitId, @ActiveStatus = A.ActiveStatus, @KitFound = 1
      FROM dbo.M_MemberMaster A
     INNER JOIN dbo.M_KitMaster B ON A.KitId = B.KitId
     WHERE B.RowStatus = 'Y' AND A.IsBlock = 'N' AND A.FormNo = @FormNo;

    IF @KitFound = 1
    BEGIN
        IF @ActiveStatus = 'Y'
            -- Active ID: pehle li hui subscription se upar wale package hi allowed
            SELECT @MinSeq = MAX(B.TopupSeq)
              FROM dbo.RepurchIncome A
             INNER JOIN dbo.M_KitMaster B ON A.KitId = B.KitId
             WHERE A.FormNo = @FormNo AND B.ForType = 'S' AND A.KitId <> 0;
        ELSE
            -- Inactive ID: apne kit se upar wale
            SELECT @MinSeq = ISNULL(TopupSeq, 0) FROM dbo.M_KitMaster WHERE KitId = @KitId;
    END

    SELECT @MemberOk AS MemberOk;

    SELECT K.KitId, K.KitName, K.JoinAmount, K.KitAmount,
           CAST(CASE WHEN @MemberOk = 1 AND EXISTS (
                    SELECT 1 FROM dbo.M_KitMaster X
                     WHERE X.KitId = K.KitId
                       AND X.ActiveStatus = 'Y'
                       AND X.ForType = 'S'
                       AND X.KitId <> 1
                       AND (@MinSeq IS NULL OR X.TopupSeq > @MinSeq))
                THEN 1 ELSE 0 END AS BIT) AS IsAllowed
      FROM dbo.M_KitMaster K
     WHERE K.KitId IN (4, 6, 7, 11)
     ORDER BY K.KitAmount DESC;
END
GO

/* Subscription / Monthly payment ke baad status (webhook LoginTransaction update karta hai) */
IF OBJECT_ID('dbo.Sp_App_GetPaymentStatus', 'P') IS NOT NULL DROP PROCEDURE dbo.Sp_App_GetPaymentStatus;
GO
CREATE PROCEDURE dbo.Sp_App_GetPaymentStatus
    @OrderId VARCHAR(50),
    @FormNo  INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
           O.Orderid, O.Amount, O.KitId, K.KitName,
           CONVERT(VARCHAR(20), O.Orderdate, 120) AS OrderDate,
           UPPER(ISNULL(NULLIF(LTRIM(RTRIM(L.Status)), ''), 'PENDING')) AS Status,
           CONVERT(VARCHAR(20), L.Responsedate, 120) AS ResponseDate
      FROM dbo.OnlineTransaction O
      LEFT JOIN dbo.LoginTransaction L ON L.TransactionId = O.Orderid
      LEFT JOIN dbo.M_KitMaster K ON K.KitId = O.KitId
     WHERE O.Orderid = @OrderId
       AND O.FormNo = @FormNo
     ORDER BY L.Responsedate DESC;
END
GO

/* =====================================================================
   7. (Optional) Purana log saaf karna - 180 din se purana
   ---------------------------------------------------------------------
   DELETE FROM dbo.Tbl_AppApiExtCallLog WHERE RecTime < DATEADD(DAY, -180, GETDATE());
   DELETE FROM dbo.Tbl_AppApiLog        WHERE ReqTime < DATEADD(DAY, -180, GETDATE());
   ===================================================================== */

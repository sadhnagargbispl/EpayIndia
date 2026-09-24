/* =====================================================================
   Sp_GetPachageCondtion  -  coupon purchase se pehle package condition
   ---------------------------------------------------------------------
   Database : epayindselect   (web.config -> "constr1")
   Call     : Apppurchase-coupon-details.aspx  ->  Fund_PackageCondtion_Check()
              AppApi.aspx (reqtype couponpurchase)
              Dono  "Exec Sp_GetPachageCondtion @FormNo"  chalate hain.

   Output (1 row):
     Result  = 'OK'      -> purchase aage chalega
               'FAILED'  -> purchase rukega, Msg user ko dikhega
     Msg     = message

   Yeh SP pehle kisi DB mein nahi tha. Isliye web page par abhi tak koi
   condition lagti hi nahi thi (SP na milne par bhi purchase ho jata tha).
   Abhi yeh SP wahi behaviour rakhta hai: sab ko 'OK'.
   Koi rule lagana ho to neeche "RULES" wale hisse mein likho.
   ===================================================================== */

USE [epayindselect]
GO

IF OBJECT_ID('dbo.Sp_GetPachageCondtion', 'P') IS NOT NULL DROP PROCEDURE dbo.Sp_GetPachageCondtion;
GO
CREATE PROCEDURE dbo.Sp_GetPachageCondtion
    @FormNo VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Result VARCHAR(10) = 'OK',
            @Msg    VARCHAR(500) = 'OK';

    /* ---------------- RULES ----------------
       Rule lagana ho to yahan likho, jaise:

       IF NOT EXISTS (SELECT 1 FROM M_MemberMaster
                       WHERE FormNo = @FormNo AND ActiveStatus = 'Y')
       BEGIN
           SELECT @Result = 'FAILED', @Msg = 'Please activate your ID first.';
       END
       ---------------------------------------- */

    SELECT @Result AS Result, @Msg AS Msg;
END
GO

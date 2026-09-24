/* =====================================================================
   ePay Mobile App API  -  Object check
   ---------------------------------------------------------------------
   Kaunsa SP / function / table kis database mein hai, yeh batata hai.
   Kuch create / change nahi karta, sirf padhta hai. Kisi bhi DB par chalao.

   UsedFrom batata hai ki code kis connection se call karta hai:
     constr  = epayind
     constr1 = epayindselect
   Status = 'MISSING' wali rows mujhe bhej do.
   ===================================================================== */

SET NOCOUNT ON;

DECLARE @Obj TABLE (Name SYSNAME, UsedFrom VARCHAR(20), UsedBy VARCHAR(200));

INSERT INTO @Obj VALUES
 -- constr1 (epayindselect) se call hote hain - web pages bhi yahi use karte hain
 ('sp_Login1',               'epayindselect', 'login (AppLogin.aspx)'),
 ('Sp_GetAllPageckeDetail',  'epayindselect', 'couponlist (Apppurchase-coupon.aspx)'),
 ('Sp_GetKitAmount',         'epayindselect', 'coupondetail / couponpurchase'),
 ('Sp_GetKitDisDetailsNew',  'epayindselect', 'coupondetail'),
 ('Sp_GetPachageCondtion',   'epayindselect', 'couponpurchase (Apppurchase-coupon-details.aspx)'),
 ('Sp_GetBillNoWiseDetail',  'epayindselect', 'orderdetail (AppThankYou.aspx)'),
 ('sp_SetMonthlyActivation', 'epayindselect', 'monthlyactivate'),
 ('Sp_GetMemberName',        'epayindselect', 'monthlyactivate'),
 ('CheckDeleteAccount',      'epayindselect', 'deleteaccountcheck / deleteaccount'),
 ('ufnGetBalance',           'epayindselect', 'walletbalance / profile / coupon'),
 ('GetPetroCartReportINR',   'epayindselect', 'petrocardreport (PetroCardPurchaseReport.aspx)'),
 ('Sp_getKitPetro',          'epayindselect', 'petrocardkits (PETROCARDPurchase.aspx)'),
 ('USP_GetPanKycStatus',     'epayindselect', 'petrocardkits / petrocardform / petrocardpurchase'),
 ('Sp_GetMemberNamer',       'epayindselect', 'petrocardform / petrocardpurchase'),
 ('Sp_getGender',            'epayindselect', 'petrocardform'),
 ('Sp_GetWalletTypePetroCardINR', 'epayindselect', 'petrocardform'),
 ('Sp_PaymentPetroCardINR',  'epayind', 'petrocardpurchase (WALLET)'),
 ('sp_PaymentPetroCardINRPayDis', 'epayind', 'PetroCardPaymentGatewayPurchase.aspx webhook'),
 ('PetroOnlineTransaction',  'epayind', 'petrocardpurchase (PG) / paymentstatus'),
 ('M_STateDivMaster',        'epayind', 'petrocardform (states)'),
 ('repurchincome',           'epayind', 'petrocard (already purchased check)'),
 ('MM_kitmaster',            'epayind', 'purchasehistory (MyPurchaseDetail.aspx)'),
 ('Repurchincome_MM',        'epayind', 'purchasehistory (MyPurchaseDetail.aspx)'),
 -- constr (epayind) se call hote hain
 ('Sp_MMVoucherPurchase',    'epayind', 'couponpurchase'),
 ('Sp_GetMonthlyDetails',    'epayind', 'monthlypackages / monthlyactivate'),
 ('SA_UpdateDeleteAccount',  'epayind', 'deleteaccount'),
 ('sp_Subscription',         'epayind', 'PaymentGatewayPurchase.aspx webhook'),
 ('sp_MonthlyActivation',    'epayind', 'Paymentgatewayapp.aspx webhook'),
 ('M_MemberMaster',          'epayind', 'profile / subscription / token'),
 ('M_KitMaster',             'epayind', 'subscription / profile'),
 ('RepurchIncome',           'epayind', 'subscriptionpackages'),
 ('OnlineTransaction',       'epayind', 'subscriptionpay / monthlyactivate'),
 ('LoginTransaction',        'epayind', 'subscriptionpay / monthlyactivate / paymentstatus'),
 ('Trnactive',               'epayind', 'couponpurchase'),
 ('Trnjoining',              'epayind', 'subscriptionpay / monthlyactivate'),
 ('Trnactivecadmin',         'epayind', 'deleteaccount'),
 ('UserHistory',             'epayind', 'deleteaccount'),
 ('BannerSlider',            'epayind', 'home'),
 ('AllServices',             'epayind', 'home'),
 ('BestSelling',             'epayind', 'home'),
 ('TrustStatistics',         'epayind', 'home'),
 ('PromoBanner',             'epayind', 'home'),
 -- naye (DBScripts/AppApi_Setup.sql se bante hain)
 ('Tbl_AppApiLog',                  'epayind', 'NEW - AppApi_Setup.sql'),
 ('Tbl_AppApiExtCallLog',           'epayind', 'NEW - AppApi_Setup.sql'),
 ('Tbl_AppApiToken',                'epayind', 'NEW - AppApi_Setup.sql'),
 ('Sp_AppApiLog_Insert',            'epayind', 'NEW - AppApi_Setup.sql'),
 ('Sp_AppApiLog_Update',            'epayind', 'NEW - AppApi_Setup.sql'),
 ('Sp_AppApiExtLog_Insert',         'epayind', 'NEW - AppApi_Setup.sql'),
 ('Sp_AppApi_DuplicateGuard',       'epayind', 'NEW - AppApi_Setup.sql'),
 ('Sp_AppApi_TokenCreate',          'epayind', 'NEW - AppApi_Setup.sql'),
 ('Sp_AppApi_TokenValidate',        'epayind', 'NEW - AppApi_Setup.sql'),
 ('Sp_AppApi_TokenRevoke',          'epayind', 'NEW - AppApi_Setup.sql'),
 ('Sp_App_GetMemberProfile',        'epayind', 'NEW - AppApi_Setup.sql'),
 ('Sp_App_GetHomeData',             'epayind', 'NEW - AppApi_Setup.sql'),
 ('Sp_App_GetSubscriptionPackages', 'epayind', 'NEW - AppApi_Setup.sql'),
 ('Sp_App_GetPaymentStatus',        'epayind', 'NEW - AppApi_Setup.sql');

SELECT O.Name,
       O.UsedFrom,
       CASE WHEN OBJECT_ID('epayind.dbo.' + O.Name) IS NOT NULL THEN 'YES' ELSE '-' END       AS In_epayind,
       CASE WHEN OBJECT_ID('epayindselect.dbo.' + O.Name) IS NOT NULL THEN 'YES' ELSE '-' END AS In_epayindselect,
       CASE WHEN OBJECT_ID(O.UsedFrom + '.dbo.' + O.Name) IS NOT NULL THEN 'OK' ELSE 'MISSING' END AS Status,
       O.UsedBy
  FROM @Obj O
 ORDER BY CASE WHEN OBJECT_ID(O.UsedFrom + '.dbo.' + O.Name) IS NULL THEN 0 ELSE 1 END, O.UsedFrom, O.Name;

/* Jo SP / function sahi DB (UsedFrom) mein nahi hai lekin dusre DB mein hai,
   uska poora code. Definition wale cell par click karo -> naye tab mein khulega ->
   copy karke UsedFrom wale DB mein chala do. */
SELECT O.Name,
       O.UsedFrom AS CreateIn,
       CASE WHEN O.UsedFrom = 'epayind' THEN 'epayindselect' ELSE 'epayind' END AS CopyFrom,
       (SELECT CASE WHEN O.UsedFrom = 'epayind'
                    THEN OBJECT_DEFINITION(OBJECT_ID('epayindselect.dbo.' + O.Name))
                    ELSE OBJECT_DEFINITION(OBJECT_ID('epayind.dbo.' + O.Name)) END
               AS [processing-instruction(sql)] FOR XML PATH(''), TYPE) AS Definition
  FROM @Obj O
 WHERE OBJECT_ID(O.UsedFrom + '.dbo.' + O.Name) IS NULL
   AND OBJECT_ID(CASE WHEN O.UsedFrom = 'epayind' THEN 'epayindselect' ELSE 'epayind' END + '.dbo.' + O.Name) IS NOT NULL;

/* Synonyms: epayindselect mein kaunsa naam kis object ki taraf point karta hai */
SELECT S.name AS SynonymName, S.base_object_name AS PointsTo
  FROM epayindselect.sys.synonyms S
 WHERE S.name IN (SELECT Name FROM @Obj);

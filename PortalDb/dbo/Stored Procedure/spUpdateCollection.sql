CREATE PROCEDURE [dbo].[spUpdateCollection]
	@plngCollectionKey INT,
	@pstrFirstname VARCHAR(50),
	@pstrLastname VARCHAR(50),
	@pstrEmailAddress VARCHAR(50),
    @pdtmTransactionDate DATETIME2(0),
	@plngCollectionNumber INT,
	@plngMemberStatusKey INT,
	@plngCollectionTypeKey INT,
	@plngPaymentTypeKey INT,
	@pcurAmount DECIMAL(18,2),
	@pcurLocalAmount DECIMAL(18,2),
	@pstrCurrencyKey VARCHAR(50),
	@pstrLocationKey VARCHAR(50),
    @pstrWho VARCHAR(50),
	@pdtmWhen DATETIME2(0)
AS
BEGIN
    DECLARE @MaxVer INT;
    SET @MaxVer = (SELECT MAX(flngVer) FROM tblCollection WHERE flngCollectionKey = @plngCollectionKey) + 1;

     INSERT INTO dbo.tblCollection
    (
        flngCollectionKey,
        flngver,
        fstrFirstname,
        fstrLastname,
        fstrEmailAddress,
        fdtmTransactionDate,
        flngCollectionNumber,
        flngMemberStatusKey,
        flngCollectionTypeKey,
        flngPaymentTypeKey,
        fcurAmount,
        fcurLocalAmount,
        fstrCurrencyKey,
        fblnActive,
        fstrLocationKey,
        fstrWho,
        fdtmWhen
    )
    SELECT
        flngCollectionKey,
        @MaxVer,
        fstrFirstname,
        fstrLastname,
        fstrEmailAddress,
        fdtmTransactionDate,
        flngCollectionNumber,
        flngMemberStatusKey,
        flngCollectionTypeKey,
        flngPaymentTypeKey,
        fcurAmount,
        fcurLocalAmount,
        fstrCurrencyKey,
        fblnActive,
        fstrLocationKey,
        fstrWho,
        fdtmWhen   
   FROM tblCollection WHERE flngCollectionKey = @plngCollectionKey AND flngVer = 0

    UPDATE dbo.tblCollection
                SET
                    fstrFirstname         = @pstrFirstname,
                    fstrLastname          = @pstrLastname,
                    fstrEmailAddress      = @pstrEmailAddress,
                    fdtmTransactionDate   = @pdtmTransactionDate,
                    flngCollectionNumber  = @plngCollectionNumber,
                    flngMemberStatusKey   = @plngMemberStatusKey,
                    flngCollectionTypeKey = @plngCollectionTypeKey,
                    flngPaymentTypeKey    = @plngPaymentTypeKey,
                    fcurAmount            = @pcurAmount,
                    fcurLocalAmount       = @pcurLocalAmount,
                    fstrCurrencyKey       = @pstrCurrencyKey,
                    fblnActive            = 1,
                    fstrWho               = @pstrWho,
                    fdtmWhen              = @pdtmWhen
                WHERE flngCollectionKey   = @plngCollectionKey
                  AND fstrLocationKey     = @pstrLocationKey
                  AND flngVer             = 0

END

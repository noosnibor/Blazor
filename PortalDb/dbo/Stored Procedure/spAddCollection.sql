CREATE PROCEDURE [dbo].[spAddCollection]
    @plngCollectionKey      UNIQUEIDENTIFIER,
	@pstrFirstname          VARCHAR(50),
	@pstrLastname           VARCHAR(50),
	@pstrEmailAddress       VARCHAR(50),
    @pdtmTransactionDate    DATETIME2(0),
	@plngCollectionNumber   INT,
	@plngMemberStatusKey    INT,
	@plngCollectionTypeKey  INT,
	@plngPaymentTypeKey     INT,
	@pcurAmount             DECIMAL(18,2),
	@pcurLocalAmount        DECIMAL(18,2),
	@pstrCurrencyKey        VARCHAR(50),
	@pstrLocationKey        VARCHAR(50),  
    @pstrWho                VARCHAR(50),  
	@pdtmWhen               DATETIME2(0)

AS
BEGIN

    INSERT INTO dbo.tblCollection
    ( flngCollectionKey,
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
    VALUES
    ( @plngCollectionKey,
        0,
        @pstrFirstname,
        @pstrLastname,
        @pstrEmailAddress,
        @pdtmTransactionDate,
        @plngCollectionNumber,
        @plngMemberStatusKey,
        @plngCollectionTypeKey,
        @plngPaymentTypeKey,
        @pcurAmount,
        @pcurLocalAmount,
        @pstrCurrencyKey,
        1,
        @pstrLocationKey,
        @pstrWho, 
        @pdtmWhen
        );      

END

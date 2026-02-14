CREATE PROCEDURE [dbo].[spAddCurrency]
    @pstrCurrencyKey     VARCHAR(50),
	@pstrCurrencyType    VARCHAR(50),
    @pstrLocationKey     VARCHAR(50),
    @pcurAmount          DECIMAL(18,2),
    @pblnActive          BIT,
    @pstrWho             VARCHAR(50),
    @pdtmEffectiveFrom   DATETIME2(0),
    @pdtmEffectiveTo     DATETIME2(0)
AS
BEGIN
    INSERT INTO tblCurrency (
    fstrCurrencyKey,
    flngVer, 
    fstrCurrencyType, 
    fstrLocationKey, 
    fcurAmount, 
    fblnActive, 
    fstrWho, 
    fdtmEffectiveFrom, 
    fdtmEffectiveTo)

    VALUES (
    @pstrCurrencyKey,
    0, 
    @pstrCurrencyType, 
    @pstrLocationKey, 
    @pcurAmount, 
    @pblnActive, 
    @pstrWho, 
    @pdtmEffectiveFrom, 
    @pdtmEffectiveTo);
END

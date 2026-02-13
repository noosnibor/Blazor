CREATE PROCEDURE [dbo].[spAddCurrency]
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
    flngVer, 
    fstrCurrencyType, 
    fstrLocationKey, 
    fcurAmount, 
    fblnActive, 
    fstrWho, 
    fdtmEffectiveFrom, 
    fdtmEffectiveTo)

    VALUES (
    0, 
    @pstrCurrencyType, 
    @pstrLocationKey, 
    @pcurAmount, 
    @pblnActive, 
    @pstrWho, 
    @pdtmEffectiveFrom, 
    @pdtmEffectiveTo);
END

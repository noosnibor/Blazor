CREATE PROCEDURE [dbo].[spUpdateCurrency]
	@pstrCurrencyKey    VARCHAR(50),
	@pstrCurrencyType   VARCHAR(50),
    @pstrLocationKey    VARCHAR(50),
    @pcurAmount         DECIMAL(18,2),
    @pblnActive         BIT,
    @pstrWho            VARCHAR(50),
    @pdtmEffectiveFrom  DATETIME2(0),
    @pdtmEffectiveTo    DATETIME2(0)
AS
BEGIN
	DECLARE @MaxVer INT;
    SET @MaxVer = (SELECT MAX(flngVer) FROM tblCurrency WHERE fstrCurrencyKey = @pstrCurrencyKey) + 1;

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

    SELECT  fstrCurrencyKey,
            @MaxVer, 
            fstrCurrencyType, 
            fstrLocationKey, 
            fcurAmount, 
            fblnActive, 
            fstrWho, 
            fdtmEffectiveFrom, 
            fdtmEffectiveTo
    FROM tblCurrency WHERE fstrCurrencyKey = @pstrCurrencyKey 
    AND fstrLocationKey = @pstrLocationKey
    AND flngVer = 0

     UPDATE tblCurrency SET 
        fstrCurrencyKey         = @pstrCurrencyKey, 
        fstrCurrencyType        = @pstrCurrencyType, 
        fcurAmount              = @pcurAmount, 
        fblnActive              = @pblnActive, 
        fstrWho                 = @pstrWho, 
        fdtmEffectiveFrom       = @pdtmEffectiveFrom,
        fdtmEffectiveTo         = @pdtmEffectiveTo
        WHERE fstrCurrencyKey   = @pstrCurrencyKey
        AND fstrLocationKey     = @pstrLocationKey
        AND flngVer             = 0
END

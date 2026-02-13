CREATE PROCEDURE [dbo].[spSelectCurrency]
    @pstrCurrencyKey    VARCHAR(50) = NULL,
	@pstrLocationKey    VARCHAR(50) = NULL,
    @pblnActive         BIT         = NULL
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
        fstrCurrencyKey,
        fstrCurrencyType,
        fstrLocationKey,
        fcurAmount,
        fblnActive,
        fstrWho,
        fdtmEffectiveFrom,
        fdtmEffectiveTo
    FROM tblCurrency
    WHERE (@pstrCurrencyKey       IS NULL OR fstrCurrencyKey    = @pstrCurrencyKey)
    AND   (@pstrLocationKey       IS NULL OR fstrLocationKey    = @pstrLocationKey)
    AND   (@pblnActive            IS NULL OR fblnActive         = @pblnActive)
    AND   flngver                                               = 0
END

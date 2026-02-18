CREATE PROCEDURE [dbo].[spSummaryReport]
	@pstrLocationKey		    VARCHAR(50)     = NULL,
	@pdtmTransactionDateFrom	DATETIME2       = NULL,
    @pdtmTransactionDateTo	    DATETIME2       = NULL

AS
BEGIN
	SET NOCOUNT ON;

        -- Normalize string inputs
    SET @pstrLocationKey = NULLIF(LTRIM(RTRIM(@pstrLocationKey)), '');

    SELECT c.flngPaymentTypeKey, 
           c.flngCollectionTypeKey,
           c.fstrCurrencyKey,
           c.fstrLocationKey,
           DATENAME(MONTH, c.fdtmTransactionDate) AS fstrMonth,
            c.fdtmTransactionDate,
           SUM(c.fcurAmount) AS fcurAmount,
           SUM(c.fcurLocalAmount) AS fcurLocalAmount

    FROM tblCollection c
    WHERE   (@pstrLocationKey           IS NULL OR c.fstrLocationKey        = @pstrLocationKey)
    AND     (@pdtmTransactionDateFrom   IS NULL OR c.fdtmTransactionDate    >= @pdtmTransactionDateFrom)
    AND     (@pdtmTransactionDateTo     IS NULL OR c.fdtmTransactionDate    <= @pdtmTransactionDateTo)

    GROUP BY c.flngPaymentTypeKey,
             c.flngCollectionTypeKey,
             c.fstrCurrencyKey,
             c.fstrLocationKey,
             c.flngCollectionTypeKey,
             c.fdtmTransactionDate
END

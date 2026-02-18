CREATE PROCEDURE [dbo].[spDetailReport]
	@pstrLocationKey		VARCHAR(50),
	@pdtmFrom				DATETIME2 ,
	@pdtmTo					DATETIME2,
	@pstrFirstname			VARCHAR(50) = NULL,
	@pstrLastname			VARCHAR(50) = NULL,
	@plngCollectionTypeKey	INT			= NULL,
	@plngPaymentTypeKey		INT			= NULL
AS
BEGIN
	SET NOCOUNT ON;

	    -- Normalize string inputs
    SET @pstrFirstname = NULLIF(LTRIM(RTRIM(@pstrFirstname)), '');
    SET @pstrLastname  = NULLIF(LTRIM(RTRIM(@pstrLastname)), '');

		SELECT CONCAT(c.fstrFirstname, ' ', c.fstrLastname) AS fstrFullname,
		   c.flngMemberStatusKey,
		   c.flngCollectionTypeKey,
		   c.flngPaymentTypeKey,
		   c.fstrCurrencyKey,
		   c.fcurAmount,
		   c.fcurLocalAmount
	FROM tblCollection c
	WHERE c.fstrLocationKey = @pstrLocationKey
	AND c.fdtmTransactionDate >= @pdtmFrom
	AND c.fdtmTransactionDate <= @pdtmTo
	AND (@pstrFirstname IS NULL OR c.fstrFirstname LIKE @pstrFirstname+'%')
	AND (@pstrLastname IS NULL OR c.fstrLastname LIKE @pstrLastname+'%')
	AND (@plngCollectionTypeKey IS NULL OR c.flngCollectionTypeKey = @plngCollectionTypeKey)
	AND (@plngPaymentTypeKey IS NULL OR c.flngPaymentTypeKey = @plngPaymentTypeKey)
END
CREATE PROCEDURE [dbo].[spSelectCollection]
	@pstrLocationKey		VARCHAR(50),
	@pdtmTransactionDate	DATETIME2(0)		= NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM tblCollection
	WHERE	fstrLocationKey										   = @pstrLocationKey
	AND		(@pdtmTransactionDate	IS NULL OR fdtmTransactionDate = @pdtmTransactionDate)
	AND		flngVer												    = 0
END

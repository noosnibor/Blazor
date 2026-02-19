CREATE PROCEDURE [dbo].[spDailyReport]
	@pstrLocationKey		VARCHAR(50),
	@pdtmTransactionDate	DATETIME2(0)		= NULL,
	@plngCollectionNumber	INT					= NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM tblCollection
	WHERE	fstrLocationKey										   = @pstrLocationKey
	AND		(@pdtmTransactionDate	IS NULL OR fdtmTransactionDate = @pdtmTransactionDate)
	AND		(@plngCollectionNumber	IS NULL OR flngCollectionNumber = @plngCollectionNumber)
	AND		flngVer												    = 0
END

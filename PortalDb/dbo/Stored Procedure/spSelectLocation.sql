CREATE PROCEDURE [dbo].[spSelectLocation]
	 @pstrLocationKey   NVARCHAR(50)    = NULL,
     @pblnActive        BIT             = NULL
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
        fstrLocationKey,
        fstrLocationName,
        fstrLocationAddress,
        fblnActive,
        fstrWho,
        fdtmWhen
    FROM tblLocation
    WHERE   (@pstrLocationKey   IS NULL OR fstrLocationKey  = @pstrLocationKey)
    AND     (@pblnActive        IS NULL OR fblnActive       = @pblnActive)
    AND     flngVer                                         = 0
END

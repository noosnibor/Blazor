CREATE PROCEDURE [dbo].[spUpdateLocation]
	@pstrLocationKey        NVARCHAR(50),
    @pstrLocationName       NVARCHAR(50), 
    @pstrLocationAddress    NVARCHAR(50), 
    @pblnActive             INT, 
    @pstrWho                NVARCHAR(50),  
    @pdtmWhen               DATETIME2(0)
AS
BEGIN
    DECLARE @MaxVer INT;
    SET @MaxVer = (SELECT MAX(flngVer) FROM tblLocation WHERE fstrLocationKey = @pstrLocationKey) + 1;

     INSERT INTO tblLocation
        (
            fstrLocationKey,
            flngVer,
            fstrLocationName,
            fstrLocationAddress,
            fblnActive,
            fstrWho,
            fdtmWhen
        )
        SELECT  fstrLocationKey,
                @MaxVer,
                fstrLocationName,
                fstrLocationAddress,
                fblnActive,
                fstrWho,
                fdtmWhen
        FROM tblLocation WHERE fstrLocationKey = @pstrLocationKey AND flngVer = 0

         UPDATE tblLocation SET
            fstrLocationName        = @pstrLocationName,
            fstrLocationAddress     = @pstrLocationAddress,
            fblnActive              = @pblnActive,
            fstrWho                 = @pstrWho,
            fdtmWhen                = @pdtmWhen
            WHERE fstrLocationKey   = @pstrLocationKey
            AND flngVer             = 0
END

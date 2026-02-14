CREATE PROCEDURE [dbo].[spAddLocation]
	@pstrLocationKey          NVARCHAR(50),
    @pstrLocationName         NVARCHAR(50),
    @pstrLocationAddress      NVARCHAR(50),
    @pblnActive               BIT,
    @pstrWho                  NVARCHAR(50),
    @pdtmWhen                DATETIME2
AS
BEGIN
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
        VALUES
        (
            @pstrLocationKey,
            0,
            @pstrLocationName,
            @pstrLocationAddress,
            @pblnActive,
            @pstrWho,
            @pdtmWhen
        );
END

CREATE PROCEDURE [dbo].[spSelectUser]
    @pstrUsername       VARCHAR(50)     = NULL,
	@pstrLocationKey    VARCHAR(50)     = NULL,
    @pblnActive         BIT             = NULL
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
    FROM tblUser
    WHERE   (@pstrUsername      IS NULL OR fstrUsername         = @pstrUsername)
    AND     (@pstrLocationKey   IS NULL OR fstrLocationKey      = @pstrLocationKey)
    AND     flngVer = 0
    AND     (@pblnActive        IS NULL OR fblnActive           = @pblnActive)
END

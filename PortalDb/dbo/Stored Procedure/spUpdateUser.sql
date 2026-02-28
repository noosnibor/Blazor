CREATE PROCEDURE [dbo].[spUpdateUser]
	@plngUserKey UNIQUEIDENTIFIER,
	@pstrUsername NVARCHAR(50), 
    @pstrPassword NVARCHAR(255),  
    @pstrFirstname NVARCHAR(50), 
    @pstrLastname NVARCHAR(50), 
    @pstrEmailAddress NVARCHAR(50), 
    @pstrLocationKey NVARCHAR(10),
    @plngRoleKey INT,
    @pstrCurrencyKey NVARCHAR(10),
    @pblnActive BIT, 
    @pblnPasswordChanged BIT,
    @pdtmStart DATETIME2(0),
    @pdtmEnd DATETIME2(0),
    @pstrWho NVARCHAR(50), 
    @pdtmWhen DATETIME2
AS
BEGIN
    DECLARE @MaxVer INT;
    SET @MaxVer = ISNULL((SELECT MAX(flngVer) FROM tblUser WHERE flngUserKey = @plngUserKey), 0) + 1;

     INSERT INTO tblUser(flngUserKey,
                        flngVer,
                        fstrUsername, 
                        fstrPassword,
                        fstrFirstname, 
                        fstrLastname, 
                        fstrEmailAddress, 
                        fstrLocationKey, 
                        flngRoleKey, 
                        fstrCurrencyKey, 
                        fblnActive, 
                        fblnPasswordChanged, 
                        fdtmStart, 
                        fdtmEnd, 
                        fstrWho, 
                        fdtmWhen)
    SELECT  flngUserKey,
            @MaxVer,
            fstrUsername, 
            fstrPassword,
            fstrFirstname, 
            fstrLastname, 
            fstrEmailAddress, 
            fstrLocationKey, 
            flngRoleKey, 
            fstrCurrencyKey, 
            fblnActive, 
            fblnPasswordChanged, 
            fdtmStart, 
            fdtmEnd, 
            fstrWho, 
            fdtmWhen
    FROM    tblUser     WHERE flngUserKey = @plngUserKey AND flngVer = 0


     UPDATE tblUser SET
       fstrFirstname        = @pstrFirstname,
       fstrLastname         = @pstrLastname,
       fstrEmailAddress     = @pstrEmailAddress,
       fstrUsername         = @pstrUsername,
       fstrPassword         = @pstrPassword,
       fstrLocationKey      = @pstrLocationKey,
       flngRoleKey          = @plngRoleKey,
       fstrCurrencyKey      = @pstrCurrencyKey,
       fblnActive           = @pblnActive,
       fblnPasswordChanged  = @pblnPasswordChanged,
       fdtmStart            = @pdtmStart,
       fdtmEnd              = @pdtmEnd,
       fstrWho              = @pstrWho,
       fdtmWhen             = @pdtmWhen
       WHERE flngUserKey    = @plngUserKey
       AND flngVer          = 0
END

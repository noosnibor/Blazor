CREATE PROCEDURE [dbo].[spSelectPermission]
	@plngPermissionKey  INT = NULL,
	@plngRoleKey        INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

    SELECT flngPermissionKey,
           flngRoleKey
    FROM    tblPermission
    WHERE (@plngPermissionKey   IS NULL OR flngPermissionKey    = @plngPermissionKey)
    AND (@plngRoleKey           IS NULL OR flngRoleKey          = @plngRoleKey)
END
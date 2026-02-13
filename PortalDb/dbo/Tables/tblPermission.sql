CREATE TABLE [dbo].[tblPermission]
(
	[flngPermissionKey] INT NOT NULL, 
    [flngRoleKey] INT NOT NULL,
	CONSTRAINT PK_tblPermission PRIMARY KEY ([flngPermissionKey], [flngRoleKey])
)

CREATE TABLE [dbo].[tblUser]
(
	[flngUserKey] INT IDENTITY(1,1) NOT NULL,
    [flngVer] INT NOT NULL, 
    [fstrUsername] NVARCHAR(50) NOT NULL, 
    [fstrPassword] NVARCHAR(500) NOT NULL, 
    [fstrFirstname] NVARCHAR(50) NOT NULL, 
    [fstrLastname] NVARCHAR(50) NOT NULL, 
    [fstrEmailAddress] NVARCHAR(50) NOT NULL, 
    [fstrLocationKey] NVARCHAR(10) NOT NULL, 
    [flngRoleKey] INT NOT NULL, 
    [fstrCurrencyKey] NVARCHAR(10) NOT NULL, 
    [fblnActive] BIT NOT NULL , 
    [fblnPasswordChanged] BIT NOT NULL , 
    [fdtmStart] DATETIME2(0) NOT NULL,
    [fdtmEnd] DATETIME2(0) NOT NULL,
    [fstrWho] NVARCHAR(50) NOT NULL, 
    [fdtmWhen] DATETIME NOT NULL,
    CONSTRAINT PK_tblUser PRIMARY KEY CLUSTERED ([flngUserKey], [flngVer]),

    CONSTRAINT UQ_tblUser_Username UNIQUE ([fstrUsername], [fstrLocationKey], [fblnActive]),
)

GO

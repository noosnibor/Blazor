CREATE TABLE [dbo].[tblLocation]
(
	[fstrLocationKey] NVARCHAR(50) NOT NULL, 
    [flngVer] INT NOT NULL, 
    [fstrLocationName] NVARCHAR(50) NOT NULL, 
    [fstrLocationAddress] NVARCHAR(255) NOT NULL, 
    [fblnActive] BIT NOT NULL, 
    [fstrWho] NVARCHAR(50) NOT NULL,  
    [fdtmWhen] DATETIME2(0) NOT NULL,
    CONSTRAINT PK_tblLocation PRIMARY KEY (fstrLocationKey, flngVer),
    CONSTRAINT UQ_tblLocation UNIQUE ([fstrLocationName])
)

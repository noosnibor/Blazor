CREATE TABLE [dbo].[tblCurrency]
(
	[fstrCurrencyKey] VARCHAR(50) NOT NULL, 
    [flngVer] INT NOT NULL, 
    [fstrCurrencyType] VARCHAR(50) NOT NULL, 
    [fstrLocationKey] VARCHAR(50) NOT NULL, 
    [fcurAmount] MONEY NOT NULL,
    [fblnActive] BIT NOT NULL, 
    [fstrWho] VARCHAR(50) NOT NULL, 
    [fdtmEffectiveFrom] DATETIME2(0) NOT NULL,
    [fdtmEffectiveTo] DATETIME2(0) NULL, 
    CONSTRAINT PK_tblCurrency PRIMARY KEY (fstrCurrencyKey, flngVer, fstrLocationKey, fcurAmount),

)

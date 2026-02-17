CREATE TABLE [dbo].[tblCollection]
(
	flngCollectionKey      INT IDENTITY(1,1) NOT NULL,
    flngVer                INT NOT NULL,
    fstrFirstname          VARCHAR(50) NOT NULL,
    fstrLastname           VARCHAR(50) NOT NULL,
    fstrEmailAddress       VARCHAR(50) NOT NULL,
    fdtmTransactionDate    DATETIME2(0) NOT NULL,
    flngCollectionNumber   INT NOT NULL,
    flngMemberStatusKey    INT NOT NULL,
    flngCollectionTypeKey  INT NOT NULL,
    flngPaymentTypeKey     INT NOT NULL,
    fcurAmount             DECIMAL(18,2) NOT NULL,
    fcurLocalAmount        DECIMAL(18,2) NOT NULL,
    fstrCurrencyKey        VARCHAR(50) NOT NULL,
    fblnActive             BIT NOT NULL,
    fstrLocationKey        VARCHAR(50) NOT NULL,
    fstrWho                VARCHAR(50) NOT NULL,
    fdtmWhen               DATETIME2(0) NOT NULL,
    CONSTRAINT PK_tblCollection PRIMARY KEY CLUSTERED (flngCollectionKey, flngVer, fstrLocationKey)
)

CREATE TABLE [Users] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [Username] NVARCHAR(255) UNIQUE NOT NULL,
  [Email] NVARCHAR(255) UNIQUE NOT NULL,
  [IsModerator] BIT NOT NULL,
  [IsDeleted] BIT DEFAULT (0)
)
GO

CREATE TABLE [Countries] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [Name] NVARCHAR(255) UNIQUE NOT NULL,
  [Description] TEXT,
  [IconUrl] NVARCHAR(255) NOT NULL
)
GO

CREATE TABLE [Locations] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [Name] NVARCHAR(255) NOT NULL,
  [CountryId] INT NOT NULL
)
GO

CREATE TABLE [Images] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [Title] NVARCHAR(255),
  [Url] NVARCHAR(255) NOT NULL,
  [ManuscriptId] INT NOT NULL,
  [IsDeleted] BIT DEFAULT (0)
)
GO

CREATE TABLE [Tags] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [Name] NVARCHAR(255) NOT NULL,
  [Description] TEXT,
  [IsDeleted] BIT DEFAULT (0)
)
GO

CREATE TABLE [Manuscripts] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [Title] NVARCHAR(255) NOT NULL,
  [Description] TEXT,
  [YearWrittenStart] INT NOT NULL,
  [YearWrittenEnd] INT,
  [SourceUrl] NVARCHAR(255),
  [CreatedAt] DATETIME,
  [LocationId] INT NOT NULL,
  [AuthorId] INT NOT NULL,
  [IsDeleted] BIT DEFAULT (0),
  CHECK ([YearWrittenEnd] IS NULL OR [YearWrittenStart] <= [YearWrittenEnd])
)
GO

CREATE TABLE [ManuscriptTag] (
  [ManuscriptId] INT NOT NULL,
  [TagId] INT NOT NULL,
  PRIMARY KEY ([ManuscriptId], [TagId])
)
GO

CREATE TABLE [ImageTag] (
  [ImageId] INT NOT NULL,
  [TagId] INT NOT NULL,
  PRIMARY KEY ([ImageId], [TagId])
)
GO

-- Indexes
CREATE INDEX [IX_User_Email] ON [Users] ([Email])
GO

CREATE INDEX [IX_Countries_Name] ON [Countries] ([Name])
GO

CREATE INDEX [IX_Locations_Name] ON [Locations] ([Name])
GO

CREATE INDEX [IX_Locations_Country] ON [Locations] ([CountryId])
GO

CREATE INDEX [IX_Images_Manuscript] ON [Images] ([ManuscriptId])
GO

CREATE INDEX [IX_Tags_Name] ON [Tags] ([Name])
GO

CREATE INDEX [IX_Manuscripts_YearStart] ON [Manuscripts] ([YearWrittenStart])
GO

CREATE INDEX [IX_Manuscripts_YearEnd] ON [Manuscripts] ([YearWrittenEnd])
GO

CREATE INDEX [IX_Manuscripts_Location] ON [Manuscripts] ([LocationId])
GO

CREATE INDEX [IX_Manuscripts_Author] ON [Manuscripts] ([AuthorId])
GO

CREATE INDEX [IX_Manuscripts_YearRange] ON [Manuscripts] ([YearWrittenStart], [YearWrittenEnd])
GO

-- Named Foreign Keys
ALTER TABLE [ManuscriptTag] 
  ADD CONSTRAINT [FK_ManuscriptTag_Manuscript] FOREIGN KEY ([ManuscriptId]) REFERENCES [Manuscripts] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [ManuscriptTag] 
  ADD CONSTRAINT [FK_ManuscriptTag_Tag] FOREIGN KEY ([TagId]) REFERENCES [Tags] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [ImageTag] 
  ADD CONSTRAINT [FK_ImageTag_Image] FOREIGN KEY ([ImageId]) REFERENCES [Images] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [ImageTag] 
  ADD CONSTRAINT [FK_ImageTag_Tag] FOREIGN KEY ([TagId]) REFERENCES [Tags] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [Manuscripts] 
  ADD CONSTRAINT [FK_Manuscripts_Location] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id])
GO

ALTER TABLE [Locations] 
  ADD CONSTRAINT [FK_Locations_Country] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id])
GO

ALTER TABLE [Images] 
  ADD CONSTRAINT [FK_Images_Manuscript] FOREIGN KEY ([ManuscriptId]) REFERENCES [Manuscripts] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [Manuscripts] 
  ADD CONSTRAINT [FK_Manuscripts_User] FOREIGN KEY ([AuthorId]) REFERENCES [Users] ([Id])
GO

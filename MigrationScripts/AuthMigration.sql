-- Create UserAuth table
CREATE TABLE [UserAuth] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [UserId] INT NOT NULL UNIQUE,
  [PasswordHash] NVARCHAR(MAX) NOT NULL
)
GO

-- Add foreign key constraint to Users
ALTER TABLE [UserAuth]
  ADD CONSTRAINT [FK_UserAuth_User]
  FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
GO

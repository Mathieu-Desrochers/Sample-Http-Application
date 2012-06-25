
-------------------------------------------------------------------------
-- Issue 10001
-------------------------------------------------------------------------

-- Create the Session table.
CREATE TABLE [Session]
(
	[SessionId] INT NOT NULL IDENTITY(1, 1), 
    [SessionCode] NVARCHAR(50) NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [StartDate] DATETIME NOT NULL
)
GO

-- Create the SessionID primary key.
ALTER TABLE [Session]
ADD CONSTRAINT [PK_Session]
PRIMARY KEY CLUSTERED ([SessionID])
GO

-- Create the SessionCode index.
CREATE UNIQUE NONCLUSTERED
INDEX [IX_Session_SessionCode]
ON [Session] ([SessionCode])
GO

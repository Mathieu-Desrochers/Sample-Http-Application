
-------------------------------------------------------------------------
-- Issue 10001
-------------------------------------------------------------------------

-- Create the Session table.
CREATE TABLE [Session]
(
	[SessionID] INT NOT NULL IDENTITY(1, 1), 
	[SessionCode] NVARCHAR(50) NOT NULL, 
	[Name] NVARCHAR(50) NOT NULL, 
	[StartDate] DATE NOT NULL
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

-------------------------------------------------------------------------
-- Issue 10005
-------------------------------------------------------------------------

-- Create the CourseSchedule table.
CREATE TABLE [CourseSchedule]
(
	[CourseScheduleID] INT NOT NULL IDENTITY(1, 1),
	[CourseScheduleCode] NVARCHAR(50) NOT NULL,
	[SessionID] INT NOT NULL,
	[DayOfWeek] INT NOT NULL,
	[Time] TIME NOT NULL
)
GO

-- Create the CourseScheduleID primary key.
ALTER TABLE [CourseSchedule]
ADD CONSTRAINT [PK_CourseSchedule]
PRIMARY KEY CLUSTERED ([CourseScheduleID])
GO

-- Create the CourseScheduleCode index.
CREATE UNIQUE NONCLUSTERED
INDEX [IX_CourseSchedule_CourseScheduleCode]
ON [CourseSchedule] ([CourseScheduleCode])
GO

-- Create the SessionID foreign key.
ALTER TABLE [CourseSchedule]
ADD CONSTRAINT [FK_CourseSchedule_Session]
FOREIGN KEY ([SessionID])
REFERENCES [Session] ([SessionID])
GO

-- Create the SessionID index.
CREATE NONCLUSTERED
INDEX [IX_CourseSchedule_SessionID]
ON [CourseSchedule] ([SessionID])
GO

-------------------------------------------------------------------------
-- Issue 10006
-------------------------------------------------------------------------

-- Create the CourseGroup table.
CREATE TABLE [CourseGroup]
(
	[CourseGroupID] INT NOT NULL IDENTITY(1, 1),
	[CourseGroupCode] NVARCHAR(50) NOT NULL,
	[CourseScheduleID] INT NOT NULL,
	[PlacesCount] INT NOT NULL
)
GO

-- Create the CourseGroupID primary key.
ALTER TABLE [CourseGroup]
ADD CONSTRAINT [PK_CourseGroup]
PRIMARY KEY CLUSTERED ([CourseGroupID])
GO

-- Create the CourseGroupCode index.
CREATE UNIQUE NONCLUSTERED
INDEX [IX_CourseGroup_CourseGroupCode]
ON [CourseGroup] ([CourseGroupCode])
GO

-- Create the CourseScheduleID foreign key.
ALTER TABLE [CourseGroup]
ADD CONSTRAINT [FK_CourseGroup_CourseSchedule]
FOREIGN KEY ([CourseScheduleID])
REFERENCES [CourseSchedule] ([CourseScheduleID])
GO

-- Create the CourseScheduleID index.
CREATE NONCLUSTERED
INDEX [IX_CourseSchedule_CourseScheduleID]
ON [CourseSchedule] ([CourseScheduleID])
GO

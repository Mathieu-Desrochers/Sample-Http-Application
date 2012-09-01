
-------------------------------------------------------------------------
-- Issue 10001
-------------------------------------------------------------------------

-- Create the Session table.
CREATE TABLE [Session]
(
	[SessionId] INT NOT NULL IDENTITY(1, 1), 
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
	[CourseScheduleId] INT NOT NULL IDENTITY(1, 1),
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

-- Create the SessionID foreign key.
ALTER TABLE [CourseSchedule]
ADD CONSTRAINT [FK_CourseSchedule_Session]
FOREIGN KEY (SessionID)
REFERENCES [Session] (SessionID)
GO

-- Create the CourseScheduleCode index.
CREATE UNIQUE NONCLUSTERED
INDEX [IX_CourseSchedule_CourseScheduleCode]
ON [CourseSchedule] ([CourseScheduleCode])
GO

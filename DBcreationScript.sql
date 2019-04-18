/*
* FILE : DBcreationScript.sql
* PROJECT : PROG3070 - Project Milestone 01
* PROGRAMMER : Andrii Dushkevych, Phil Kempton
* FIRST VERSION : 2019-03-18
* DESCRIPTION :
* This file contains DB creation script and insert statements for ConfigInfo table
*/
 
--Drop if exists and create database
DROP DATABASE IF EXISTS AdvSqlProjectDB
GO
CREATE DATABASE AdvSqlProjectDB
GO
SET ANSI_NULLS ON
GO
 
SET QUOTED_IDENTIFIER ON
GO
 
--Create table Part
CREATE TABLE [AdvSqlProjectDB].[dbo].[Part](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [varchar](255) NULL,
PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
 
--Create table ConfigInfo
CREATE TABLE [AdvSqlProjectDB].[dbo].[ConfigInfo](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Key] [nvarchar](255) NOT NULL,
    [Value] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
 
--Insert configuration data into ConfigInfo table
INSERT INTO [AdvSqlProjectDB].[dbo].[ConfigInfo]([Key], [Value]) VALUES
('HarnessBinAmount', '55'),
('ReflectorBinAmount', '35'),
('HousingBinAmount', '24'),
('LensBinAmount', '40'),
('BulbBinAmount', '60'),
('BezelBinAmount', '75'),
('CriticalBinAmount', '5'),
('ExperiencedWorkerAssemblyTimeSeconds', '60'),
('ExperiencedWorkerAssemblyTimeAdjustmentPrecentage', '10'),
('NewWorkerAssemblyTimeAdjustmentPrecentage', '50'),
('SuperWorkerAssemblyTimeAdjustmentPrecentage', '-15'),
('ExperiencedWorkerDefectRatePercentage', '0.5'),
('NewWorkerDefectRatePercentage', '0.85'),
('SuperWorkerDefectRatePercentage', '0.15'),
('TrayCapacity', '60'),
('CardPickUpTimeoutSeconds', '300'),
('BinRefilTimeSeconds', '300'),
('TimeFactor','60')
GO
 
--Create table EmployeeType
CREATE TABLE [AdvSqlProjectDB].[dbo].[EmployeeType](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](255) NOT NULL,
    [TimeFactorConfItemId] [int] NOT NULL,
    [DefectRateConfItemId] [int] NOT NULL,
PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
 
ALTER TABLE [AdvSqlProjectDB].[dbo].[EmployeeType] ADD FOREIGN KEY([DefectRateConfItemId])
REFERENCES [AdvSqlProjectDB].[dbo].[ConfigInfo] ([Id])
GO
 
ALTER TABLE [AdvSqlProjectDB].[dbo].[EmployeeType] ADD FOREIGN KEY([TimeFactorConfItemId])
REFERENCES [AdvSqlProjectDB].[dbo].[ConfigInfo] ([Id])
GO
 
--Create table Employee
CREATE TABLE [AdvSqlProjectDB].[dbo].[Employee](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [TypeId] [int] NOT NULL,
PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
 
ALTER TABLE [AdvSqlProjectDB].[dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([TypeId])
REFERENCES [AdvSqlProjectDB].[dbo].[EmployeeType] ([Id])
GO
 
--Create table Tray
CREATE TABLE [AdvSqlProjectDB].[dbo].[Tray](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Number] [int] NOT NULL,
    [LampAmountConfItemId] [int] NOT NULL,
    [IsFull] [BIT] NOT NULL DEFAULT 0,
    [WorkstationId] [INT] NOT NULL,
PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
 
ALTER TABLE [AdvSqlProjectDB].[dbo].[Tray]  WITH CHECK ADD FOREIGN KEY([LampAmountConfItemId])
REFERENCES [AdvSqlProjectDB].[dbo].[ConfigInfo] ([Id])
GO
 
 
--Create table Workstation
CREATE TABLE [AdvSqlProjectDB].[dbo].[Workstation](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [EmployeeId] [int] NOT NULL,
PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
 
ALTER TABLE [AdvSqlProjectDB].[dbo].[Workstation]  WITH CHECK ADD FOREIGN KEY([EmployeeId])
REFERENCES [AdvSqlProjectDB].[dbo].[Employee] ([Id])
GO
 
ALTER TABLE [AdvSqlProjectDB].[dbo].[Tray]  WITH CHECK ADD FOREIGN KEY([WorkstationId])
REFERENCES [AdvSqlProjectDB].[dbo].[Workstation] ([Id])
GO
 
--Create table Bin
CREATE TABLE [AdvSqlProjectDB].[dbo].[Bin](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [PartId] [int] NOT NULL,
    [WorkstationId] [int] NOT NULL,
    [PartCount] [int] NOT NULL,
    [AmountConfItemId] [int] NOT NULL,
PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
 
ALTER TABLE [AdvSqlProjectDB].[dbo].[Bin]  WITH CHECK ADD FOREIGN KEY([AmountConfItemId])
REFERENCES [AdvSqlProjectDB].[dbo].[ConfigInfo] ([Id])
GO
 
ALTER TABLE [AdvSqlProjectDB].[dbo].[Bin]  WITH CHECK ADD FOREIGN KEY([PartId])
REFERENCES [AdvSqlProjectDB].[dbo].[Part] ([Id])
GO
 
ALTER TABLE [AdvSqlProjectDB].[dbo].[Bin]  WITH CHECK ADD FOREIGN KEY([WorkstationId])
REFERENCES [AdvSqlProjectDB].[dbo].[Workstation] ([Id])
GO
 
 
--Create table Lamp
CREATE TABLE [AdvSqlProjectDB].[dbo].[Lamp](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Number] [int] NOT NULL,
    [TestPassed] [bit] NULL,
    [TrayId] [int] NOT NULL,
PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
 
ALTER TABLE [AdvSqlProjectDB].[dbo].[Lamp]  WITH CHECK ADD FOREIGN KEY([TrayId])
REFERENCES [AdvSqlProjectDB].[dbo].[Tray] ([Id])
GO
 
--Create table Runner
CREATE TABLE [AdvSqlProjectDB].[dbo].[Runner](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [WorkstationId] [int] NOT NULL,
    [isRunning] [bit] NOT NULL,
PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
 
ALTER TABLE [AdvSqlProjectDB].[dbo].[Runner]  WITH CHECK ADD FOREIGN KEY([WorkstationId])
REFERENCES [AdvSqlProjectDB].[dbo].[Workstation] ([Id])
GO

--Create table KanbanInfo
CREATE TABLE [AdvSqlProjectDB].[dbo].[KanbanInfo](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [WorkstationId] [int] NOT NULL,
	[EmployeeType] [nvarchar](255) NOT NULL,
	[CompletedTrays] [int] NOT NULL DEFAULT 0,
	[CompletedLamps] [int] NOT NULL DEFAULT 0,
	[TestedLamps] [int] NOT NULL DEFAULT 0,
	[DefectRate] [float] NOT NULL DEFAULT 0)
GO

ALTER TABLE [AdvSqlProjectDB].[dbo].[KanbanInfo]  WITH CHECK ADD FOREIGN KEY([WorkstationId])
REFERENCES [AdvSqlProjectDB].[dbo].[Workstation] ([Id])
GO

-- Trigger: tr_InsertWorkstationIntoKanban
-- Description: This trigger inserts WorkstationId into KanbanInfo table after creating new Workstation
-- Parameters: n/a
-- Returns: n/a
USE [AdvSqlProjectDB]
GO
CREATE TRIGGER [tr_InsertWorkstationIntoKanban] ON [AdvSqlProjectDB].[dbo].[Workstation]
AFTER INSERT
AS
BEGIN
	DECLARE @wsIdToInsert		INT
	DECLARE @empTypeToInsert	NVARCHAR(255)

	SET @wsIdToInsert = (SELECT MAX(Id) FROM [AdvSqlProjectDB].[dbo].[Workstation])
	SET @empTypeToInsert = (SELECT [Name] FROM [AdvSqlProjectDB].[dbo].[EmployeeType] et
								JOIN [AdvSqlProjectDB].[dbo].[Employee] e ON e.TypeId = et.Id
								JOIN [AdvSqlProjectDB].[dbo].[Workstation] ws ON ws.EmployeeId = e.Id
							WHERE ws.Id = @wsIdToInsert)

	INSERT INTO [AdvSqlProjectDB].[dbo].[KanbanInfo](WorkstationId, EmployeeType)
	VALUES(@wsIdToInsert, @empTypeToInsert)
END
GO
 
-- Insert initial Employee Types
INSERT INTO [AdvSqlProjectDB].[dbo].[EmployeeType] (Name, TimeFactorConfItemId, DefectRateConfItemId) VALUES ('Experienced', 9, 12);
INSERT INTO [AdvSqlProjectDB].[dbo].[EmployeeType] (Name, TimeFactorConfItemId, DefectRateConfItemId) VALUES ('Rookie', 10, 13);
INSERT INTO [AdvSqlProjectDB].[dbo].[EmployeeType] (Name, TimeFactorConfItemId, DefectRateConfItemId) VALUES ('Super', 11, 14);
 
-- Insert initial Employees (1 of each type)
--INSERT INTO [AdvSqlProjectDB].[dbo].[Employee] (TypeId) VALUES (3);
--INSERT INTO [AdvSqlProjectDB].[dbo].[Employee] (TypeId) VALUES (2);
--INSERT INTO [AdvSqlProjectDB].[dbo].[Employee] (TypeId) VALUES (1);
 
-- Insert initial Part types
INSERT INTO [AdvSqlProjectDB].[dbo].[Part] (Name) VALUES ('Harness');
INSERT INTO [AdvSqlProjectDB].[dbo].[Part] (Name) VALUES ('Reflector');
INSERT INTO [AdvSqlProjectDB].[dbo].[Part] (Name) VALUES ('Housing');
INSERT INTO [AdvSqlProjectDB].[dbo].[Part] (Name) VALUES ('Lens');
INSERT INTO [AdvSqlProjectDB].[dbo].[Part] (Name) VALUES ('Bulb');
INSERT INTO [AdvSqlProjectDB].[dbo].[Part] (Name) VALUES ('Bezel');
 
 
-- Procedure: start_new_lamp
-- Description: This procedure should be called when a worstation starts making a new lamp
--              This procedure decrements all bins belonging to a worksation by 1
--              This procedure should be called synchronously so that the worksation doesn't start counting
--                  down remaining time to assemble parts until the bins have parts in them.
-- Parameters: 
--      @StationId: The Id of the workstation who calls this procedure
-- Returns: n/a
USE [AdvSqlProjectDB]
GO
CREATE PROCEDURE start_new_lamp
@StationId nvarchar(50)
AS
    DECLARE
    @MinPartCount [INT];
 
    SET @MinPartCount = (SELECT MIN(PartCount) FROM Bin WHERE WorkstationId = @StationId);
 
    -- Wait until we have parts
    WHILE (@MinPartCount = 0)
    BEGIN
        SET @MinPartCount = (SELECT MIN(PartCount) FROM Bin WHERE WorkstationId = @StationId);
        WAITFOR DELAY '00:00:01';
    END
 
   
    -- Decrement all bins belonging to this Workstation
    UPDATE Bin
    SET Bin.PartCount = PartCount - 1
    WHERE WorkstationId = @StationId;
GO
 
 
-- Procedure: SwapTray
-- Description: This procedure Swaps out a full tray for a new tray
-- Parameters: 
--      @StationId: The Id of the workstation who calls this procedure
--      @TrayId:    The Id of the Tray to swap out
-- Returns: n/a
CREATE PROCEDURE SwapTray @StationId nvarchar(50), @TrayId nvarchar(50)
AS
    -- Declare local variables to store relevant row id's
    DECLARE
    @TrayCapacity INT,      -- Max capacity of a tray
    @Rand INT,              -- Will store random value beteen 0.00 and 100.00, used to process defects
    @EmployeeId INT,        -- Id of the workstations employee
    @EmployeeTypeId INT,    -- Id of the type of employee
    @DefectRate FLOAT,      -- Decimal number that is defect rate
    @DefectConfId INT,      -- Id in config info table that is our defect rate
    @DefectRateString VARCHAR(20),  -- String of defect rate
    @LampNum INT = 1;       -- Keep track of which lamp in tray is being tested
 
    -- Get the defect rate
    SELECT @EmployeeId = EmployeeId FROM Workstation WITH (NOLOCK) WHERE Id = @StationId;
    SELECT @EmployeeTypeId = TypeId FROM Employee WITH (NOLOCK) WHERE Id = @EmployeeId;
    SELECT @DefectConfId = DefectRateConfItemId FROM EmployeeType WITH (NOLOCK) WHERE Id = @EmployeeTypeId;
    SELECT @DefectRateString = [Value] FROM ConfigInfo WITH (NOLOCK) WHERE [Id] = @DefectConfId;
    SET @DefectRate = CAST(@DefectRateString AS DECIMAL);
 
    -- Get Tray Capacity to loop through all lamps, assigning defect status
    SET @TrayCapacity = (SELECT CAST(Value AS INT)
                         FROM ConfigInfo
                         WHERE Id = 15);
 
        -- Set the Tray as isFull = 1
        UPDATE Tray
        SET isFull = 1
        WHERE Id = @TrayId;
 
        -- Assign all the lamps in the finished tray a Test Result based on the defect rate
        WHILE (@LampNum <= @TrayCapacity)
        BEGIN
            SELECT @Rand = RAND()*100;
            IF (@Rand <= @DefectRate)
            -- Lamp Failed Test
                UPDATE LAMP SET TestPassed = 0 WHERE TrayId = @TrayId AND Number = @LampNum;
            ELSE
            BEGIN
            -- Lamp Passed Test
                UPDATE LAMP SET TestPassed = 1 WHERE TrayId = @TrayId AND Number = @LampNum;
            END;
            SET @LampNum = @LampNum + 1;
        END;
 
        -- Insert new tray into tray table
        INSERT INTO Tray (Number, LampAmountConfItemId, IsFull, WorkstationId)
        VALUES (0, 15, 0, @StationId);
GO
 
 
-- Procedure: finish_lamp
-- Description: This procedure should be called when a worstation finishes a lamp.
--              This procedure inserts a new lamp into the current tray.
--              This procedure will also swap out the current tray for a new one if it's full.
--              This procedure shoudl be calle asynchronously so that the workstation can still start a new part
--                  even if runner is getting more bins
-- Parameters: 
--      @StationId: The Id of the workstation who calls this procedure
-- Returns: n/a
CREATE PROCEDURE finish_lamp @StationId nvarchar(50)
AS
    -- Declare some variables for this procedure
    DECLARE
    @TrayId INT,        -- Id of the calling workstation's tray
    @TrayCapacity INT,  -- Max capacity of a tray
    @LampPosition INT   -- Position in tray of this newly inserted lamp (1 - @TrayCapacity)
 
 
    -- Add tray for this workstation if one isn't currently
    IF NOT EXISTS(SELECT Id FROM Tray WHERE WorkstationId = @StationId AND isFull = 0)
    BEGIN
        INSERT INTO Tray (Number, LampAmountConfItemId, IsFull, WorkstationId)
        VALUES (0, 15, 0, @StationId);
    END
 
    -- Get The Id of current Tray
    SELECT @TrayId = Id
    FROM Tray
    WHERE WorkstationId = @StationId AND isFull = 0;
 
    -- Increment number of lamps in current tray
    UPDATE Tray
    SET Number = Number + 1
    WHERE Id = @TrayId;
 
    -- Get Position of this new lamp
    SELECT @LampPosition = Number
    FROM Tray
    WHERE Id = @TrayId;
 
    -- Insert the new lamp into the current Tray
    INSERT INTO [AdvSqlProjectDB].[dbo].[Lamp] (Number, TestPassed, TrayId) VALUES (@LampPosition, NULL, @TrayId);
 
    -- Get Tray Capacity to check if tray is full
    SET @TrayCapacity = (SELECT CAST(Value AS INT)
                         FROM ConfigInfo
                         WHERE Id = 15);
 
    -- If the current tray is full, swap it out for a new one.
    IF @LampPosition = @TrayCapacity
    BEGIN
        EXEC SwapTray @StationId, @TrayId;
    END
GO
 
-- Procedure: refill_bin
-- Description: This procedure refills a part bin.
--              Current count is added to new bin.
-- Parameters: 
--      @BinId: The Id of the bin
-- Returns: n/a
CREATE PROCEDURE refill_bin @BinId INT
AS
	-- Declare local variables
    DECLARE
    @CurrentAmount INT,
    @MaxCapacityConfigId INT,
    @BinMaxCapacity INT;
 
	-- refill the bin
    SELECT @CurrentAmount = PartCount FROM BIN WHERE Id = @BinId;
    SELECT @MaxCapacityConfigId = AmountConfItemId FROM Bin WHERE Id = @BinId;
    SELECT @BinMaxCapacity = [Value] FROM ConfigInfo WHERE Id = @MaxCapacityConfigId;
    UPDATE Bin SET PartCount = @CurrentAmount + @BinMaxCapacity WHERE Id = @BinId;
GO

-- Procedure: sp_updateKanbanInfo
-- Description: This procedure updates KanbanInfo table with current workstations stats
-- Parameters: no params
-- Returns: nothing
CREATE PROCEDURE sp_updateKanbanInfo
AS
BEGIN
	DECLARE @workstationId	INT
	DECLARE @completedTrays INT
	DECLARE @completedLamps INT
	DECLARE @testedLamps	INT
	DECLARE @defectedLamps	INT
	DECLARE @defectRate		DECIMAL(4,2)

	DECLARE ws_cursor CURSOR FOR
	SELECT [Id] FROM [AdvSqlProjectDB].[dbo].[Workstation] WITH (NOLOCK)

	OPEN ws_cursor  
	FETCH NEXT FROM ws_cursor INTO @workstationId

	WHILE @@FETCH_STATUS = 0  
	BEGIN
		SET @completedTrays = 0
		SET @completedLamps = 0
		SET @testedLamps = 0
		SET @defectedLamps = 0
		SET @defectRate = 0
		--get values for workstation
		SET @completedTrays = (	SELECT COUNT([Id]) FROM [AdvSqlProjectDB].[dbo].[Tray] WITH (NOLOCK)
								WHERE [WorkstationId] = @workstationId
								AND [IsFull] = 1)
								
		SET @completedLamps = (	SELECT COUNT([Id]) FROM [AdvSqlProjectDB].[dbo].[Lamp] WITH (NOLOCK)
								WHERE [TrayId] IN (	SELECT [Id]
													FROM [AdvSqlProjectDB].[dbo].[Tray]
													WHERE [WorkstationId] = @workstationId))
													
		SET @testedLamps = (	SELECT COUNT([Id]) FROM [AdvSqlProjectDB].[dbo].[Lamp] WITH (NOLOCK)
								WHERE [TestPassed] IS NOT NULL
								AND [TrayId] IN (	SELECT [Id]
													FROM [AdvSqlProjectDB].[dbo].[Tray]
													WHERE [WorkstationId] = @workstationId))

		SET @defectedLamps = (	SELECT COUNT([Id]) FROM [AdvSqlProjectDB].[dbo].[Lamp] WITH (NOLOCK)
								WHERE [TestPassed] = 0
								AND [TrayId] IN (	SELECT [Id]
													FROM [AdvSqlProjectDB].[dbo].[Tray]
													WHERE [WorkstationId] = @workstationId))

		IF @testedLamps != 0
		BEGIN
			SET @defectRate =  (SELECT 100.00 * @defectedLamps/@testedLamps)
		END

		--update values for workstation
		UPDATE	[AdvSqlProjectDB].[dbo].[KanbanInfo]
		SET		[CompletedTrays] = @completedTrays,
				[CompletedLamps] = @completedLamps,
				[TestedLamps] = @testedLamps,
				[DefectRate] = @defectRate
		WHERE	[WorkstationId] = @workstationId

		FETCH NEXT FROM ws_cursor INTO @workstationId 
	END 

	CLOSE ws_cursor  
	DEALLOCATE ws_cursor
	
END
GO
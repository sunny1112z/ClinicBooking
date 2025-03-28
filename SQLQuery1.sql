-- Create database
CREATE DATABASE ClinicBooking;
GO

USE ClinicBooking;
GO

-- Table: Roles
CREATE TABLE Roles (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL
);

-- Table: Gender
CREATE TABLE Gender (
    GenderID INT IDENTITY(1,1) PRIMARY KEY,
    GenderName NVARCHAR(10) NOT NULL
);

-- Table: Cities
CREATE TABLE Cities (
    CityID INT IDENTITY(1,1) PRIMARY KEY,
    CityName NVARCHAR(50) NOT NULL
);

-- Table: Users
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    Phone NVARCHAR(20) UNIQUE NOT NULL,
    Username NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    GenderID INT NULL,
    Address NVARCHAR(255) NULL,
    NationalID NVARCHAR(20) UNIQUE NULL,
    CityID INT NULL,
    BloodType NVARCHAR(10) NULL,
    AdditionalInfo NVARCHAR(MAX) NULL,
    Avatar NVARCHAR(255) NULL,
    RoleID INT NOT NULL DEFAULT 1,  -- 1: Patient, 2: Doctor, 3: Admin
    FOREIGN KEY (GenderID) REFERENCES Gender(GenderID),
    FOREIGN KEY (CityID) REFERENCES Cities(CityID),
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

-- Table: Departments
CREATE TABLE Departments (
    DepartmentID INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL
);

-- Table: Doctors
CREATE TABLE Doctors (
    DoctorID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    DepartmentID INT NULL,
    Qualification NVARCHAR(100) NULL,
    DoctorInfo NVARCHAR(MAX) NULL,
    ProfileImage NVARCHAR(255) NULL,
    ZoomInfo NVARCHAR(255) NULL,
    Status BIT NOT NULL DEFAULT 1,
    RoleID INT NOT NULL DEFAULT 2,  -- Mặc định bác sĩ có RoleID = 2
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID),
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

-- Table: Appointments
CREATE TABLE Appointments (
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    Subject NVARCHAR(255) NOT NULL,
    Description NVARCHAR(500) NULL,
    StartTime SMALLDATETIME NOT NULL,
    EndTime SMALLDATETIME NOT NULL,
    Status INT NOT NULL DEFAULT 0,  -- 0: Pending, 1: Confirmed, 2: Completed, 3: Canceled
    ZoomInfo NVARCHAR(255) NULL,
    MedicalResult NVARCHAR(500) NULL,
    UserID INT NOT NULL,
    DoctorID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID)
);

-- Table: FAQ
CREATE TABLE FAQ (
    FAQID INT IDENTITY(1,1) PRIMARY KEY,
    Question NVARCHAR(MAX) NOT NULL,
    Answer NVARCHAR(MAX) NULL,
    UserID INT NULL,
    DoctorID INT NULL,
    SentDate SMALLDATETIME NULL,
    Notes NVARCHAR(MAX) NULL,
    Status INT NOT NULL DEFAULT 0,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID)
);

-- Table: News
CREATE TABLE News (
    NewsID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(MAX) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    Image NVARCHAR(255) NULL,
    Summary NVARCHAR(MAX) NULL,
    PublishDate SMALLDATETIME NOT NULL,
    Category NVARCHAR(50) NOT NULL
);

-- Table: DiseaseStatistics
CREATE TABLE DiseaseStatistics (
    StatisticID INT IDENTITY(1,1) PRIMARY KEY,
    Country NVARCHAR(100) NOT NULL,
    Year INT NOT NULL,
    Disease NVARCHAR(50) NOT NULL,
    Cases INT NOT NULL,
    Deaths INT NOT NULL
);

-- Table: RatingLevels
CREATE TABLE RatingLevels (
    RatingLevelID INT IDENTITY(1,1) PRIMARY KEY,
    RatingDescription NVARCHAR(50) NOT NULL
);

-- Table: Ratings
CREATE TABLE Ratings (
    RatingID INT IDENTITY(1,1) PRIMARY KEY,
    Content NVARCHAR(MAX) NULL,
    RatingLevelID INT NOT NULL,
    UserID INT NULL,
    DoctorID INT NULL,
    AppointmentID INT NULL,
    FOREIGN KEY (RatingLevelID) REFERENCES RatingLevels(RatingLevelID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID),
    FOREIGN KEY (AppointmentID) REFERENCES Appointments(AppointmentID)
);

-- Thêm dữ liệu mẫu vào bảng Roles
INSERT INTO Roles (RoleName) VALUES ('Patient'), ('Doctor'), ('Admin');
ALTER TABLE Users ADD isActive INT NOT NULL DEFAULT 1;
CREATE TABLE [dbo].[WorkSchedules] (
    [ScheduleID] INT IDENTITY(1,1) PRIMARY KEY,
    [DoctorID] INT NOT NULL,
    [WorkDate] DATE NOT NULL, -- Ngày làm việc
    [StartTime] TIME NOT NULL, -- Giờ bắt đầu
    [EndTime] TIME NOT NULL,   -- Giờ kết thúc
    [Status] INT NOT NULL DEFAULT 1, -- Trạng thái (1: Hoạt động, 0: Nghỉ)
    [CreatedBy] INT NOT NULL, -- Ai tạo lịch (Admin)
    FOREIGN KEY ([DoctorID]) REFERENCES [dbo].[Doctors]([DoctorID]),
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users]([UserID])
);
ALTER TABLE [dbo].[Appointments] ADD [ScheduleID] INT NULL;
ALTER TABLE [dbo].[Appointments] ADD CONSTRAINT FK_Appointments_WorkSchedules 
FOREIGN KEY ([ScheduleID]) REFERENCES [dbo].[WorkSchedules]([ScheduleID]);

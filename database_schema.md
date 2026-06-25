## Database Schema

CREATE DATABASE CourseRegistrationDb;
GO

USE CourseRegistrationDb;
GO

CREATE TABLE Tenants
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE TABLE Learners
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    FullName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(250) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_Learners_Tenants
        FOREIGN KEY (TenantId) REFERENCES Tenants(Id)
);
GO

CREATE UNIQUE INDEX UX_Learners_TenantId_Email
ON Learners(TenantId, Email);
GO

CREATE TABLE Courses
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    Title NVARCHAR(250) NOT NULL,
    Capacity INT NOT NULL,
    RegisteredCount INT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    RowVersion ROWVERSION NOT NULL,

    CONSTRAINT FK_Courses_Tenants
        FOREIGN KEY (TenantId) REFERENCES Tenants(Id),

    CONSTRAINT CK_Courses_Capacity
        CHECK (Capacity > 0),

    CONSTRAINT CK_Courses_RegisteredCount
        CHECK (RegisteredCount >= 0 AND RegisteredCount <= Capacity)
);
GO

CREATE TABLE CourseRegistrations
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    LearnerId UNIQUEIDENTIFIER NOT NULL,
    CourseId UNIQUEIDENTIFIER NOT NULL,
    RegisteredAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    RegisteredBy NVARCHAR(200) NOT NULL,

    CONSTRAINT FK_CourseRegistrations_Learners
        FOREIGN KEY (LearnerId) REFERENCES Learners(Id),

    CONSTRAINT FK_CourseRegistrations_Courses
        FOREIGN KEY (CourseId) REFERENCES Courses(Id)
);
GO

CREATE UNIQUE INDEX UX_CourseRegistrations_TenantId_LearnerId_CourseId
ON CourseRegistrations(TenantId, LearnerId, CourseId);
GO

CREATE INDEX IX_CourseRegistrations_TenantId_CourseId
ON CourseRegistrations(TenantId, CourseId);
GO

CREATE TABLE AuditLogs
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    TenantId UNIQUEIDENTIFIER NOT NULL,
    Action NVARCHAR(100) NOT NULL,
    EntityName NVARCHAR(100) NOT NULL,
    EntityId NVARCHAR(100) NOT NULL,
    PerformedBy NVARCHAR(200) NOT NULL,
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Details NVARCHAR(1000) NULL
);
GO


INSERT INTO Tenants
(
    Id,
    Name,
    IsActive,
    CreatedAtUtc
)
VALUES
(
    '11111111-1111-1111-1111-111111111111',
    'Demo Tenant',
    1,
    SYSUTCDATETIME()
);

INSERT INTO Learners
(
    Id,
    TenantId,
    FullName,
    Email,
    IsActive,
    CreatedAtUtc
)
VALUES
(
    '22222222-2222-2222-2222-222222222222',
    '11111111-1111-1111-1111-111111111111',
    'Umair Arshad',
    'umair@example.com',
    1,
    SYSUTCDATETIME()
);

INSERT INTO Courses
(
    Id,
    TenantId,
    Title,
    Capacity,
    RegisteredCount,
    IsActive,
    CreatedAtUtc
)
VALUES
(
    '33333333-3333-3333-3333-333333333333',
    '11111111-1111-1111-1111-111111111111',
    '.NET Clean Architecture',
    5,
    0,
    1,
    SYSUTCDATETIME()
);

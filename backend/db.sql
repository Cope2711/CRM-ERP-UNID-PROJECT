-- Eliminar la base de datos si existe 
DROP DATABASE IF EXISTS ERPCRMUNID;

-- Crear la base de datos
CREATE DATABASE ERPCRMUNID;
USE ERPCRMUNID;

-- Eliminar tablas si existen
DROP TABLE IF EXISTS RolePermissions;
DROP TABLE IF EXISTS Permissions;
DROP TABLE IF EXISTS RefreshTokens;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Roles;
DROP TABLE IF EXISTS TestTable;


CREATE TABLE TestTable
(
    TestTableID   UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    TestTableCamp VARCHAR(50) NOT NULL
);


CREATE TABLE Permissions
(
    PermissionId   UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    PermissionName VARCHAR(100) NOT NULL,
    Description    VARCHAR(255) NULL
);


CREATE TABLE Roles
(
    RoleId   UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    RoleName VARCHAR(50) NOT NULL
);

CREATE TABLE RolePermissions
(
    RolePermissionId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    RoleId           UNIQUEIDENTIFIER NOT NULL,
    PermissionId     UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles (RoleId),
    FOREIGN KEY (PermissionId) REFERENCES Permissions (PermissionId)
);

CREATE TABLE Users
(
    UserId        UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    UserUserName  VARCHAR(50) UNIQUE  NOT NULL,
    UserFirstName VARCHAR(50)         NOT NULL,
    UserLastName  VARCHAR(50)         NOT NULL,
    UserEmail     VARCHAR(255) UNIQUE NOT NULL,
    UserPassword  VARCHAR(255)        NOT NULL,
    IsActive      BIT              DEFAULT 1,
    CreatedDate   DATETIME         DEFAULT GETDATE(),
    UpdatedDate   DATETIME         DEFAULT GETDATE(),
    RoleId        UNIQUEIDENTIFIER    NOT NULL,
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles (RoleId) ON DELETE CASCADE
);


CREATE TABLE RefreshTokens
(
    RefreshTokenId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    UserId         UNIQUEIDENTIFIER     NOT NULL,
    Token          NVARCHAR(200) UNIQUE NOT NULL,
    ExpiresAt      DATETIME             NOT NULL,
    RevokedAt      DATETIME             NULL,
    CONSTRAINT FK_RefreshTokens_Users FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE CASCADE
);


INSERT INTO Roles (RoleName)
VALUES ('Admin'),
       ('User'),
       ('Guest');


INSERT INTO Users (UserUserName, UserFirstName, UserLastName, UserEmail, UserPassword, IsActive, RoleId)
VALUES ('admin', 'Admin', 'User', 'admin@admin.com',
        '$2b$12$H4hFo5E9XkP5vwsWfvBi8ea.uh1Vz/5RrG0k3Wu3CC5Y1DuhLK3We', 1,
        (SELECT RoleId FROM Roles WHERE RoleName = 'Admin'));


INSERT INTO Permissions (PermissionName, Description)
VALUES ('Manage_Users', 'Ability to manage users'),
       ('View_Reports', 'Access to view reports'),
       ('Edit_Content', 'Permission to edit content');

INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT r.RoleId, p.PermissionId
FROM Roles r
         JOIN Permissions p ON p.PermissionName IN ('Manage_Users', 'View_Reports', 'Edit_Content')
WHERE r.RoleName = 'Admin';


INSERT INTO RefreshTokens (UserId, Token, ExpiresAt)
VALUES ((SELECT UserId FROM Users WHERE UserUserName = 'admin'),
        'sample_refresh_token_123',
        DATEADD(DAY, 30, GETDATE()));

-- Consultas para verificar la inserción de datos
SELECT *
FROM Users;
SELECT *
FROM Roles;
SELECT *
FROM Permissions;
SELECT *
FROM RolePermissions;
SELECT *
FROM RefreshTokens;

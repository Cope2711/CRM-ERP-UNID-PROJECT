-- Eliminar la base de datos si existe 
DROP DATABASE IF EXISTS ERPCRMUNID;

-- Crear la base de datos
CREATE DATABASE ERPCRMUNID;
USE ERPCRMUNID;

-- Eliminar tablas si existen
DROP TABLE IF EXISTS RolesPermissions;
DROP TABLE IF EXISTS Permissions;
DROP TABLE IF EXISTS RefreshTokens;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Roles;
DROP TABLE IF EXISTS TestTable;
DROP TABLE IF EXISTS Resources;
DROP TABLE IF EXISTS PermissionsResources;
DROP TABLE IF EXISTS UsersRoles;


CREATE TABLE TestTable
(
    TestTableID   UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    TestTableCamp VARCHAR(50) NOT NULL
);


CREATE TABLE Resources
(
    ResourceId   UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    ResourceName VARCHAR(50) NOT NULL UNIQUE,
);

CREATE TABLE Permissions
(
    PermissionId          UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    PermissionName        VARCHAR(100) NOT NULL,
    PermissionDescription VARCHAR(255) NULL
);


CREATE TABLE Roles
(
    RoleId      UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    RoleName    VARCHAR(50)  NOT NULL,
    RoleDescription VARCHAR(255) NULL
);

CREATE TABLE RolesPermissions
(
    RolePermissionId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    RoleId           UNIQUEIDENTIFIER NOT NULL,
    PermissionId     UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles (RoleId) ON DELETE CASCADE,
    FOREIGN KEY (PermissionId) REFERENCES Permissions (PermissionId) ON DELETE CASCADE
);

CREATE TABLE PermissionsResources
(
    PermissionResourceId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    PermissionId         UNIQUEIDENTIFIER NOT NULL,
    ResourceId           UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (PermissionId) REFERENCES Permissions (PermissionId) ON DELETE CASCADE,
    FOREIGN KEY (ResourceId) REFERENCES Resources (ResourceId) ON DELETE CASCADE
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
);

CREATE TABLE UsersRoles
(
    UserRoleId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    UserId     UNIQUEIDENTIFIER NOT NULL,
    RoleId     UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES Roles (RoleId) ON DELETE CASCADE
);

CREATE TABLE RefreshTokens
(
    RefreshTokenId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    UserId         UNIQUEIDENTIFIER     NOT NULL,
    Token          NVARCHAR(200) UNIQUE NOT NULL,
    ExpiresAt      DATETIME             NOT NULL,
    RevokedAt      DATETIME             NULL,
    FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE CASCADE
);


INSERT INTO Roles (RoleName)
VALUES ('Admin'),
       ('User'),
       ('Guest');


INSERT INTO Users (UserUserName, UserFirstName, UserLastName, UserEmail, UserPassword, IsActive)
VALUES ('admin', 'Admin', 'User', 'admin@admin.com',
        '$2b$12$H4hFo5E9XkP5vwsWfvBi8ea.uh1Vz/5RrG0k3Wu3CC5Y1DuhLK3We', 1);
INSERT INTO Users (UserUserName, UserFirstName, UserLastName, UserEmail, UserPassword, IsActive)
VALUES ('test-user', 'Test', 'User', 'test-user@test.com',
        '$2b$12$H4hFo5E9XkP5vwsWfvBi8ea.uh1Vz/5RrG0k3Wu3CC5Y1DuhLK3We', 1);

INSERT INTO UsersRoles (UserId, RoleId)
VALUES ((SELECT UserId FROM Users WHERE UserEmail = 'admin@admin.com'),
        (SELECT RoleId FROM Roles WHERE RoleName = 'Admin'));

INSERT INTO Permissions (PermissionName, PermissionDescription)
VALUES ('Manage_Users', 'Ability to manage users'),
       ('View_Reports', 'Access to view reports'),
       ('Edit_Content', 'Permission to edit content');

INSERT INTO RolesPermissions (RoleId, PermissionId)
SELECT r.RoleId, p.PermissionId
FROM Roles r
         JOIN Permissions p ON p.PermissionName IN ('Manage_Users', 'View_Reports', 'Edit_Content')
WHERE r.RoleName = 'Admin';

INSERT INTO Resources (ResourceName)
VALUES ('Users'),
       ('Roles'),
       ('Permissions');


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
FROM RolesPermissions;
SELECT *
FROM RefreshTokens;

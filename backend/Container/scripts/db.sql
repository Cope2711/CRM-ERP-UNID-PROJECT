-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ERPCRMUNID')
    BEGIN
        CREATE DATABASE ERPCRMUNID;
    END
GO

USE ERPCRMUNID;
GO

-- Crear un usuario de SQL Server
IF NOT EXISTS (SELECT name FROM sys.syslogins WHERE name = 'erp_user')
    BEGIN
        CREATE LOGIN erp_user WITH PASSWORD = 'YourStrongPassword123!';
    END
GO

-- Crear un usuario dentro de la base de datos y asignar permisos
USE ERPCRMUNID;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'erp_user')
    BEGIN
        CREATE USER erp_user FOR LOGIN erp_user;
        ALTER ROLE db_owner ADD MEMBER erp_user;
    END
GO

-- Eliminar tablas si existen
DROP TABLE IF EXISTS TestTable;
DROP TABLE IF EXISTS RolesPermissionsResources;
DROP TABLE IF EXISTS UsersRoles;
DROP TABLE IF EXISTS RefreshTokens;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Resources;
DROP TABLE IF EXISTS Roles;
DROP TABLE IF EXISTS Permissions;

CREATE TABLE TestTable
(
    TestTableID   UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    TestTableCamp VARCHAR(50) NOT NULL
);


CREATE TABLE Resources
(
    ResourceId          UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    ResourceName        VARCHAR(50) NOT NULL UNIQUE,
    ResourceDescription VARCHAR(255)
);

CREATE TABLE Permissions
(
    PermissionId          UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    PermissionName        VARCHAR(100) NOT NULL,
    PermissionDescription VARCHAR(255) NULL
);


CREATE TABLE Roles
(
    RoleId          UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    RoleName        VARCHAR(50)  NOT NULL,
    RoleDescription VARCHAR(255) NULL
);

CREATE TABLE RolesPermissionsResources
(
    RolePermissionId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    RoleId           UNIQUEIDENTIFIER NOT NULL,
    PermissionId     UNIQUEIDENTIFIER NOT NULL,
    ResourceId       UNIQUEIDENTIFIER,
    FOREIGN KEY (RoleId) REFERENCES Roles (RoleId) ON DELETE CASCADE,
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
    Token          NVARCHAR(255) UNIQUE NOT NULL,
    DeviceId       NVARCHAR(255)        NOT NULL,
    ExpiresAt      DATETIME             NOT NULL,
    RevokedAt      DATETIME             NULL,
    FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE CASCADE
);

-- Insertar Roles
DECLARE @RoleId_Admin UNIQUEIDENTIFIER = 'aad0f879-79bf-42b5-b829-3e14b9ef0e4b';
DECLARE @RoleId_User UNIQUEIDENTIFIER = '523a8c97-735e-41f7-b4b2-16f92791adf5';
DECLARE @RoleId_Guest UNIQUEIDENTIFIER = 'd9b540dd-7e8e-4aa8-a97c-3cdf3a4b08d4';

INSERT INTO Roles (RoleId, RoleName, RoleDescription)
VALUES (@RoleId_Admin, 'Admin', 'Admin role'),
       (@RoleId_User, 'User', 'User role'),
       (@RoleId_Guest, 'Guest', 'Guest role');

-- Insertar Usuarios
DECLARE @UserId_Admin UNIQUEIDENTIFIER = '172422a0-5164-4470-acae-72022d3820b1';
DECLARE @UserId_Test UNIQUEIDENTIFIER = '2c0180d4-040c-4c00-b8f9-31f7a1e72259';

INSERT INTO Users (UserId, UserUserName, UserFirstName, UserLastName, UserEmail, UserPassword, IsActive)
VALUES (@UserId_Admin, 'admin', 'Admin', 'User', 'admin@admin.com',
        '$2a$10$H/STMY/cHyRA4LHxLJMUWuajKp4Fw5TiKF.UdGo5hzKqQWTMshKlW', 1),
       (@UserId_Test, 'test-user', 'Test', 'User', 'test-user@test.com',
        '$2b$12$H4hFo5E9XkP5vwsWfvBi8ea.uh1Vz/5RrG0k3Wu3CC5Y1DuhLK3We', 1);

-- Insertar UsersRoles
DECLARE @UserRoleId_Admin UNIQUEIDENTIFIER = '842193b4-5048-4cd9-be60-b7ca34319286';
DECLARE @UserRoleId_Test UNIQUEIDENTIFIER = 'fe904dcf-eeb1-4a71-a229-71185cc15453';

INSERT INTO UsersRoles (UserRoleId, UserId, RoleId)
VALUES (@UserRoleId_Admin, @UserId_Admin, @RoleId_Admin),
       (@UserRoleId_Test, @UserId_Test, @RoleId_User);

-- Insertar Permisos
DECLARE @PermissionId_View UNIQUEIDENTIFIER = '7521ffd2-80e6-4970-8ab3-0d454a377d22';
DECLARE @PermissionId_ViewReports UNIQUEIDENTIFIER = 'a5088356-4272-4939-b18b-971811fd29e8';
DECLARE @PermissionId_EditContent UNIQUEIDENTIFIER = '2a831d9d-1245-451e-8b02-de6542f74574';
DECLARE @PermissionId_Create UNIQUEIDENTIFIER = '99f766ee-3fd5-4e33-9771-d3821322acea';
DECLARE @PermissionId_AssignRole UNIQUEIDENTIFIER = '5c748c35-a4f5-48d6-a320-32287c8649a9';
DECLARE @PermissionId_RevokeRole UNIQUEIDENTIFIER = '47a2f03a-5f0b-4d73-b535-200a643e7849';
DECLARE @PermissionId_AssignPermission UNIQUEIDENTIFIER = '554b4b5a-cae7-414c-91f8-75df725b526d';
DECLARE @PermissionId_RevokePermission UNIQUEIDENTIFIER = '9037e10c-38ea-40a6-b364-d68f86203c11';
DECLARE @PermissionId_Delete UNIQUEIDENTIFIER = '722399bc-76f4-4bfa-950d-85e8b93f7af5';
DECLARE @PermissionId_DeactivateUser UNIQUEIDENTIFIER = '10d321bd-b667-40c9-adb0-50e62d37c4cc';
DECLARE @PermissionId_ActivateUser UNIQUEIDENTIFIER = 'a43b1178-931e-4eed-9742-30af024ec05b';

INSERT INTO Permissions (PermissionId, PermissionName, PermissionDescription)
VALUES (@PermissionId_View, 'View', 'Ability to view resources'),
       (@PermissionId_ViewReports, 'View_Reports', 'Access to view reports'),
       (@PermissionId_EditContent, 'Edit_Content', 'Permission to edit content'),
       (@PermissionId_Create, 'Create', 'Create objects'),
       (@PermissionId_AssignRole, 'Assign_Role_To_User', 'Assign role to user'),
       (@PermissionId_RevokeRole, 'Revoke_Role_To_User', 'Revoke role to user'),
       (@PermissionId_AssignPermission, 'Assign_Permission', 'Assign permission to role'),
       (@PermissionId_RevokePermission, 'Revoke_Permission', 'Revoke permission to role'),
       (@PermissionId_Delete, 'Delete', 'Delete objects'),
       (@PermissionId_DeactivateUser, 'Deactivate_User', 'Deactivate user'),
        (@PermissionId_ActivateUser, 'Activate_User', 'Activate user')

-- Insertar Recursos
DECLARE @ResourceId_Users UNIQUEIDENTIFIER = 'd161ec8c-7c31-4eb4-a331-82ef9e45903e';
DECLARE @ResourceId_Roles UNIQUEIDENTIFIER = '5688d987-b031-4780-af66-1a99f2fa69dd';
DECLARE @ResourceId_Permissions UNIQUEIDENTIFIER = '09e63ef5-71ca-4dcb-8f69-4997b02c1e6d';
DECLARE @ResourceId_UsersRoles UNIQUEIDENTIFIER = '85fac418-d875-4f3c-8094-c2d614a58f15';
DECLARE @ResourceId_RolesPermissionsResources UNIQUEIDENTIFIER = '67f53f8f-1848-4156-8ee9-ec9e02bd5836';
DECLARE @ResourceId_Resources UNIQUEIDENTIFIER = '6193cd07-1a2c-4a7e-95e0-00bb27dbf7c3';

INSERT INTO Resources (ResourceId, ResourceName, ResourceDescription)
VALUES (@ResourceId_Users, 'Users', 'Users module'),
       (@ResourceId_Roles, 'Roles', 'Roles module'),
       (@ResourceId_Permissions, 'Permissions', 'Permissions module'),
       (@ResourceId_UsersRoles, 'UsersRoles', 'Users roles module'),
       (@ResourceId_RolesPermissionsResources, 'RolesPermissionsResources', 'Roles permissions resources module'),
       (@ResourceId_Resources, 'Resources', 'Resources module')

-- Insertar Permisos a los Roles
INSERT INTO RolesPermissionsResources (RolePermissionId, RoleId, PermissionId, ResourceId)
VALUES (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_Users),
       (NEWID(), @RoleId_Admin, @PermissionId_EditContent, @ResourceId_Users),
       (NEWID(), @RoleId_Admin, @PermissionId_DeactivateUser, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_EditContent, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_Create, @ResourceId_Users),
       (NEWID(), @RoleId_Admin, @PermissionId_AssignRole, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_RevokeRole, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_UsersRoles),
       (NEWID(), @RoleId_Admin, @PermissionId_AssignPermission, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_RevokePermission, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_RolesPermissionsResources),
       (NEWID(), @RoleId_Admin, @PermissionId_EditContent, @ResourceId_Roles),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_Roles),
       (NEWID(), @RoleId_Admin, @PermissionId_Create, @ResourceId_Roles),
       (NEWID(), @RoleId_Admin, @PermissionId_Delete, @ResourceId_Roles),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_Resources),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_Permissions),
       (NEWID(), @RoleId_Admin, @PermissionId_ActivateUser, NULL)


-- Consultas para verificar la inserción de datos
SELECT *
FROM Users;
SELECT *
FROM Roles;
SELECT *
FROM Permissions;
SELECT *
FROM RolesPermissionsResources;
SELECT *
FROM RefreshTokens;
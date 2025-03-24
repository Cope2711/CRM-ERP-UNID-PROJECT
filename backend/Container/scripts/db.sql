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

DROP TABLE IF EXISTS UsersBranches;
DROP TABLE IF EXISTS UsersRoles;
DROP TABLE IF EXISTS RolesPermissionsResources;
DROP TABLE IF EXISTS PasswordRecoveryTokens;
DROP TABLE IF EXISTS Inventory;
DROP TABLE IF EXISTS Branches;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Brands;
DROP TABLE IF EXISTS TestTable;
DROP TABLE IF EXISTS Resources;
DROP TABLE IF EXISTS Permissions;
DROP TABLE IF EXISTS Roles;
DROP TABLE IF EXISTS RefreshTokens;
DROP TABLE IF EXISTS Users;

CREATE TABLE Branches
(
    BranchId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    BranchName VARCHAR(100) UNIQUE NOT NULL,
    BranchAddress VARCHAR(255) NOT NULL,
    BranchPhone VARCHAR(20) NULL,
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Brands
(
    BrandId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    BrandName VARCHAR(50) NOT NULL UNIQUE,
    BrandDescription VARCHAR(255) NULL,
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Products
(
    ProductId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    ProductName VARCHAR(50) NOT NULL,
    ProductPrice DECIMAL(10,2) NOT NULL,
    ProductDescription VARCHAR(255) NULL,
    IsActive BIT DEFAULT 1,
    BrandId UNIQUEIDENTIFIER NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Products_Brands FOREIGN KEY (BrandId)
        REFERENCES Brands (BrandId) ON DELETE CASCADE
);

CREATE TABLE Inventory
(
    InventoryId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    ProductId UNIQUEIDENTIFIER NOT NULL,
    BranchId UNIQUEIDENTIFIER NOT NULL,
    Quantity INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Inventory_Products FOREIGN KEY (ProductId)
        REFERENCES Products (ProductId) ON DELETE CASCADE,
    CONSTRAINT FK_Inventory_Branches FOREIGN KEY (BranchId)
        REFERENCES Branches (BranchId) ON DELETE CASCADE 
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

CREATE TABLE UsersBranches
(
    UserBranchId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    BranchId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_UsersBranches_Users FOREIGN KEY (UserId)
        REFERENCES Users (UserId) ON DELETE CASCADE,
    CONSTRAINT FK_UsersBranches_Branches FOREIGN KEY (BranchId)
        REFERENCES Branches (BranchId) ON DELETE CASCADE,
    CONSTRAINT UQ_UsersBranches UNIQUE (UserId, BranchId)
);

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
    RolePriority    FLOAT      NOT NULL,
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

CREATE TABLE PasswordRecoveryTokens
(
    ResetId UNIQUEIDENTIFIER PRIMARY KEY NOT NULL, -- Clave primaria
    UserId UNIQUEIDENTIFIER NOT  NULL, -- Clave foránea 
    ResetToken NVARCHAR(MAX) NOT NULL, -- Token de restablecimiento
    ExpiresAt DATETIME2(0) NOT NULL, -- Fecha de expiración del token
    CreatedAt DATETIME2(0) NOT NULL 

    -- relación con la tabla de usuarios
    CONSTRAINT FK_PasswordResets_Users FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
        ON DELETE CASCADE
);

-- Insertar Roles
DECLARE @RoleId_Admin UNIQUEIDENTIFIER = 'aad0f879-79bf-42b5-b829-3e14b9ef0e4b';
DECLARE @RoleId_User UNIQUEIDENTIFIER = '523a8c97-735e-41f7-b4b2-16f92791adf5';
DECLARE @RoleId_Guest UNIQUEIDENTIFIER = 'd9b540dd-7e8e-4aa8-a97c-3cdf3a4b08d4';

INSERT INTO Roles (RoleId, RoleName, RolePriority, RoleDescription)
VALUES (@RoleId_Admin, 'Admin', 10, 'Admin role'),
       (@RoleId_User, 'User', 5, 'User role'),
       (@RoleId_Guest, 'Guest', 4.5, 'Guest role');

-- Insertar Usuarios
DECLARE @UserId_Admin UNIQUEIDENTIFIER = '172422a0-5164-4470-acae-72022d3820b1';
DECLARE @UserId_Test UNIQUEIDENTIFIER = '2c0180d4-040c-4c00-b8f9-31f7a1e72259';
DECLARE @UserId_TestDeactivated UNIQUEIDENTIFIER = '6a820228-e221-4e80-9b0a-b8eda7042f30';

INSERT INTO Users (UserId, UserUserName, UserFirstName, UserLastName, UserEmail, UserPassword, IsActive)
VALUES (@UserId_Admin, 'admin', 'Admin', 'User', 'admin@admin.com',
        '$2a$10$H/STMY/cHyRA4LHxLJMUWuajKp4Fw5TiKF.UdGo5hzKqQWTMshKlW', 1),
       (@UserId_Test, 'test-user', 'Test', 'User', 'test-user@test.com',
        '$2a$10$H/STMY/cHyRA4LHxLJMUWuajKp4Fw5TiKF.UdGo5hzKqQWTMshKlW', 1),
       (@UserId_TestDeactivated, 'test-user-deactivated', 'TestDeactivated', 'User', 'test-user-deactivated@test.com',
        '$2a$10$H/STMY/cHyRA4LHxLJMUWuajKp4Fw5TiKF.UdGo5hzKqQWTMshKlW', 0)

-- Insertar ejemplos de sucursales
DECLARE @BranchId_HermosilloMiguelHidalgo UNIQUEIDENTIFIER = '4bf33a98-874d-4673-98bb-b958ddc68c94';
DECLARE @BranchId_CampoReal UNIQUEIDENTIFIER = 'b0821f0a-20ab-4f64-8c00-5b95d331a836';
DECLARE @BranchId_PuertoRico UNIQUEIDENTIFIER = 'b3a28df0-fd7d-405e-9820-3d0f137a9ff9';

INSERT INTO Branches (BranchId, BranchName, BranchAddress, BranchPhone, IsActive)
VALUES
    (@BranchId_HermosilloMiguelHidalgo, 'Hermosillo Miguel Hidalgo', 'Calle 123 Nº 1, Hermosillo, Sonora, Mexico', NULL, 1),
    (@BranchId_CampoReal, 'Campo Real', 'Calle 123 Nº 1, Hermosillo, Sonora, Mexico', NULL, 1),
    (@BranchId_PuertoRico, 'Puerto Rico', 'Calle 123 Nº 1, Hermosillo, Sonora, Mexico', NULL, 1);


-- Insertar UsersRoles
DECLARE @UserRoleId_Admin UNIQUEIDENTIFIER = '842193b4-5048-4cd9-be60-b7ca34319286';
DECLARE @UserRoleId_Test UNIQUEIDENTIFIER = 'fe904dcf-eeb1-4a71-a229-71185cc15453';
DECLARE @UserRoleId_TestDeactivated UNIQUEIDENTIFIER = '327a4fd3-03bc-430f-a0ca-a2262624abf7';

INSERT INTO UsersRoles (UserRoleId, UserId, RoleId)
VALUES (@UserRoleId_Admin, @UserId_Admin, @RoleId_Admin),
       (@UserRoleId_Test, @UserId_Test, @RoleId_User),
       (@UserRoleId_TestDeactivated, @UserId_TestDeactivated, @RoleId_User)

-- Insertar UsersBranches
DECLARE @UserBranchId_AdminHermosillo UNIQUEIDENTIFIER = 'bd4e931d-09bd-42c2-9249-ccf22533136d';
DECLARE @UserBranchId_AdminCampoReal UNIQUEIDENTIFIER = 'd6759449-fe42-4fff-ab94-38c575fcaa8c';
DECLARE @UserBranchId_TestHermosillo UNIQUEIDENTIFIER = '7f26e050-0e3a-4b3e-90bd-8db17ee012cf';
DECLARE @UserBranch_Id_TestDeactivatedPuertoRico UNIQUEIDENTIFIER = 'a15c7696-0400-40e8-a789-16949f72c6a9';

INSERT INTO UsersBranches (UserBranchId, UserId, BranchId)
VALUES
    (@UserBranchId_AdminHermosillo, @UserId_Admin, @BranchId_HermosilloMiguelHidalgo),
    (@UserBranchId_AdminCampoReal, @UserId_Admin, @BranchId_CampoReal),
    (@UserBranchId_TestHermosillo, @UserId_Test, @BranchId_HermosilloMiguelHidalgo),
    (@UserBranch_Id_TestDeactivatedPuertoRico, @UserId_TestDeactivated, @BranchId_PuertoRico)

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
DECLARE @PermissionId_AssignBranch UNIQUEIDENTIFIER = 'dfbd5729-c8a9-4474-acab-766893ae82f9';
DECLARE @PermissionId_RevokeBranch UNIQUEIDENTIFIER = '083daf3a-fb59-4714-83f0-fc2bcd0f1374';

INSERT INTO Permissions (PermissionId, PermissionName, PermissionDescription)
VALUES (@PermissionId_View, 'View', 'Ability to view resources'),
       (@PermissionId_ViewReports, 'View_Reports', 'Access to view reports'),
       (@PermissionId_EditContent, 'Edit_Content', 'Permission to edit content'),
       (@PermissionId_Create, 'Create', 'Create objects'),
       (@PermissionId_AssignRole, 'Assign_Role', 'Assign role to user'),
       (@PermissionId_RevokeRole, 'Revoke_Role', 'Revoke role to user'),
       (@PermissionId_AssignPermission, 'Assign_Permission', 'Assign permission to role'),
       (@PermissionId_RevokePermission, 'Revoke_Permission', 'Revoke permission to role'),
       (@PermissionId_Delete, 'Delete', 'Delete objects'),
       (@PermissionId_DeactivateUser, 'Deactivate_User', 'Deactivate user'),
        (@PermissionId_ActivateUser, 'Activate_User', 'Activate user'),
        (@PermissionId_AssignBranch, 'Assign_Branch', 'Assign branch to user'),
        (@PermissionId_RevokeBranch, 'Revoke_Branch', 'Revoke branch to user')

-- Insertar Recursos
DECLARE @ResourceId_Users UNIQUEIDENTIFIER = 'd161ec8c-7c31-4eb4-a331-82ef9e45903e';
DECLARE @ResourceId_Roles UNIQUEIDENTIFIER = '5688d987-b031-4780-af66-1a99f2fa69dd';
DECLARE @ResourceId_Permissions UNIQUEIDENTIFIER = '09e63ef5-71ca-4dcb-8f69-4997b02c1e6d';
DECLARE @ResourceId_UsersRoles UNIQUEIDENTIFIER = '85fac418-d875-4f3c-8094-c2d614a58f15';
DECLARE @ResourceId_RolesPermissionsResources UNIQUEIDENTIFIER = '67f53f8f-1848-4156-8ee9-ec9e02bd5836';
DECLARE @ResourceId_Resources UNIQUEIDENTIFIER = '6193cd07-1a2c-4a7e-95e0-00bb27dbf7c3';
DECLARE @ResourceId_Products UNIQUEIDENTIFIER = '6e62f20f-39ca-4a21-a52d-126c59ccb338';
DECLARE @ResourceId_Brands UNIQUEIDENTIFIER = '708eb498-6ad5-447c-ba76-13cba1f08dc7';
DECLARE @ResourceId_Inventory UNIQUEIDENTIFIER = 'b0f8c2e0-f5a1-4a3e-b5e5-c4e8f0f9c7b7';
DECLARE @ResourceId_Branches UNIQUEIDENTIFIER = '55dc724f-a1aa-4d73-a7ed-5bef93b72be9';
DECLARE @ResourceId_UsersBranches UNIQUEIDENTIFIER = 'ef53fcb2-2e6f-4104-9b1d-7c5164851b3e';

INSERT INTO Resources (ResourceId, ResourceName, ResourceDescription)
VALUES (@ResourceId_Users, 'Users', 'Users module'),
       (@ResourceId_Roles, 'Roles', 'Roles module'),
       (@ResourceId_Permissions, 'Permissions', 'Permissions module'),
       (@ResourceId_UsersRoles, 'UsersRoles', 'Users roles module'),
       (@ResourceId_RolesPermissionsResources, 'RolesPermissionsResources', 'Roles permissions resources module'),
       (@ResourceId_Resources, 'Resources', 'Resources module'),
       (@ResourceId_Products, 'Products', 'Products module'),
       (@ResourceId_Brands, 'Brands', 'Brands module'),
       (@ResourceId_Inventory, 'Inventory', 'Inventory module'),
       (@ResourceId_Branches, 'Branches', 'Branches module'),
       (@ResourceId_UsersBranches, 'UsersBranches', 'Users branches module')

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
       (NEWID(), @RoleId_Admin, @PermissionId_ActivateUser, NULL),
       (NEWID(), @RoleId_User, @PermissionId_EditContent, @ResourceId_Users),
       (NEWID(), @ROleId_User, @PermissionId_RevokeRole, NULL),
       (NEWID(), @ROleId_User, @PermissionId_RevokePermission, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_Products),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_Brands),
       (NEWID(), @RoleId_Admin, @PermissionId_Create, @ResourceId_Brands),
       (NEWID(), @RoleId_Admin, @PermissionId_EditContent, @ResourceId_Brands),
       (NEWID(), @RoleId_Admin, @PermissionId_Create, @ResourceId_Products),
       (NEWID(), @RoleId_Admin, @PermissionId_EditContent, @ResourceId_Products),
       (NEWID(), @RoleId_Admin, @PermissionId_Create, @ResourceId_Inventory),
       (NEWID(), @RoleId_Admin, @PermissionId_EditContent, @ResourceId_Inventory),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_Inventory),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_Branches),
       (NEWID(), @RoleId_Admin, @PermissionId_Create, @ResourceId_Branches),
       (NEWID(), @RoleId_Admin, @PermissionId_EditContent, @ResourceId_Branches),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_UsersBranches),
       (NEWID(), @RoleId_Admin, @PermissionId_AssignBranch, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_RevokeBranch, NULL)

-- Insertar ejemplos de marcas
DECLARE @BrandId_Apple UNIQUEIDENTIFIER = 'c3146b6f-b50f-4b26-8e77-827fc538b7d1';
DECLARE @BrandId_Samsung UNIQUEIDENTIFIER = '809fb57d-ff80-496f-88c3-7b50f0d9b55d';
DECLARE @BrandId_Nike UNIQUEIDENTIFIER = '5b23b9a8-bd17-4b2a-8e61-b9863a8f77b5';

INSERT INTO Brands (BrandId, BrandName, BrandDescription, IsActive)
VALUES
    (@BrandId_Apple, 'Apple', 'Apple Inc. - Premium electronics', 1),
    (@BrandId_Samsung, 'Samsung', 'Samsung Electronics - Leading technology company', 1),
    (@BrandId_Nike, 'Nike', 'Nike Inc. - Sportswear and equipment', 1);

-- Insertar ejemplos de productos
DECLARE @ProductId_iPhone13 UNIQUEIDENTIFIER = '5b22cc12-191b-4fe9-9878-bbc1575fa8a7';
DECLARE @ProductId_MacBookPro UNIQUEIDENTIFIER = '420a7d01-dd70-417f-872f-aa9f1e1df436';
DECLARE @ProductId_iPadPro UNIQUEIDENTIFIER = 'f96bf5e4-9579-404e-9350-087b1ef1305e';
DECLARE @ProductId_GalaxyS21 UNIQUEIDENTIFIER = '3179499a-621c-4c07-8acb-e7a057cf4753';
DECLARE @ProductId_GalaxyTabS7 UNIQUEIDENTIFIER = 'b342289c-c165-4b85-ab6d-2b030f68b171';
DECLARE @ProductId_SamsungQLEDTV UNIQUEIDENTIFIER = 'aa93da89-ba99-4b47-823c-26c5332f6600';
DECLARE @ProductId_NikeAirMax270 UNIQUEIDENTIFIER = 'de30da34-c216-488e-8b3b-3cba0fb71baa';
DECLARE @ProductId_NikeZoomX UNIQUEIDENTIFIER = 'e9ade468-590a-4c0e-bfbe-91ab9dbb6830';
DECLARE @ProductId_NikeDriFitTShirt UNIQUEIDENTIFIER = 'ac99dcd4-b451-416d-bb12-59b706c5db30';

INSERT INTO Products (ProductId, ProductName, ProductPrice, ProductDescription, IsActive, BrandId)  
VALUES
    (@ProductId_iPhone13, 'iPhone 13', 999.99, 'Latest iPhone model', 1, @BrandId_Apple),
    (@ProductId_MacBookPro, 'MacBook Pro', 1999.99, 'High-performance laptop', 1, @BrandId_Apple),
    (@ProductId_iPadPro, 'iPad Pro', 799.99, 'Powerful tablet for work and entertainment', 1, @BrandId_Apple),
    (@ProductId_GalaxyS21, 'Galaxy S21', 899.99, 'Samsung flagship phone', 1, @BrandId_Samsung),
    (@ProductId_GalaxyTabS7, 'Galaxy Tab S7', 649.99, 'High-end Android tablet', 1, @BrandId_Samsung),
    (@ProductId_SamsungQLEDTV, 'Samsung QLED TV', 1500.00, 'Smart TV with stunning display', 1, @BrandId_Samsung),
    (@ProductId_NikeAirMax270, 'Nike Air Max 270', 120.00, 'Comfortable running shoes', 1, @BrandId_Nike),
    (@ProductId_NikeZoomX, 'Nike ZoomX Vaporfly Next%', 250.00, 'High-performance running shoes', 1, @BrandId_Nike),
    (@ProductId_NikeDriFitTShirt, 'Nike Dri-FIT T-shirt', 30.00, 'Breathable athletic shirt', 1, @BrandId_Nike);

-- Insertar ejemplos de inventario
DECLARE @InventoryId_iPhone13Hermosillo UNIQUEIDENTIFIER = '3674ad48-1d4c-4492-b21e-a4263237f26f';
DECLARE @InventoryId_MacBookProHermosillo UNIQUEIDENTIFIER = '5034a408-399e-4d0b-ade4-ff6157a2381a';
DECLARE @InventoryId_iPadProHermosillo UNIQUEIDENTIFIER = 'b6ca588f-21c1-46b5-980d-79c10c074fb6';
DECLARE @InventoryId_GalaxyS21Hermosillo UNIQUEIDENTIFIER = 'f0e79d04-a71c-4f98-b789-bb957a6d8bba';
DECLARE @InventoryId_iPadProCampoReal UNIQUEIDENTIFIER = 'da8aabc1-fa09-43c0-8e27-17f1d839b653';
DECLARE @InventoryId_GalaxyS21CampoReal UNIQUEIDENTIFIER = '9ca54354-1744-4a2d-b4d8-3d1baddd74d7';
DECLARE @InventoryId_GalaxyTabS7CampoReal UNIQUEIDENTIFIER = '702949c9-bdd5-4720-96e4-f8593f9b7bc7';
DECLARE @InventoryId_SamsungQLEDTVCampoReal UNIQUEIDENTIFIER = 'a5f0e332-e494-438c-8507-13e2e6f987d9';
DECLARE @InventoryId_NikeAirMax270CampoReal UNIQUEIDENTIFIER = 'c90f6718-aace-4aa8-8d17-546c287980c2';
DECLARE @InventoryId_NikeZoomXCampoReal UNIQUEIDENTIFIER = 'b857ab9e-5a6c-45c5-bfa9-100db2ac3d7f';
DECLARE @InventoryId_NikeDriFitTShirtCampoReal UNIQUEIDENTIFIER = '695fad36-e817-4383-bea4-8ca68ae7b719';

INSERT INTO Inventory (InventoryID, ProductID, BranchId, Quantity, CreatedDate, UpdatedDate)
VALUES
    (@InventoryId_iPhone13Hermosillo, @ProductId_iPhone13, @BranchId_HermosilloMiguelHidalgo, 10, GETUTCDATE(), GETUTCDATE()),
    (@InventoryId_MacBookProHermosillo, @ProductId_MacBookPro, @BranchId_HermosilloMiguelHidalgo, 20, GETUTCDATE(), GETUTCDATE()),
    (@InventoryId_iPadProHermosillo, @ProductId_iPadPro, @BranchId_HermosilloMiguelHidalgo, 30, GETUTCDATE(), GETUTCDATE()),
    (@InventoryId_GalaxyS21Hermosillo, @ProductId_GalaxyS21, @BranchId_HermosilloMiguelHidalgo, 40, GETUTCDATE(), GETUTCDATE()),
    (@InventoryId_iPadProCampoReal, @ProductId_iPadPro, @BranchId_CampoReal, 30, GETUTCDATE(), GETUTCDATE()),
    (@InventoryId_GalaxyS21CampoReal, @ProductId_GalaxyS21, @BranchId_CampoReal, 40, GETUTCDATE(), GETUTCDATE()),
    (@InventoryId_GalaxyTabS7CampoReal, @ProductId_GalaxyTabS7, @BranchId_CampoReal, 50, GETUTCDATE(), GETUTCDATE()),
    (@InventoryId_SamsungQLEDTVCampoReal, @ProductId_SamsungQLEDTV, @BranchId_CampoReal, 60, GETUTCDATE(), GETUTCDATE()),
    (@InventoryId_NikeAirMax270CampoReal, @ProductId_NikeAirMax270, @BranchId_CampoReal, 70, GETUTCDATE(), GETUTCDATE()),
    (@InventoryId_NikeZoomXCampoReal, @ProductId_NikeZoomX, @BranchId_CampoReal, 80, GETUTCDATE(), GETUTCDATE()),
    (@InventoryId_NikeDriFitTShirtCampoReal, @ProductId_NikeDriFitTShirt, @BranchId_CampoReal, 90, GETUTCDATE(), GETUTCDATE());

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
SELECT *
FROM PasswordRecoveryTokens;
Select * 
from Brands;
Select * 
from Products;
Select *
from Branches;
Select *
from UsersBranches;
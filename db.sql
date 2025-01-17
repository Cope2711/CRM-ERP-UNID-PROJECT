﻿DROP
DATABASE IF EXISTS ERPCRMUNID;

CREATE
DATABASE ERPCRMUNID;

USE
ERPCRMUNID;

DROP TABLE IF EXISTS TestTable;
DROP TABLE IF EXISTS Users;

CREATE TABLE TestTable
(
    TestTableID   UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    TestTableCamp VARCHAR(50) NOT NULL
);

CREATE TABLE Users
(
    UserId        UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    UserUserName  VARCHAR(50) UNIQUE NOT NULL,
    UserFirstName VARCHAR(50)        NOT NULL,
    UserLastName  VARCHAR(50)        NOT NULL,
    UserEmail     VARCHAR(255) UNIQUE NOT NULL,
    UserPassword  VARCHAR(255)       NOT NULL,
    IsActive      BIT              DEFAULT 1,
    CreatedDate   DATETIME         DEFAULT GETDATE(),
    UpdatedDate   DATETIME         DEFAULT GETDATE()
);

INSERT INTO Users (UserUserName, UserFirstName, UserLastName, UserEmail, UserPassword, IsActive)
VALUES ('admin', 'Admin', 'User', 'admin@admin.com', '123456', 1);

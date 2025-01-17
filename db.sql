﻿DROP DATABASE IF EXISTS ERPCRMUNID;

CREATE DATABASE ERPCRMUNID;

USE ERPCRMUNID;

DROP TABLE IF EXISTS TestTable;

CREATE TABLE TestTable (
    TestTableID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    TestTableCamp VARCHAR(50) NOT NULL
);
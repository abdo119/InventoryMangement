CREATE DATABASE InventoryDB;
GO

USE InventoryDB;
GO
-- Create Table Users
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) CHECK (Role IN ('Admin', 'Viewer')) NOT NULL
);
-- Create Table Products
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    QuantityInStock INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    SupplierName NVARCHAR(100) NOT NULL
);

-- Create Table AuditLogs

   CREATE TABLE AuditLogs (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    ActionType NVARCHAR(50) NOT NULL,
    ProductId INT NOT NULL,
    Username NVARCHAR(100) NOT NULL,
    Timestamp DATETIME DEFAULT GETDATE(),
    OldValues NVARCHAR(MAX) CHECK (ISJSON(OldValues) = 1), -- JSON data
    NewValues NVARCHAR(MAX) CHECK (ISJSON(NewValues) = 1), -- JSON data
    
);
-- Insert Admin 
INSERT INTO Users (Username, PasswordHash, Role)
VALUES ('admin', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Admin'); --  username: admin Password:123
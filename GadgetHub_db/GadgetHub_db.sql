CREATE DATABASE GadgetHub_db;

USE GadgetHub_db;

CREATE TABLE Admins (
    AdminID INT PRIMARY KEY,    
    AdminName NVARCHAR(50) NOT NULL, 
    AdminEmail NVARCHAR(100) UNIQUE NOT NULL, 
    Password NVARCHAR(255) NOT NULL, 
);

CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    ProductBrand NVARCHAR(100) NOT NULL, 
    ProductDetail NVARCHAR(MAX) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Confirmed'))
); 

CREATE TABLE Distributors ( 
    DistributorID INT PRIMARY KEY, 
    DistributorName NVARCHAR(100) NOT NULL, 
    DistributorEmail NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL, 
    DistributorPhone NVARCHAR(15) NOT NULL, 
    Location NVARCHAR(50) NOT NULL,
);

CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY, 
    CustomerName NVARCHAR(100) NOT NULL,
    CustomerEmail NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL, 
    Address NVARCHAR(200) NOT NULL,  
    CustomerPhone NVARCHAR(15) 
);

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY, 
    CustomerName NVARCHAR(100) NOT NULL, 
    ProductName NVARCHAR(100) NOT NULL, 
    Quantity INT NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Confirmed')) 
);

CREATE TABLE Quotations (
    QuotationID INT PRIMARY KEY, 
    productName NVARCHAR(100) NOT NULL,  
    Price DECIMAL(10,2) NOT NULL, 
    Quantity INT NOT NULL, 
);

CREATE TABLE OrderRequests (
    OrderRequestID INT PRIMARY KEY, 
    OrderID INT NOT NULL, 
    DistributorName NVARCHAR(100) NOT NULL,
    OrderPrice DECIMAL(10,2) NOT NULL, 
    OrderDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Confirmed', 'Cancelled')) 
);

CREATE TABLE QuotationRequests (
    QuotationRequestID INT PRIMARY KEY,  
    DistributorName NVARCHAR(100) NOT NULL,
    QuotationPrice DECIMAL(10,2) NOT NULL, 
    QuotationDate DATETIME DEFAULT GETDATE(), 
    Status NVARCHAR(50) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Confirmed', 'Cancelled')) -- Quotation status
);

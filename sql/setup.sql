-- =============================================================
-- Products API - Database Setup Script
-- Run this script against SQL Server to create the database,
-- table, and all stored procedures.
-- =============================================================

-- Create database (skip if it already exists)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ProductsDb')
BEGIN
    CREATE DATABASE ProductsDb;
END
GO

USE ProductsDb;
GO

-- =============================================================
-- TABLE: Products
-- =============================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
BEGIN
    CREATE TABLE Products (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Name        NVARCHAR(200)   NOT NULL,
        Description NVARCHAR(500)   NULL,
        Price       DECIMAL(18,2)   NOT NULL,
        CreatedDate DATETIME        NOT NULL DEFAULT GETDATE()
    );
END
GO

-- =============================================================
-- SP: sp_GetAllProducts
-- =============================================================
IF OBJECT_ID('sp_GetAllProducts', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetAllProducts;
GO

CREATE PROCEDURE sp_GetAllProducts
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, Description, Price, CreatedDate
    FROM   Products
    ORDER  BY CreatedDate DESC;
END
GO

-- =============================================================
-- SP: sp_GetProductById
-- =============================================================
IF OBJECT_ID('sp_GetProductById', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetProductById;
GO

CREATE PROCEDURE sp_GetProductById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, Description, Price, CreatedDate
    FROM   Products
    WHERE  Id = @Id;
END
GO

-- =============================================================
-- SP: sp_CreateProduct
-- =============================================================
IF OBJECT_ID('sp_CreateProduct', 'P') IS NOT NULL
    DROP PROCEDURE sp_CreateProduct;
GO

CREATE PROCEDURE sp_CreateProduct
    @Name        NVARCHAR(200),
    @Description NVARCHAR(500) = NULL,
    @Price       DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Products (Name, Description, Price, CreatedDate)
    VALUES (@Name, @Description, @Price, GETDATE());

    SELECT SCOPE_IDENTITY() AS NewId;
END
GO

-- =============================================================
-- SP: sp_UpdateProduct
-- =============================================================
IF OBJECT_ID('sp_UpdateProduct', 'P') IS NOT NULL
    DROP PROCEDURE sp_UpdateProduct;
GO

CREATE PROCEDURE sp_UpdateProduct
    @Id          INT,
    @Name        NVARCHAR(200),
    @Description NVARCHAR(500) = NULL,
    @Price       DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Products
    SET    Name        = @Name,
           Description = @Description,
           Price       = @Price
    WHERE  Id = @Id;
END
GO

-- =============================================================
-- SP: sp_DeleteProduct
-- =============================================================
IF OBJECT_ID('sp_DeleteProduct', 'P') IS NOT NULL
    DROP PROCEDURE sp_DeleteProduct;
GO

CREATE PROCEDURE sp_DeleteProduct
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Products WHERE Id = @Id;
END
GO

-- =============================================================
-- Sample data (optional)
-- =============================================================
INSERT INTO Products (Name, Description, Price, CreatedDate) VALUES
    ('Laptop Pro 15',   'High-performance laptop with 16GB RAM and 512GB SSD', 1299.99, GETDATE()),
    ('Wireless Mouse',  'Ergonomic wireless mouse with 2.4GHz connectivity',     29.99, GETDATE()),
    ('Mechanical Keyboard', 'Tenkeyless mechanical keyboard with blue switches', 89.99, GETDATE());
GO

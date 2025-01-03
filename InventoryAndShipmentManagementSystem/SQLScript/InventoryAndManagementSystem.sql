USE [master]
GO
/****** Object:  Database [IASMS]    Script Date: 03-01-2025 11:19:34 ******/
CREATE DATABASE [IASMS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'IASMS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\IASMS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'IASMS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\IASMS_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [IASMS] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [IASMS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [IASMS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [IASMS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [IASMS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [IASMS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [IASMS] SET ARITHABORT OFF 
GO
ALTER DATABASE [IASMS] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [IASMS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [IASMS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [IASMS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [IASMS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [IASMS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [IASMS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [IASMS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [IASMS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [IASMS] SET  ENABLE_BROKER 
GO
ALTER DATABASE [IASMS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [IASMS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [IASMS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [IASMS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [IASMS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [IASMS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [IASMS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [IASMS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [IASMS] SET  MULTI_USER 
GO
ALTER DATABASE [IASMS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [IASMS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [IASMS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [IASMS] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [IASMS] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [IASMS] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [IASMS] SET QUERY_STORE = OFF
GO
USE [IASMS]
GO
/****** Object:  Table [dbo].[tbl_Product]    Script Date: 03-01-2025 11:19:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](100) NULL,
	[Quantity] [int] NULL,
	[Price] [decimal](18, 2) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Shipment]    Script Date: 03-01-2025 11:19:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Shipment](
	[ShipmentId] [int] IDENTITY(1,1) NOT NULL,
	[ShipmentDate] [date] NULL,
	[ProductId] [int] NULL,
	[Quantity] [int] NULL,
	[ShipmentName] [varchar](255) NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tbl_Product] ON 

INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (14, N'Android Mobiles', 10, CAST(25000.00 AS Decimal(18, 2)), CAST(N'2025-01-01T08:28:43.040' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (15, N'IPhones', 50, CAST(1500000.00 AS Decimal(18, 2)), CAST(N'2025-01-01T08:30:12.750' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (16, N'Laptops', 8, CAST(60000.00 AS Decimal(18, 2)), CAST(N'2025-01-01T08:44:21.453' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (18, N'Desktop', 0, CAST(20000.00 AS Decimal(18, 2)), CAST(N'2025-01-01T08:54:58.550' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (61, N'Calculater', 20, CAST(100.00 AS Decimal(18, 2)), CAST(N'2025-01-03T10:28:49.697' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (62, N'Fan', 20, CAST(10000.00 AS Decimal(18, 2)), CAST(N'2025-01-03T07:35:38.120' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (63, N'Mirror', 20, CAST(10000.00 AS Decimal(18, 2)), CAST(N'2025-01-03T07:36:04.993' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (64, N'Cycle', 1, CAST(4000.00 AS Decimal(18, 2)), CAST(N'2025-01-02T16:55:12.027' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (70, N'Wasjing Machine', 15, CAST(10000.00 AS Decimal(18, 2)), CAST(N'2025-01-03T07:34:49.940' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (75, N'IPad', 10, CAST(100000.00 AS Decimal(18, 2)), CAST(N'2025-01-03T10:29:12.273' AS DateTime), N'Sanjay Nigam')
SET IDENTITY_INSERT [dbo].[tbl_Product] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_Shipment] ON 

INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (1, CAST(N'2025-01-02' AS Date), 18, 4, N'Ground')
INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (2, CAST(N'2025-01-02' AS Date), 18, 3, N'air')
INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (3, CAST(N'2025-01-02' AS Date), 18, 3, N'Sea')
INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (4, CAST(N'2025-01-02' AS Date), 16, 2, N'Air')
INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (5, CAST(N'2025-01-02' AS Date), 64, 5, N'Ground')
INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (6, CAST(N'2025-01-02' AS Date), 64, 5, N'Ground')
INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (8, CAST(N'2025-01-03' AS Date), 70, 5, N'Air')
INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (9, CAST(N'2025-01-03' AS Date), 64, 1, N'Ground')
INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (10, CAST(N'2025-01-03' AS Date), 64, 1, N'Ground')
INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (11, CAST(N'2025-01-03' AS Date), 64, 1, N'Ground')
INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (7, CAST(N'2025-01-02' AS Date), 64, 5, N'Ground')
INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (12, CAST(N'2025-01-03' AS Date), 64, 1, N'Ground')
SET IDENTITY_INSERT [dbo].[tbl_Shipment] OFF
GO
/****** Object:  StoredProcedure [dbo].[Usp_Assign_ProductToShipment]    Script Date: 03-01-2025 11:19:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--drop table [tbl_Shipment]

--CREATE TABLE [dbo].[tbl_Shipment](
--	[ShipmentId] [int] IDENTITY(1,1) NOT NULL,
--	[ShipmentDate] [date] NULL,
--	[ProductId] [int] NULL,
--	[Quantity] [int] ,
--	[ShipmentName] [varchar](255) NULL
--) ON [PRIMARY]
--GO

--ALTER TABLE [dbo].[tbl_Shipment]  WITH CHECK ADD FOREIGN KEY([ProductId])
--REFERENCES [dbo].[tbl_Product] ([ProductId])
--GO

CREATE proc [dbo].[Usp_Assign_ProductToShipment]
(
@ProductId INT,
@Quantity INT,
@ShipmentName varchar(50),
@IsSuccess bit out,
@IsError bit out,
@ErrorMsg Varchar(50) out
)
AS
begin
        insert into tbl_Shipment(ShipmentDate,ProductId,Quantity,ShipmentName)
	    values (GETDATE(),ISNULL(@ProductId,''),ISNULL(@Quantity,0),ISNULL(@ShipmentName,''))

	    declare @RemainingQuantity int,@TotalQuantity int

	   select @TotalQuantity=  Quantity from tbl_Product where ProductId=@ProductId
	   select @Quantity as quantity
	   select @TotalQuantity as TotalQuantity
	   set @RemainingQuantity=@TotalQuantity-@Quantity

	   select @RemainingQuantity as RemainingQuantity

	  
	   update tbl_Product
	   set Quantity=ISNULL(@RemainingQuantity,0)
	   where ProductId=@ProductId



	    set @IsSuccess=1
		set @IsError=1
		set @ErrorMsg='Product assigned to shipment successfully!'
  
end


GO
/****** Object:  StoredProcedure [dbo].[Usp_Delete_Product]    Script Date: 03-01-2025 11:19:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--drop table tbl_Product

-- Create Inventory Table
--CREATE TABLE tbl_Product (
--    ProductId INT PRIMARY KEY IDENTITY(1,1),
--    ProductName VARCHAR(100),
--    Quantity INT,
--    Price DECIMAL(18, 2),
--	CreatedDate datetime,
--	CreatedBy varchar(50)
--);

-- Create Shipment Table
--CREATE TABLE tbl_Shipment (
--    ShipmentId INT PRIMARY KEY IDENTITY(1,1),
--    ShipmentDate DATE,
--    ProductId INT,
--    Quantity INT,
--    ShippingAddress VARCHAR(255),
--    FOREIGN KEY (ProductId) REFERENCES tbl_Product(ProductId)
--);

--Create proc Usp_Save_Product_Details
--(
--@ProductName Varchar(100),
--@Quantity INT,
--@Price DECIMAL(18, 2),
--@CreatedBy Varchar(50),
--@IsSuccess bit out,
--@IsError bit out,
--@ErrorMsg Varchar(50) out
--)
--As
--begin

--         insert into tbl_Product(ProductName,Quantity,Price,CreatedDate,CreatedBy)
--		 values (Isnull(@ProductName,''),isnull(@Quantity,0),isnull(@Price,0.0),GETDATE(),ISNULL(@CreatedBy,''))

--		 set @IsSuccess=1
--		 set @IsError=1
--		 set @ErrorMsg='Product details has been added successfully!'

--end

--alter proc Usp_Update_Product_Details
--(
--@ProductId INT,
--@ProductName Varchar(100),
--@Quantity INT,
--@Price DECIMAL(18, 2),
--@CreatedBy Varchar(50),
--@IsSuccess bit out,
--@IsError bit out,
--@ErrorMsg Varchar(50) out
--)
--As
--begin
--     Update tbl_Product
--	 set ProductName=ISNULL(@ProductName,''),Quantity=ISNULL(@Quantity,0),
--	 Price=ISNULL(@Price,0.0),CreatedDate=GETDATE(),CreatedBy=ISNULL(@CreatedBy,'')
--	 where ProductId=@ProductId

--	  set @IsSuccess=1
--	  set @IsError=1
--	  set @ErrorMsg='Product details has been updated successfully!'


--end 


--Create proc Usp_GetAllProducts
--As
--Begin
--     Select * from tbl_Product (nolock)
--end

--Create proc Usp_Get_Product_DetailsById
--(
--@ProductId INT
--)
--As
--begin
--	Select * from tbl_Product (nolock) 
--	where ProductId=@ProductId
--end

Create proc [dbo].[Usp_Delete_Product]
(
@ProductId INT,
@IsSuccess bit out,
@IsError bit out,
@ErrorMsg Varchar(50) out
)
As
begin
     Delete from tbl_Product 
	 where ProductId=@ProductId

	  set @IsSuccess=1
	  set @IsError=1
	  set @ErrorMsg='Product details has been deleted successfully!'

end
GO
/****** Object:  StoredProcedure [dbo].[Usp_Get_Product_DetailsById]    Script Date: 03-01-2025 11:19:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--drop table tbl_Product

-- Create Inventory Table
--CREATE TABLE tbl_Product (
--    ProductId INT PRIMARY KEY IDENTITY(1,1),
--    ProductName VARCHAR(100),
--    Quantity INT,
--    Price DECIMAL(18, 2),
--	CreatedDate datetime,
--	CreatedBy varchar(50)
--);

-- Create Shipment Table
--CREATE TABLE tbl_Shipment (
--    ShipmentId INT PRIMARY KEY IDENTITY(1,1),
--    ShipmentDate DATE,
--    ProductId INT,
--    Quantity INT,
--    ShippingAddress VARCHAR(255),
--    FOREIGN KEY (ProductId) REFERENCES tbl_Product(ProductId)
--);

--Create proc Usp_Save_Product_Details
--(
--@ProductName Varchar(100),
--@Quantity INT,
--@Price DECIMAL(18, 2),
--@CreatedBy Varchar(50),
--@IsSuccess bit out,
--@IsError bit out,
--@ErrorMsg Varchar(50) out
--)
--As
--begin

--         insert into tbl_Product(ProductName,Quantity,Price,CreatedDate,CreatedBy)
--		 values (Isnull(@ProductName,''),isnull(@Quantity,0),isnull(@Price,0.0),GETDATE(),ISNULL(@CreatedBy,''))

--		 set @IsSuccess=1
--		 set @IsError=1
--		 set @ErrorMsg='Product details has been added successfully!'

--end

--alter proc Usp_Update_Product_Details
--(
--@ProductId INT,
--@ProductName Varchar(100),
--@Quantity INT,
--@Price DECIMAL(18, 2),
--@CreatedBy Varchar(50),
--@IsSuccess bit out,
--@IsError bit out,
--@ErrorMsg Varchar(50) out
--)
--As
--begin
--     Update tbl_Product
--	 set ProductName=ISNULL(@ProductName,''),Quantity=ISNULL(@Quantity,0),
--	 Price=ISNULL(@Price,0.0),CreatedDate=GETDATE(),CreatedBy=ISNULL(@CreatedBy,'')
--	 where ProductId=@ProductId

--	  set @IsSuccess=1
--	  set @IsError=1
--	  set @ErrorMsg='Product details has been updated successfully!'


--end 


--Create proc Usp_GetAllProducts
--As
--Begin
--     Select * from tbl_Product (nolock)
--end

Create proc [dbo].[Usp_Get_Product_DetailsById]
(
@ProductId INT
)
As
begin
	Select * from tbl_Product (nolock) 
	where ProductId=@ProductId
end
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetAllProducts]    Script Date: 03-01-2025 11:19:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Usp_GetAllProducts]
As
Begin
     Select * from tbl_Product (nolock) where Quantity>0
end
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetProduct_ShipmentDetails]    Script Date: 03-01-2025 11:19:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from tbl_Product

--select * from tbl_Shipment

CREATE proc [dbo].[Usp_GetProduct_ShipmentDetails]
As
begin 
      select a.ProductId,ProductName,ShipmentId,ShipmentName,ShipmentDate,a.Quantity from tbl_Shipment a(nolock)
	  inner join tbl_Product b(nolock)
	  on a.ProductId=b.ProductId
end 


GO
/****** Object:  StoredProcedure [dbo].[Usp_Save_Product_Details]    Script Date: 03-01-2025 11:19:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--drop table tbl_Product

-- Create Inventory Table
--CREATE TABLE tbl_Product (
--    ProductId INT PRIMARY KEY IDENTITY(1,1),
--    ProductName VARCHAR(100),
--    Quantity INT,
--    Price DECIMAL(18, 2),
--	CreatedDate datetime,
--	CreatedBy varchar(50)
--);

-- Create Shipment Table
--CREATE TABLE tbl_Shipment (
--    ShipmentId INT PRIMARY KEY IDENTITY(1,1),
--    ShipmentDate DATE,
--    ProductId INT,
--    Quantity INT,
--    ShippingAddress VARCHAR(255),
--    FOREIGN KEY (ProductId) REFERENCES tbl_Product(ProductId)
--);

Create proc [dbo].[Usp_Save_Product_Details]
(
@ProductName Varchar(100),
@Quantity INT,
@Price DECIMAL(18, 2),
@CreatedBy Varchar(50),
@IsSuccess bit out,
@IsError bit out,
@ErrorMsg Varchar(50) out
)
As
begin

         insert into tbl_Product(ProductName,Quantity,Price,CreatedDate,CreatedBy)
		 values (Isnull(@ProductName,''),isnull(@Quantity,0),isnull(@Price,0.0),GETDATE(),ISNULL(@CreatedBy,''))

		 set @IsSuccess=1
		 set @IsError=1
		 set @ErrorMsg='Product details has been added successfully!'

end


GO
/****** Object:  StoredProcedure [dbo].[Usp_Update_Product_Details]    Script Date: 03-01-2025 11:19:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Usp_Update_Product_Details]
(
@ProductId INT,
@ProductName Varchar(100),
@Quantity INT,
@Price DECIMAL(18, 2),
@CreatedBy Varchar(50),
@IsSuccess bit out,
@IsError bit out,
@ErrorMsg Varchar(50) out
)
As
begin
     Update tbl_Product
	 set ProductName=ISNULL(@ProductName,''),Quantity=ISNULL(@Quantity,0),
	 Price=ISNULL(@Price,0.0),CreatedDate=GETDATE(),CreatedBy=ISNULL(@CreatedBy,'')
	 where ProductId=@ProductId

	  set @IsSuccess=1
	  set @IsError=1
	  set @ErrorMsg='Product details has been updated successfully!'


end 




GO
USE [master]
GO
ALTER DATABASE [IASMS] SET  READ_WRITE 
GO

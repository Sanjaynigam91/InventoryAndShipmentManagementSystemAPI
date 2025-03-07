USE [IASMS]
GO
/****** Object:  Table [dbo].[tbl_Product]    Script Date: 03-03-2025 17:29:39 ******/
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
/****** Object:  Table [dbo].[tbl_Shipment]    Script Date: 03-03-2025 17:29:40 ******/
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

INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (1, N'IPhones', 50, CAST(150000.00 AS Decimal(18, 2)), CAST(N'2025-03-02T10:28:26.293' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (2, N'OnePlus', 100, CAST(25000.00 AS Decimal(18, 2)), CAST(N'2025-03-02T10:40:55.693' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (3, N'Paper', 100, CAST(10.00 AS Decimal(18, 2)), CAST(N'2025-03-02T10:41:28.523' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (4, N'Pepsi', 390, CAST(1000.00 AS Decimal(18, 2)), CAST(N'2025-03-02T10:41:54.930' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (5, N'Tables', 100, CAST(15000.00 AS Decimal(18, 2)), CAST(N'2025-03-02T10:42:24.273' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (7, N'TShirt', 5, CAST(500.00 AS Decimal(18, 2)), CAST(N'2025-03-02T11:58:23.393' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (8, N'Jeans', 5, CAST(500.00 AS Decimal(18, 2)), CAST(N'2025-03-02T12:18:48.877' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (9, N'Mouse', 150, CAST(1000.00 AS Decimal(18, 2)), CAST(N'2025-03-02T12:46:02.333' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (10, N'Chair', 200, CAST(150.00 AS Decimal(18, 2)), CAST(N'2025-03-02T13:19:55.770' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (11, N'Water Bottle', 500, CAST(40.00 AS Decimal(18, 2)), CAST(N'2025-03-03T15:01:44.207' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (15, N'Mouse', 150, CAST(1000.00 AS Decimal(18, 2)), CAST(N'2025-03-02T15:26:06.013' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (17, N'Mouse', 150, CAST(1000.00 AS Decimal(18, 2)), CAST(N'2025-03-02T15:31:22.520' AS DateTime), N'Sanjay Nigam')
INSERT [dbo].[tbl_Product] ([ProductId], [ProductName], [Quantity], [Price], [CreatedDate], [CreatedBy]) VALUES (23, N'Bat', 100, CAST(1000.00 AS Decimal(18, 2)), CAST(N'2025-03-03T15:09:05.037' AS DateTime), N'Sanjay Nigam')
SET IDENTITY_INSERT [dbo].[tbl_Product] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_Shipment] ON 

INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (1, CAST(N'2025-03-03' AS Date), 4, 100, N'Air')
INSERT [dbo].[tbl_Shipment] ([ShipmentId], [ShipmentDate], [ProductId], [Quantity], [ShipmentName]) VALUES (2, CAST(N'2025-03-03' AS Date), 4, 10, N'Air')
SET IDENTITY_INSERT [dbo].[tbl_Shipment] OFF
GO
/****** Object:  StoredProcedure [dbo].[Usp_Assign_ProductToShipment]    Script Date: 03-03-2025 17:29:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
/****** Object:  StoredProcedure [dbo].[Usp_Delete_Product]    Script Date: 03-03-2025 17:29:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Usp_Delete_Product]
(
@ProductId INT,
@IsSuccess bit out,
@IsError bit out,
@ErrorMsg Varchar(50) out
)
As
begin    
	  IF EXISTS (select ProductId from tbl_Product where ProductId=@ProductId)
	  begin
			  Delete from tbl_Product 
			  where ProductId=@ProductId

			  set @IsSuccess=1
			  set @IsError=1
			  set @ErrorMsg='Product details has been deleted successfully!'
	  end
	  else 
	 begin
		      set @IsSuccess=0
			  set @IsError=0
			  set @ErrorMsg='This product is not belong to our records!'
	 end

     

end



GO
/****** Object:  StoredProcedure [dbo].[Usp_Get_Product_DetailsById]    Script Date: 03-03-2025 17:29:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
/****** Object:  StoredProcedure [dbo].[Usp_GetAllProducts]    Script Date: 03-03-2025 17:29:40 ******/
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
/****** Object:  StoredProcedure [dbo].[Usp_GetProduct_ShipmentDetails]    Script Date: 03-03-2025 17:29:40 ******/
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
/****** Object:  StoredProcedure [dbo].[Usp_Save_Product_Details]    Script Date: 03-03-2025 17:29:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Usp_Save_Product_Details]
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
/****** Object:  StoredProcedure [dbo].[Usp_Update_Product_Details]    Script Date: 03-03-2025 17:29:40 ******/
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
     IF EXISTS (select ProductId from tbl_Product where ProductId=@ProductId)
	 begin
		 Update tbl_Product
		 set ProductName=ISNULL(@ProductName,''),Quantity=ISNULL(@Quantity,0),
		 Price=ISNULL(@Price,0.0),CreatedDate=GETDATE(),CreatedBy=ISNULL(@CreatedBy,'')
		 where ProductId=@ProductId

		  set @IsSuccess=1
		  set @IsError=1
		  set @ErrorMsg='Product details has been updated successfully!'
	  end
	  else
	  begin
	      set @IsSuccess=0
		  set @IsError=0
		  set @ErrorMsg='This product is not belong to our records!'

	  end

end 










GO

USE [LCPC]
GO
/****** Object:  Table [dbo].[CateInfo]    Script Date: 2024-03-15 20:54:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CateInfo](
	[Id] [nvarchar](50) NOT NULL,
	[CateName] [nvarchar](50) NOT NULL,
	[ParentId] [nvarchar](50) NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_CateInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomerInfo]    Script Date: 2024-03-15 20:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerInfo](
	[Id] [nvarchar](50) NOT NULL,
	[CustomerCode] [nvarchar](100) NOT NULL,
	[CustomerName] [nvarchar](100) NOT NULL,
	[CustomerUser] [nvarchar](max) NULL,
	[TelNumber] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](100) NULL,
	[Address] [nvarchar](100) NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](100) NULL,
	[Enable] [bit] NOT NULL,
	[NameSpell] [nvarchar](200) NULL,
 CONSTRAINT [PK_CustomerInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExtraOrder]    Script Date: 2024-03-15 20:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtraOrder](
	[Id] [nvarchar](50) NOT NULL,
	[ExtraType] [int] NOT NULL,
	[TypeName] [nvarchar](100) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[Enable] [bit] NOT NULL,
	[OrderCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_ExtraOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderInfo]    Script Date: 2024-03-15 20:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderInfo](
	[Id] [nvarchar](50) NOT NULL,
	[OrderCode] [nvarchar](100) NOT NULL,
	[OrderTime] [nvarchar](50) NOT NULL,
	[OrderUser] [nvarchar](50) NOT NULL,
	[OrderTel] [nvarchar](50) NOT NULL,
	[OrderPay] [nvarchar](50) NULL,
	[OrderClient] [nvarchar](50) NOT NULL,
	[OrderMoney] [decimal](18, 2) NOT NULL,
	[OrderStatus] [int] NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[Enable] [bit] NOT NULL,
	[ActuailMoney] [decimal](18, 2) NULL,
	[OffsetMoney] [decimal](18, 2) NULL,
	[OrderUserId] [nvarchar](50) NULL,
 CONSTRAINT [PK_OrderInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderInfoDetail]    Script Date: 2024-03-15 20:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderInfoDetail](
	[Id] [nvarchar](50) NOT NULL,
	[ProductId] [nvarchar](100) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[ProductCode] [nvarchar](100) NOT NULL,
	[OrderCount] [int] NOT NULL,
	[OrderSigle] [decimal](18, 2) NOT NULL,
	[OrderPrice] [decimal](18, 2) NOT NULL,
	[OrderId] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[Enable] [bit] NOT NULL,
	[UnitName] [varchar](50) NULL,
 CONSTRAINT [PK_OrderInfoDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductInfo]    Script Date: 2024-03-15 20:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductInfo](
	[Id] [nvarchar](50) NOT NULL,
	[ProductCode] [nvarchar](100) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[ProductModel] [nvarchar](100) NULL,
	[CateId] [nvarchar](50) NOT NULL,
	[UnitId] [nvarchar](50) NOT NULL,
	[SupilerId] [nvarchar](50) NOT NULL,
	[ConversionRate] [nvarchar](50) NULL,
	[InventoryCount] [int] NOT NULL,
	[InitialCost] [decimal](18, 2) NOT NULL,
	[Purchase] [decimal](18, 2) NOT NULL,
	[SellPrice] [decimal](18, 2) NOT NULL,
	[Wholesale] [decimal](18, 2) NOT NULL,
	[MaxStock] [int] NOT NULL,
	[MinStock] [int] NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[Enable] [bit] NOT NULL,
	[SupplierId] [nvarchar](50) NULL,
	[NameSpell] [nvarchar](200) NULL,
 CONSTRAINT [PK_ProductInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PurchaseInDetail]    Script Date: 2024-03-15 20:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseInDetail](
	[Id] [nvarchar](450) NOT NULL,
	[PurchaseInId] [nvarchar](max) NULL,
	[ProductId] [nvarchar](50) NOT NULL DEFAULT (N''),
	[ProductCode] [nvarchar](50) NOT NULL DEFAULT (N''),
	[ProductModel] [nvarchar](50) NULL,
	[ProductName] [nvarchar](100) NOT NULL DEFAULT (N''),
	[ProductCount] [int] NOT NULL,
	[ProductPrice] [decimal](18, 2) NOT NULL,
	[ProductAll] [decimal](18, 2) NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL DEFAULT (N''),
	[Remark] [nvarchar](200) NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_PurchaseInDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PurchaseInOrder]    Script Date: 2024-03-15 20:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseInOrder](
	[Id] [nvarchar](50) NOT NULL,
	[InOrderTime] [nvarchar](50) NOT NULL,
	[ChannelType] [int] NOT NULL,
	[Logistics] [nvarchar](100) NULL,
	[InUser] [nvarchar](100) NULL,
	[InPhone] [nvarchar](50) NULL,
	[SupplierId] [nvarchar](50) NULL,
	[InCount] [int] NOT NULL,
	[InPrice] [decimal](18, 2) NOT NULL,
	[InOStatus] [int] NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[Enable] [bit] NOT NULL,
	[PurchaseCode] [nvarchar](50) NOT NULL DEFAULT (N''),
 CONSTRAINT [PK_PurchaseInOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PurchaseOutOrder]    Script Date: 2024-03-15 20:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOutOrder](
	[Id] [nvarchar](50) NOT NULL,
	[PurchaseCode] [nvarchar](100) NOT NULL,
	[OrderTime] [nvarchar](max) NULL,
	[InOrderCode] [nvarchar](100) NOT NULL,
	[SupilerId] [nvarchar](50) NOT NULL,
	[InUser] [nvarchar](50) NOT NULL,
	[Logicse] [nvarchar](100) NULL,
	[OutOrderPrice] [decimal](18, 2) NOT NULL,
	[InPhone] [nvarchar](100) NOT NULL,
	[OutStatus] [int] NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[Enable] [bit] NOT NULL,
	[OutOrderCount] [int] NULL,
 CONSTRAINT [PK_PurchaseOutOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PurchaseOutOrderDetail]    Script Date: 2024-03-15 20:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOutOrderDetail](
	[Id] [nvarchar](50) NOT NULL,
	[ProductCode] [nvarchar](50) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[ProductModel] [nvarchar](50) NULL,
	[InCount] [int] NOT NULL,
	[InPrice] [decimal](18, 2) NOT NULL,
	[OutCount] [int] NOT NULL,
	[OutPrice] [decimal](18, 2) NOT NULL,
	[OutAllPrice] [decimal](18, 2) NOT NULL,
	[PurchaseId] [nvarchar](50) NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_PurchaseOutOrderDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RuleInfo]    Script Date: 2024-03-15 20:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleInfo](
	[Id] [nvarchar](50) NOT NULL,
	[RuleType] [int] NOT NULL,
	[RuleName] [nvarchar](50) NOT NULL,
	[RulePix] [nvarchar](20) NOT NULL,
	[Formatter] [nvarchar](50) NOT NULL,
	[IdentityNum] [int] NOT NULL,
	[RuleAppend] [int] NOT NULL,
	[NowValue] [int] NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_RuleInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SupplierInfo]    Script Date: 2024-03-15 20:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupplierInfo](
	[Id] [nvarchar](50) NOT NULL,
	[SupNumber] [nvarchar](100) NULL,
	[SupName] [nvarchar](100) NULL,
	[SupTel] [nvarchar](100) NULL,
	[SupPhone] [nvarchar](100) NULL,
	[ProviderUser] [nvarchar](100) NULL,
	[Address] [nvarchar](200) NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[Enable] [bit] NOT NULL,
	[ProviderUserT] [nvarchar](100) NULL,
	[SupPhoneT] [nvarchar](100) NULL,
	[SupTelT] [nvarchar](100) NULL,
 CONSTRAINT [PK_SupplierInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemDicInfo]    Script Date: 2024-03-15 20:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemDicInfo](
	[Id] [nvarchar](50) NOT NULL,
	[DicType] [int] NOT NULL,
	[DicName] [nvarchar](100) NOT NULL,
	[DicCode] [nvarchar](100) NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_SystemDicInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserInfo]    Script Date: 2024-03-15 20:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInfo](
	[Id] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[UserPass] [nvarchar](50) NOT NULL,
	[UserTel] [nvarchar](50) NULL,
	[UserAddress] [nvarchar](100) NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[OrderInfoDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderInfoDetail_OrderInfo_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrderInfo] ([Id])
GO
ALTER TABLE [dbo].[OrderInfoDetail] CHECK CONSTRAINT [FK_OrderInfoDetail_OrderInfo_OrderId]
GO
ALTER TABLE [dbo].[PurchaseOutOrder]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseOutOrder_SupplierInfo_SupilerId] FOREIGN KEY([SupilerId])
REFERENCES [dbo].[SupplierInfo] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PurchaseOutOrder] CHECK CONSTRAINT [FK_PurchaseOutOrder_SupplierInfo_SupilerId]
GO
ALTER TABLE [dbo].[PurchaseOutOrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseOutOrderDetail_PurchaseOutOrder_PurchaseId] FOREIGN KEY([PurchaseId])
REFERENCES [dbo].[PurchaseOutOrder] ([Id])
GO
ALTER TABLE [dbo].[PurchaseOutOrderDetail] CHECK CONSTRAINT [FK_PurchaseOutOrderDetail_PurchaseOutOrder_PurchaseId]
GO

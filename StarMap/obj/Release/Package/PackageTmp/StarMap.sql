USE [master]
GO
/****** Object:  Database [starmap]    Script Date: 1/21/2016 5:10:27 PM ******/
CREATE DATABASE [starmap] ON  PRIMARY 
( NAME = N'StarMap', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLSERVER2008R2\MSSQL\DATA\starmap.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'StarMap_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLSERVER2008R2\MSSQL\DATA\starmap_0.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [starmap] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [starmap].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [starmap] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [starmap] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [starmap] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [starmap] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [starmap] SET ARITHABORT OFF 
GO
ALTER DATABASE [starmap] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [starmap] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [starmap] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [starmap] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [starmap] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [starmap] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [starmap] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [starmap] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [starmap] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [starmap] SET  DISABLE_BROKER 
GO
ALTER DATABASE [starmap] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [starmap] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [starmap] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [starmap] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [starmap] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [starmap] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [starmap] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [starmap] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [starmap] SET  MULTI_USER 
GO
ALTER DATABASE [starmap] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [starmap] SET DB_CHAINING OFF 
GO
USE [starmap]
GO
/****** Object:  User [starmapv]    Script Date: 1/21/2016 5:10:27 PM ******/
CREATE USER [starmapv] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[starmapv]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [starmapv]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [starmapv]
GO
ALTER ROLE [db_datareader] ADD MEMBER [starmapv]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [starmapv]
GO
/****** Object:  Schema [starmapv]    Script Date: 1/21/2016 5:10:27 PM ******/
CREATE SCHEMA [starmapv]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 1/21/2016 5:10:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 1/21/2016 5:10:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 1/21/2016 5:10:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[User_Id] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 1/21/2016 5:10:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[UserId] [nvarchar](128) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 1/21/2016 5:10:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 1/21/2016 5:10:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Category]    Script Date: 1/21/2016 5:10:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Image] [varchar](200) NOT NULL,
	[Lang] [nchar](2) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Event]    Script Date: 1/21/2016 5:10:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Event](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Address] [nvarchar](200) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Location] [nvarchar](1000) NOT NULL,
	[ThumbImage] [varchar](200) NULL,
	[DetailImage] [nvarchar](200) NULL,
	[ThumbDescription] [nvarchar](200) NULL,
	[DetailDescription] [nvarchar](max) NULL,
	[PublicDate] [datetime] NULL,
	[CreateDate] [datetime] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[CreatedBy] [nvarchar](128) NOT NULL,
	[Lang] [nchar](2) NOT NULL,
	[CityId] [int] NULL,
	[CountryId] [int] NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_Event_IsActive]  DEFAULT ((0)),
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GoldPoint]    Script Date: 1/21/2016 5:10:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GoldPoint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Address] [nvarchar](200) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Location] [nvarchar](1000) NOT NULL,
	[ThumbImage] [varchar](200) NULL,
	[DetailImage] [nvarchar](200) NULL,
	[ThumbDescription] [nvarchar](200) NULL,
	[DetailDescription] [nvarchar](max) NULL,
	[PublicDate] [datetime] NULL,
	[CreateDate] [datetime] NOT NULL,
	[Rate] [varchar](5) NULL,
	[CategoryId] [int] NOT NULL,
	[CreatedBy] [nvarchar](128) NOT NULL,
	[Lang] [nchar](2) NOT NULL,
	[CityId] [int] NULL,
	[CountryId] [int] NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_GoldPoint_IsActive]  DEFAULT ((0)),
 CONSTRAINT [PK_GoldPoint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sale]    Script Date: 1/21/2016 5:10:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sale](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Address] [nvarchar](200) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Location] [nvarchar](1000) NOT NULL,
	[ThumbImage] [varchar](200) NULL,
	[DetailImage] [nvarchar](200) NULL,
	[ThumbDescription] [nvarchar](200) NULL,
	[DetailDescription] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[IsHot] [bit] NULL,
	[CategoryId] [int] NOT NULL,
	[CreatedBy] [nvarchar](128) NOT NULL,
	[Lang] [nchar](2) NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[CityId] [int] NULL,
	[CountryId] [int] NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_Sale_IsActive]  DEFAULT ((0)),
 CONSTRAINT [PK_Sale] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'201512020911192_InitialCreate', N'StarMap.Models.ApplicationDbContext', 0x1F8B0800000000000400DD5CDB72DB36107DEF4CFF81C3A7A433116DE725F548C938B2DD7A1AD999C8C96B0626211953126409C895BFAD0FFDA4FE4201DE495C085E74715E3C12B9D85DEC1E2C6E47FEEF9F7FA71FB6816F3DC198A010CFECD3C9896D41EC861EC2EB99BDA1AB37EFEC0FEF7FFE697AE5055BEB5B2EF796CBB19698CCEC474AA373C721EE230C009904C88D4312AEE8C40D030778A1737672F2AB737AEA40A6C266BA2C6BFA6583290A60F2857D9D87D88511DD007F117AD027D973F6669968B56E410049045C38B39714C40B104D5249DBBAF011605E2CA1BFB22D80714801653E9E7F257049E310AF97117B00FCFBE70832B915F009CC7C3F2FC54DBB7172C6BBE1940D7B85C12E3AC8BA78C542419FB97B493767F68D0793475F421F562599EC1FF0B9F6803DFA1C87118C99345C15ED6DCBA9B7739A0D8B669536DC051EDF98E5DEB61660FB09E2357D64A8387B675BD7680BBDFC4916C5AF1831A8B04634DEB0AFB71BDF070F3E2CDE3B5A9BFCAFC62AFB38DCEAD429C3AB0D7A062B869AD8B63E020233BF18AC267942D2978E51F2B8ECDC0728D87F066F307D7B26894B65702C6918C3DF208631A0D0FB0C2885312E9D6FCB5CD2316E6C84F41958FA06FCCD0E4CDD8227B44EE2D1309AE6F90BF49397E411451220249E7D4F45AFE330E0A35592FE44E2FB32DCC42EEF42A815BB07F11AD29E08AEA1F4072E1BBC7F23950EBDA1CF8090BFC3D8FB1D90C79D1B5B427713B3ECB13A14447BC47A823E322EDA7318B7A03D1F14A6AE7E0AD708B7BB9A88695D2D25B4AE56C4BABACA95B57BCAA5B48E16025A3F4B29999BBDEA47D2F53E458437160B895434B1C11E3F21AF39AD2A5AE4C2CC93EEA52AF76CDFE5AAD1CD7D9BAFC56CE7C6874DA99D46AE6E4A950CF0C143A2EF6ABCC388E026FACCC2878276EEEFE1609524C5A8CCA6929A329BFCD181AA9492616A30F83B4C06AD5E7646FE0521A18B12F754B3C177C9BAF20A7B96D920956FA6D269C65A6C7C8A221FB9ECF1CCFE45086CAB99627654EDD9EA164EEDE658BCC397D087145A176E7A1A3007C4059E8844163FAFFE840D5F18735BC09FB3E4D218204CC5B18EB08B22E01BF5A3D1DA70B5CEBD2BEC34DF5CC208626ED4285F260E28EB9A53D86A84AE2D5253A70243737496C3DB043592B12E074D5A30FA41532C1512233203C7854CA11B7B06A6902B13FBF249F450B8342E9A92EABE235CBEFC8A2974E310B87CB1F5B2B29537C18CEC144B0E9AF4B8B3273465C707FBC6A632A0E9A2892581B214C0380BEA45C47D48842F1FF84BB8A5424879C325A4F573817211269D138480D5950861D369AC24A683DA368D9D94E5E7365A8DD952B083DAB65896C5B2A1B492648DC3D95ABC22ACDFB1364168BC362EFA288F9A502F8C57C30AC582CE1AEC9978C76855B657FA6029166A1D966A8A1E6548308C94B838ABA895AA1A2340266852AC183AAC19C60BD02190543D64D6474A35877599C5149DCA2BAA61B024F3D608D1CAF7E9C59453DE883BE995787E75EE28EECEA70B104508AF2B77E9D9136B995EA4CFDF2CBB5F5E07A90EC725923BECC2DBC2120D63B0868DB7CC34F3F41AC5845E020A1E003FF1997B8120D66582CD4D56E7593183F954914BF3CFB5FBDF8C56509F90C5E55DD6FE9AF52EE06BC4E43E55927779738BF31A800F62C96DDA3CF43701562F59D5ADD33BB16AFBF489A861EA34FC1796A442A48475533DEC4649A90F8601C9A9DED5F7CF8DBCF9E0DC309C7A28597A27D7D60D664155C525226E8C028401F37017796A89EA0DE19FEF56AF14D8E71EBF3EBE004B5B735BE200289F9A6BAADFFA56B5D5DF986B6C5CED5655365EED1D029ACC1F5FE2859155DF91EC75688D5002B385C648E992708D3AE74CA1633723B6C221AA2AA93CEEA82B630909CAB2E74789816C9337120624B7F69D31A0D0A1ABBB4D1CA84EBAD45A1A77E455652D2C01B5CEDAC577AD8CAB590447828AD156AFF27BEBCE98E8B68A1D0712F9317E558BEA687FF749AB6FD45A6A7AB683352FDB5983CEA5996F49859AACDB9A8A71332AEC890AF9D975C583DECE290FD17BCF3A6ABF9A7B6E31DBC2D6BB295260ADD88237B6DAD36CDBDBCE6517F6C1A9886DE5358A61E79950184CB8C064F9973FF7118B6129B00018AD20A1F7E19F10CFECB393D3B30625BE073DDD21C4F38F99A38E9F40EC3E8258249B0CA0A0E74A5F0560FB7A2C5AF96149B9A3C5A9DC48B5C4AA2FAF76904229777690C6C6FE6150347B61E5403F1D40BCB6B4FE38A02378845F0B0C4A8DF88B8041EA8AB96DFF39FEE149B6A3951F29877634ED128AEC3E61F06288A5A305BCCE1B1D39D6A352190F4390D1DCF00EA5520EE6DE1C849CF8B20836E684C4F4D6FC10DC40F5CDEF40E2D9208029CE2A764EE6FA918985C754C20E8DAF431430637C1D4DFDEA48103C2680654CB76104C5170731D5DD8F1C638A93BCD141D63846CBB76F02F543713C2E3B686A234EA6C76733DB7B081908D2A5E305896E2195F3BA54C652F42A8D65C7B54A6372B68FCE58864CADC54C466F56CE5E6AA7679AB133F5B6E58CC5560EA71185536F599EDD43F23C952CB06E844E3D3BF4251039AB1916A9515D28895A76E34BA16C0EC7C5BE43B14352E6F060D4EAA6E2D26C27AC4BF18A87CD94957F6AC4E66B82D6A50A4E5EC1D0ADCD9185CC0D5E85F95CDDF02817699CA02C20051E9B402F628A56C0A5ECB50B09497E7F9B9163AE8207E8DDE0BB0D8D36947519060F7EED448F4FF93AFB09B5B4EEF3F42E4A7EC13A4617989B887501DEE18F1BE47B85DFD792A31E850ABE96C8CEA9792E293FAF5E3F179A6E436CA8280B5FB104BA8741E43365E40E2FC11354FBD61EC37AC4A69708AC6310904C47D99E7D65F0F382EDFBFF01008A8C5E964B0000, N'6.0.0-20911')
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [PasswordHash], [SecurityStamp], [Discriminator]) VALUES (N'9678e596-87af-44c6-81a2-735b25c3ef96', N'admin', N'AGh6b+57/QRd92W03jx0bzw3JgRy4PIMLLz3bPlHzKwVq5G4CSW/ykf1cNjg8vMb6w==', N'e4eaf3f9-bfe5-4585-a4a7-f614bf7171e7', N'StarMapUser')
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([Id], [Name], [Image], [Lang]) VALUES (7, N'Test category', N'/Upload/2016/1/938-appskfc.png', N'en')
INSERT [dbo].[Category] ([Id], [Name], [Image], [Lang]) VALUES (8, N'Nhà hàng', N'/Upload/2015/12/cardechecklight.PNG', N'vi')
INSERT [dbo].[Category] ([Id], [Name], [Image], [Lang]) VALUES (9, N'Gaming center', N'/Upload/2015/12/402-images.jpg', N'en')
INSERT [dbo].[Category] ([Id], [Name], [Image], [Lang]) VALUES (10, N'Museum', N'/Upload/2015/12/Museum.png', N'en')
INSERT [dbo].[Category] ([Id], [Name], [Image], [Lang]) VALUES (11, N'quán ăn', N'/Upload/2015/12/ccinput.png', N'vi')
INSERT [dbo].[Category] ([Id], [Name], [Image], [Lang]) VALUES (12, N'Trụng tâm giải trí (gaming center)', N'/Upload/2015/12/images.jpg', N'vi')
INSERT [dbo].[Category] ([Id], [Name], [Image], [Lang]) VALUES (15, N'Restaurant', N'/Upload/2016/1/Restaurant-icon-227x2221.jpg', N'en')
INSERT [dbo].[Category] ([Id], [Name], [Image], [Lang]) VALUES (22, N'abc (edited 2)', N'/Upload/2016/1/488-appskfc.png', N'vi')
SET IDENTITY_INSERT [dbo].[Category] OFF
SET IDENTITY_INSERT [dbo].[Event] ON 

INSERT [dbo].[Event] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (2, N'event test', N'adfsd', N'1231233', N'21.0333333,105.85', N'/Upload/2015/12/96-ccinput.png', NULL, N'aaaa', N'bbbb', CAST(N'2015-12-25 05:20:00.000' AS DateTime), CAST(N'2015-12-25 17:20:30.097' AS DateTime), 8, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'vi', NULL, NULL, 1)
INSERT [dbo].[Event] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (4, N'abc', N'hanoi', N'090 459 16 476666', N'20.995996, 105.845357', N'/Upload/2016/1/220-09092014162926.png', N'/Upload/2016/1/224-appskfc.png', N'aaaaaaaaaaaaaaaaaaaaa', N'aaa', CAST(N'2016-01-15 11:03:00.000' AS DateTime), CAST(N'2016-01-09 11:04:38.213' AS DateTime), 11, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'vi', NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Event] OFF
SET IDENTITY_INSERT [dbo].[GoldPoint] ON 

INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (3, N'Nhà hàng 1', N'133 a hi hi', N'01686326049', N'21.029151, 105.853904', N'/Upload/2015/12/263-cardecheckgray.PNG', NULL, N'a hi hi', N'b hi hi', NULL, CAST(N'2015-12-20 03:08:19.257' AS DateTime), N'1.5', 8, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'vi', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (4, N'Nhà Hàng Thủy Tạ', N'1-6 Lê Thái Tổ - Quận Hoàn Kiếm - Hà Nội', N'043828 9347', N'21.031154, 105.851102', N'/Upload/2015/12/ThuyTaSmallImg.jpg', N'/Upload/2015/12/ThuyTaSmallImg2.jpg', N' Thương hiệu Thủy Tạ đã có một uy tín, danh tiếng lâu năm với bề dày hơn 50 năm lịch sử phát triển.', N' Thương hiệu Thủy Tạ đã có một uy tín, danh tiếng lâu năm với bề dày hơn 50 năm lịch sử phát triển của Công ty. Khởi đầu trong lĩnh vực kinh doanh thương mại, dịch vụ, thương hiệu Thủy Tạ trước hết gắn liền với tên tuổi các nhà hàng, cửa hàng nổi tiếng của Công ty  Thủy Tạ. 
    Chắc không chỉ mọi người dân Hà Nội, mà còn hầu hết người dân Việt Nam và du khách nước ngoài đã đến Thủ đô đều biết đến nhà hàng Cà phê Thủy Tạ với vị trí địa lý đặc biệt, là nhà hàng duy nhất nằm bên bờ Hồ Gươm, thắng cảnh di tích lịch sử nổi tiếng, trái tim của Thủ đô, điều này đã làm cho sản phẩm và dịch vụ của nhà hàng Thủy Tạ có một nét rất riêng và ấn tượng. ', NULL, CAST(N'2015-12-23 15:43:10.793' AS DateTime), N'4', 8, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'vi', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (5, N'Thuy Ta Restaurant', N'No. 01, Le Thai To Street, Hoan Kiem, Hanoi', N'043828 9347', N'21.031199, 105.851092', N'/Upload/2015/12/71-ThuyTaSmallImg.jpg', N'/Upload/2015/12/74-ThuyTaSmallImg2.jpg', N'Thuy Ta Restaurant Coffee is the only floating restaurant beside Hoan Kiem lake, where you can enjoy panoramic lake, in a quiet space, gently but very romantic Hoan Kiem Lake.', N'Thuy Ta Restaurant Coffee is the only floating restaurant beside Hoan Kiem lake, where you can enjoy panoramic lake, in a quiet space, gently but very romantic Hoan Kiem Lake, you can li sat sipping coffee, enjoying ice cream flavors or traditional small party organized for family, friends and loved ones.', NULL, CAST(N'2015-12-23 15:57:58.063' AS DateTime), N'4', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (7, N'Arena gaming center', N'140 Trần Đại Nghĩa str, Hai Bà Trưng, Hà Nộiiii', N'090 459 16 476666', N'20.995996, 105.84535702', N'/Upload/2015/12/70-IMG20151222130532.jpg', N'/Upload/2015/12/Arena2trandainghia8.jpg', N'Arena gaming center - For the professional
Website/Fanpage: arenagc.vn', N'One of the most luxury gaming center in Hanoi
Good and quick services, has food and drink beside video games
High quality and big quantity gaming gear
Many discounts programs every month', NULL, CAST(N'2015-12-29 10:22:46.603' AS DateTime), N'5', 9, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (8, N'ádasdsa', N'ádasdas', NULL, N'20.99599, 105.84535', N'/Upload/2016/1/312-appskfc.png', NULL, N'ádasdasd', N'adsadasd', NULL, CAST(N'2015-12-29 10:30:45.507' AS DateTime), N'0.5', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (10, N'ádsadasdsa', N'adasdasdas', N'090 459 16 47', N'20.995996, 105.845357', NULL, NULL, N'ádasdasdasd', N'ádasdasdasdas', NULL, CAST(N'2015-12-29 10:47:48.650' AS DateTime), N'3.5', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (15, N'sdsd', N'hanoi', NULL, N'20.995996, 105.845357', N'/Upload/2016/1/884-Restaurant-icon-227x2221.jpg', N'/Upload/2016/1/887-Restaurant-icon-227x2221.jpg', NULL, NULL, NULL, CAST(N'2016-01-09 08:47:33.950' AS DateTime), N'3', 8, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (16, N'Abc123', N'140 Trần Đại Nghĩa str, Hai Bà Trưng, Hà Nội', NULL, N'20.995996, 105.845357', NULL, NULL, N'123', N'123', NULL, CAST(N'2016-01-09 09:19:46.073' AS DateTime), N'3', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (17, N'Abc1', N'140 Trần Đại Nghĩa str, Hai Bà Trưng, Hà Nội', NULL, N'20.995996, 105.845357', NULL, NULL, N'Abc1', N'Abc1', NULL, CAST(N'2016-01-18 13:37:49.690' AS DateTime), N'3', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (18, N'Abc2', N'140 Trần Đại Nghĩa str, Hai Bà Trưng, Hà Nội', NULL, N'20.995996, 105.845357', NULL, NULL, N'123', N'123', NULL, CAST(N'2016-01-18 13:38:47.213' AS DateTime), N'1.5', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (19, N'Abc4', N'20.995996, 105.845357', NULL, N'20.995996, 105.845357', NULL, NULL, N'123', N'123', NULL, CAST(N'2016-01-18 13:39:44.030' AS DateTime), N'0', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (20, N'Abc5', N'140 Trần Đại Nghĩa str, Hai Bà Trưng, Hà Nội', NULL, N'21.0333333,105.85', NULL, NULL, N'ABC', N'abc', NULL, CAST(N'2016-01-18 13:40:19.087' AS DateTime), N'0', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (21, N'Abc6', N'20.995996, 105.845357', N'01686326049', N'21.0333333,105.85', NULL, NULL, N'aa', N'vvv', NULL, CAST(N'2016-01-18 13:40:48.130' AS DateTime), N'4', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (22, N'Abc7', N'133 a hi hi', N'01686326049', N'20.995996, 105.845357', NULL, NULL, N'dd', N'ee', NULL, CAST(N'2016-01-18 13:41:09.260' AS DateTime), N'2', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (23, N'Abc8', N'140 Trần Đại Nghĩa str, Hai Bà Trưng, Hà Nội', N'1212121212', N'21.0333333,105.85', NULL, NULL, N'abc', N'abc', NULL, CAST(N'2016-01-18 13:41:35.563' AS DateTime), N'0', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (24, N'Abc9', N'140 Trần Đại Nghĩa str, Hai Bà Trưng, Hà Nội', N'1212121212', N'21.0333333,105.85', NULL, NULL, N'fff', N'aa', NULL, CAST(N'2016-01-18 13:42:00.163' AS DateTime), N'4', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (25, N'abc10', N'test', NULL, N'21.0333333,105.85', NULL, NULL, N'fff', N'ggg', NULL, CAST(N'2016-01-18 13:42:28.187' AS DateTime), N'0', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (26, N'Note coffee', N'64 Lương Văn Can', NULL, N'21.0333333,105.85', NULL, NULL, NULL, NULL, NULL, CAST(N'2016-01-18 15:32:54.663' AS DateTime), N'5', 15, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (27, N'Z coffee to go', N'ngõ 13 núi trúc', NULL, N'21.0333333,105.85', NULL, NULL, NULL, NULL, NULL, CAST(N'2016-01-18 15:33:52.257' AS DateTime), N'5', 15, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (28, N'Highland coffee', N'40 cát linh', NULL, N'21.0333333,105.85', NULL, NULL, NULL, NULL, NULL, CAST(N'2016-01-18 15:34:50.623' AS DateTime), N'5', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (29, N'Starbucks', N'32 hàng bài', NULL, N'21.0333333,105.85', NULL, NULL, NULL, NULL, NULL, CAST(N'2016-01-18 15:35:29.643' AS DateTime), N'5', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (30, N'city view cafe', N'số 7 hàng bài', NULL, N'21.0333333,105.85', NULL, NULL, NULL, NULL, NULL, CAST(N'2016-01-18 15:36:21.427' AS DateTime), N'5', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (31, N'gòn quán', N'71 giang văn minh', NULL, N'21.0333333,105.85', NULL, NULL, NULL, NULL, NULL, CAST(N'2016-01-18 15:36:55.230' AS DateTime), N'5', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (32, N'royal city', N'74 nguyễn trãi', NULL, N'21.0333333,105.85', NULL, NULL, NULL, NULL, NULL, CAST(N'2016-01-18 15:37:41.960' AS DateTime), N'4', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (33, N'Nhà heo tró', N'107 Hàng Đào', NULL, N'21.0333333,105.85', NULL, NULL, NULL, NULL, NULL, CAST(N'2016-01-18 15:38:30.183' AS DateTime), N'0.5', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (34, N'Cung điện Mju Xuynh :**', N'6 thái thịnh 2', NULL, N'21.0333333,105.85', NULL, NULL, NULL, NULL, NULL, CAST(N'2016-01-18 15:39:06.293' AS DateTime), N'5', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
INSERT [dbo].[GoldPoint] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [PublicDate], [CreateDate], [Rate], [CategoryId], [CreatedBy], [Lang], [CityId], [CountryId], [IsActive]) VALUES (35, N'thiên đường', N'Ai biết', NULL, N'21.0333333,105.85', NULL, NULL, NULL, NULL, NULL, CAST(N'2016-01-18 15:40:03.097' AS DateTime), N'5', 7, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[GoldPoint] OFF
SET IDENTITY_INSERT [dbo].[Sale] ON 

INSERT [dbo].[Sale] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [CreateDate], [IsHot], [CategoryId], [CreatedBy], [Lang], [StartDate], [EndDate], [CityId], [CountryId], [IsActive]) VALUES (1, N'test sale', N'aaaa', N'0904591647', N'21.0333333,105.85', N'/Upload/2015/12/259-cardechecklight.PNG', NULL, N'hhrh', N'detal', CAST(N'2015-12-25 17:20:06.250' AS DateTime), 1, 8, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'vi', CAST(N'2015-12-31 14:41:00.000' AS DateTime), CAST(N'2016-02-19 18:41:00.000' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[Sale] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [CreateDate], [IsHot], [CategoryId], [CreatedBy], [Lang], [StartDate], [EndDate], [CityId], [CountryId], [IsActive]) VALUES (10, N'âaaaa', NULL, NULL, N'20.995996, 105.845357', NULL, NULL, NULL, NULL, CAST(N'2016-01-16 03:18:58.073' AS DateTime), 0, 8, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'vi', NULL, CAST(N'2016-01-19 00:00:00.000' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[Sale] ([Id], [Name], [Address], [Mobile], [Location], [ThumbImage], [DetailImage], [ThumbDescription], [DetailDescription], [CreateDate], [IsHot], [CategoryId], [CreatedBy], [Lang], [StartDate], [EndDate], [CityId], [CountryId], [IsActive]) VALUES (11, N'Test promotion 1', N'133 a hi hi', N'1212121212', N'21.0333333,105.85', N'/Upload/2016/1/credit-cards-1.png', N'/Upload/2016/1/460-credit-cards-1.png', N'asdasdasdasda', N'aqraerfsdfasdf ad fasf asd d asdasd', CAST(N'2016-01-17 01:03:47.447' AS DateTime), 1, 9, N'9678e596-87af-44c6-81a2-735b25c3ef96', N'en', CAST(N'2016-01-25 00:00:00.000' AS DateTime), CAST(N'2016-01-25 00:00:00.000' AS DateTime), NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Sale] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_User_Id]    Script Date: 1/21/2016 5:10:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_User_Id] ON [dbo].[AspNetUserClaims]
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 1/21/2016 5:10:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_RoleId]    Script Date: 1/21/2016 5:10:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 1/21/2016 5:10:27 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_AspNetUsers] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_AspNetUsers]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Category]
GO
ALTER TABLE [dbo].[GoldPoint]  WITH CHECK ADD  CONSTRAINT [FK_GoldPoint_AspNetUsers] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[GoldPoint] CHECK CONSTRAINT [FK_GoldPoint_AspNetUsers]
GO
ALTER TABLE [dbo].[GoldPoint]  WITH CHECK ADD  CONSTRAINT [FK_GoldPoint_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[GoldPoint] CHECK CONSTRAINT [FK_GoldPoint_Category]
GO
ALTER TABLE [dbo].[Sale]  WITH CHECK ADD  CONSTRAINT [FK_Sale_AspNetUsers] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Sale] CHECK CONSTRAINT [FK_Sale_AspNetUsers]
GO
ALTER TABLE [dbo].[Sale]  WITH CHECK ADD  CONSTRAINT [FK_Sale_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Sale] CHECK CONSTRAINT [FK_Sale_Category]
GO
USE [master]
GO
ALTER DATABASE [starmap] SET  READ_WRITE 
GO

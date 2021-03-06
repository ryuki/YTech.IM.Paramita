USE [DB_IM_PARAMITA]
GO
/****** Object:  Table [dbo].[M_BRAND]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[M_BRAND](
	[BRAND_ID] [nvarchar](50) NOT NULL,
	[BRAND_NAME] [nvarchar](50) NOT NULL,
	[BRAND_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_BRAND_1] PRIMARY KEY CLUSTERED 
(
	[BRAND_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_MENU_ACCESS]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_MENU_ACCESS](
	[MENU_ACCESS_ID] [nvarchar](50) NOT NULL,
	[USER_NAME] [nvarchar](50) NOT NULL,
	[MENU_ID] [nvarchar](50) NOT NULL,
	[MENU_ACCESS] [bit] NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_MENU_ACCESS_1] PRIMARY KEY CLUSTERED 
(
	[MENU_ACCESS_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_REFERENCE]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_REFERENCE](
	[REFERENCE_ID] [nvarchar](50) NOT NULL,
	[REFERENCE_TYPE] [nvarchar](50) NOT NULL,
	[REFERENCE_VALUE] [nvarchar](50) NULL,
	[REFERENCE_DESC] [nvarchar](50) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
	[REFERENCE_STATUS] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
 CONSTRAINT [PK_T_REFERENCE] PRIMARY KEY CLUSTERED 
(
	[REFERENCE_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_REC_PERIOD]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_REC_PERIOD](
	[REC_PERIOD_ID] [nvarchar](50) NOT NULL,
	[PERIOD_FROM] [datetime] NOT NULL,
	[PERIOD_TO] [datetime] NOT NULL,
	[PERIOD_TYPE] [nvarchar](50) NOT NULL,
	[PERIOD_STATUS] [nvarchar](50) NULL,
	[PERIOD_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_REC_PERIOD] PRIMARY KEY CLUSTERED 
(
	[REC_PERIOD_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[REF_ADDRESS]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[REF_ADDRESS](
	[ADDRESS_ID] [nvarchar](50) NOT NULL,
	[ADDRESS_LINE1] [nvarchar](50) NULL,
	[ADDRESS_LINE2] [nvarchar](50) NULL,
	[ADDRESS_LINE3] [nvarchar](50) NULL,
	[ADDRESS_PHONE] [nvarchar](50) NULL,
	[ADDRESS_FAX] [nvarchar](50) NULL,
	[ADDRESS_CITY] [nvarchar](50) NULL,
	[ADDRESS_CONTACT] [nvarchar](50) NULL,
	[ADDRESS_CONTACT_MOBILE] [nvarchar](50) NULL,
	[ADDRESS_EMAIL] [nvarchar](50) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_ADDRESS] PRIMARY KEY CLUSTERED 
(
	[ADDRESS_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 04/19/2011 16:18:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Split]
(
	@RowData nvarchar(2000),
	@SplitOn nvarchar(5)
)  
RETURNS @RtnValue table 
(
	Id int identity(1,1),
	Data nvarchar(100)
) 
AS  
BEGIN 
	Declare @Cnt int
	Set @Cnt = 1

	While (Charindex(@SplitOn,@RowData)>0)
	Begin
		Insert Into @RtnValue (data)
		Select 
			Data = ltrim(rtrim(Substring(@RowData,1,Charindex(@SplitOn,@RowData)-1)))

		Set @RowData = Substring(@RowData,Charindex(@SplitOn,@RowData)+1,len(@RowData))
		Set @Cnt = @Cnt + 1
	End
	
	Insert Into @RtnValue (data)
	Select Data = ltrim(rtrim(@RowData))

	Return
END
GO
/****** Object:  Table [dbo].[M_ACCOUNT_CAT]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_ACCOUNT_CAT](
	[ACCOUNT_CAT_ID] [nvarchar](50) NOT NULL,
	[ACCOUNT_CAT_NAME] [nvarchar](50) NULL,
	[ACCOUNT_CAT_TYPE] [nvarchar](50) NULL,
	[ACCOUNT_CAT_STATUS] [nvarchar](50) NULL,
	[ACCOUNT_CAT_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_ACCOUNT_CAT] PRIMARY KEY CLUSTERED 
(
	[ACCOUNT_CAT_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[REF_PERSON]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[REF_PERSON](
	[PERSON_ID] [nvarchar](50) NOT NULL,
	[PERSON_FIRST_NAME] [nvarchar](50) NULL,
	[PERSON_LAST_NAME] [nvarchar](50) NULL,
	[PERSON_DOB] [datetime] NULL,
	[PERSON_POB] [nvarchar](50) NULL,
	[PERSON_GENDER] [nvarchar](50) NULL,
	[PERSON_PHONE] [nvarchar](50) NULL,
	[PERSON_MOBILE] [nvarchar](50) NULL,
	[PERSON_EMAIL] [nvarchar](50) NULL,
	[PERSON_RELIGION] [nvarchar](50) NULL,
	[PERSON_RACE] [nvarchar](50) NULL,
	[PERSON_ID_CARD_TYPE] [nvarchar](50) NULL,
	[PERSON_ID_CARD_NO] [nvarchar](50) NULL,
	[PERSON_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [nchar](10) NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_REF_PERSON] PRIMARY KEY CLUSTERED 
(
	[PERSON_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_UNIT]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_UNIT](
	[UNIT_ID] [nvarchar](50) NOT NULL,
	[UNIT_NO] [nvarchar](50) NULL,
	[UNIT_TYPE] [nvarchar](50) NULL,
	[UNIT_LAND_WIDE] [int] NULL,
	[UNIT_WIDE] [int] NULL,
	[UNIT_LOCATION] [nvarchar](50) NULL,
	[UNIT_PRICE] [numeric](18, 5) NULL,
	[UNIT_STATUS] [nvarchar](50) NULL,
	[UNIT_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_UNIT] PRIMARY KEY CLUSTERED 
(
	[UNIT_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[M_JOB_TYPE]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_JOB_TYPE](
	[JOB_TYPE_ID] [nvarchar](50) NOT NULL,
	[JOB_TYPE_NAME] [nvarchar](50) NULL,
	[JOB_TYPE_STATUS] [nvarchar](50) NULL,
	[JOB_TYPE_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_JOB_TYPE] PRIMARY KEY CLUSTERED 
(
	[JOB_TYPE_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[M_DEPARTMENT]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[M_DEPARTMENT](
	[DEPARTMENT_ID] [nvarchar](50) NOT NULL,
	[DEPARTMENT_NAME] [nvarchar](50) NOT NULL,
	[DEPARTMENT_STATUS] [nvarchar](50) NULL,
	[DEPARTMENT_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_DEPARTMENT] PRIMARY KEY CLUSTERED 
(
	[DEPARTMENT_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[M_ITEM_CAT]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_ITEM_CAT](
	[ITEM_CAT_ID] [nvarchar](50) NOT NULL,
	[ITEM_CAT_NAME] [nvarchar](50) NOT NULL,
	[ITEM_CAT_DESC] [nvarchar](50) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_ITEM_CAT] PRIMARY KEY CLUSTERED 
(
	[ITEM_CAT_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_TRANS]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_TRANS](
	[TRANS_ID] [nvarchar](50) NOT NULL,
	[WAREHOUSE_ID] [nvarchar](50) NULL,
	[WAREHOUSE_ID_TO] [nvarchar](50) NULL,
	[TRANS_DATE] [datetime] NULL,
	[TRANS_BY] [nvarchar](50) NULL,
	[TRANS_FACTUR] [nvarchar](50) NULL,
	[EMPLOYEE_ID] [nvarchar](50) NULL,
	[TRANS_DUE_DATE] [datetime] NULL,
	[TRANS_PAYMENT_METHOD] [nvarchar](50) NULL,
	[TRANS_SUB_TOTAL] [numeric](18, 5) NULL,
	[TRANS_DISC] [numeric](18, 5) NULL,
	[TRANS_TAX] [numeric](18, 5) NULL,
	[TRANS_STATUS] [nvarchar](50) NULL,
	[TRANS_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_TRANS_1] PRIMARY KEY CLUSTERED 
(
	[TRANS_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[M_COST_CENTER]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_COST_CENTER](
	[COST_CENTER_ID] [nvarchar](50) NOT NULL,
	[EMPLOYEE_ID] [nvarchar](50) NULL,
	[COST_CENTER_NAME] [nvarchar](50) NULL,
	[COST_CENTER_TOTAL_BUDGET] [numeric](18, 5) NULL,
	[COST_CENTER_STATUS] [nvarchar](50) NULL,
	[COST_CENTER_START_DATE] [datetime] NULL,
	[COST_CENTER_END_DATE] [datetime] NULL,
	[COST_CENTER_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_COST_CENTER] PRIMARY KEY CLUSTERED 
(
	[COST_CENTER_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[M_WAREHOUSE]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_WAREHOUSE](
	[WAREHOUSE_ID] [nvarchar](50) NOT NULL,
	[WAREHOUSE_NAME] [nvarchar](50) NOT NULL,
	[ADDRESS_ID] [nvarchar](50) NULL,
	[COST_CENTER_ID] [nvarchar](50) NULL,
	[WAREHOUSE_TYPE] [nvarchar](50) NULL,
	[EMPLOYEE_ID] [nvarchar](50) NULL,
	[WAREHOUSE_STATUS] [nvarchar](50) NULL,
	[WAREHOUSE_IS_DEFAULT] [bit] NULL,
	[WAREHOUSE_PHOTO] [image] NULL,
	[WAREHOUSE_DESC] [nvarchar](50) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_WAREHOUSE_1] PRIMARY KEY CLUSTERED 
(
	[WAREHOUSE_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_TRANS_DET]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_TRANS_DET](
	[TRANS_DET_ID] [nvarchar](50) NOT NULL,
	[TRANS_ID] [nvarchar](50) NOT NULL,
	[ITEM_ID] [nvarchar](50) NOT NULL,
	[ITEM_UOM_ID] [nvarchar](50) NULL,
	[TRANS_DET_NO] [int] NULL,
	[TRANS_DET_QTY] [numeric](18, 5) NULL,
	[TRANS_DET_PRICE] [numeric](18, 5) NULL,
	[TRANS_DET_DISC] [numeric](18, 5) NULL,
	[TRANS_DET_TOTAL] [numeric](18, 5) NULL,
	[TRANS_DET_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_TRANS_DET_1] PRIMARY KEY CLUSTERED 
(
	[TRANS_DET_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[M_ITEM]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_ITEM](
	[ITEM_ID] [nvarchar](50) NOT NULL,
	[ITEM_CAT_ID] [nvarchar](50) NULL,
	[BRAND_ID] [nvarchar](50) NULL,
	[ITEM_NAME] [nvarchar](50) NOT NULL,
	[ITEM_STATUS] [nvarchar](50) NULL,
	[ITEM_PHOTO] [image] NULL,
	[ITEM_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_ITEM] PRIMARY KEY CLUSTERED 
(
	[ITEM_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_JOURNAL]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_JOURNAL](
	[JOURNAL_ID] [nvarchar](50) NOT NULL,
	[COST_CENTER_ID] [nvarchar](50) NULL,
	[JOURNAL_TYPE] [nvarchar](50) NULL,
	[JOURNAL_VOUCHER_NO] [nvarchar](50) NULL,
	[JOURNAL_PIC] [nvarchar](50) NULL,
	[JOURNAL_DATE] [datetime] NULL,
	[JOURNAL_EVIDENCE_NO] [nvarchar](50) NULL,
	[JOURNAL_AMMOUNT] [numeric](18, 5) NULL,
	[JOURNAL_STATUS] [nvarchar](50) NULL,
	[JOURNAL_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_JOURNAL] PRIMARY KEY CLUSTERED 
(
	[JOURNAL_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_TRANS_UNIT]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_TRANS_UNIT](
	[TRANS_UNIT_ID] [nvarchar](50) NOT NULL,
	[TRANS_UNIT_FACTUR] [nvarchar](50) NULL,
	[UNIT_ID] [nvarchar](50) NULL,
	[CUSTOMER_ID] [nvarchar](50) NULL,
	[TRANS_UNIT_DATE] [datetime] NULL,
	[TRANS_UNIT_PRICE] [numeric](18, 5) NULL,
	[TRANS_UNIT_STATUS] [nvarchar](50) NULL,
	[TRANS_UNIT_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
	[COST_CENTER_ID] [nvarchar](50) NULL,
	[TRANS_UNIT_PAYMENT_METHOD] [nvarchar](50) NULL,
 CONSTRAINT [PK_T_TRANS_UNIT] PRIMARY KEY CLUSTERED 
(
	[TRANS_UNIT_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_REAL]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_REAL](
	[REAL_ID] [nvarchar](50) NOT NULL,
	[REAL_DATE] [datetime] NULL,
	[COST_CENTER_ID] [nvarchar](50) NULL,
	[REAL_PERCENT_VALUE] [numeric](18, 5) NULL,
	[REAL_STATUS] [nvarchar](50) NULL,
	[REAL_DESC] [nvarchar](50) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_REAL] PRIMARY KEY CLUSTERED 
(
	[REAL_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[M_PRODUCT]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_PRODUCT](
	[PRODUCT_ID] [nvarchar](50) NOT NULL,
	[COST_CENTER_ID] [nvarchar](50) NULL,
	[PRODUCT_NAME] [nvarchar](50) NULL,
	[PRODUCT_QTY] [numeric](18, 5) NULL,
	[PRODUCT_STATUS] [nvarchar](50) NULL,
	[PRODUCT_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_PRODUCT] PRIMARY KEY CLUSTERED 
(
	[PRODUCT_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_REC_ACCOUNT]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_REC_ACCOUNT](
	[REC_ACCOUNT_ID] [nvarchar](50) NOT NULL,
	[REC_PERIOD_ID] [nvarchar](50) NOT NULL,
	[COST_CENTER_ID] [nvarchar](50) NULL,
	[ACCOUNT_ID] [nvarchar](50) NOT NULL,
	[ACCOUNT_STATUS] [nvarchar](50) NULL,
	[REC_ACCOUNT_START] [numeric](18, 5) NULL,
	[REC_ACCOUNT_END] [numeric](18, 5) NULL,
	[REC_ACCOUNT_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_REC_ACCOUNT] PRIMARY KEY CLUSTERED 
(
	[REC_ACCOUNT_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_STOCK]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_STOCK](
	[STOCK_ID] [nvarchar](50) NOT NULL,
	[ITEM_ID] [nvarchar](50) NOT NULL,
	[WAREHOUSE_ID] [nvarchar](50) NOT NULL,
	[TRANS_DET_ID] [nvarchar](50) NOT NULL,
	[STOCK_DATE] [datetime] NULL,
	[STOCK_QTY] [numeric](18, 5) NULL,
	[STOCK_PRICE] [numeric](18, 5) NULL,
	[STOCK_STATUS] [nvarchar](50) NULL,
	[STOCK_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_STOCK_1] PRIMARY KEY CLUSTERED 
(
	[STOCK_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_STOCK_CARD]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_STOCK_CARD](
	[STOCK_CARD_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[ITEM_ID] [nvarchar](50) NOT NULL,
	[WAREHOUSE_ID] [nvarchar](50) NOT NULL,
	[TRANS_DET_ID] [nvarchar](50) NULL,
	[STOCK_CARD_DATE] [datetime] NULL,
	[STOCK_CARD_STATUS] [bit] NULL,
	[STOCK_CARD_QTY] [numeric](18, 5) NULL,
	[STOCK_CARD_SALDO] [numeric](18, 5) NULL,
	[STOCK_CARD_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_STOCK_CARD_1] PRIMARY KEY CLUSTERED 
(
	[STOCK_CARD_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_STOCK_ITEM]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_STOCK_ITEM](
	[ITEM_ID] [nvarchar](50) NOT NULL,
	[WAREHOUSE_ID] [nvarchar](50) NOT NULL,
	[ITEM_STOCK_MAX] [numeric](18, 5) NULL,
	[ITEM_STOCK_MIN] [numeric](18, 5) NULL,
	[ITEM_STOCK] [numeric](18, 5) NULL,
	[ITEM_STOCK_RACK] [nvarchar](50) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
	[STOCK_ITEM_ID] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_T_STOCK_ITEM] PRIMARY KEY CLUSTERED 
(
	[STOCK_ITEM_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_STOCK_REF]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_STOCK_REF](
	[STOCK_ID] [nvarchar](50) NOT NULL,
	[STOCK_REF_ID] [nvarchar](50) NOT NULL,
	[STOCK_REF_QTY] [numeric](18, 5) NULL,
	[STOCK_REF_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
	[STOCK_REF_DATE] [datetime] NULL,
	[STOCK_REF_PRICE] [numeric](18, 5) NULL,
	[STOCK_REF_STATUS] [nvarchar](50) NULL,
	[TRANS_DET_ID] [nvarchar](50) NULL,
 CONSTRAINT [PK_T_STOCK_REF] PRIMARY KEY CLUSTERED 
(
	[STOCK_REF_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_TRANS_REF]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_TRANS_REF](
	[TRANS_ID] [nvarchar](50) NOT NULL,
	[TRANS_ID_REF] [nvarchar](50) NOT NULL,
	[TRANS_REF_STATUS] [nvarchar](50) NULL,
	[TRANS_REF_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
	[TRANS_REF_ID] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_T_TRANS_REF] PRIMARY KEY CLUSTERED 
(
	[TRANS_REF_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[M_ITEM_UOM]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[M_ITEM_UOM](
	[ITEM_UOM_ID] [nvarchar](50) NOT NULL,
	[ITEM_ID] [nvarchar](50) NOT NULL,
	[ITEM_UOM_NAME] [nvarchar](50) NULL,
	[ITEM_UOM_REF_ID] [nvarchar](50) NULL,
	[ITEM_UOM_CONVERTER_VALUE] [numeric](18, 5) NULL,
	[ITEM_UOM_SALE_PRICE] [numeric](18, 5) NULL,
	[ITEM_UOM_PURCHASE_PRICE] [numeric](18, 5) NULL,
	[ITEM_UOM_HPP_PRICE] [numeric](18, 5) NULL,
	[ITEM_UOM_DESC] [nvarchar](50) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_ITEM_UOM_1] PRIMARY KEY CLUSTERED 
(
	[ITEM_UOM_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[M_CUSTOMER]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_CUSTOMER](
	[CUSTOMER_ID] [nvarchar](50) NOT NULL,
	[PERSON_ID] [nvarchar](50) NULL,
	[ADDRESS_ID] [nvarchar](50) NULL,
	[CUSTOMER_STATUS] [nvarchar](50) NULL,
	[CUSTOMER_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_CUSTOMER] PRIMARY KEY CLUSTERED 
(
	[CUSTOMER_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[M_SUPPLIER]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_SUPPLIER](
	[SUPPLIER_ID] [nvarchar](50) NOT NULL,
	[SUPPLIER_NAME] [nvarchar](50) NOT NULL,
	[ADDRESS_ID] [nvarchar](50) NULL,
	[SUPPLIER_STATUS] [nvarchar](50) NULL,
	[SUPPLIER_MAX_DEBT] [numeric](18, 5) NULL,
	[SUPPLIER_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_SUPPLIER] PRIMARY KEY CLUSTERED 
(
	[SUPPLIER_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[M_ACCOUNT]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_ACCOUNT](
	[ACCOUNT_ID] [nvarchar](50) NOT NULL,
	[ACCOUNT_CAT_ID] [nvarchar](50) NULL,
	[ACCOUNT_PARENT_ID] [nvarchar](50) NULL,
	[ACCOUNT_STATUS] [nvarchar](50) NULL,
	[ACCOUNT_NAME] [nvarchar](100) NULL,
	[ACCOUNT_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_ACCOUNT] PRIMARY KEY CLUSTERED 
(
	[ACCOUNT_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[M_EMPLOYEE]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_EMPLOYEE](
	[EMPLOYEE_ID] [nvarchar](50) NOT NULL,
	[PERSON_ID] [nvarchar](50) NULL,
	[DEPARTMENT_ID] [nvarchar](50) NULL,
	[EMPLOYEE_STATUS] [nvarchar](50) NULL,
	[EMPLOYEE_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_EMPLOYEE] PRIMARY KEY CLUSTERED 
(
	[EMPLOYEE_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_JOURNAL_DET]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_JOURNAL_DET](
	[JOURNAL_DET_ID] [nvarchar](50) NOT NULL,
	[JOURNAL_ID] [nvarchar](50) NOT NULL,
	[ACCOUNT_ID] [nvarchar](50) NOT NULL,
	[JOURNAL_DET_NO] [int] NULL,
	[JOURNAL_DET_STATUS] [nvarchar](50) NULL,
	[JOURNAL_DET_AMMOUNT] [numeric](18, 5) NULL,
	[JOURNAL_DET_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
	[JOURNAL_DET_EVIDENCE_NO] [nvarchar](50) NULL,
 CONSTRAINT [PK_T_JOURNAL_DET] PRIMARY KEY CLUSTERED 
(
	[JOURNAL_DET_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_JOURNAL_REF]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_JOURNAL_REF](
	[JOURNAL_REF_ID] [nvarchar](50) NOT NULL,
	[REFERENCE_TABLE] [nvarchar](50) NOT NULL,
	[REFERENCE_TYPE] [nvarchar](50) NOT NULL,
	[REFERENCE_ID] [nvarchar](50) NOT NULL,
	[JOURNAL_ID] [nvarchar](50) NOT NULL,
	[JOURNAL_REF_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_JOURNAL_REF] PRIMARY KEY CLUSTERED 
(
	[JOURNAL_REF_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[M_ACCOUNT_REF]    Script Date: 04/19/2011 16:18:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_ACCOUNT_REF](
	[ACCOUNT_REF_ID] [nvarchar](50) NOT NULL,
	[REFERENCE_TABLE] [nvarchar](50) NOT NULL,
	[REFERENCE_TYPE] [nvarchar](50) NOT NULL,
	[REFERENCE_ID] [nvarchar](50) NOT NULL,
	[ACCOUNT_ID] [nvarchar](50) NOT NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_ACCOUNT_REF] PRIMARY KEY CLUSTERED 
(
	[ACCOUNT_REF_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_REC_ACCOUNT]    Script Date: 04/19/2011 16:18:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_UPDATE_REC_ACCOUNT]
  @period_id AS nvarchar(50)
, @cost_center_id AS nvarchar(50)
, @account_id AS nvarchar(50)
, @ammount AS numeric
   AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--search parent of account by recursive select
	with ctechild(COST_CENTER_ID, ACCOUNT_ID, ACCOUNT_PARENT_ID, ACCOUNT_LEVEL) as 
	( 
		select r.COST_CENTER_ID, acc.ACCOUNT_ID, acc.ACCOUNT_PARENT_ID, 0 as ACCOUNT_LEVEL
		from dbo.T_REC_ACCOUNT r  inner join dbo.M_ACCOUNT acc on r.ACCOUNT_ID = acc.ACCOUNT_ID 
		where r.ACCOUNT_ID = @account_id
			and r.COST_CENTER_ID = @cost_center_id
			and r.REC_PERIOD_ID = @period_id
		union all
		select r.COST_CENTER_ID, acc.ACCOUNT_ID,acc.ACCOUNT_PARENT_ID, c.ACCOUNT_LEVEL+1
		from dbo.T_REC_ACCOUNT r inner join dbo.M_ACCOUNT acc on r.ACCOUNT_ID = acc.ACCOUNT_ID 
		inner join ctechild c
			on c.ACCOUNT_PARENT_ID = acc.ACCOUNT_ID
		where r.COST_CENTER_ID = @cost_center_id
			and r.REC_PERIOD_ID = @period_id
	)
	--update m_account and his parents, add balance if status = debet
	update dbo.T_REC_ACCOUNT
	set REC_ACCOUNT_END = REC_ACCOUNT_END + @ammount
	from ctechild i, dbo.T_REC_ACCOUNT a
	where i.COST_CENTER_ID = a.COST_CENTER_ID
		and i.ACCOUNT_ID = a.ACCOUNT_ID
		and a.REC_PERIOD_ID = @period_id;

END
GO
/****** Object:  StoredProcedure [dbo].[SP_CLOSING]    Script Date: 04/19/2011 16:18:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CLOSING]
@periodId nvarchar(50),
@periodType nvarchar(50),
@periodFrom datetime,
@periodTo datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

--get last recap account
with cteLastRecapAccount(ACCOUNT_ID, REC_ACCOUNT_END) as
(
	select r.ACCOUNT_ID 
		, r.REC_ACCOUNT_END
	from dbo.T_REC_ACCOUNT r
	where r.REC_PERIOD_ID = (select top 1 p.REC_PERIOD_ID from dbo.T_REC_ACCOUNT t, dbo.T_REC_PERIOD p
where t.REC_PERIOD_ID=p.REC_PERIOD_ID and p.PERIOD_TO < @periodFrom and p.PERIOD_TYPE = @periodType)
)

--insert recap account
insert into dbo.T_REC_ACCOUNT
(
	REC_ACCOUNT_ID
	, REC_PERIOD_ID
	, COST_CENTER_ID
	, ACCOUNT_ID
	, ACCOUNT_STATUS 
	, REC_ACCOUNT_START
	, REC_ACCOUNT_END
	, REC_ACCOUNT_DESC
	, DATA_STATUS
	, CREATED_BY
	, CREATED_DATE
)

select cast(newid() as nvarchar(50)) REC_ACCOUNT_ID
	, @periodId REC_PERIOD_ID
	, a.COST_CENTER_ID COST_CENTER_ID
	, a.ACCOUNT_ID ACCOUNT_ID
	, null ACCOUNT_STATUS
	, 0 REC_ACCOUNT_START
	, 0 REC_ACCOUNT_END
	, null REC_ACCOUNT_DESC
	, 'NEW' DATA_STATUS
	, '' CREATED_BY
	, getdate() CREATED_DATE
from (
		select cost.COST_CENTER_ID, acc.ACCOUNT_ID
		from dbo.M_ACCOUNT acc 
			inner join dbo.M_COST_CENTER cost 
				on 1=1
		where acc.ACCOUNT_CAT_ID in 
			(
				SELECT ACCOUNT_CAT_ID FROM dbo.M_ACCOUNT_CAT WHERE ACCOUNT_CAT_TYPE = 'LR'
			)
	) a

union all

select cast(newid() as nvarchar(50)) REC_ACCOUNT_ID
	, @periodId REC_PERIOD_ID
	, a.COST_CENTER_ID COST_CENTER_ID
	, a.ACCOUNT_ID ACCOUNT_ID
	, null ACCOUNT_STATUS
	, isnull(a.REC_ACCOUNT_END,0) REC_ACCOUNT_START
	, isnull(a.REC_ACCOUNT_END,0) REC_ACCOUNT_END
	, null REC_ACCOUNT_DESC
	, 'NEW' DATA_STATUS
	, '' CREATED_BY
	, getdate() CREATED_DATE
from (
		select cost.COST_CENTER_ID, acc.ACCOUNT_ID, cte.REC_ACCOUNT_END
		from dbo.M_ACCOUNT acc 
			inner join dbo.M_COST_CENTER cost 
				on 1=1
			left join cteLastRecapAccount cte
				on cte.account_id = acc.account_id
		where acc.ACCOUNT_CAT_ID in 
			(
				SELECT ACCOUNT_CAT_ID FROM dbo.M_ACCOUNT_CAT WHERE ACCOUNT_CAT_TYPE = 'NERACA'
			)
	) a
;

declare @vaccount_id as nvarchar(50);
declare @vcost_center_id as nvarchar(50);
declare @vsaldo as numeric ;
declare account_cursor cursor for
		select a.COST_CENTER_ID
			, a.ACCOUNT_ID 
			, sum(a.jurnal) as SALDO
		from (
			select  j.COST_CENTER_ID
				, det.ACCOUNT_ID 
				, case det.JOURNAL_DET_STATUS
					when 'D' then sum(det.JOURNAL_DET_AMMOUNT)
					when 'K' then sum(det.JOURNAL_DET_AMMOUNT*-1)
				  end jurnal
			from dbo.T_JOURNAL j, dbo.T_JOURNAL_DET det
			where j.JOURNAL_ID = det.JOURNAL_ID 
				and j.JOURNAL_DATE between @periodFrom and @periodTo
			group by  j.COST_CENTER_ID
				, det.ACCOUNT_ID 
				, det.JOURNAL_DET_STATUS
		) a
		group by  a.COST_CENTER_ID, a.ACCOUNT_ID

OPEN account_cursor 
		FETCH NEXT FROM account_cursor INTO @vcost_center_id, @vaccount_id, @vsaldo
		--Fetch next record
		WHILE @@FETCH_STATUS = 0
		BEGIN

exec [SP_UPDATE_REC_ACCOUNT]
	@period_id = @periodId
	, @cost_center_id = @vcost_center_id
	, @account_id = @vaccount_id
	, @ammount = @vsaldo

			FETCH NEXT FROM account_cursor INTO @vcost_center_id, @vaccount_id, @vsaldo
END

CLOSE account_cursor --Close cursor
DEALLOCATE account_cursor --Deallocate cursor


--insert RL account
declare @account_rl_id nvarchar(50);
declare @lr as numeric ;
select @account_rl_id = REFERENCE_VALUE
from dbo.T_REFERENCE r where r.REFERENCE_TYPE = 'LRDitahanAccountId';
declare lr_cursor cursor for
		select	t.COST_CENTER_ID
			, case det.JOURNAL_DET_STATUS
					when 'D' then sum(det.JOURNAL_DET_AMMOUNT)
					when 'K' then sum(det.JOURNAL_DET_AMMOUNT*-1)
				end lr
		from dbo.T_JOURNAL t
			inner join dbo.T_JOURNAL_DET det
				on t.JOURNAL_ID = det.JOURNAL_ID
			inner join dbo.M_ACCOUNT acc 
				on det.ACCOUNT_ID = acc.ACCOUNT_ID
		where t.JOURNAL_DATE between @periodFrom and @periodTo
			and acc.ACCOUNT_CAT_ID in 
			(
				SELECT ACCOUNT_CAT_ID FROM dbo.M_ACCOUNT_CAT WHERE ACCOUNT_CAT_TYPE = 'LR'
			)
		group by t.COST_CENTER_ID, det.JOURNAL_DET_STATUS


OPEN lr_cursor 
		FETCH NEXT FROM lr_cursor INTO @vcost_center_id, @lr
		--Fetch next record
		WHILE @@FETCH_STATUS = 0
		BEGIN

	exec [SP_UPDATE_REC_ACCOUNT]
	@period_id = @periodId
	, @cost_center_id = @vcost_center_id
	, @account_id = @account_rl_id
	, @ammount = @lr

			FETCH NEXT FROM lr_cursor INTO @vcost_center_id,  @lr
END

CLOSE lr_cursor --Close cursor
DEALLOCATE lr_cursor --Deallocate cursor

select @lr as RL
END
GO
/****** Object:  ForeignKey [FK_M_ACCOUNT_M_ACCOUNT]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_ACCOUNT]  WITH CHECK ADD  CONSTRAINT [FK_M_ACCOUNT_M_ACCOUNT] FOREIGN KEY([ACCOUNT_PARENT_ID])
REFERENCES [dbo].[M_ACCOUNT] ([ACCOUNT_ID])
GO
ALTER TABLE [dbo].[M_ACCOUNT] CHECK CONSTRAINT [FK_M_ACCOUNT_M_ACCOUNT]
GO
/****** Object:  ForeignKey [FK_M_ACCOUNT_M_ACCOUNT_CAT]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_ACCOUNT]  WITH CHECK ADD  CONSTRAINT [FK_M_ACCOUNT_M_ACCOUNT_CAT] FOREIGN KEY([ACCOUNT_CAT_ID])
REFERENCES [dbo].[M_ACCOUNT_CAT] ([ACCOUNT_CAT_ID])
GO
ALTER TABLE [dbo].[M_ACCOUNT] CHECK CONSTRAINT [FK_M_ACCOUNT_M_ACCOUNT_CAT]
GO
/****** Object:  ForeignKey [FK_M_ACCOUNT_REF_M_ACCOUNT]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_ACCOUNT_REF]  WITH CHECK ADD  CONSTRAINT [FK_M_ACCOUNT_REF_M_ACCOUNT] FOREIGN KEY([ACCOUNT_ID])
REFERENCES [dbo].[M_ACCOUNT] ([ACCOUNT_ID])
GO
ALTER TABLE [dbo].[M_ACCOUNT_REF] CHECK CONSTRAINT [FK_M_ACCOUNT_REF_M_ACCOUNT]
GO
/****** Object:  ForeignKey [FK_M_COST_CENTER_M_EMPLOYEE]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_COST_CENTER]  WITH CHECK ADD  CONSTRAINT [FK_M_COST_CENTER_M_EMPLOYEE] FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[M_EMPLOYEE] ([EMPLOYEE_ID])
GO
ALTER TABLE [dbo].[M_COST_CENTER] CHECK CONSTRAINT [FK_M_COST_CENTER_M_EMPLOYEE]
GO
/****** Object:  ForeignKey [FK_M_CUSTOMER_REF_ADDRESS]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_CUSTOMER]  WITH CHECK ADD  CONSTRAINT [FK_M_CUSTOMER_REF_ADDRESS] FOREIGN KEY([ADDRESS_ID])
REFERENCES [dbo].[REF_ADDRESS] ([ADDRESS_ID])
GO
ALTER TABLE [dbo].[M_CUSTOMER] CHECK CONSTRAINT [FK_M_CUSTOMER_REF_ADDRESS]
GO
/****** Object:  ForeignKey [FK_M_CUSTOMER_REF_PERSON]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_CUSTOMER]  WITH CHECK ADD  CONSTRAINT [FK_M_CUSTOMER_REF_PERSON] FOREIGN KEY([PERSON_ID])
REFERENCES [dbo].[REF_PERSON] ([PERSON_ID])
GO
ALTER TABLE [dbo].[M_CUSTOMER] CHECK CONSTRAINT [FK_M_CUSTOMER_REF_PERSON]
GO
/****** Object:  ForeignKey [FK_M_EMPLOYEE_M_DEPARTMENT]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_EMPLOYEE]  WITH CHECK ADD  CONSTRAINT [FK_M_EMPLOYEE_M_DEPARTMENT] FOREIGN KEY([DEPARTMENT_ID])
REFERENCES [dbo].[M_DEPARTMENT] ([DEPARTMENT_ID])
GO
ALTER TABLE [dbo].[M_EMPLOYEE] CHECK CONSTRAINT [FK_M_EMPLOYEE_M_DEPARTMENT]
GO
/****** Object:  ForeignKey [FK_M_EMPLOYEE_REF_PERSON]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_EMPLOYEE]  WITH CHECK ADD  CONSTRAINT [FK_M_EMPLOYEE_REF_PERSON] FOREIGN KEY([PERSON_ID])
REFERENCES [dbo].[REF_PERSON] ([PERSON_ID])
GO
ALTER TABLE [dbo].[M_EMPLOYEE] CHECK CONSTRAINT [FK_M_EMPLOYEE_REF_PERSON]
GO
/****** Object:  ForeignKey [FK_M_ITEM_M_BRAND]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_ITEM]  WITH CHECK ADD  CONSTRAINT [FK_M_ITEM_M_BRAND] FOREIGN KEY([BRAND_ID])
REFERENCES [dbo].[M_BRAND] ([BRAND_ID])
GO
ALTER TABLE [dbo].[M_ITEM] CHECK CONSTRAINT [FK_M_ITEM_M_BRAND]
GO
/****** Object:  ForeignKey [FK_M_ITEM_M_ITEM_CAT]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_ITEM]  WITH CHECK ADD  CONSTRAINT [FK_M_ITEM_M_ITEM_CAT] FOREIGN KEY([ITEM_CAT_ID])
REFERENCES [dbo].[M_ITEM_CAT] ([ITEM_CAT_ID])
GO
ALTER TABLE [dbo].[M_ITEM] CHECK CONSTRAINT [FK_M_ITEM_M_ITEM_CAT]
GO
/****** Object:  ForeignKey [FK_M_ITEM_UOM_M_ITEM]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_ITEM_UOM]  WITH CHECK ADD  CONSTRAINT [FK_M_ITEM_UOM_M_ITEM] FOREIGN KEY([ITEM_ID])
REFERENCES [dbo].[M_ITEM] ([ITEM_ID])
GO
ALTER TABLE [dbo].[M_ITEM_UOM] CHECK CONSTRAINT [FK_M_ITEM_UOM_M_ITEM]
GO
/****** Object:  ForeignKey [FK_M_ITEM_UOM_M_ITEM_UOM]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_ITEM_UOM]  WITH CHECK ADD  CONSTRAINT [FK_M_ITEM_UOM_M_ITEM_UOM] FOREIGN KEY([ITEM_UOM_REF_ID])
REFERENCES [dbo].[M_ITEM_UOM] ([ITEM_UOM_ID])
GO
ALTER TABLE [dbo].[M_ITEM_UOM] CHECK CONSTRAINT [FK_M_ITEM_UOM_M_ITEM_UOM]
GO
/****** Object:  ForeignKey [FK_M_PRODUCT_M_COST_CENTER]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_PRODUCT]  WITH CHECK ADD  CONSTRAINT [FK_M_PRODUCT_M_COST_CENTER] FOREIGN KEY([COST_CENTER_ID])
REFERENCES [dbo].[M_COST_CENTER] ([COST_CENTER_ID])
GO
ALTER TABLE [dbo].[M_PRODUCT] CHECK CONSTRAINT [FK_M_PRODUCT_M_COST_CENTER]
GO
/****** Object:  ForeignKey [FK_M_SUPPLIER_REF_ADDRESS]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_SUPPLIER]  WITH CHECK ADD  CONSTRAINT [FK_M_SUPPLIER_REF_ADDRESS] FOREIGN KEY([ADDRESS_ID])
REFERENCES [dbo].[REF_ADDRESS] ([ADDRESS_ID])
GO
ALTER TABLE [dbo].[M_SUPPLIER] CHECK CONSTRAINT [FK_M_SUPPLIER_REF_ADDRESS]
GO
/****** Object:  ForeignKey [FK_M_WAREHOUSE_M_COST_CENTER]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_WAREHOUSE]  WITH CHECK ADD  CONSTRAINT [FK_M_WAREHOUSE_M_COST_CENTER] FOREIGN KEY([COST_CENTER_ID])
REFERENCES [dbo].[M_COST_CENTER] ([COST_CENTER_ID])
GO
ALTER TABLE [dbo].[M_WAREHOUSE] CHECK CONSTRAINT [FK_M_WAREHOUSE_M_COST_CENTER]
GO
/****** Object:  ForeignKey [FK_M_WAREHOUSE_M_EMPLOYEE]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_WAREHOUSE]  WITH CHECK ADD  CONSTRAINT [FK_M_WAREHOUSE_M_EMPLOYEE] FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[M_EMPLOYEE] ([EMPLOYEE_ID])
GO
ALTER TABLE [dbo].[M_WAREHOUSE] CHECK CONSTRAINT [FK_M_WAREHOUSE_M_EMPLOYEE]
GO
/****** Object:  ForeignKey [FK_M_WAREHOUSE_REF_ADDRESS]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[M_WAREHOUSE]  WITH CHECK ADD  CONSTRAINT [FK_M_WAREHOUSE_REF_ADDRESS] FOREIGN KEY([ADDRESS_ID])
REFERENCES [dbo].[REF_ADDRESS] ([ADDRESS_ID])
GO
ALTER TABLE [dbo].[M_WAREHOUSE] CHECK CONSTRAINT [FK_M_WAREHOUSE_REF_ADDRESS]
GO
/****** Object:  ForeignKey [FK_T_JOURNAL_M_COST_CENTER]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_JOURNAL]  WITH CHECK ADD  CONSTRAINT [FK_T_JOURNAL_M_COST_CENTER] FOREIGN KEY([COST_CENTER_ID])
REFERENCES [dbo].[M_COST_CENTER] ([COST_CENTER_ID])
GO
ALTER TABLE [dbo].[T_JOURNAL] CHECK CONSTRAINT [FK_T_JOURNAL_M_COST_CENTER]
GO
/****** Object:  ForeignKey [FK_T_JOURNAL_DET_M_ACCOUNT]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_JOURNAL_DET]  WITH CHECK ADD  CONSTRAINT [FK_T_JOURNAL_DET_M_ACCOUNT] FOREIGN KEY([ACCOUNT_ID])
REFERENCES [dbo].[M_ACCOUNT] ([ACCOUNT_ID])
GO
ALTER TABLE [dbo].[T_JOURNAL_DET] CHECK CONSTRAINT [FK_T_JOURNAL_DET_M_ACCOUNT]
GO
/****** Object:  ForeignKey [FK_T_JOURNAL_DET_T_JOURNAL]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_JOURNAL_DET]  WITH CHECK ADD  CONSTRAINT [FK_T_JOURNAL_DET_T_JOURNAL] FOREIGN KEY([JOURNAL_ID])
REFERENCES [dbo].[T_JOURNAL] ([JOURNAL_ID])
GO
ALTER TABLE [dbo].[T_JOURNAL_DET] CHECK CONSTRAINT [FK_T_JOURNAL_DET_T_JOURNAL]
GO
/****** Object:  ForeignKey [FK_T_JOURNAL_REF_T_JOURNAL]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_JOURNAL_REF]  WITH CHECK ADD  CONSTRAINT [FK_T_JOURNAL_REF_T_JOURNAL] FOREIGN KEY([JOURNAL_ID])
REFERENCES [dbo].[T_JOURNAL] ([JOURNAL_ID])
GO
ALTER TABLE [dbo].[T_JOURNAL_REF] CHECK CONSTRAINT [FK_T_JOURNAL_REF_T_JOURNAL]
GO
/****** Object:  ForeignKey [FK_T_REAL_M_COST_CENTER]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_REAL]  WITH CHECK ADD  CONSTRAINT [FK_T_REAL_M_COST_CENTER] FOREIGN KEY([COST_CENTER_ID])
REFERENCES [dbo].[M_COST_CENTER] ([COST_CENTER_ID])
GO
ALTER TABLE [dbo].[T_REAL] CHECK CONSTRAINT [FK_T_REAL_M_COST_CENTER]
GO
/****** Object:  ForeignKey [FK_T_REC_ACCOUNT_M_ACCOUNT]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_REC_ACCOUNT]  WITH CHECK ADD  CONSTRAINT [FK_T_REC_ACCOUNT_M_ACCOUNT] FOREIGN KEY([ACCOUNT_ID])
REFERENCES [dbo].[M_ACCOUNT] ([ACCOUNT_ID])
GO
ALTER TABLE [dbo].[T_REC_ACCOUNT] CHECK CONSTRAINT [FK_T_REC_ACCOUNT_M_ACCOUNT]
GO
/****** Object:  ForeignKey [FK_T_REC_ACCOUNT_M_COST_CENTER]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_REC_ACCOUNT]  WITH CHECK ADD  CONSTRAINT [FK_T_REC_ACCOUNT_M_COST_CENTER] FOREIGN KEY([COST_CENTER_ID])
REFERENCES [dbo].[M_COST_CENTER] ([COST_CENTER_ID])
GO
ALTER TABLE [dbo].[T_REC_ACCOUNT] CHECK CONSTRAINT [FK_T_REC_ACCOUNT_M_COST_CENTER]
GO
/****** Object:  ForeignKey [FK_T_REC_ACCOUNT_T_REC_PERIOD]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_REC_ACCOUNT]  WITH CHECK ADD  CONSTRAINT [FK_T_REC_ACCOUNT_T_REC_PERIOD] FOREIGN KEY([REC_PERIOD_ID])
REFERENCES [dbo].[T_REC_PERIOD] ([REC_PERIOD_ID])
GO
ALTER TABLE [dbo].[T_REC_ACCOUNT] CHECK CONSTRAINT [FK_T_REC_ACCOUNT_T_REC_PERIOD]
GO
/****** Object:  ForeignKey [FK_T_STOCK_M_ITEM]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_STOCK]  WITH CHECK ADD  CONSTRAINT [FK_T_STOCK_M_ITEM] FOREIGN KEY([ITEM_ID])
REFERENCES [dbo].[M_ITEM] ([ITEM_ID])
GO
ALTER TABLE [dbo].[T_STOCK] CHECK CONSTRAINT [FK_T_STOCK_M_ITEM]
GO
/****** Object:  ForeignKey [FK_T_STOCK_M_WAREHOUSE]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_STOCK]  WITH CHECK ADD  CONSTRAINT [FK_T_STOCK_M_WAREHOUSE] FOREIGN KEY([WAREHOUSE_ID])
REFERENCES [dbo].[M_WAREHOUSE] ([WAREHOUSE_ID])
GO
ALTER TABLE [dbo].[T_STOCK] CHECK CONSTRAINT [FK_T_STOCK_M_WAREHOUSE]
GO
/****** Object:  ForeignKey [FK_T_STOCK_T_TRANS_DET]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_STOCK]  WITH CHECK ADD  CONSTRAINT [FK_T_STOCK_T_TRANS_DET] FOREIGN KEY([TRANS_DET_ID])
REFERENCES [dbo].[T_TRANS_DET] ([TRANS_DET_ID])
GO
ALTER TABLE [dbo].[T_STOCK] CHECK CONSTRAINT [FK_T_STOCK_T_TRANS_DET]
GO
/****** Object:  ForeignKey [FK_T_STOCK_CARD_M_ITEM]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_STOCK_CARD]  WITH CHECK ADD  CONSTRAINT [FK_T_STOCK_CARD_M_ITEM] FOREIGN KEY([ITEM_ID])
REFERENCES [dbo].[M_ITEM] ([ITEM_ID])
GO
ALTER TABLE [dbo].[T_STOCK_CARD] CHECK CONSTRAINT [FK_T_STOCK_CARD_M_ITEM]
GO
/****** Object:  ForeignKey [FK_T_STOCK_CARD_M_WAREHOUSE]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_STOCK_CARD]  WITH CHECK ADD  CONSTRAINT [FK_T_STOCK_CARD_M_WAREHOUSE] FOREIGN KEY([WAREHOUSE_ID])
REFERENCES [dbo].[M_WAREHOUSE] ([WAREHOUSE_ID])
GO
ALTER TABLE [dbo].[T_STOCK_CARD] CHECK CONSTRAINT [FK_T_STOCK_CARD_M_WAREHOUSE]
GO
/****** Object:  ForeignKey [FK_T_STOCK_CARD_T_TRANS_DET]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_STOCK_CARD]  WITH CHECK ADD  CONSTRAINT [FK_T_STOCK_CARD_T_TRANS_DET] FOREIGN KEY([TRANS_DET_ID])
REFERENCES [dbo].[T_TRANS_DET] ([TRANS_DET_ID])
GO
ALTER TABLE [dbo].[T_STOCK_CARD] CHECK CONSTRAINT [FK_T_STOCK_CARD_T_TRANS_DET]
GO
/****** Object:  ForeignKey [FK_T_STOCK_ITEM_M_ITEM]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_STOCK_ITEM]  WITH CHECK ADD  CONSTRAINT [FK_T_STOCK_ITEM_M_ITEM] FOREIGN KEY([ITEM_ID])
REFERENCES [dbo].[M_ITEM] ([ITEM_ID])
GO
ALTER TABLE [dbo].[T_STOCK_ITEM] CHECK CONSTRAINT [FK_T_STOCK_ITEM_M_ITEM]
GO
/****** Object:  ForeignKey [FK_T_STOCK_ITEM_M_WAREHOUSE]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_STOCK_ITEM]  WITH CHECK ADD  CONSTRAINT [FK_T_STOCK_ITEM_M_WAREHOUSE] FOREIGN KEY([WAREHOUSE_ID])
REFERENCES [dbo].[M_WAREHOUSE] ([WAREHOUSE_ID])
GO
ALTER TABLE [dbo].[T_STOCK_ITEM] CHECK CONSTRAINT [FK_T_STOCK_ITEM_M_WAREHOUSE]
GO
/****** Object:  ForeignKey [FK_T_STOCK_REF_T_STOCK]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_STOCK_REF]  WITH CHECK ADD  CONSTRAINT [FK_T_STOCK_REF_T_STOCK] FOREIGN KEY([STOCK_ID])
REFERENCES [dbo].[T_STOCK] ([STOCK_ID])
GO
ALTER TABLE [dbo].[T_STOCK_REF] CHECK CONSTRAINT [FK_T_STOCK_REF_T_STOCK]
GO
/****** Object:  ForeignKey [FK_T_TRANS_M_EMPLOYEE]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_TRANS]  WITH CHECK ADD  CONSTRAINT [FK_T_TRANS_M_EMPLOYEE] FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[M_EMPLOYEE] ([EMPLOYEE_ID])
GO
ALTER TABLE [dbo].[T_TRANS] CHECK CONSTRAINT [FK_T_TRANS_M_EMPLOYEE]
GO
/****** Object:  ForeignKey [FK_T_TRANS_M_WAREHOUSE]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_TRANS]  WITH CHECK ADD  CONSTRAINT [FK_T_TRANS_M_WAREHOUSE] FOREIGN KEY([WAREHOUSE_ID])
REFERENCES [dbo].[M_WAREHOUSE] ([WAREHOUSE_ID])
GO
ALTER TABLE [dbo].[T_TRANS] CHECK CONSTRAINT [FK_T_TRANS_M_WAREHOUSE]
GO
/****** Object:  ForeignKey [FK_T_TRANS_M_WAREHOUSE1]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_TRANS]  WITH CHECK ADD  CONSTRAINT [FK_T_TRANS_M_WAREHOUSE1] FOREIGN KEY([WAREHOUSE_ID_TO])
REFERENCES [dbo].[M_WAREHOUSE] ([WAREHOUSE_ID])
GO
ALTER TABLE [dbo].[T_TRANS] CHECK CONSTRAINT [FK_T_TRANS_M_WAREHOUSE1]
GO
/****** Object:  ForeignKey [FK_T_TRANS_DET_M_ITEM]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_TRANS_DET]  WITH CHECK ADD  CONSTRAINT [FK_T_TRANS_DET_M_ITEM] FOREIGN KEY([ITEM_ID])
REFERENCES [dbo].[M_ITEM] ([ITEM_ID])
GO
ALTER TABLE [dbo].[T_TRANS_DET] CHECK CONSTRAINT [FK_T_TRANS_DET_M_ITEM]
GO
/****** Object:  ForeignKey [FK_T_TRANS_DET_M_ITEM_UOM]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_TRANS_DET]  WITH CHECK ADD  CONSTRAINT [FK_T_TRANS_DET_M_ITEM_UOM] FOREIGN KEY([ITEM_UOM_ID])
REFERENCES [dbo].[M_ITEM_UOM] ([ITEM_UOM_ID])
GO
ALTER TABLE [dbo].[T_TRANS_DET] CHECK CONSTRAINT [FK_T_TRANS_DET_M_ITEM_UOM]
GO
/****** Object:  ForeignKey [FK_T_TRANS_DET_T_TRANS]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_TRANS_DET]  WITH CHECK ADD  CONSTRAINT [FK_T_TRANS_DET_T_TRANS] FOREIGN KEY([TRANS_ID])
REFERENCES [dbo].[T_TRANS] ([TRANS_ID])
GO
ALTER TABLE [dbo].[T_TRANS_DET] CHECK CONSTRAINT [FK_T_TRANS_DET_T_TRANS]
GO
/****** Object:  ForeignKey [FK_T_TRANS_REF_T_TRANS]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_TRANS_REF]  WITH CHECK ADD  CONSTRAINT [FK_T_TRANS_REF_T_TRANS] FOREIGN KEY([TRANS_ID])
REFERENCES [dbo].[T_TRANS] ([TRANS_ID])
GO
ALTER TABLE [dbo].[T_TRANS_REF] CHECK CONSTRAINT [FK_T_TRANS_REF_T_TRANS]
GO
/****** Object:  ForeignKey [FK_T_TRANS_REF_T_TRANS1]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_TRANS_REF]  WITH CHECK ADD  CONSTRAINT [FK_T_TRANS_REF_T_TRANS1] FOREIGN KEY([TRANS_ID_REF])
REFERENCES [dbo].[T_TRANS] ([TRANS_ID])
GO
ALTER TABLE [dbo].[T_TRANS_REF] CHECK CONSTRAINT [FK_T_TRANS_REF_T_TRANS1]
GO
/****** Object:  ForeignKey [FK_T_TRANS_UNIT_M_COST_CENTER]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_TRANS_UNIT]  WITH CHECK ADD  CONSTRAINT [FK_T_TRANS_UNIT_M_COST_CENTER] FOREIGN KEY([COST_CENTER_ID])
REFERENCES [dbo].[M_COST_CENTER] ([COST_CENTER_ID])
GO
ALTER TABLE [dbo].[T_TRANS_UNIT] CHECK CONSTRAINT [FK_T_TRANS_UNIT_M_COST_CENTER]
GO
/****** Object:  ForeignKey [FK_T_TRANS_UNIT_M_CUSTOMER]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_TRANS_UNIT]  WITH CHECK ADD  CONSTRAINT [FK_T_TRANS_UNIT_M_CUSTOMER] FOREIGN KEY([CUSTOMER_ID])
REFERENCES [dbo].[M_CUSTOMER] ([CUSTOMER_ID])
GO
ALTER TABLE [dbo].[T_TRANS_UNIT] CHECK CONSTRAINT [FK_T_TRANS_UNIT_M_CUSTOMER]
GO
/****** Object:  ForeignKey [FK_T_TRANS_UNIT_T_UNIT]    Script Date: 04/19/2011 16:18:42 ******/
ALTER TABLE [dbo].[T_TRANS_UNIT]  WITH CHECK ADD  CONSTRAINT [FK_T_TRANS_UNIT_T_UNIT] FOREIGN KEY([UNIT_ID])
REFERENCES [dbo].[T_UNIT] ([UNIT_ID])
GO
ALTER TABLE [dbo].[T_TRANS_UNIT] CHECK CONSTRAINT [FK_T_TRANS_UNIT_T_UNIT]
GO

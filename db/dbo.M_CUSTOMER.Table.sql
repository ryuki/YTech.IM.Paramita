USE [DB_IM_PARAMITA]
GO
/****** Object:  Table [dbo].[M_CUSTOMER]    Script Date: 10/19/2013 02:45:22 ******/
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
ALTER TABLE [dbo].[M_CUSTOMER]  WITH CHECK ADD  CONSTRAINT [FK_M_CUSTOMER_REF_ADDRESS] FOREIGN KEY([ADDRESS_ID])
REFERENCES [dbo].[REF_ADDRESS] ([ADDRESS_ID])
GO
ALTER TABLE [dbo].[M_CUSTOMER] CHECK CONSTRAINT [FK_M_CUSTOMER_REF_ADDRESS]
GO
ALTER TABLE [dbo].[M_CUSTOMER]  WITH CHECK ADD  CONSTRAINT [FK_M_CUSTOMER_REF_PERSON] FOREIGN KEY([PERSON_ID])
REFERENCES [dbo].[REF_PERSON] ([PERSON_ID])
GO
ALTER TABLE [dbo].[M_CUSTOMER] CHECK CONSTRAINT [FK_M_CUSTOMER_REF_PERSON]
GO

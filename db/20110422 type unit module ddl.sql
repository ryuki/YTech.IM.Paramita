CREATE TABLE [dbo].[M_UNIT_TYPE](
	[UNIT_TYPE_ID] [nvarchar](50) NOT NULL,
	[COST_CENTER_ID] [nvarchar](50) NULL,
	[UNIT_TYPE_NAME] [nvarchar](50) NULL,
	[UNIT_TYPE_TOTAL] [int] NULL,
	[UNIT_TYPE_STATUS] [nvarchar](50) NULL,
	[UNIT_TYPE_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_UNIT_TYPE] PRIMARY KEY CLUSTERED 
(
	[UNIT_TYPE_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[M_UNIT_TYPE]  WITH CHECK ADD  CONSTRAINT [FK_M_UNIT_TYPE_M_COST_CENTER] FOREIGN KEY([COST_CENTER_ID])
REFERENCES [dbo].[M_COST_CENTER] ([COST_CENTER_ID])
GO

ALTER TABLE [dbo].[M_UNIT_TYPE] CHECK CONSTRAINT [FK_M_UNIT_TYPE_M_COST_CENTER]
GO

alter table [T_TRANS] add [UNIT_TYPE_ID] [nvarchar](50) NULL

ALTER TABLE [dbo].[T_TRANS]  WITH CHECK ADD  CONSTRAINT [FK_T_TRANS_M_UNIT_TYPE] FOREIGN KEY([UNIT_TYPE_ID])
REFERENCES [dbo].[M_UNIT_TYPE] ([UNIT_TYPE_ID])
GO

ALTER TABLE [dbo].[T_TRANS] CHECK CONSTRAINT [FK_T_TRANS_M_UNIT_TYPE]
GO


alter table [T_TRANS_DET] add [JOB_TYPE_ID] [nvarchar](50) NULL

ALTER TABLE [dbo].[T_TRANS_DET]  WITH CHECK ADD  CONSTRAINT [FK_T_TRANS_DET_M_JOB_TYPE] FOREIGN KEY([JOB_TYPE_ID])
REFERENCES [dbo].[M_JOB_TYPE] ([JOB_TYPE_ID])
GO

ALTER TABLE [dbo].[T_TRANS_DET] CHECK CONSTRAINT [FK_T_TRANS_DET_M_JOB_TYPE]

go


sp_RENAME 'dbo.T_UNIT.UNIT_TYPE', 'UNIT_TYPE_ID' , 'COLUMN'


ALTER TABLE [dbo].[T_UNIT]  WITH CHECK ADD  CONSTRAINT [FK_T_UNIT_M_UNIT_TYPE] FOREIGN KEY([UNIT_TYPE_ID])
REFERENCES [dbo].[M_UNIT_TYPE] ([UNIT_TYPE_ID])
GO

ALTER TABLE [dbo].[T_UNIT] CHECK CONSTRAINT [FK_T_UNIT_M_UNIT_TYPE]
GO
USE [DB_IM_PARAMITA]
GO
/****** Object:  View [dbo].[V_JOURNAL_DET_FLOW]    Script Date: 10/19/2013 02:45:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create VIEW [dbo].[V_JOURNAL_DET_FLOW]
AS
SELECT  a.ROWNUMBER
,a.[JOURNAL_DET_ID]
      ,a.[JOURNAL_ID]
      ,a.[ACCOUNT_ID]
      ,a.[JOURNAL_DET_NO]
      ,a.[JOURNAL_DET_STATUS]
      ,a.[JOURNAL_DET_AMMOUNT]
      ,a.[JOURNAL_DET_DESC]
      ,a.[DATA_STATUS]
      ,a.[CREATED_BY]
      ,a.[CREATED_DATE]
      ,a.[MODIFIED_BY]
      ,a.[MODIFIED_DATE]
      ,a.[ROW_VERSION]
      ,a.[JOURNAL_DET_EVIDENCE_NO] 
                     , SUM(CASE b.[JOURNAL_DET_STATUS] WHEN 'D' THEN b.[JOURNAL_DET_AMMOUNT] ELSE b.[JOURNAL_DET_AMMOUNT] * - 1 END) AS SALDO 
FROM        dbo.V_JOURNAL_DET AS  a
INNER JOIN
                     dbo.V_JOURNAL_DET AS b ON a.ROWNUMBER >= b.ROWNUMBER AND a.ACCOUNT_ID = b.ACCOUNT_ID
GROUP BY  a.ROWNUMBER
,a.[JOURNAL_DET_ID]
      ,a.[JOURNAL_ID]
      ,a.[ACCOUNT_ID]
      ,a.[JOURNAL_DET_NO]
      ,a.[JOURNAL_DET_STATUS]
      ,a.[JOURNAL_DET_AMMOUNT]
      ,a.[JOURNAL_DET_DESC]
      ,a.[DATA_STATUS]
      ,a.[CREATED_BY]
      ,a.[CREATED_DATE]
      ,a.[MODIFIED_BY]
      ,a.[MODIFIED_DATE]
      ,a.[ROW_VERSION]
      ,a.[JOURNAL_DET_EVIDENCE_NO]
GO

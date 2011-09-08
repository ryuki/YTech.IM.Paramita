
ALTER VIEW [dbo].[V_JOURNAL_DET]
AS

SELECT row_number() OVER (ORDER BY a.[ACCOUNT_ID], b.JOURNAL_DATE, b.JOURNAL_VOUCHER_NO ASC) AS ROWNUMBER
,   a.[JOURNAL_DET_ID]
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
                     --, SUM(CASE b.[JOURNAL_DET_STATUS] WHEN 'DEBET' THEN b.[JOURNAL_DET_AMMOUNT] ELSE b.[JOURNAL_DET_AMMOUNT] * - 1 END) AS SALDO 
FROM        dbo.T_JOURNAL_DET AS a INNER JOIN
                  dbo.T_JOURNAL AS b on a.JOURNAL_ID = b.JOURNAL_ID

--INNER JOIN
--                     dbo.T_JOURNAL_DET AS b ON a.[JOURNAL_DET_ID] >= b.[JOURNAL_DET_ID] AND a.ACCOUNT_ID = b.ACCOUNT_ID
--GROUP BY  a.[JOURNAL_DET_ID]
--      ,a.[JOURNAL_ID]
--      ,a.[ACCOUNT_ID]
--      ,a.[JOURNAL_DET_NO]
--      ,a.[JOURNAL_DET_STATUS]
--      ,a.[JOURNAL_DET_AMMOUNT]
--      ,a.[JOURNAL_DET_DESC]
--      ,a.[DATA_STATUS]
--      ,a.[CREATED_BY]
--      ,a.[CREATED_DATE]
--      ,a.[MODIFIED_BY]
--      ,a.[MODIFIED_DATE]
--      ,a.[ROW_VERSION]
--      ,a.[JOURNAL_DET_EVIDENCE_NO] 
GO

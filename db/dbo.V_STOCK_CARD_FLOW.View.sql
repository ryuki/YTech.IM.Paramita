USE [DB_IM_PARAMITA]
GO
/****** Object:  View [dbo].[V_STOCK_CARD_FLOW]    Script Date: 10/19/2013 02:45:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create VIEW [dbo].[V_STOCK_CARD_FLOW]
AS
SELECT  a.ROWNUMBER,
a.STOCK_CARD_ID, 
a.ITEM_ID, 
a.WAREHOUSE_ID, 
a.TRANS_DET_ID, 
a.STOCK_CARD_DATE, 
a.STOCK_CARD_STATUS, 
a.STOCK_CARD_QTY, 
a.STOCK_CARD_SALDO, 
a.STOCK_CARD_DESC, 
a.DATA_STATUS, 
a.CREATED_BY, 
a.CREATED_DATE, 
a.MODIFIED_BY, 
a.MODIFIED_DATE, 
a.ROW_VERSION
 
                     , SUM(CASE b.STOCK_CARD_STATUS WHEN 1 THEN b.STOCK_CARD_QTY ELSE b.STOCK_CARD_QTY * - 1 END) AS SALDO 
FROM        dbo.V_STOCK_CARD AS  a
INNER JOIN
                     dbo.V_STOCK_CARD AS b ON a.ROWNUMBER >= b.ROWNUMBER AND a.WAREHOUSE_ID = b.WAREHOUSE_ID AND a.ITEM_ID = b.ITEM_ID
GROUP BY  a.ROWNUMBER,
a.STOCK_CARD_ID, 
a.ITEM_ID, 
a.WAREHOUSE_ID, 
a.TRANS_DET_ID, 
a.STOCK_CARD_DATE, 
a.STOCK_CARD_STATUS, 
a.STOCK_CARD_QTY, 
a.STOCK_CARD_SALDO, 
a.STOCK_CARD_DESC, 
a.DATA_STATUS, 
a.CREATED_BY, 
a.CREATED_DATE, 
a.MODIFIED_BY, 
a.MODIFIED_DATE, 
a.ROW_VERSION
GO

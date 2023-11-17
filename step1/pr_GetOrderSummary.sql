USE NorthWind
GO


IF EXISTS (SELECT * from dbo.sysobjects WHERE Id = object_id(N'dbo.pr_GetOrderSummary') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE dbo.pr_GetOrderSummary
GO

CREATE PROCEDURE pr_GetOrderSummary 
	@StartDate DATETIME, 
	@EndDate DATETIME,
	@EmployeeID INT NULL , 
	@CustomerID NCHAR(5) NULL
AS

SELECT Emp.TitleOfCourtesy + ' ' + emp.FirstName + ' ' + emp.LastName AS EmployeeFullName,
	shi.CompanyName AS [Shipper CompanyName],
	cus.CompanyName AS [Customer CompanyName], 
	COUNT(ord.OrderID) AS NumberOfOrders, --spec said NumberOfOders assuming it's a spelling mistake
	ord.OrderDate AS [Date],
	SUM(freight) AS TotalFreightCost ,
	COUNT(DISTINCT orddet.ProductID) as NumberOfDifferentProducts, --Assumed this is distrinct products ordered, as opposed to a count of the order details or sum of the quantities
	SUM((orddet.unitPrice * orddet.Quantity)-(orddet.unitPrice * orddet.Quantity * orddet.Discount)) AS TotalOrderValue -- includes discount, no rounding specified

FROM [Order Details] orddet
INNER JOIN Orders ord ON orddet.OrderID = ord.OrderID
INNER JOIN Employees emp ON ord.EmployeeID = emp.EmployeeID
INNER JOIN Shippers shi ON ord.ShipVia = shi.ShipperID
INNER JOIN Customers cus ON ord.CustomerID = cus.CustomerID

WHERE ord.OrderDate BETWEEN @StartDate AND @EndDate
AND ((@EmployeeID IS NULL) OR (ord.EmployeeID = @EmployeeID))
AND ((@CustomerID IS NULL) OR (ord.CustomerID = @CustomerID))

GROUP BY ord.OrderDate,
	Emp.TitleOfCourtesy + ' ' + emp.FirstName + ' ' + emp.LastName, 
	cus.CompanyName,
	shi.CompanyName

/*
ORDER BY  ord.OrderDate ASC -- no order was specified, uncomment for testing
*/


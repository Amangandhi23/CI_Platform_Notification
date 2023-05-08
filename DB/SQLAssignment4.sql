select * FROM Categories

select * FROM CustomerCustomerDemo

select * FROM CustomerDemographics
select * FROM Customers
select * FROM EmployeeTerritories

select * FROM Employees
  
select * FROM Orders
select * FROM [Order Details]
select * FROM Products
select * FROM Region

select * FROM Shippers
select * FROM Suppliers
select * FROM Territories





/*Query1*/
CREATE PROCEDURE que1
AS
	SELECT CustomerID, AVG(Freight) as AvgFreight
	FROM Orders
	GROUP BY CustomerID
GO
insert into orders values ( 'AROUT', 8,'1997-08-25 00:00:00.000', '1997-08-01 00:00:00.000','1997-01-01 00:00:00.000', 1,80, 'Wolski Zajazd', 'ul. Filtrowa 68',
'Warszawa', 'Tachira', 24100, 'brazil')

exec que1
UPDATE Orders SET Freight=12 WHERE OrderID = 10272
SELECT * FROM Orders WHERE OrderID = 10272

CREATE TRIGGER tr_que1_update
ON orders
INSTEAD OF UPDATE
AS
BEGIN 
	Declare @OrderID int
	Declare @CustomerID varchar(50)
	Declare @Freight money
	Declare @AvgFreight money

	Declare @t_ave TABLE(CustomerID nchar(5), AvgFreight money)
	INSERT @t_ave
	exec que1
	
	Select * Into #Temptable FROM Inserted

	While(Exists(Select OrderID from #TempTable))
      Begin
		Select TOP 1 @OrderID = OrderID, @CustomerID = CustomerID, @Freight=Freight
		FROM #Temptable
		SET @AvgFreight = (SELECT AvgFreight FROM @t_ave WHERE CustomerID = @CustomerID)

		Print @Freight
		Print @AvgFreight

			IF @Freight > @AvgFreight 
			BEGIN	
				RAISERROR ('ABOVE AVERAGE',16,1)
			END
			ELSE 
			BEGIN
				UPDATE Orders SET Freight = @Freight WHERE OrderID=@OrderID 
			END

		Delete from #TempTable where OrderID = @OrderID
		
      End
END

CREATE TRIGGER tr_que1_insert
ON orders
INSTEAD OF INSERT
AS
BEGIN 
	Declare @OrderID int
	Declare @CustomerID varchar(50)
	Declare @Freight money
	Declare @AvgFreight money
	Declare @t_ave TABLE(CustomerID nchar(5), AvgFreight money)
	INSERT @t_ave
	exec que1
	
	Select * Into #Temptable FROM Inserted

	While(Exists(Select OrderID from #TempTable))
      Begin
		Select TOP 1 @OrderID = OrderID, @CustomerID = CustomerID, @Freight=Freight
		FROM #Temptable
		SET @AvgFreight = (SELECT AvgFreight FROM @t_ave WHERE CustomerID = @CustomerID)

		IF @Freight > @AvgFreight 
		BEGIN	
			RAISERROR ('ABOVE AVERAGE',16,1)
		END
		ELSE 
		BEGIN
			INSERT INTO Orders (CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry)
			SELECT CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry
			From Inserted
		END

		Delete from #TempTable where OrderID = @OrderID
		
      End
END

/*Query2*/
ALTER proc Proc_Employee_sales_bycountry
	@ShipCountry nvarchar(10)
	as
	begin
		SELECT Employees.EmployeeID,Employees.[LastName],Employees.[FirstName],sum(UnitPrice) as Sales,COUNT(Orders.EmployeeID) as [Orders],Orders.ShipCountry
		FROM Employees 
		INNER JOIN Orders
		ON Orders.EmployeeID = Employees.EmployeeID
		INNER JOIN [Order Details]
		ON [Order Details].orderID = Orders.orderID
		Where ShipCountry = @ShipCountry
		GROUP BY Employees.EmployeeID, Orders.ShipCountry,Employees.[LastName],Employees.[FirstName]
	end

Execute Proc_Employee_sales_bycountry 'UK'

/*Query3*/
ALTER PROC Proc_sales_by_year
	@year int 
	as
	begin
		SELECT year(ShippedDate) as Year ,sum(UnitPrice) as Sales, COUNT(Orders.EmployeeID) as Orders
		FROM Orders 
		INNER JOIN [Order Details]
		ON [Order Details].orderID = Orders.orderID
	/*	LEFT JOIN Employees
		ON Orders.EmployeeID = Employees.EmployeeID */
		Where year(ShippedDate) = @year
		GROUP BY year(ShippedDate)
	end

execute sp_rename 'Proc_Employee_sales_by' , 'Proc_sales_by_year';

execute Proc_sales_by_year '1998'

/*Query4*/
ALTER PROC Proc_sales_by_Categories
	@categoriename varchar(50)
	as
	begin
		SELECT CategoryName ,sum([Order Details].UnitPrice) as Sales, COUNT(Orders.EmployeeID) as Orders
		FROM Orders , [Order Details], Products,Categories
		Where CategoryName = @categoriename
		and [Order Details].ProductID = Products.ProductID
		and [Order Details].OrderID = Orders.OrderID
		and Products.CategoryID = Categories.CategoryID
		GROUP BY CategoryName
	end

execute Proc_sales_by_Categories 'Dairy Products'


/*Query5*/
ALTER PROC Proc_10_most_expensive_products
	as
	begin
		SELECT TOP(10) ProductName , UnitPrice
		FROM Products
		Order BY UnitPrice desc
	end

execute Proc_10_most_expensive_products

/*Query6*/
ALTER PROCEDURE [insert_Order Details_1]
(@OrderID_1 int,
@ProductID_2 int,
@UnitPrice_3 money = NULL,
@Quantity_4 smallint,
@Discount_5 real = 0)
AS
INSERT INTO [Northwind].[dbo].[Order Details]
( [OrderID], [ProductID], [UnitPrice], [Quantity], [Discount])
VALUES ( @OrderID_1, @ProductID_2, @UnitPrice_3, @Quantity_4, @Discount_5)

execute [insert_Order Details_1] 10248,51,12.34,2,0
SELECT * FROM [Order Details]
WHERE OrderID = 10248 and ProductID = 51

/*Query7*/

CREATE PROCEDURE [update_Order Details_1]
(
@OrderID_1 int,
@ProductID_2 int,
@NewQuantity_4 smallint= NULL,
@NewUnitPrice_3 money = NULL,
@NewDiscount_5 real = NULL
)
AS
UPDATE [Northwind].[dbo].[Order Details]
SET [Quantity] = @NewQuantity_4, [UnitPrice] = @NewUnitPrice_3, [Discount] = @NewDiscount_5
WHERE ( [OrderID] = @OrderID_1 AND 
		[ProductID] = @ProductID_2 )

execute [update_Order Details_1] 10248,11,12.00,12,0
SELECT * FROM [Order Details]
WHERE OrderID = 10248 and ProductID = 11

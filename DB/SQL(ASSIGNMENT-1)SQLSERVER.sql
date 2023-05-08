CREATE TABLE Products (
    ProductID int NOT NULL IDENTITY(1,1),
    ProductName varchar(255) NOT NULL,
	SupplierID int NOT NULL,
	CategoryID int NOT NULL,
	QuantityPerUnit varchar(100) NOT NULL,
	UnitPrice int NOT NULL DEFAULT '0',
	UnitsInStock int NOT NULL,
	UnitsOnOrder int NOT NULL,
	ReorderLevel int NOT NULL, 
    Discontinued varchar(5) NOT NULL,
	PRIMARY KEY (ProductID)
);


INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Chai',1,1,'10 boxes x 20 bags',18,39,0,10,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Chang',1,1,'24 - 12 oz bottles',19,17,40,25,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Aniseed Syrup',1,2,'12 - 550 ml bottles',10,13,70,25,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Chef Anton''s Cajun Seasoning',2,2,'48 - 6 oz jars',22,53,0,0,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Chef Anton''s Gumbo Mix',2,2,'36 boxes',21.35,0,0,0,1)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Grandma''s Boysenberry Spread',3,2,'12 - 8 oz jars',25,120,0,25,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Uncle Bob''s Organic Dried Pears',3,7,'12 - 1 lb pkgs.',30,15,0,10,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Northwoods Cranberry Sauce',3,2,'12 - 12 oz jars',40,6,0,0,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Mishi Kobe Niku',4,6,'18 - 500 g pkgs.',97,29,0,0,1)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Ikura',4,8,'12 - 200 ml jars',31,31,0,0,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Queso Cabrales',5,4,'1 kg pkg.',21,22,30,30,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Queso Manchego La Pastora',5,4,'10 - 500 g pkgs.',38,86,0,0,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Konbu',6,8,'2 kg box',6,24,0,5,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Tofu',6,7,'40 - 100 g pkgs.',23.25,35,0,0,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Genen Shouyu',6,2,'24 - 250 ml bottles',15.5,39,0,5,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Pavlova',7,3,'32 - 500 g boxes',17.45,29,0,10,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Alice Mutton',7,6,'20 - 1 kg tins',39,0,0,0,1)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Carnarvon Tigers',7,8,'16 kg pkg.',62.5,42,0,0,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Teatime Chocolate Biscuits',8,3,'10 boxes x 12 pieces',9.2,25,0,5,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Sir Rodney''s Marmalade',8,3,'30 gift boxes',81,40,0,0,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Sir Rodney''s Scones',8,3,'24 pkgs. x 4 pieces',10,3,40,5,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Gustaf''s Knäckebröd',9,5,'24 - 500 g pkgs.',21,104,0,25,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Tunnbröd',9,5,'12 - 250 g pkgs.',9,61,0,25,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Guaraná Fantástica',10,1,'12 - 355 ml cans',4.5,20,0,0,1)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('NuNuCa Nuß-Nougat-Creme',11,3,'20 - 450 g glasses',14,76,0,30,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Gumbär Gummibärchen',11,3,'100 - 250 g bags',31.23,15,0,0,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Schoggi Schokolade',11,3,'100 - 100 g pieces',43.9,49,0,30,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Rössle Sauerkraut',12,7,'25 - 825 g cans',45.6,26,0,0,1)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Thüringer Rostbratwurst',12,6,'50 bags x 30 sausgs.',123.79,0,0,0,1)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Nord-Ost Matjeshering',13,8,'10 - 200 g glasses',25.89,10,0,15,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Gorgonzola Telino',14,4,'12 - 100 g pkgs',12.5,0,70,20,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Mascarpone Fabioli',14,4,'24 - 200 g pkgs.',32,9,40,25,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Geitost',15,4,'500 g',2.5,112,0,20,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Sasquatch Ale',16,1,'24 - 12 oz bottles',14,111,0,15,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Steeleye Stout',16,1,'24 - 12 oz bottles',18,20,0,15,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Inlagd Sill',17,8,'24 - 250 g  jars',19,112,0,20,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Gravad lax',17,8,'12 - 500 g pkgs.',26,11,50,25,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Côte de Blaye',18,1,'12 - 75 cl bottles',263.5,17,0,15,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Chartreuse verte',18,1,'750 cc per bottle',18,69,0,5,0)
INSERT "Products"("ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued") VALUES('Boston Crab Meat',19,8,'24 - 4 oz tins',18.4,123,0,30,0)


SELECT * FROM Products;

SELECT ProductID,ProductName,UnitPrice
FROM Products
Where UnitPrice < 20;

SELECT ProductID,ProductName,UnitPrice
FROM Products
Where UnitPrice BETWEEN 15 AND 25;

SELECT ProductName,UnitPrice
FROM Products
Where UnitPrice BETWEEN 15 AND 25;

SELECT ProductName,UnitPrice
FROM Products
Where UnitPrice > ( SELECT AVG(UnitPrice) FROM Products);

SELECT TOP 10 ProductName,UnitPrice
FROM Products
ORDER BY UnitPrice DESC;

SELECT COUNT(ProductName)
FROM Products
GROUP BY Discontinued;

SELECT ProductName,UnitsOnOrder,UnitsInStock
FROM Products
WHERE UnitsInStock < UnitsOnOrder;


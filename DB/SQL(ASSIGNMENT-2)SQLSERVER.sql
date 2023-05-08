CREATE TABLE dbo.Salesman(
Salesman_id INT PRIMARY KEY,
[Name] VARCHAR(50) not null,
City VARCHAR(50) not null,
Commission FLOAT not null
)

INSERT INTO dbo.salesman VALUES (5001, 'James Hoog', 'New York', 0.15);
INSERT INTO dbo.salesman VALUES (5002, 'Nail Knite', ' Paris', 0.13);
INSERT INTO dbo.salesman VALUES (5005, 'Pit Alex', 'London', 0.11);
INSERT INTO dbo.salesman VALUES (5006, 'Mc Lyon ', 'Paris', 0.14);
INSERT INTO dbo.salesman VALUES (5007, 'Paul Adam', 'Rome', 0.13);
INSERT INTO dbo.salesman VALUES (5003, 'Lauson Hen', 'San Jose', 0.12);

select * from dbo.Salesman

CREATE TABLE dbo.customer(
Customer_id int primary key,
Cust_name varchar(50) not null,
City varchar(50) not null,
Grade int,
Salesman_id int
) 

ALTER TABLE dbo.customer
   ADD CONSTRAINT FK_customer_Salesman_id FOREIGN KEY (Salesman_id)
      REFERENCES salesman (Salesman_id);


INSERT INTO dbo.customer VALUES (3002, 'Nick Rimando', 'New York',100, 5001);
INSERT INTO dbo.customer VALUES (3007, 'Brad Davis', 'New York',200, 5001);
INSERT INTO dbo.customer VALUES (3005, 'Graham Zusi', 'California',200, 5002);
INSERT INTO dbo.customer VALUES (3008, 'Julian Green', 'London',300, 5002);
INSERT INTO dbo.customer VALUES (3004, 'Fabian Johnson', 'Paris',300, 5006);
INSERT INTO dbo.customer VALUES (3009, 'Geoff Cameron', 'Berlin',100, 5003);
INSERT INTO dbo.customer VALUES (3003, 'Jozy Altidor', 'Moscow',200, 5007);
INSERT INTO dbo.customer(Customer_id,Cust_name,City,Salesman_id) VALUES (3001, 'Brad Guzan ', 'London', 5005);

select * from dbo.customer


create table dbo.orders(
ord_no int primary key,
purch_amt int not null,
ord_date date not null,
Customer_id int,
Salesman_id int
)

ALTER TABLE dbo.orders
   ADD CONSTRAINT FK_orders_Customer_id FOREIGN KEY (Customer_id)
      REFERENCES customer (Customer_id);

ALTER TABLE dbo.orders
   ADD CONSTRAINT FK_orders_Salesman_id FOREIGN KEY (Salesman_id)
      REFERENCES Salesman (Salesman_id);

INSERT INTO dbo.orders VALUES (70009, 270.65, '2012-09-10', 3001,5005);
INSERT INTO dbo.orders VALUES (70002,65.26,'2012-10-05',3002,5001);
INSERT INTO dbo.orders VALUES (70004,110.50,'2012-08-17',3009,5003);
INSERT INTO dbo.orders VALUES (70005,2400.60,'2012-07-27',3007,5001);
INSERT INTO dbo.orders VALUES (70008,5760.00,'2012-09-10',3002,5001);
INSERT INTO dbo.orders VALUES (70010,1983.43,'2012-10-10',3004,5006);
INSERT INTO dbo.orders VALUES (70003,2480.40,'2012-10-10',3009,5003);
INSERT INTO dbo.orders VALUES (70011,75.29,'2012-08-17',3003,5007);
INSERT INTO dbo.orders VALUES (70013,3045.60,'2012-04-25',3002,5001);
INSERT INTO dbo.orders VALUES (70001,150.50,'2012-10-05',3005,5002);
INSERT INTO dbo.orders VALUES (70007,948.50	,'2012-09-10',3005,5002);
INSERT INTO dbo.orders VALUES (70012,250.45,'2012-06-27',3008,5002);


select * from dbo.orders

/*Query1*/
SELECT	Salesman.[Name] as Salesman, customer.Cust_name, customer.City
FROM Salesman , customer
WHERE Salesman.City = customer.City

/*Query2*/
SELECT	orders.ord_no , orders.purch_amt ,customer.Cust_name, customer.City
FROM orders,customer
WHERE orders.Customer_id = customer.Customer_id 
AND orders.purch_amt BETWEEN 500 AND 2000;

/*Query3*/
SELECT customer.Cust_name, Salesman.City, Salesman.[Name] AS Salesman, Salesman.Commission
FROM customer
Inner JOIN Salesman
ON customer.Salesman_id = Salesman.Salesman_id

/*Query4*/
SELECT customer.Cust_name, Salesman.City, Salesman.[Name] AS Salesman, Salesman.Commission
FROM customer
Inner JOIN Salesman
ON  customer.Salesman_id = Salesman.Salesman_id
WHERE Salesman.Commission > 0.12

/*Query5*/
SELECT customer.Cust_name, Salesman.City, Salesman.[Name] AS Salesman, Salesman.Commission
FROM customer
Inner JOIN Salesman
ON  customer.Salesman_id = Salesman.Salesman_id
WHERE Salesman.Commission > 0.12 AND customer.City<>Salesman.City

/*Query6*/
SELECT orders.ord_no,orders.ord_date,orders.purch_amt,customer.Cust_name,customer.Grade,Salesman.[Name] as Salesman,Salesman.Commission
FROM orders
Inner Join Salesman ON orders.Salesman_id = Salesman.Salesman_id
Inner Join customer ON orders.Customer_id = customer.Customer_id

/*Query7*/
SELECT orders.ord_no,orders.ord_date,orders.purch_amt,customer.Customer_id,customer.Cust_name,customer.City
,customer.Grade,Salesman.Salesman_id,Salesman.[Name] as Salesman_Name,Salesman.Commission
FROM orders
Join Salesman ON orders.Salesman_id = Salesman.Salesman_id
Join customer ON orders.Customer_id = customer.Customer_id

/*Query8*/
SELECT customer.Cust_name,customer.City as Customer_city,customer.Grade,Salesman.[Name] as Salesman_Name,Salesman.City as Salesman_city
FROM customer
LEFT OUTER JOIN Salesman
ON customer.Salesman_id = Salesman.Salesman_id
ORDER BY customer.Customer_id ASC

/*Query9*/
SELECT customer.Cust_name,customer.City as Customer_city,customer.Grade,Salesman.[Name] as Salesman_Name,Salesman.City as Salesman_city
FROM customer
LEFT OUTER JOIN Salesman
ON customer.Salesman_id = Salesman.Salesman_id
WHERE customer.Grade < 300
ORDER BY customer.Customer_id ASC

/*Query10*/
SELECT customer.Cust_name,customer.City,orders.ord_no,orders.ord_date,orders.purch_amt
FROM customer
JOIN orders
ON customer.Customer_id = orders.Customer_id
ORDER BY orders.ord_date

/*Query11*/
SELECT customer.Cust_name,customer.City,orders.ord_no,orders.ord_date,orders.purch_amt,Salesman.[Name] as Salesman_name,Salesman.Commission
FROM customer
JOIN orders ON customer.Customer_id = orders.Customer_id
JOIN Salesman ON customer.Salesman_id = Salesman.Salesman_id

/*Query12*/
SELECT *
FROM customer
join Salesman 
ON customer.Salesman_id = Salesman.Salesman_id
ORDER BY Salesman.Salesman_id

/*Query13*/
SELECT Salesman.[Name] as Salesman_Name, customer.Cust_name,customer.City,customer.Grade,orders.ord_no,orders.ord_date,orders.purch_amt
FROM customer
JOIN Salesman ON Salesman.Salesman_id = customer.Salesman_id
JOIN orders ON orders.Customer_id = customer.Customer_id

/*Query14 & Query15*/
SELECT customer.Cust_name,customer.City,customer.Grade,Salesman.[Name] as Salesman_Name,orders.ord_no,orders.ord_date,orders.purch_amt
FROM customer
RIGHT JOIN Salesman ON Salesman.Salesman_id = customer.Salesman_id
LEFT JOIN orders ON orders.Customer_id = customer.Customer_id
WHERE orders.purch_amt >=2000
AND customer.Grade is not null;

/*Query16*/
SELECT customer.City, customer.City,orders.ord_no,orders.ord_date,orders.purch_amt
FROM customer
LEFT JOIN orders ON customer.Customer_id = orders.Customer_id
WHERE customer.Grade is not null 

/*Query17*/
SELECT *
FROM Salesman
cross join customer

/*Query18*/
SELECT *
FROM Salesman
cross join customer
WHERE Salesman.City= customer.City

/*Query19*/
SELECT *
FROM Salesman
cross join customer
WHERE Salesman.City is not null
AND customer.Grade is not null


/*Query20*/
SELECT *
FROM Salesman
cross join customer
WHERE Salesman.City <> customer.City
AND customer.Grade is not null





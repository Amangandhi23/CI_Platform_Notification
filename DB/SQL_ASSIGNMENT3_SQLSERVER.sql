CREATE TABLE Department (
	dept_id decimal(2,0) NOT NULL PRIMARY KEY,
	dept_name varchar(14) default NULL,
);

CREATE TABLE Employee (
	emp_id decimal(4,0) NOT NULL PRIMARY KEY,
	emp_name varchar(10) default NULL,
	mngr_id decimal(4,0) default NULL,
	salary decimal(7,2) default NULL,
	dept_id decimal(2,0) default NULL
);

ALTER TABLE Employee
	ADD CONSTRAINT FK_Employee_dept_id FOREIGN KEY (dept_id)
	REFERENCES Department (dept_id);

INSERT INTO Department VALUES ('10','ACCOUNTING');
INSERT INTO Department VALUES ('20','RESEARCH');
INSERT INTO Department VALUES ('30','SALES');
INSERT INTO Department VALUES ('40','OPERATIONS');



INSERT INTO Employee VALUES ('7369','SMITH','7902','800.00','20');
INSERT INTO Employee VALUES ('7499','ALLEN','7698','1600.00','30');
INSERT INTO Employee VALUES ('7521','WARD','7698','1250.00','30');
INSERT INTO Employee VALUES ('7566','JONES','7839','2975.00','20');
INSERT INTO Employee VALUES ('7654','MARTIN','7698','1250.00','30');
INSERT INTO Employee VALUES ('7698','BLAKE','7839','2850.00','30');
INSERT INTO Employee VALUES ('7782','CLARK','7839','2450.00','10');
INSERT INTO Employee VALUES ('7788','SCOTT','7566','3000.00','20');
INSERT INTO Employee VALUES ('7839','KING',NULL,'5000.00','10');
INSERT INTO Employee VALUES ('7844','TURNER','7698','1500.00','30');
INSERT INTO Employee VALUES ('7876','ADAMS','7788','1100.00','20');
INSERT INTO Employee VALUES ('7900','JAMES','7698','950.00','30');
INSERT INTO Employee VALUES ('7902','FORD','7566','3000.00','20');
INSERT INTO Employee VALUES ('7934','MILLER','7782','1300.00','10');

SELECT * FROM Department
SELECT * FROM Employee

/*Query1*/
SELECT d.dept_name,e.emp_name,salary
FROM Employee as e
left join Department as d
on e.dept_id = d.dept_id
WHERE salary in(
SELECT MAX(salary)
FROM Employee
group by dept_id
)
order by d.dept_id

/*Query2*/
SELECT dept_name,department.dept_id,count(employee.dept_id)
FROM Employee
full JOIN Department
on Employee.dept_id = Department.dept_id
GROUP BY Department.dept_id,dept_name
HAVING COUNT(employee.dept_id)<=3;


/*Query3*/
SELECT dept_name,department.dept_id,count(employee.dept_id) as total_employee
FROM Employee
full JOIN Department
on Employee.dept_id = Department.dept_id
GROUP BY Department.dept_id,dept_name


/*Query4*/
SELECT dept_name,department.dept_id,sum(salary) as total_salary
FROM Employee
full JOIN Department
on Employee.dept_id = Department.dept_id
GROUP BY Department.dept_id,dept_name






USE TestDB_SShenoy;
GO

CREATE TABLE Departments (
    DepartmentId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Employees (
    EmployeeId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Department NVARCHAR(100) NOT NULL,
    JoinDate DATE NOT NULL
);
GO

CREATE TABLE Attendances (
    AttendanceId INT PRIMARY KEY IDENTITY(1,1),
    EmployeeId INT NOT NULL,
    Date DATE NOT NULL,
    Status NVARCHAR(10) NOT NULL CHECK (Status IN ('Present', 'Absent')),
    FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
);
GO

INSERT INTO Departments (Name) VALUES 
('Human Resources'),
('Engineering'),
('Finance'),
('Marketing');
GO

INSERT INTO Employees (Name, Email, Department, JoinDate) VALUES
('Alice Johnson', 'alice.johnson@example.com', 'Human Resources', '2024-06-01'),
('Bob Smith', 'bob.smith@example.com', 'Engineering', '2024-06-05'),
('Clara Lee', 'clara.lee@example.com', 'Finance', '2024-06-10');
GO

INSERT INTO Attendances (EmployeeId, Date, Status) VALUES
(1, '2024-06-18', 'Present'),
(2, '2024-06-18', 'Absent'),
(3, '2024-06-18', 'Present'),
(1, '2024-06-19', 'Present');
GO

SELECT * FROM Departments;
SELECT * FROM Employees;
SELECT * FROM Attendances;




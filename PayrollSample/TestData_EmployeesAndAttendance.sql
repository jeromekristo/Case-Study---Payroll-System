-- =============================================
-- Test Data: 5 Employees with Attendance Records
-- Cutoff Period: December 1-15, 2025
-- =============================================

USE PayrollDB;
GO

-- =============================================
-- Step 1: Insert 5 Test Employees
-- =============================================

-- Employee 1: Hourly Worker
INSERT INTO Users (FirstName, LastName, Username, Password, Role, salary_rate, salary_type, Status)
VALUES ('John', 'Smith', 'jsmith', 'Password123!', 'Employee', 25.00, 'Hourly', 'Active');

-- Employee 2: Hourly Worker (Higher Rate)
INSERT INTO Users (FirstName, LastName, Username, Password, Role, salary_rate, salary_type, Status)
VALUES ('Maria', 'Garcia', 'mgarcia', 'Password123!', 'Employee', 30.00, 'Hourly', 'Active');

-- Employee 3: Monthly Salary Worker
INSERT INTO Users (FirstName, LastName, Username, Password, Role, salary_rate, salary_type, Status)
VALUES ('David', 'Johnson', 'djohnson', 'Password123!', 'Employee', 5000.00, 'Monthly', 'Active');

-- Employee 4: Daily Worker
INSERT INTO Users (FirstName, LastName, Username, Password, Role, salary_rate, salary_type, Status)
VALUES ('Sarah', 'Williams', 'swilliams', 'Password123!', 'Employee', 200.00, 'Daily', 'Active');

-- Employee 5: Hourly Worker (Part-time)
INSERT INTO Users (FirstName, LastName, Username, Password, Role, salary_rate, salary_type, Status)
VALUES ('Michael', 'Brown', 'mbrown', 'Password123!', 'Employee', 20.00, 'Hourly', 'Active');

GO

-- =============================================
-- Step 2: Insert Attendance Records for Dec 1-15, 2025
-- =============================================

-- Get UserIDs (assuming they were just inserted)
DECLARE @JohnID INT = (SELECT UserID FROM Users WHERE Username = 'jsmith');
DECLARE @MariaID INT = (SELECT UserID FROM Users WHERE Username = 'mgarcia');
DECLARE @DavidID INT = (SELECT UserID FROM Users WHERE Username = 'djohnson');
DECLARE @SarahID INT = (SELECT UserID FROM Users WHERE Username = 'swilliams');
DECLARE @MichaelID INT = (SELECT UserID FROM Users WHERE Username = 'mbrown');

-- Employee 1 (John Smith) - Full-time, 8 hours per day, Dec 1-12 (12 days, skipping weekends)
-- Dec 1 (Monday) - Dec 12 (Friday) = 10 working days (excluding weekends)
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@JohnID, '2025-12-01', '08:00:00', '17:00:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@JohnID, '2025-12-02', '08:00:00', '17:00:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@JohnID, '2025-12-03', '08:00:00', '16:30:00'); -- 8.5 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@JohnID, '2025-12-04', '08:00:00', '17:00:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@JohnID, '2025-12-05', '08:00:00', '17:00:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@JohnID, '2025-12-08', '08:00:00', '17:00:00'); -- 9 hours (Monday)
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@JohnID, '2025-12-09', '08:00:00', '17:00:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@JohnID, '2025-12-10', '08:00:00', '17:00:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@JohnID, '2025-12-11', '08:00:00', '16:00:00'); -- 8 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@JohnID, '2025-12-12', '08:00:00', '17:00:00'); -- 9 hours
-- Total: ~87.5 hours

-- Employee 2 (Maria Garcia) - Full-time, varied hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MariaID, '2025-12-01', '09:00:00', '18:00:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MariaID, '2025-12-02', '09:00:00', '18:00:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MariaID, '2025-12-03', '09:00:00', '17:30:00'); -- 8.5 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MariaID, '2025-12-04', '09:00:00', '18:00:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MariaID, '2025-12-05', '09:00:00', '18:00:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MariaID, '2025-12-08', '09:00:00', '18:00:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MariaID, '2025-12-09', '09:00:00', '18:00:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MariaID, '2025-12-10', '09:00:00', '18:00:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MariaID, '2025-12-11', '09:00:00', '17:00:00'); -- 8 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MariaID, '2025-12-12', '09:00:00', '18:00:00'); -- 9 hours
-- Total: ~87.5 hours

-- Employee 3 (David Johnson) - Monthly salary, full-time
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@DavidID, '2025-12-01', '08:30:00', '17:30:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@DavidID, '2025-12-02', '08:30:00', '17:30:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@DavidID, '2025-12-03', '08:30:00', '17:00:00'); -- 8.5 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@DavidID, '2025-12-04', '08:30:00', '17:30:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@DavidID, '2025-12-05', '08:30:00', '17:30:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@DavidID, '2025-12-08', '08:30:00', '17:30:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@DavidID, '2025-12-09', '08:30:00', '17:30:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@DavidID, '2025-12-10', '08:30:00', '17:30:00'); -- 9 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@DavidID, '2025-12-11', '08:30:00', '16:30:00'); -- 8 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@DavidID, '2025-12-12', '08:30:00', '17:30:00'); -- 9 hours
-- Total: ~87.5 hours

-- Employee 4 (Sarah Williams) - Daily worker, 6 days
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@SarahID, '2025-12-01', '07:00:00', '15:00:00'); -- 8 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@SarahID, '2025-12-02', '07:00:00', '15:00:00'); -- 8 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@SarahID, '2025-12-03', '07:00:00', '15:00:00'); -- 8 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@SarahID, '2025-12-04', '07:00:00', '15:00:00'); -- 8 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@SarahID, '2025-12-05', '07:00:00', '15:00:00'); -- 8 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@SarahID, '2025-12-08', '07:00:00', '15:00:00'); -- 8 hours
-- Total: 48 hours

-- Employee 5 (Michael Brown) - Part-time, 4 days
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MichaelID, '2025-12-01', '10:00:00', '14:00:00'); -- 4 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MichaelID, '2025-12-03', '10:00:00', '14:00:00'); -- 4 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MichaelID, '2025-12-05', '10:00:00', '14:00:00'); -- 4 hours
INSERT INTO Attendance (UserID, [date], time_in, time_out) VALUES (@MichaelID, '2025-12-08', '10:00:00', '14:00:00'); -- 4 hours
-- Total: 16 hours

GO

-- =============================================
-- Verification Query (Optional - to check the data)
-- =============================================
/*
SELECT 
    u.FirstName + ' ' + u.LastName AS EmployeeName,
    u.salary_type AS SalaryType,
    u.salary_rate AS SalaryRate,
    COUNT(a.attendance_id) AS DaysWorked,
    SUM(CAST(DATEDIFF(SECOND, a.time_in, a.time_out) AS DECIMAL(10,2)) / 3600.0) AS TotalHours
FROM Users u
LEFT JOIN Attendance a ON u.UserID = a.UserID 
    AND a.[date] >= '2025-12-01' 
    AND a.[date] <= '2025-12-15'
WHERE u.Username IN ('jsmith', 'mgarcia', 'djohnson', 'swilliams', 'mbrown')
GROUP BY u.UserID, u.FirstName, u.LastName, u.salary_type, u.salary_rate
ORDER BY u.LastName, u.FirstName;
*/



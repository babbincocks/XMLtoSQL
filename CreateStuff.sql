USE Sandbox

CREATE TABLE XMLConversionTest
(
ID INT PRIMARY KEY,
FirstName VARCHAR(100) NOT NULL,
LastName VARCHAR(100) NOT NULL,
SSN VARCHAR(12) NOT NULL,
Email VARCHAR(100) NOT NULL,
Gender VARCHAR(6) NOT NULL
)

CREATE PROC sp_InsertXML
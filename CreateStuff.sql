USE Sandbox

--DROP TABLE XMLConversionTest
CREATE TABLE XMLConversionTest
(
ID INT IDENTITY(1,1) PRIMARY KEY,
FirstName NVARCHAR(70) NOT NULL,
LastName NVARCHAR(70) NOT NULL,
SSN VARCHAR(12) NOT NULL,
Email VARCHAR(70) NOT NULL,
Gender VARCHAR(6) NOT NULL
)

GO
CREATE PROC sp_InsertXML
(
@FirstName NVARCHAR(70),
@LastName NVARCHAR(70),
@SSN VARCHAR(12),
@Email VARCHAR(70),
@Gender VARCHAR(6),
@ID INT OUT
)
AS
BEGIN

IF EXISTS(SELECT * FROM XMLConversionTest WHERE SSN = @SSN)
BEGIN
UPDATE XMLConversionTest
SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Gender = @Gender
WHERE SSN = @SSN
END

ELSE
BEGIN
INSERT XMLConversionTest(FirstName, LastName, SSN, Email, Gender)
VALUES (@FirstName, @LastName, @SSN, @Email, @Gender)

SET @ID = SCOPE_IDENTITY()
END
END
GO

--SELECT * FROM XMLConversionTest ORDER BY SSN

--DELETE FROM XMLConversionTest
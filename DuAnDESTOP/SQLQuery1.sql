
CREATE DATABASE CafeManager
GO

USE CafeManager
GO

-- Food
-- Table
-- FoodCategory
-- Account
-- Bill
-- BillInfo

CREATE TABLE TableFood
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Bàn chưa có tên',
	status NVARCHAR(100) NOT NULL DEFAULT N'Trống'	-- Trống || Có người
)
GO

CREATE TABLE Account
(
	UserName NVARCHAR(100) PRIMARY KEY,	
	DisplayName NVARCHAR(100) NOT NULL DEFAULT N'NO NAME',
	PassWord NVARCHAR(1000) NOT NULL DEFAULT 0,
	Type INT NOT NULL  DEFAULT 0 -- 1: admin && 0: staff
)
GO

CREATE TABLE FoodCategory
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên'
)
GO

CREATE TABLE Food
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idCategory INT NOT NULL,
	price FLOAT NOT NULL DEFAULT 0
	
	FOREIGN KEY (idCategory) REFERENCES dbo.FoodCategory(id)
)
GO

CREATE TABLE Bill
(
	id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE,
	idTable INT NOT NULL,
	status INT NOT NULL DEFAULT 0 -- 1: đã thanh toán && 0: chưa thanh toán
	
	FOREIGN KEY (idTable) REFERENCES dbo.TableFood(id)
)
GO

CREATE TABLE BillInfo
(
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idFood INT NOT NULL,
	count INT NOT NULL DEFAULT 0
	
	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id)
)
GO

-- thêm account
INSERT INTO dbo.Account
        ( UserName ,
          DisplayName ,
          PassWord ,
          Type)
VALUES  ( N'K9' , -- UserName - nvarchar(100)
          N'RongK9' , -- DisplayName - nvarchar(100)
          N'1' , -- PassWord - nvarchar(1000)
          1  -- Type - int
        )
INSERT INTO dbo.Account
        ( UserName ,
          DisplayName ,
          PassWord ,
          Type
        )
VALUES  ( N'staff' , -- UserName - nvarchar(100)
          N'staff' , -- DisplayName - nvarchar(100)
          N'1' , -- PassWord - nvarchar(1000)
          0  -- Type - int
        )

GO
-- thêm bàn
DECLARE @i INT = 1;
while (@i <= 10) 
	BEGIN 
		INSERT INTO TableFood(name) VALUES (N'Bàn ' + CAST(@i as nvarchar(100)));
		SET @i = @i + 1;
	END
GO

-- thêm loại đồ ăn
INSERT INTO FoodCategory(name) VALUES 
(N'Đồ uống'),
(N'Kem'),
(N'Đồ ăn vặt'),
(N'Trái cây')
GO

-- thêm đồ ăn
INSERT INTO Food (name, idCategory, price) VALUES
(N'Cafe Sữa', 1, 15.000),
(N'Cafe Đá', 1, 20.000),
(N'Trà Sữa', 1, 10.000),
(N'Kem Ốc Quế', 2, 25.000),
(N'Kem SôDa', 2, 19.000),
(N'Hạt Dưa', 3, 10.000),
(N'Khoai Tây Chiên', 3, 15.000),
(N'Táo(500G)', 4, 30.000),
(N'Vải(500G)', 4, 35.000),
(N'Mận(500G)', 4, 28.000)
GO

-- thêm BILL
INSERT INTO Bill (DateCheckIn, DateCheckOut, idTable, status) VALUES 
(GETDATE(), null, 5, 0)
INSERT INTO Bill (DateCheckIn, DateCheckOut, idTable, status) VALUES 
(GETDATE(), GETDATE(), 7, 1)
GO
select * from Bill

-- thêm BillInfo
INSERT INTO BillInfo(idBill, idFood, count) VALUES 
(1, 1, 4),
(1, 4, 3),
(1, 8, 4),
(2, 2, 4),
(2, 5, 4),
(2, 9, 1),
(2, 3, 4)
GO

select * from BillInfo


UPDATE TableFood SET status = N'Có Người' WHERE id = 7;

-- lấy acccount theo tên usertName
CREATE PROC USP_GetAccountByUserName
@userName nvarchar(100)
AS 
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName
END
GO

EXEC dbo.USP_GetAccountByUserName @userName = N'staff' -- nvarchar(100)

-- check tài khoản mật khẩu tồn tại
CREATE PROC USP_CHECK_AccountByUserNameAndPassword 
@userName nvarchar(100),
@passWord nvarchar(100)
AS
BEGIN 
	SELECT COUNT(*) FROM Account WHERE UserName = @userName and PassWord = @passWord;
END
GO
EXEC dbo.USP_CHECK_AccountByUserNameAndPassword @userName = N'K9', @passWord =  N'0';
--SELECT COUNT(*) FROM Account WHERE UserName = N'K9' and PassWord = N'0';

-- getALL tableFoot
CREATE PROC USP_GetAllTableFoot 
AS
BEGIN
	SELECT * FROM TableFood
END
GO
EXEC dbo.USP_GetAllTableFoot 


--lấy ra id bàng ăn chưa thanh toán 
ALTER PROC USP_GetBillUnpaid
@idTable INT
AS
BEGIN
	SELECT * FROM Bill WHERE idTable = @idTable AND status = 0;
END
GO

EXEC dbo.USP_GetBillUnpaid @idTable = 5


-- lấy ra kiểu dữ liệu cho C# mà không có đối tượng trong sql làm thành đối tượng cửa C#
CREATE PROC USP_GetBillAndFootFromat
@idTable int
AS
BEGIN 
	SELECT Food.name, BillInfo.count, Food.price, BillInfo.count * Food.price as TotalPrice FROM Food
	JOIN BillInfo on Food.id = BillInfo.id JOIN  Bill on Bill.id = BillInfo.idBill WHERE idTable = @idTable and status = 0;
END
GO

EXEC dbo.USP_GetBillAndFootFromat @idTable = 5


SELECT * FROM FoodCategory
SELECT * FROM Food

--- thuộc về nút thêm trong chương trình
ALTER PROC USP_CheckInsertBillInfo
@idBill INT, @idFood INT, @CountAdd INT
AS
BEGIN 
	DECLARE @CHECK INT; 
	DECLARE @countOld INT;
	SELECT @CHECK = COUNT(*) FROM BillInfo WHERE idBill = @idBill and idFood = @idFood;
	SELECT @countOld = BillInfo.count FROM BillInfo WHERE idBill = @idBill and idFood = @idFood;
	
	IF(@CHECK > 0) 
		BEGIN 
			UPDATE BillInfo SET count = @countOld + @CountAdd WHERE idBill = @idBill and idFood = @idFood;
			UPDATE BillInfo SET count = 0 WHERE count < 0 and idBill = @idBill and idFood = @idFood;
			DELETE FROM BillInfo WHERE idBill = @idBill and idFood = @idFood and count = 0;
		END
	ELSE 
		BEGIN 
			INSERT INTO BillInfo(idBill, idFood, count) VALUES (@idBill, @idFood, @CountAdd)
		END
END
go
EXEC USP_CheckInsertBillInfo @idBill = 4, @idFood = 10, @CountAdd = -4



UPDATE Bill SET DateCheckOut = GETDATE(), status = 1 WHERE idTable = 2;

-- chech bàng và update cái bang
CREATE TRIGGER UTG_UpdateBillByTable 
ON Bill FOR INSERT, UPDATE 
AS
BEGIN
	DECLARE @id_Bill INT

	SELECT @id_Bill = id FROM inserted

	DECLARE @id_Table INT

	DECLARE @status INT;
	
	SELECT @id_Table = idTable, @status = status FROM Bill WHERE @id_Bill = id;
	
	IF(@status = 0)
		BEGIN 
			UPDATE TableFood SET status = N'Có Người' WHERE id = @id_Table;
		END
	ELSE IF(@status = 1)
		BEGIN 
			UPDATE TableFood SET status = N'Trống' WHERE id = @id_Table;
		END
END

select * from bill
select * from BillInfo


delete BillInfo
delete Bill
UPDATE BillInfo SET 


-- chưa làm phần này
SELECT * INTO TableTmp FROM BillInfo WHERE idBill = 16
DROP TABLE TableTmp
UPDATE BillInfo SET idBill = 16 WHERE idBill = 17;
UPDATE BillInfo SET idBill = 17 WHERE id IN(SELECT id FROM TableTmp)
select * FROM TableTmp

CREATE PROC USP_CHUYENBAN 
@id_Table1 INT, @id_Table2 
AS
BEGIN
	DECLARE @idBill1 INT
	DECLARE @idBill2 INT
	SELECT FROM 
END

select * from Bill
ALTER TABLE BILL ADD totalPrice float not null default 0
ALTER TABLE BILL
select totalPrice from Bill

select  Bill.id ,name, DateCheckIn, DateCheckOut, Bill.totalPrice from Bill,TableFood 
WhERE TableFood.id = Bill.idTable and DateCheckIn >= '20250510' and DateCheckOut <= '20250515' and Bill.status = 1

select id from bill where idTable = 6 and status = 1


CREATE PROC USP_GETBillByDate
@DateCheckIn date, @DateCheckOut date
AS
BEGIN 
	select  Bill.id ,name, DateCheckIn, DateCheckOut, Bill.totalPrice from Bill,TableFood 
	WhERE TableFood.id = Bill.idTable
	and DateCheckIn >= @DateCheckIn and DateCheckOut <= @DateCheckOut and Bill.status = 1
END

EXEC USP_GETBillByDate @DateCheckIn = '20250510', @DateCheckOut ='20250515'

DELETE FROM BillInfo
DELETE FROM Bill

select name, idCategory, price from food
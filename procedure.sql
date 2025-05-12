USE TourismDB
GO

CREATE PROCEDURE CreateAccount
    @Username NVARCHAR(100),
    @Password NVARCHAR(100),
    @Email NVARCHAR(100),
	@OTP NVARCHAR(6)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewId NVARCHAR(10);
    DECLARE @MaxId NVARCHAR(10);

    -- Lấy ID lớn nhất hiện tại (ACCxxx)
    SELECT @MaxId = MAX(Id) FROM Accounts;

    -- Nếu chưa có tài khoản nào, gán là ACC001
    IF @MaxId IS NULL
        SET @NewId = 'ACC001';
    ELSE
        SET @NewId = 'ACC' + RIGHT('000' + CAST(CAST(SUBSTRING(@MaxId, 4, 3) AS INT) + 1 AS VARCHAR), 3);

    -- Thêm tài khoản mới
    INSERT INTO Accounts (Id, Username, Password, Email, Role, OTP, isConfirmed)
    VALUES (@NewId, @Username, @Password, @Email, 1, @OTP, 0);

    -- Trả về bản ghi mới tạo
    SELECT * FROM Accounts WHERE Id = @NewId;
END
GO

CREATE PROCEDURE CreateTour
    @Name NVARCHAR(100),
    @ShortDescription NVARCHAR(MAX),
    @DetailDescription NVARCHAR(MAX),
    @ScheduleDescription NVARCHAR(MAX),
    @CategoryId NVARCHAR(10),
    @Duration NVARCHAR(20),
    @Price DECIMAL(18, 2),
    @MaxCapacity INT,
	@LocationId NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NewId NVARCHAR(10), @MaxId NVARCHAR(10);

    SELECT @MaxId = MAX(Id) FROM Tours;
    IF @MaxId IS NULL
        SET @NewId = 'TOUR001';
    ELSE
        SET @NewId = 'TOUR' + RIGHT('000' + CAST(CAST(SUBSTRING(@MaxId, 5, 3) AS INT) + 1 AS VARCHAR), 3);

    INSERT INTO Tours (Id, Name, Short_description, Detail_description, Schedule_description, Category_Id, Duration, Price, Max_capacity, Create_at, Update_at, Location_id)
    VALUES (@NewId, @Name, @ShortDescription, @DetailDescription, @ScheduleDescription, @CategoryId, @Duration, @Price, @MaxCapacity, GETDATE(), GETDATE(), @LocationId );

    SELECT * FROM Tours WHERE Id = @NewId;
END

GO

CREATE PROCEDURE CreateImage
    @TourId NVARCHAR(10),
    @Url NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NewId NVARCHAR(10), @MaxId NVARCHAR(10);

    SELECT @MaxId = MAX(Id) FROM Images;
    IF @MaxId IS NULL
        SET @NewId = 'IMG001';
    ELSE
        SET @NewId = 'IMG' + RIGHT('000' + CAST(CAST(SUBSTRING(@MaxId, 4, 3) AS INT) + 1 AS VARCHAR), 3);

    INSERT INTO Images (Id, Tour_Id, Url)
    VALUES (@NewId, @TourId, @Url);

    SELECT * FROM Images WHERE Id = @NewId;
END

GO
CREATE PROCEDURE CreateSchedule
    @TourId NVARCHAR(10),
    @StartDate DATE,
    @Discount FLOAT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewId NVARCHAR(10), 
            @MaxId NVARCHAR(10),
            @MaxCapacity INT,
            @Price DECIMAL(18, 2),
            @AdultPrice DECIMAL(18, 2),
            @ChildrenPrice DECIMAL(18, 2);

    SELECT 
        @MaxCapacity = Max_capacity,
        @Price = Price
    FROM Tours
    WHERE Id = @TourId;

    SET @AdultPrice = @Price - (@Price * @Discount / 100);
    SET @ChildrenPrice =@Price - (@Price * 0.7 * @Discount / 100);

    SELECT @MaxId = MAX(Id) FROM Schedules;

    IF @MaxId IS NULL
        SET @NewId = 'SCHE001';
    ELSE
        SET @NewId = 'SCHE' + RIGHT('000' + CAST(CAST(SUBSTRING(@MaxId, 5, 3) AS INT) + 1 AS VARCHAR), 3);

    INSERT INTO Schedules (
        Id, Tour_Id, Start_date, Available, Status, 
        Adult_price, Children_price, Discount, Create_at
    )
    VALUES (
        @NewId, @TourId, @StartDate, @MaxCapacity, 1, 
        @AdultPrice, @ChildrenPrice, @Discount, GETDATE()
    );

    SELECT * FROM Schedules WHERE Id = @NewId;
END;

go

CREATE PROCEDURE UpdateScheduleDiscount
    @ScheduleId NVARCHAR(10),
    @Discount FLOAT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TourId NVARCHAR(10),
            @Price DECIMAL(18, 2),
            @AdultPrice DECIMAL(18, 2),
            @ChildrenPrice DECIMAL(18, 2);

    -- Lấy Tour_Id từ Schedule
    SELECT @TourId = Tour_Id
    FROM Schedules
    WHERE Id = @ScheduleId;

    -- Lấy giá gốc từ Tour
    SELECT @Price = Price
    FROM Tours
    WHERE Id = @TourId;

    -- Tính lại giá
    SET @AdultPrice = @Price - (@Price * @Discount / 100);
    SET @ChildrenPrice = @Price - (@Price * 0.7 * @Discount / 100);

    -- Cập nhật Schedule
    UPDATE Schedules
    SET 
        Discount = @Discount,
        Adult_price = @AdultPrice,
        Children_price = @ChildrenPrice
    WHERE Id = @ScheduleId;

    -- Trả về bản ghi đã cập nhật
    SELECT * FROM Schedules WHERE Id = @ScheduleId;
END;



CREATE PROCEDURE CreateCustomer
	--@Name NVARCHAR(100),
	--@DateOfBirth DATE,
	--@Gender BIT,
	@Email VARCHAR(100)
	--@Phone VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NewId VARCHAR(10);
	DECLARE @MaxId VARCHAR(10);

	SELECT @MaxId = MAX(Id) From Customers;

	IF @MaxId IS NULL
		SET @NewId = 'CUS001';
	ELSE
		SET @NewId = 'CUS' + RIGHT('000' + CAST(CAST(SUBSTRING(@MaxId, 4, 3) AS INT) + 1 AS VARCHAR), 3);

	INSERT INTO Customers (Id, Name, DateOfBirth, Gender, Email, Phone) 
	VALUES (@NewId, '', '', '', @Email, '');

	SELECT * FROM Customers WHERE Id = @NewId;
END

GO
CREATE PROCEDURE CreateBooking
	@CustomerId VARCHAR(10),
	@ScheduleId VARCHAR(10),
	@NumberOfAdultBookings INT,
	@NumberOfChildrenBookings INT,
	@TotalPrice DECIMAL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NewId VARCHAR(10);
	DECLARE @MaxId VARCHAR(10);
	DECLARE @NewAvailable INT;

	-- Lấy mã lớn nhất để tạo ID mới
	SELECT @MaxId = MAX(Id) FROM Bookings;

	IF @MaxId IS NULL
		SET @NewId = 'BOOK001';
	ELSE
		SET @NewId = 'BOOK' + RIGHT('000' + CAST(CAST(SUBSTRING(@MaxId, 5, 3) AS INT) + 1 AS VARCHAR), 3);

	-- Insert booking mới
	INSERT INTO Bookings (
		Id, Customer_Id, Schedule_Id, Booking_date, 
		Number_of_adult_bookings, Number_of_children_bookings, 
		Total_price, Status
	)
	VALUES (
		@NewId, @CustomerId, @ScheduleId, GETDATE(), 
		@NumberOfAdultBookings, @NumberOfChildrenBookings, 
		@TotalPrice, 1
	);

	-- Cập nhật available trong bảng Schedules
	UPDATE Schedules
	SET Available = Available - @NumberOfAdultBookings - @NumberOfChildrenBookings
	WHERE Id = @ScheduleId;

	-- Kiểm tra nếu Available = 0 thì cập nhật Status = 0
	SELECT @NewAvailable = Available FROM Schedules WHERE Id = @ScheduleId;

	IF @NewAvailable = 0
	BEGIN
		UPDATE Schedules
		SET Status = 0
		WHERE Id = @ScheduleId;
	END

	-- Trả về booking vừa tạo
	SELECT * FROM Bookings WHERE Id = @NewId;
END

go
CREATE PROCEDURE CreatePayment
	@BookingId VARCHAR(10),
	@Amount DECIMAL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NewId VARCHAR(10);
	DECLARE @MaxId VARCHAR(10);

	SELECT @MaxId = MAX(Id) From Payments;

	IF @MaxId IS NULL
		SET @NewId = 'PAY001';
	ELSE
		SET @NewId = 'PAY' + RIGHT('000' + CAST(CAST(SUBSTRING(@MaxId, 4, 3) AS INT) + 1 AS VARCHAR), 3);

	INSERT INTO Payments(Id, Booking_Id, Amount, Status, Payment_date)
	VALUES (@NewId, @BookingId, @Amount, 1, GETDATE());

	SELECT * FROM Payments WHERE Id = @NewId;
END
go
CREATE PROCEDURE CreateReview
    @TourId VARCHAR(10),
    @CustomerId VARCHAR(10),
    @Rating INT,
    @Comment NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewId VARCHAR(10);
    DECLARE @MaxId VARCHAR(10);

    SELECT @MaxId = MAX(Id) FROM Reviews;

    IF @MaxId IS NULL
        SET @NewId = 'REV001';
    ELSE
        SET @NewId = 'REV' + RIGHT('000' + CAST(CAST(SUBSTRING(@MaxId, 4, 3) AS INT) + 1 AS VARCHAR), 3);

    -- Thêm review mới
    INSERT INTO Reviews (Id, Tour_Id, Customer_Id, Rating, Comment, Create_at)
    VALUES (@NewId, @TourId, @CustomerId, @Rating, @Comment, GETDATE());

    -- Trả về review vừa tạo
    SELECT * FROM Reviews WHERE Id = @NewId;
END




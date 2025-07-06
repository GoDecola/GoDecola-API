-- Criação da tabela de usuários
CREATE TABLE UserTypes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(20) NOT NULL UNIQUE
);

-- Inserção dos tipos de usuários
INSERT INTO UserTypes (Name) VALUES
('Client'),
('Administrator'),
('Attendant'),
('Manager');

-- Tabela de usuários
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserTypeId INT NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Phone VARCHAR(20),
    CPF VARCHAR(30) UNIQUE,
    Passport VARCHAR(30) UNIQUE,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Users_UserType FOREIGN KEY (UserTypeId) REFERENCES UserTypes(Id)
);

-- Tabela de pacotes de viagem
CREATE TABLE TravelPackages (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    Destination VARCHAR(100) NOT NULL,
    Duration VARCHAR(50),
    BasePrice DECIMAL(10,2) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    IsActive BIT NOT NULL DEFAULT 1
);

-- Tabela de lista de desejos
CREATE TABLE WishList (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    PackageId INT NOT NULL,
    CONSTRAINT FK_WishList_User FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_WishList_Package FOREIGN KEY (PackageId) REFERENCES TravelPackages(Id)
);

-- Tabela de faixas de datas dos pacotes
CREATE TABLE PackageDateRanges (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PackageId INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_DateRange_Package FOREIGN KEY (PackageId) REFERENCES TravelPackages(Id)
);

-- Tabela de mídias dos pacotes
CREATE TABLE PackageMedia (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PackageId INT NOT NULL,
    MediaType VARCHAR(20) NOT NULL, 
    FileName VARCHAR(255) NOT NULL,
    Url VARCHAR(255) NOT NULL,
    CONSTRAINT FK_Media_Package FOREIGN KEY (PackageId) REFERENCES TravelPackages(Id)
);

-- Tabela de reservas
CREATE TABLE Reservations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PackageId INT NOT NULL,
    PackageDateRangeId INT NOT NULL,
    UserId INT NOT NULL,
    ReservationDate DATETIME NOT NULL DEFAULT GETDATE(),
    NumTravelers INT NOT NULL,
    Status VARCHAR(50) NOT NULL DEFAULT 'pending',
    TotalAmount DECIMAL(10,2) NOT NULL,
    ReservationCode VARCHAR(50) NOT NULL UNIQUE,
    CONSTRAINT FK_Reservation_Package FOREIGN KEY (PackageId) REFERENCES TravelPackages(Id),
    CONSTRAINT FK_Reservation_DateRange FOREIGN KEY (PackageDateRangeId) REFERENCES PackageDateRanges(Id),
    CONSTRAINT FK_Reservation_User FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Tabela de métodos de pagamento
CREATE TABLE PaymentMethods (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Type VARCHAR(50) NOT NULL UNIQUE 
);

-- Tabela de descontos
CREATE TABLE Discounts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Code VARCHAR(50) UNIQUE,
    Description VARCHAR(255) NOT NULL,
    Percentage INT NOT NULL,
    ValidFrom DATE NOT NULL,
    ValidUntil DATE NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

-- Tabela de pagamentos
CREATE TABLE Payments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ReservationId INT NOT NULL,
    PaymentMethodId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    PaymentDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status VARCHAR(50) NOT NULL DEFAULT 'pending', 
    DiscountAppliedId INT NULL,
    CONSTRAINT FK_Payment_Reservation FOREIGN KEY (ReservationId) REFERENCES Reservations(Id),
    CONSTRAINT FK_Payment_Method FOREIGN KEY (PaymentMethodId) REFERENCES PaymentMethods(Id),
    CONSTRAINT FK_Payment_Discount FOREIGN KEY (DiscountAppliedId) REFERENCES Discounts(Id)
);

-- Tabela de avaliações
CREATE TABLE Reviews (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PackageId INT NOT NULL,
    UserId INT NOT NULL,
    Rating INT NOT NULL,
    Comment TEXT,
    ReviewDate DATETIME NOT NULL DEFAULT GETDATE(),
    IsApproved BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Review_Package FOREIGN KEY (PackageId) REFERENCES TravelPackages(Id),
    CONSTRAINT FK_Review_User FOREIGN KEY (UserId) REFERENCES Users(Id)
);
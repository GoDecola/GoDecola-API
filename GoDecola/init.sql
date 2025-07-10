IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE TABLE [Discounts] (
        [Id] uniqueidentifier NOT NULL,
        [Code] nvarchar(max) NULL,
        [Description] nvarchar(max) NOT NULL,
        [Percentage] int NOT NULL,
        [ValidFrom] datetime2 NOT NULL,
        [ValidUntil] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Discounts] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE TABLE [PaymentMethods] (
        [Id] uniqueidentifier NOT NULL,
        [Type] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_PaymentMethods] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE TABLE [TravelPackages] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Destination] nvarchar(max) NOT NULL,
        [Duration] nvarchar(max) NULL,
        [BasePrice] decimal(10,2) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_TravelPackages] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE TABLE [UserTypes] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_UserTypes] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE TABLE [PackageDateRanges] (
        [Id] uniqueidentifier NOT NULL,
        [PackageId] uniqueidentifier NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NOT NULL,
        [Price] decimal(10,2) NOT NULL,
        [IsAvailable] bit NOT NULL,
        CONSTRAINT [PK_PackageDateRanges] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PackageDateRanges_TravelPackages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [TravelPackages] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE TABLE [PackageMedias] (
        [Id] uniqueidentifier NOT NULL,
        [PackageId] uniqueidentifier NOT NULL,
        [MediaType] nvarchar(max) NOT NULL,
        [FileName] nvarchar(max) NOT NULL,
        [Url] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_PackageMedias] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PackageMedias_TravelPackages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [TravelPackages] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] uniqueidentifier NOT NULL,
        [UserTypeId] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NULL,
        [CPF] nvarchar(max) NULL,
        [Passport] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Users_UserTypes_UserTypeId] FOREIGN KEY ([UserTypeId]) REFERENCES [UserTypes] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE TABLE [Reservations] (
        [Id] uniqueidentifier NOT NULL,
        [PackageId] uniqueidentifier NOT NULL,
        [PackageDateRangeId] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [ReservationDate] datetime2 NOT NULL,
        [NumTravelers] int NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [TotalAmount] decimal(10,2) NOT NULL,
        [ReservationCode] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Reservations] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Reservations_PackageDateRanges_PackageDateRangeId] FOREIGN KEY ([PackageDateRangeId]) REFERENCES [PackageDateRanges] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Reservations_TravelPackages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [TravelPackages] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Reservations_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE TABLE [Reviews] (
        [Id] uniqueidentifier NOT NULL,
        [PackageId] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Rating] int NOT NULL,
        [Comment] nvarchar(max) NULL,
        [ReviewDate] datetime2 NOT NULL,
        [IsApproved] bit NOT NULL,
        CONSTRAINT [PK_Reviews] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Reviews_TravelPackages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [TravelPackages] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Reviews_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE TABLE [WishLists] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [PackageId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_WishLists] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_WishLists_TravelPackages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [TravelPackages] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_WishLists_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE TABLE [Payments] (
        [Id] uniqueidentifier NOT NULL,
        [ReservationId] uniqueidentifier NOT NULL,
        [PaymentMethodId] uniqueidentifier NOT NULL,
        [Amount] decimal(10,2) NOT NULL,
        [PaymentDate] datetime2 NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [DiscountAppliedId] uniqueidentifier NULL,
        CONSTRAINT [PK_Payments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Payments_Discounts_DiscountAppliedId] FOREIGN KEY ([DiscountAppliedId]) REFERENCES [Discounts] ([Id]),
        CONSTRAINT [FK_Payments_PaymentMethods_PaymentMethodId] FOREIGN KEY ([PaymentMethodId]) REFERENCES [PaymentMethods] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Payments_Reservations_ReservationId] FOREIGN KEY ([ReservationId]) REFERENCES [Reservations] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PackageDateRanges_PackageId] ON [PackageDateRanges] ([PackageId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PackageMedias_PackageId] ON [PackageMedias] ([PackageId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Payments_DiscountAppliedId] ON [Payments] ([DiscountAppliedId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Payments_PaymentMethodId] ON [Payments] ([PaymentMethodId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Payments_ReservationId] ON [Payments] ([ReservationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Reservations_PackageDateRangeId] ON [Reservations] ([PackageDateRangeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Reservations_PackageId] ON [Reservations] ([PackageId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Reservations_UserId] ON [Reservations] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Reviews_PackageId] ON [Reviews] ([PackageId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Reviews_UserId] ON [Reviews] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Users_UserTypeId] ON [Users] ([UserTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_WishLists_PackageId] ON [WishLists] ([PackageId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_WishLists_UserId] ON [WishLists] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250709194647_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250709194647_InitialCreate', N'9.0.7');
END;

COMMIT;
GO


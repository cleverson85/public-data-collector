IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CurrencyRatesDb')
BEGIN
    CREATE DATABASE [CurrencyRatesDb];
END
GO

USE [CurrencyRatesDb];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CurrencyRates')
BEGIN
    CREATE TABLE [CurrencyRates] (
        [Id] uniqueidentifier NOT NULL,
        [TargetCurrency] nvarchar(450) NOT NULL,
        [Code] nvarchar(max) NOT NULL,
        [Rate] decimal(18,2) NOT NULL,
        [Date] datetime2 NOT NULL,
        CONSTRAINT [PK_CurrencyRates] PRIMARY KEY ([Id])
    );
END
GO
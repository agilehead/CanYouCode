USE [master]
IF EXISTS(SELECT * FROM sys.databases WHERE NAME = 'CanYouCodeDb')
BEGIN
	ALTER DATABASE [CanYouCodeDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE [CanYouCodeDb]
END
CREATE DATABASE [CanYouCodeDb]
GO

IF NOT EXISTS (SELECT * FROM sysdatabases WHERE (name = 'Roulette')) 
BEGIN
	create database Roulette
END

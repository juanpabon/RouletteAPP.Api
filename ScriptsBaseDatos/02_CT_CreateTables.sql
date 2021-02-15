--Use Roulette


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='roulette' AND user_name(uid)='dbo')
BEGIN 
	Create Table roulette
	(
		idRoulette int identity primary key,
		state int not null default 0,
		creationDate datetime not null default getdate(),
		openedDate datetime null,
		closedDate datetime null,
		numberWinner int null,
		colorWinner varchar(5) null
	)
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='buts' AND user_name(uid)='dbo')
BEGIN 
	Create Table bets
	(
		idBet int identity primary key,
		idUser int not null,
		moneyBet numeric(18,2) not null,
		number int null,
		color varchar(5) null,
		moneyWon numeric(18,2) null default 0,
		idRoulette int not null foreign key references roulette(idRoulette)
	)
END
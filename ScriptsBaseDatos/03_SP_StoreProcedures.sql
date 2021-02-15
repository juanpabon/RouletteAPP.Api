IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_AddRoulette' AND user_name(uid)='dbo')
DROP PROCEDURE [dbo].[sp_AddRoulette]
GO
	Create Procedure sp_AddRoulette
	@idRoulette int out
	As
	Insert Into roulette (state,creationDate)
	Values(0,CONVERT(NVARCHAR(30), GETDATE(), 126))

	Select @idRoulette=@@IDENTITY
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_OpenRoulette' AND user_name(uid)='dbo')
DROP PROCEDURE [dbo].[sp_OpenRoulette]
GO
	Create Procedure sp_OpenRoulette
	@idRoulette int
	As
	Update roulette
	Set 
		state=1,
		openedDate=CONVERT(NVARCHAR(30), GETDATE(), 126)
	Where idRoulette=@idRoulette	
Go
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ValidateBetRoulette' AND user_name(uid)='dbo')
DROP PROCEDURE [dbo].[sp_ValidateBetRoulette]
GO
	Create Procedure sp_ValidateBetRoulette
	@idRoulette int,
	@remainingMoney numeric(18,2) out
	As
	Select @remainingMoney=sum(moneyBet) from but where idRoulette=@idRoulette
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ValidateStateRoulette' AND user_name(uid)='dbo')
DROP PROCEDURE [dbo].[sp_ValidateStateRoulette]
GO
	Create Procedure sp_ValidateStateRoulette
	@idRoulette int,
	@state int output
	As
	Select @state=state from roulette where idRoulette=@idRoulette
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_AddBet' AND user_name(uid)='dbo')
DROP PROCEDURE [dbo].[sp_AddBet]
GO
	Create Procedure sp_AddBet
	@idRoulette int,
	@moneyBet numeric(18,2),
	@idUser int,
	@number int=null,
	@color varchar(5)=null
	As
	Insert Into bets (moneyBet,idUser,number,color,idRoulette)
	Values(@moneyBet,@idUser,@number,@color,@idRoulette)
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ClosedRoulette' AND user_name(uid)='dbo')
DROP PROCEDURE [dbo].[sp_ClosedRoulette]
GO
	Create Procedure sp_ClosedRoulette
	@idRoulette int,
	@numberWinner int,
	@colorWinner varchar(5)
	As
	Update roulette
	Set 
		state=3,
		numberWinner=@numberWinner,
		colorWinner=@colorWinner,
		closedDate=CONVERT(NVARCHAR(30), GETDATE(), 126)
	Where idRoulette=@idRoulette
	
	Update but
	Set
		moneyBet=moneyBet*5
	Where idRoulette=@idRoulette and number=@numberWinner
	
	Update but
	Set
		moneyBet=moneyBet*1.8
	Where idRoulette=@idRoulette and color=@colorWinner	
Go
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ListBetsRoulette' AND user_name(uid)='dbo')
DROP PROCEDURE [dbo].[sp_ListBetsRoulette]
GO
	Create Procedure sp_ListBetsRoulette
	@idRoulette int
	As
	Select idBet,idUser,moneyBet,number,color,moneyWon from bet 
	Where idRoulette=@idRoulette
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_ListRoulettes' AND user_name(uid)='dbo')
DROP PROCEDURE [dbo].[sp_ListRoulettes]
GO
	Create Procedure sp_ListRoulettes
	As
	Select idRoulette,case When state=0 then 'Created' when state=1 then 'Opened' else 'Closed' end state from roulette
GO
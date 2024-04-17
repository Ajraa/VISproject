CREATE OR ALTER PROCEDURE [dbo].[SpJoinTeam](@player_id INT, 
                                            @team_id INT) 
AS
BEGIN
	DECLARE @p_avg INT; 
	
	BEGIN TRANSACTION;

	BEGIN TRY
		INSERT INTO Player_team(player_id, team_id)
		VALUES(@player_id, @team_id);

		SET @p_avg = ( 
			SELECT avg(Player.rating)
			FROM Player_team 
			JOIN Player ON player_team.player_id = Player.player_id 
			WHERE Player_team.team_id = @team_id
		);

		UPDATE TEAM
		SET rating =  @p_avg  WHERE team_id = @team_id;

		COMMIT
		RETURN @p_avg;
	END TRY
	BEGIN CATCH
		ROLLBACK
		RETURN -1
	END CATCH
END;
CREATE OR ALTER PROCEDURE [dbo].[SpGenerateXML] (
	@player_id INT,
	@team_id INT,
	@ret VARCHAR(MAX) OUTPUT
)
AS
BEGIN
	DECLARE @sql NVARCHAR(300);

	IF NOT EXISTS(SELECT * FROM player where player_id = @player_id)
	BEGIN
		SET @ret = 'NO_DATA';
		RETURN;
	END;

	BEGIN TRY	
		SET @ret = (
			SELECT '<player>' + CHAR(10) +
					CHAR(9) + '<player_id>' + CONVERT(VARCHAR(5), player_id) + '</player_id>' + CHAR(10) +
					CHAR(9) + '<first_name>' + first_name + '</first_name>' + CHAR(10) +
					CHAR(9) + '<last_name>' + last_name + '</last_name>' + CHAR(10) +
					CHAR(9) + '<nickname>' + nickname + '</nickname>' + CHAR(10) +
					CHAR(9) + '<rating>' + CONVERT(VARCHAR(6), rating) + '</rating>' + CHAR(10)
			FROM player
			WHERE player_id = @player_id
		);

		CREATE TABLE TempTeams (
			id INT NOT NULL,
			team_name VARCHAR(50) NOT NULL,
			tag char(3) NOT NULL,
			captain INT,
			rating INT
		);

		SET @sql = 'INSERT INTO TempTeams SELECT t.team_id, t.team_name, t.tag, t.captain, t.rating FROM team t JOIN player_team p ON t.team_id = p.team_id WHERE p.player_id = ' + CONVERT(VARCHAR(5),@player_id);

		IF @team_id IS NOT NULL
			SET @sql = @sql + ' AND p.team_id = ' + CONVERT(VARCHAR(5),@team_id);

		EXECUTE sp_executesql @sql;

		DECLARE @t_tid INT;
		DECLARE @t_tname VARCHAR;
		DECLARE @t_tag CHAR(3);
		DECLARE @t_trating INT;
		DECLARE @t_tpid INT;
		DECLARE @t_firstname VARCHAR;
		DECLARE @t_lastname VARCHAR;
		DECLARE @t_nickname VARCHAR;
		DECLARE @t_prating INT;

		DECLARE c_team CURSOR FOR
		SELECT tt.id, tt.team_name, tt.tag, tt.rating, p.player_id, p.first_name, p.last_name, p.nickname, p.rating
		FROM TempTeams tt JOIN player p ON tt.captain = p.player_id;

		OPEN c_team;

		FETCH NEXT FROM c_team INTO @t_tid, @t_tname, @t_tag, @t_trating, @t_tpid, @t_firstname, @t_lastname, @t_nickname, @t_prating;

		SET @ret = @ret + CHAR(9) + '<teams>' + CHAR(10);
		WHILE @@FETCH_STATUS = 0
		BEGIN

		SET @ret = @ret +
			CHAR(9) + '<team>' + CHAR(10) +
				CHAR(9) + CHAR(9) + '<team_id>' +  CONVERT(VARCHAR(5),@t_tid) + '</team_id>' + CHAR(10) +
				CHAR(9) + CHAR(9) + '<team_name>' + @t_tname + '</team_name>' + CHAR(10) +
				CHAR(9) + CHAR(9) + '<team_tag>' + @t_tag  + '</team_tag>' + CHAR(10) +
				CHAR(9) + CHAR(9) + '<team_rating>' +  CONVERT(VARCHAR(6),@t_trating) + '</team_rating>' + CHAR(10) +
				CHAR(9) + CHAR(9) + '<captain>' + CHAR(10) +
					CHAR(9) + CHAR(9) + CHAR(9) + '<player_id>' +  CONVERT(VARCHAR(5),@t_tpid) + '</player_id>' + CHAR(10) +
					CHAR(9) + CHAR(9) + CHAR(9) + '<first_name>' + @t_firstname + '</first_name>' + CHAR(10) +
					CHAR(9) + CHAR(9) + CHAR(9) + '<last_name>' + @t_lastname + '</last_name>' + CHAR(10) +
					CHAR(9) + CHAR(9) + CHAR(9) + '<nickname>' + @t_nickname + '</nickname>' + CHAR(10) +
					CHAR(9) + CHAR(9) + CHAR(9) + '<rating>' +  CONVERT(VARCHAR(6),@t_prating) + '</rating>' + CHAR(10) +
				CHAR(9) + CHAR(9) + '</captain>' + CHAR(10) +
			CHAR(9) + '</team>' + CHAR(10);

		FETCH NEXT FROM c_team INTO @t_tid, @t_tname, @t_tag, @t_trating, @t_tpid, @t_firstname, @t_lastname, @t_nickname, @t_prating;
		END
		SET @ret = @ret + CHAR(9) + '</teams>' + CHAR(10);

		CLOSE c_team;
		DEALLOCATE c_team;

		SET @ret = @ret + '</player>';
		IF OBJECT_ID('TempTeams', 'U') IS NOT NULL
			DROP TABLE TempTeams;
	END TRY
	BEGIN CATCH
		IF OBJECT_ID('TempTeams', 'U') IS NOT NULL
			DROP TABLE TempTeams;
		SET @ret = 'ERROR';
		RETURN;
	END CATCH
END;
CREATE TABLE player (
	player_id INT IDENTITY PRIMARY KEY NOT NULL,
	first_name VARCHAR (50) NOT NULL,
	last_name VARCHAR (50) NOT NULL,
	nickname VARCHAR (20) NOT NULL,
	password VARCHAR (20) NOT NULL,
	rating INT NOT NULL
)
GO

CREATE TABLE team (
	team_id INT IDENTITY PRIMARY KEY NOT NULL,
	team_name VARCHAR (50) NOT NULL,
	tag CHAR (3) NOT NULL,
	captain INT NOT NULL CONSTRAINT FK_player_team FOREIGN KEY REFERENCES player (player_id),
	rating INT NOT NULL
)
GO

CREATE TABLE Player_team (
	player_id INT NOT NULL CONSTRAINT FK_player_team_player FOREIGN KEY REFERENCES player (player_id),
	team_id INT NOT NULL CONSTRAINT FK_player_team_team FOREIGN KEY REFERENCES team (team_id)
)
GO

CREATE TABLE founder (
	founder_id INT IDENTITY PRIMARY KEY NOT NULL,
	first_name VARCHAR (50) NOT NULL,
	last_name VARCHAR (50) NOT NULL,
	password VARCHAR (20) NOT NULL,
	nickname VARCHAR (20) NOT NULL
)
GO

CREATE TABLE tournament (
	tournament_id INT IDENTITY PRIMARY KEY NOT NULL,
	name VARCHAR (50) NOT NULL,
	price INT NULL,
	prize INT NULL,
	min_rating INT NULL,
	max_rating INT NULL,
	founder_id INT NOT NULL CONSTRAINT FK_tournament_founder FOREIGN KEY REFERENCES founder (founder_id),
	winner INT NULL CONSTRAINT FK_tournament_team FOREIGN KEY REFERENCES team (team_id)
)
GO

CREATE TABLE tournament_team (
	tournament_id INT NOT NULL CONSTRAINT FK_tournament_team_tournament FOREIGN KEY REFERENCES tournament (tournament_id),
	team_id INT NOT NULL CONSTRAINT FK_tournament_team_team FOREIGN KEY REFERENCES team (team_id)
)
GO

CREATE TABLE match (
	match_id INT IDENTITY PRIMARY KEY NOT NULL,
	tournament_id INT NOT NULL CONSTRAINT FK_match_tournament FOREIGN KEY REFERENCES tournament (tournament_id),
	match_state VARCHAR (9) NOT NULL,
	winner INT NULL CONSTRAINT FK_match_team FOREIGN KEY REFERENCES team (team_id)
)
GO

CREATE TABLE team_match (
	team_id INT NOT NULL CONSTRAINT FK_team_match_team FOREIGN KEY REFERENCES team (team_id),
	match_id INT NOT NULL CONSTRAINT FK_team_match_match FOREIGN KEY REFERENCES match (match_id)
)
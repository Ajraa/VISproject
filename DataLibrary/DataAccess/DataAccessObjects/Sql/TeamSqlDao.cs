using Dapper;
using DataLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess.DataAccessObjects.Sql
{
    internal class TeamSqlDao : ITeamDao
    {
        public void ChangeCaptain(int player_id, int team_id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "UPDATE team " +
                    "SET captain = @player_id " +
                    "WHERE team_id = @team_id";
                var parameters = new DynamicParameters();
                parameters.Add("@team_id", team_id);
                parameters.Add("@player_id", player_id);
                connection.Execute(sql, parameters);
            }
        }

        public TeamModel CreateTeam(TeamModel tm)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "INSERT INTO team(team.team_name, team.tag, team.captain, team.rating) " +
                    "OUTPUT INSERTED.team_id " +
                    "VALUES (@name, @tag, @captain, @rating)";
                var parameters = new DynamicParameters();
                parameters.Add("@name", tm.Team_Name);
                parameters.Add("@tag", tm.Tag);
                parameters.Add("@captain", GlobalConfig.CurrentPlayer.Player_Id);
                parameters.Add("@rating", tm.Rating);

                tm.Team_Id = connection.ExecuteScalar<int>(sql, parameters);

                tm.Players.Add(GlobalConfig.CurrentPlayer);
                GlobalConfig.CurrentPlayer.Teams.Add(tm);

                string sql2 = "INSERT INTO Player_team(player_team.player_id, player_team.team_id) " +
                    "VALUES (@player, @team)";
                var parameters2 = new DynamicParameters();
                parameters2.Add("@player", GlobalConfig.CurrentPlayer.Player_Id);
                parameters2.Add("@team", tm.Team_Id);

                connection.Execute(sql2, parameters2);
            }
            return tm;
        }

        public TeamModel GetById(int v)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
               
                string sql = "SELECT team.team_id, team.tag, team.team_name, team.rating, player.player_id, player.first_name, player.last_name, player.nickname, player.password, player.rating " +
                    "FROM team " +
                    "WHERE team_id = @id";

                var parameters = new DynamicParameters();
                parameters.Add("@id", v);
                var team = connection.Query<TeamModel, PlayerModel, TeamModel>(sql,
                    map: (team, captain) =>
                    {
                        team.Captain = captain;
                        return team;
                    },
                    parameters, splitOn: "player_id").FirstOrDefault();
                return team;
            }
        }

        public List<TeamModel> GetTeams_All()
        {
            List<TeamModel> teams = new List<TeamModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "SELECT team.team_id, team.tag, team.team_name, team.rating, player.player_id, player.first_name, player.last_name, player.nickname, player.password, player.rating " +
                    "FROM team " +
                    "JOIN Player_team ON Player_team.team_id = team.team_id " +
                    "JOIN player ON Player_team.player_id = player.player_id ";
                    
                var parameters = new DynamicParameters();
                teams = connection.Query<TeamModel, PlayerModel, TeamModel>(sql,
                    map: (team, captain) =>
                    {
                        team.Captain = captain;
                        return team;
                    },
                    splitOn: "player_id").ToList();
            }
            return teams;
        }

        public List<TeamModel> GetTeams_Except(int id)
        {
            List<TeamModel> teams = new List<TeamModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "SELECT team.team_id, team.tag, team.team_name, team.rating, player.player_id, player.first_name, player.last_name, player.nickname, player.password, player.rating " +
                    "FROM team " +
                    "JOIN Player_team ON Player_team.team_id = team.team_id " +
                    "JOIN player ON team.captain = player.player_id " +
                    "EXCEPT " +
                    "SELECT team.team_id, team.tag, team.team_name, team.rating, player.player_id, player.first_name, player.last_name, player.nickname, player.password, player.rating " +
                    "FROM team " +
                    "JOIN Player_team ON Player_team.team_id = team.team_id " +
                    "JOIN player ON team.captain = player.player_id " +
                    "WHERE Player_team.player_id = @id";
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                teams = connection.Query<TeamModel, PlayerModel, TeamModel>(sql,
                     map: (team, captain) =>
                     {
                         team.Captain = captain;
                         return team;
                     },
                     parameters, splitOn: "player_id").ToList();
            }
           
            return teams;
        }

        public List<TeamModel> GetTeams_Match(int id)
        {
            List<TeamModel> teams = new List<TeamModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
               string sql = "SELECT team.team_id, team.tag, team.team_name, team.rating, team.captain, player.player_id, player.first_name, player.last_name, player.nickname, player.password, player.rating " +
                    "FROM team " +
                    "JOIN Player_team ON Player_team.team_id = team.team_id " +
                    "JOIN player ON player.player_id = team.captain " +
                    "JOIN team_match ON team_match.team_id = team.team_id " +
                    "WHERE team_match.match_id = @matchId";

                var parameters = new DynamicParameters();
                parameters.Add("@matchId", id);

                teams = connection.Query<TeamModel, PlayerModel, TeamModel>(sql,
                    map: (team, player) =>
                    {
                        team.Captain = player;
                        return team;
                    },
                    parameters, splitOn: "captain").ToList();
            }
            return teams;
        }

        public List<TeamModel> GetTeams_MatchExcept(int tournament_id, int match_id)
        {
            List<TeamModel> teams = new List<TeamModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "SELECT team.team_id, team.tag, team.team_name, team.rating, team.captain, player.player_id, player.first_name, player.last_name, player.nickname, player.password, player.rating " +
                    "FROM team " +
                    "JOIN Player_team ON Player_team.team_id = team.team_id " +
                    "JOIN player ON player.player_id = team.captain " +
                    "JOIN tournament_team ON tournament_team.team_id = team.team_id " +
                    "WHERE tournament_team.tournament_id = @tournament_id " +
                    "EXCEPT " +
                    "SELECT team.team_id, team.tag, team.team_name, team.rating, team.captain, player.player_id, player.first_name, player.last_name, player.nickname, player.password, player.rating " +
                    "FROM team " +
                    "JOIN Player_team ON Player_team.team_id = team.team_id " +
                    "JOIN player ON player.player_id = team.captain " +
                    "JOIN tournament_team ON tournament_team.team_id = team.team_id " +
                    "JOIN team_match ON team_match.team_id = team.team_id " +
                    "WHERE tournament_team.tournament_id = @tournament_id AND team_match.match_id = @match_id";

                var parameters = new DynamicParameters();
                parameters.Add("@tournament_id", tournament_id);
                parameters.Add("@match_id", match_id);

                teams = connection.Query<TeamModel, PlayerModel, TeamModel>(sql,
                    map: (team, player) =>
                    {
                        team.Captain = player;
                        return team;
                    },
                    parameters, splitOn: "captain").ToList();
            }
            return teams;
        }

        public List<TeamModel> GetTeams_Player(int id)
        {
            List<TeamModel> teams = new List<TeamModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db))) 
            {
                string sql = "SELECT team.team_id, team.tag, team.team_name, team.rating, team.captain, player.player_id, player.first_name, player.last_name, player.nickname, player.password, player.rating " +
                    "FROM team " +
                    "JOIN Player_team ON Player_team.team_id = team.team_id " +
                    "JOIN player ON player.player_id = team.captain " +
                    "WHERE player_team.player_id = @id";

                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                teams = connection.Query<TeamModel, PlayerModel, TeamModel>(sql,
                    map: (team, player) =>
                    {
                        team.Captain = player;
                        return team;
                    },
                    parameters, splitOn: "captain").ToList();
            }
            return teams;
        }

        public List<TeamModel> GetTeams_Tournament(int id)
        {
            List<TeamModel> teams = new List<TeamModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "SELECT team.team_id, team.tag, team.team_name, team.rating, team.captain, player.player_id, player.first_name, player.last_name, player.nickname, player.password, player.rating " +
                    "FROM team " +
                    "JOIN Player_team ON Player_team.team_id = team.team_id " +
                    "JOIN player ON player.player_id = team.captain " +
                    "JOIN tournament_team ON tournament_team.team_id = team.team_id " +
                    "WHERE tournament_team.tournament_id = @id";

                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                teams = connection.Query<TeamModel, PlayerModel, TeamModel>(sql,
                    map: (team, player) =>
                    {
                        team.Captain = player;
                        return team;
                    },
                    parameters, splitOn: "captain").ToList();
            }
            return teams;
        }

        public void JoinTournament(int team_id, int tournament_id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "INSERT INTO tournament_team (tournament_id, team_id)" +
                    "VALUES (@tournament_id, @team_id)";
                var parameters = new DynamicParameters();
                parameters.Add("@tournament_id", tournament_id);
                parameters.Add("@team_id", team_id);
                connection.Execute(sql, parameters);
            }
        }

        public int KickPlayer(int player_id, int team_id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "DELETE Player_team " +
                    "WHERE team_id = @team_id AND player_id = @player_id";
                var parameters = new DynamicParameters();
                parameters.Add("@team_id", team_id);
                parameters.Add("@player_id", player_id);
                connection.Execute(sql, parameters);

                sql = "UPDATE team " +
                   "SET rating = ( " +
                   "SELECT sum(rating) / count(*) " +
                   "FROM player " +
                   "JOIN Player_team ON player.player_id = Player_team.player_id " +
                   "WHERE Player_team.team_id = team.team_id " +
                   ") WHERE team.team_id = @team_id";
                parameters = new DynamicParameters();
                parameters.Add("@team_id", team_id);
                connection.Execute(sql, parameters);

                sql = "SELECT team.rating FROM team WHERE team.team_id = @team_id";
                return connection.ExecuteScalar<int>(sql, parameters);
            }
        }

        public void UpdateTeam(TeamModel tm)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "UPDATE team " +
                    "SET team_name = @teamName, tag = @tag " +
                    "WHERE team_id = @teamId";
                var parameters = new DynamicParameters();
                parameters.Add("@teamName", tm.Team_Name);
                parameters.Add("@tag", tm.Tag);
                parameters.Add("@teamId", tm.Team_Id);

                connection.Execute(sql, parameters);
            }
        }
    }
}

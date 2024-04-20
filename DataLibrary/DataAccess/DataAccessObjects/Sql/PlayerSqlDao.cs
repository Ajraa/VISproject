using System;
using System.Data;
using DataLibrary.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Model;
using System.Data.SqlClient;
using Dapper;
using System.Diagnostics;

namespace DataLibrary.DataAccess.DataAccessObjects.Sql
{
    public class PlayerSqlDao : IPlayerDao
    {
        public PlayerModel CreatePlayer(PlayerModel pm)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "INSERT INTO Player(first_name, last_name, nickname, password, rating) " +
                    "OUTPUT INSERTED.player_id " +
                    "VALUES(@firstName, @lastName, @nickName, @password, @rating)";
                var parameters = new DynamicParameters();
                parameters.Add("@firstName", pm.First_Name);
                parameters.Add("@lastName", pm.Last_Name);
                parameters.Add("@nickName", pm.NickName);
                parameters.Add("@password", pm.Password);
                parameters.Add("@rating", pm.Rating);

                pm.Teams = new List<TeamModel>();
                pm.Player_Id = connection.ExecuteScalar<int>(sql, parameters);
            }
            return pm;
        }

        public PlayerModel GetById(int v)
        {
            PlayerModel pm = null;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            { 

            }
            return pm;
        }

        public List<PlayerModel> GetPlayers_All()
        {
            List<PlayerModel> pm = new List<PlayerModel>();
            
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                
            }
            return pm;
        }

        public List<PlayerModel> GetPlayers_Team(int id)
        {
            List<PlayerModel> pm = new List<PlayerModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "SELECT player.player_id, player.first_name, player.last_name, player.nickname, player.password, player.rating " +
                             "FROM player " +
                             "JOIN Player_team ON player.player_id = Player_team.player_id " +
                             "WHERE Player_team.team_id = @id";
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                pm = connection.Query<PlayerModel>(sql, parameters).ToList();
            }
            return pm;
        }

        public int JoinTeam(int team_id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string sql = "INSERT INTO Player_team (player_id, team_id) " +
                                "VALUES (@player_id, @team_id)";
                        var parameters = new DynamicParameters();

                        parameters.Add("@player_id", GlobalConfig.CurrentPlayer.Player_Id);
                        parameters.Add("@team_id", team_id);
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

                        var ret = connection.ExecuteScalar<int>(sql, parameters);
                        transaction.Commit();
                        return ret;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return -1;
                    }
                }
            }
        }

        public PlayerModel Login(string username, string password)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "SELECT * " +
                    "FROM player " +
                    "WHERE nickname = @username AND password = @password";
                var parameters = new DynamicParameters();
                parameters.Add("@username", username);
                parameters.Add("@password", password);
                try
                {
                    var pm = connection.QuerySingle<PlayerModel>(sql, parameters);
                    pm.Teams = GlobalConfig.Connection.TeamDao.GetTeams_Player(pm.Player_Id);
                    return pm;
                }
                catch
                {
                    return null;
                }



            }
        }

        public void UpdatePlayer()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "UPDATE player " +
                    "SET player.first_name = @first_name, player.last_name = @last_name, player.nickname = @nickname, player.password = @password " +
                    "WHERE player.player_id = @player_id";

                var parameters = new DynamicParameters();
                parameters.Add("@first_name", GlobalConfig.CurrentPlayer.First_Name);
                parameters.Add("@last_name", GlobalConfig.CurrentPlayer.Last_Name);
                parameters.Add("@nickname", GlobalConfig.CurrentPlayer.NickName);
                parameters.Add("@password", GlobalConfig.CurrentPlayer.Password);
                parameters.Add("@player_id", GlobalConfig.CurrentPlayer.Player_Id);

                connection.Execute(sql, parameters);
            }
        }

        public string GenerateXml(int player_id, int? team_id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                var parameters = new DynamicParameters();
                parameters.Add("player_id", player_id);
                parameters.Add("team_id", team_id);
                parameters.Add("ret", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);
                connection.Execute("SpGenerateXML", parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<string>("ret");
            }
        }
    }
}

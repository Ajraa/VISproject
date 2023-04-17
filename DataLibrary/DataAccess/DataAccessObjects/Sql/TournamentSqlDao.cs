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
    internal class TournamentSqlDao : ITournamentDao
    {
        public TournamentModel CreateTournament(TournamentModel tm)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "INSERT into tournament(name, price, prize, min_rating, max_rating, founder_id, winner) " +
                    "OUTPUT INSERTED.tournament_id " +
                    "VALUES (@name, @price, @prize, @min_rating, @max_rating, @founder_id, NULL)";
                var parameters = new DynamicParameters();
                parameters.Add("@name", tm.Name);
                parameters.Add("@price", tm.Price);
                parameters.Add("@prize", tm.Prize);
                parameters.Add("@min_rating", tm.Min_Rating);
                parameters.Add("@max_rating", tm.Max_Rating);
                parameters.Add("@founder_id", GlobalConfig.CurrentFounder.Founder_Id);

                tm.Tournament_Id = connection.ExecuteScalar<int>(sql, parameters);
            }
            return tm;
        }

        public TournamentModel GetById(int v)
        {
            throw new NotImplementedException();
        }

        public List<TournamentModel> GetTournaments_All()
        {
            List<TournamentModel> tournaments = new List<TournamentModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "SELECT tournament.tournament_id, tournament.name, tournament.price, tournament.prize, tournament.prize, tournament.min_rating, tournament.max_rating, " +
                    "team.team_id, team.team_name, team.tag, team.captain, " +
                    "founder.founder_id, founder.first_name, founder.last_name, founder.password, founder.nickname, " +
                    "player.player_id, player.first_name, player.nickname, player.last_name, player.rating " +
                    "FROM tournament " +
                    "JOIN founder ON founder.founder_id = tournament.founder_id " +
                    "LEFT JOIN team ON team.team_id = tournament.winner " +
                    "LEFT JOIN player ON player.player_id = team.captain ";

               
                tournaments = connection.Query<TournamentModel, TeamModel, FounderModel, PlayerModel, TournamentModel>(sql,
                    map: (tournament, team, founder, player) =>
                    {
                        team.Captain = player;
                        tournament.winner = team;
                        tournament.Founder = founder;
                        return tournament;
                    },
                    splitOn: "team_id, founder_id, player_id").ToList();
            }
            return tournaments;
        }

        public List<TournamentModel> GetTournaments_Except(int id, int rating)
        {
            List<TournamentModel> tournaments = new List<TournamentModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "SELECT tournament.tournament_id, tournament.name, tournament.price, tournament.prize, tournament.prize, tournament.min_rating, tournament.max_rating, " +
                    "team.team_id, team.team_name, team.tag, team.captain, " +
                    "founder.founder_id, founder.first_name, founder.last_name, founder.password, founder.nickname, " +
                    "player.player_id, player.first_name, player.nickname, player.last_name, player.rating " +
                    "FROM tournament " +
                    "JOIN founder ON founder.founder_id = tournament.founder_id " +
                    "LEFT JOIN team ON team.team_id = tournament.winner " +
                    "LEFT JOIN player ON player.player_id = team.captain " +
                    "EXCEPT " +
                    "SELECT tournament.tournament_id, tournament.name, tournament.price, tournament.prize, tournament.prize, tournament.min_rating, tournament.max_rating, " +
                    "team.team_id, team.team_name, team.tag, team.captain, " +
                    "founder.founder_id, founder.first_name, founder.last_name, founder.password, founder.nickname, " +
                    "player.player_id, player.first_name, player.nickname, player.last_name, player.rating " +
                    "FROM tournament " +
                    "JOIN founder ON founder.founder_id = tournament.founder_id " +
                    "LEFT JOIN team ON team.team_id = tournament.winner " +
                    "LEFT JOIN player ON player.player_id = team.captain " +
                    "JOIN tournament_team ON tournament.tournament_id = tournament_team.tournament_id " +
                    "WHERE tournament_team.team_id = @id AND tournament.min_rating <= @rating";

                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@rating", rating);
                tournaments = connection.Query<TournamentModel, TeamModel, FounderModel, PlayerModel, TournamentModel>(sql,
                    map: (tournament, team, founder, player) =>
                    {
                        team.Captain = player;
                        tournament.winner = team;
                        tournament.Founder = founder;
                        return tournament;
                    },
                    parameters, splitOn: "max_rating, captain, nickname").ToList();
            }
            return tournaments;
        }

        public List<TournamentModel> GetTournaments_Founder(int id)
        {
            List<TournamentModel> tournaments = new List<TournamentModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "SELECT tournament.tournament_id, tournament.name, tournament.price, tournament.prize, tournament.prize, tournament.min_rating, tournament.max_rating, " +
                    "team.team_id, team.team_name, team.tag, team.captain, " +
                    "founder.founder_id, founder.first_name, founder.last_name, founder.password, founder.nickname, " +
                    "player.player_id, player.first_name, player.nickname, player.last_name, player.rating " +
                    "FROM tournament " +
                    "JOIN founder ON founder.founder_id = tournament.founder_id " +
                    "LEFT JOIN team ON team.team_id = tournament.winner " +
                    "LEFT JOIN player ON player.player_id = team.captain " +
                    "WHERE tournament.founder_id = @id";

                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                tournaments = connection.Query<TournamentModel, TeamModel, FounderModel, PlayerModel, TournamentModel>(sql,
                    map: (tournament, team, founder, player) =>
                    {
                        team.Captain = player;
                        tournament.winner = team;
                        tournament.Founder = founder;
                        return tournament;
                    },
                    parameters, splitOn: "max_rating, captain, nickname").ToList();
            }
            return tournaments;
        }

        public void removeMatch(int match_id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "REMOVE team_match " +
                    "WHERE match_id = @matchId";

                var parameters = new DynamicParameters();
                parameters.Add("@matchId", match_id);

                connection.Execute(sql, parameters);

                sql = "REMOVE match " +
                    "WHERE match_id = @matchId";
                connection.Execute(sql, parameters);

            }
        }

        public void removeTeam(int tournament_id, int team_id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "REMOVE team_tournament " +
                    "WHERE team_id = @teamId AND tournament_id = @tournamentID";

                var parameters = new DynamicParameters();
                parameters.Add("@teamId", team_id);
                parameters.Add("@tournamentId", tournament_id);

                connection.Execute(sql, parameters);
            }
        }

        public void setWinner(int tournament_id, int team_id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db))) 
            {
                string sql = "UPDATE tournament " +
                    "SET winner = @teamId " +
                    "WHERE tournament_id = @tournamentId";
                var parameters = new DynamicParameters();
                parameters.Add("@teamId", team_id);
                parameters.Add("@tournamentId", tournament_id);

                connection.Execute(sql, parameters);
            }
        }

        public void UpdateTournament(TournamentModel tm)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "UPDATE tournament " +
                    "SET name = @name, price = @price, prize = @prize, min_rating = @min_rating, max_rating = @max_rating " +
                    "WHERE tournament_id = @tournamentId";

                var parameters = new DynamicParameters();
                parameters.Add("@name", tm.Name);
                parameters.Add("@price", tm.Price);
                parameters.Add("@prize", tm.Prize);
                parameters.Add("@min_rating", tm.Min_Rating);
                parameters.Add("@max_rating", tm.Max_Rating);
                parameters.Add("@tournament_id", tm.Tournament_Id);

                connection.Execute(sql, parameters);
            }
                
        }
    }
}

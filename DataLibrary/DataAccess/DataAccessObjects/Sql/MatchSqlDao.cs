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
    internal class MatchSqlDao : IMatchDao
    {
        public void addTeam(int team_id, int match_id)
        {

            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "INSERT INTO team_match (team_id, match_id) " +
                    "VALUES (@teamId, @matchId)";
                var parameters = new DynamicParameters();
                parameters.Add("@teamId", team_id);
                parameters.Add("@matchId", match_id);

                connection.Execute(sql, parameters);
            }
        }

        public int CreateMatch(MatchModel mm)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "INSERT INTO match (tournament_id, match_state, winner) " +
                    "OUTPUT INSERTED.match_id " +
                    "VALUES (@tournament_id, @match_state, NULL)";
                var parameters = new DynamicParameters();
                parameters.Add("@tournament_id", mm.Tournament.Tournament_Id);
                parameters.Add("@match_state", mm.Match_State);

                return connection.ExecuteScalar<int>(sql, parameters);
            }
        }

        public List<MatchModel> GetMatches_All()
        {
            throw new NotImplementedException();
        }

        public List<MatchModel> GetMatches_Tournament(int tournament_id)
        {
            List<MatchModel> matches = new List<MatchModel>();
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "SELECT match_id, match_state, " +
                    "tournament.tournament_id, tournament.name, tournament.price, tournament.prize, tournament.prize, tournament.min_rating, tournament.max_rating, " +
                    "team.team_id, team.tag, team.rating " +
                    "FROM match " +
                    "JOIN tournament ON tournament.tournament_id = match.tournament_id " +
                    "LEFT JOIN team ON team.team_id = match.winner " +
                    "WHERE match.tournament_id = @tournamentId";

                var parameters = new DynamicParameters();
                parameters.Add("@tournamentId", tournament_id);

                matches = connection.Query<MatchModel, TournamentModel, TeamModel, MatchModel>(sql,
                    map: (match, tournament, winner) =>
                    { 
                        match.Tournament = tournament;
                        match.Winner = winner;
                        return match;
                    },
                    parameters, splitOn : "match_state, max_rating").ToList();
            }
            return matches;
        }

        public void setWinner(int team_id, int match_id)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "UPDATE match " +
                    "SET winner = @teamId " +
                    "WHERE match_id = @matchId";
                var parameters = new DynamicParameters();
                parameters.Add("@teamId", team_id);
                parameters.Add("@matchId", match_id);

                connection.Execute(sql, parameters);
            }
        }

        public void UpdateMatch(MatchModel mm)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "UPDATE match " +
                    "SET match_state = @matchState " +
                    "WHERE match_id = @matchId";
                var parameters = new DynamicParameters();
                parameters.Add("@matchState", mm.Match_State);
                parameters.Add("@matchId", mm.Match_Id);

                connection.Execute(sql, parameters);
            }
        }
    }
}

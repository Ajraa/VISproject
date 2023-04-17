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
    internal class FounderSqlDao : IFounderDao
    {
        public FounderModel CreateFounder(FounderModel fm)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "INSERT INTO founder(first_name, last_name, nickname, password)" +
                    "OUTPUT INSERTED.founder_id " +
                    "VALUES(@firstName, @lastName, @nickName, @password)";
                var parameters = new DynamicParameters();
                parameters.Add("@firstName", fm.First_Name);
                parameters.Add("@lastName", fm.Last_Name);
                parameters.Add("@nickName", fm.Nickname);
                parameters.Add("@password", fm.Password);

                fm.TournamentList = new List<TournamentModel>();
                fm.Founder_Id = connection.ExecuteScalar<int>(sql, parameters);
            }
            return fm;
        }

        public FounderModel GetById(int v)
        {
            throw new NotImplementedException();
        }

        public FounderModel Login(string username, string password)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "SELECT * " +
                    "FROM founder " +
                    "WHERE nickname = @username AND password = @password";
                var parameters = new DynamicParameters();
                parameters.Add("@username", username);
                parameters.Add("@password", password);
                try
                {
                    var fm = connection.QuerySingle<FounderModel>(sql, parameters);
                    fm.TournamentList = GlobalConfig.Connection.TournamentDao.GetTournaments_Founder(fm.Founder_Id);
                    return fm;
                }
                catch
                {
                    return null;
                }
            }
        }

        public void UpdateFounder()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(SqlConnector.Db)))
            {
                string sql = "UPDATE founder" +
                    "SET first_name = @firstName, last_name = @lastName, nickname = @nickName, password =  @password " +
                    "WHERE founder_id = @founderId";
                var parameters = new DynamicParameters();
                parameters.Add("@firstName", GlobalConfig.CurrentFounder.First_Name);
                parameters.Add("@lastName", GlobalConfig.CurrentFounder.Last_Name);
                parameters.Add("@nickName", GlobalConfig.CurrentFounder.Nickname);
                parameters.Add("@password", GlobalConfig.CurrentFounder.Password);
                parameters.Add("@founderId", GlobalConfig.CurrentFounder.Founder_Id);

                connection.Execute(sql, parameters);
            }
        }
    }
}

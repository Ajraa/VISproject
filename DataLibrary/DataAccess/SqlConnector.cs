using DataLibrary.DataAccess.DataAccessObjects;
using DataLibrary.DataAccess.DataAccessObjects.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        public static string Db { get; } = "Tournament";
        public IPlayerDao PlayerDao => new PlayerSqlDao();

        public ITeamDao TeamDao => new TeamSqlDao();

        public IFounderDao FounderDao => new FounderSqlDao();

        public ITournamentDao TournamentDao => new TournamentSqlDao();

        public IMatchDao MatchDao => new MatchSqlDao();
    }
}

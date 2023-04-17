using DataLibrary.DataAccess.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess
{
    public interface IDataConnection
    {
        public IPlayerDao PlayerDao { get; }
        public ITeamDao TeamDao { get; }
        public IFounderDao FounderDao { get; }
        public ITournamentDao TournamentDao { get; }
        public IMatchDao MatchDao { get; }
    }
}

using DataLibrary.DataAccess.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.DataAccess.DataAccessObjects.Text;


namespace DataLibrary.DataAccess
{
    internal class TextConnector : IDataConnection
    {
        public IPlayerDao PlayerDao => new PlayerTextDao();

        public ITeamDao TeamDao => new TeamTextDao();

        public IFounderDao FounderDao => new FounderTextDao();

        public ITournamentDao TournamentDao => new TournamentTextDao();

        public IMatchDao MatchDao => new MatchTextDao();
    }
}

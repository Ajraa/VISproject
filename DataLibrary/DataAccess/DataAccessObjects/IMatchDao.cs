using System;
using DataLibrary.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess
{
    public interface IMatchDao
    {
        int CreateMatch(MatchModel mm);
        List<MatchModel> GetMatches_All();
        void UpdateMatch(MatchModel mm);
        void setWinner(int team_id, int match_id);
        void addTeam(int team_id, int match_id);
        List<MatchModel> GetMatches_Tournament(int tournament_id);
    }
}

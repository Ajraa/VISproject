using System;
using DataLibrary.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess.DataAccessObjects
{
    public interface ITournamentDao
    {

        TournamentModel CreateTournament(TournamentModel tm);
        TournamentModel GetById(int v);
        List<TournamentModel> GetTournaments_All();
        List<TournamentModel> GetTournaments_Founder(int id);
        List<TournamentModel> GetTournaments_Except(int id, int rating);
        void UpdateTournament(TournamentModel tm);
        void setWinner(int tournament_id, int team_id);
        void removeTeam(int tournament_id, int team_id);
        void removeMatch(int match_id);
    }
}

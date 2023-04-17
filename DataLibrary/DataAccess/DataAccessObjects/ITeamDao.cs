using System;
using DataLibrary.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess.DataAccessObjects
{
    public interface ITeamDao
    {
        TeamModel CreateTeam(TeamModel tm);
        TeamModel GetById(int id);
        List<TeamModel> GetTeams_All();
        List<TeamModel> GetTeams_Player(int id);
        List<TeamModel> GetTeams_Except(int id);
        List<TeamModel> GetTeams_Tournament(int id);
        List<TeamModel> GetTeams_MatchExcept(int tournament_id, int match_id);
        List<TeamModel> GetTeams_Match(int id);
        void JoinTournament(int team_id, int tournament_id);
        int KickPlayer(int player_id, int team_id);
        void ChangeCaptain(int player_id, int team_id);
        void UpdateTeam(TeamModel tm);
    }
}

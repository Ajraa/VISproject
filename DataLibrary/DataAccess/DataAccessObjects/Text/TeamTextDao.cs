using DataLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary;

namespace DataLibrary.DataAccess.DataAccessObjects.Text
{
    internal class TeamTextDao : ITeamDao
    {
        public void ChangeCaptain(int player_id, int team_id)
        {
            List<TeamModel> teams = TextConnectorUtils.TeamFileName.FullFilePath().LoadFile().ConvertToTeamModel();
            teams.ForEach(t => t.Captain.Player_Id = t.Team_Id == team_id ? t.Captain.Player_Id = player_id : t.Captain.Player_Id = t.Captain.Player_Id);
            teams.SaveToFile(TextConnectorUtils.TeamFileName);
        }

        public TeamModel CreateTeam(TeamModel tm)
        {
            List<TeamModel> teams = TextConnectorUtils.TeamFileName.FullFilePath().LoadFile().ConvertToTeamModel();
            tm.Team_Id = teams.Count > 0 ? (teams.Last().Team_Id + 1) : 1;

            teams.Add(tm);
            teams.SaveToFile(TextConnectorUtils.TeamFileName);

            tm.Players.Add(GlobalConfig.CurrentPlayer);
            GlobalConfig.CurrentPlayer.Teams.Add(tm);
            TextConnectorUtils.PlayerTeamFileName.InsertAssocTable(GlobalConfig.CurrentPlayer.Player_Id, tm.Team_Id);

            return tm;
        }

        public TeamModel GetById(int id)
        {
            return TextConnectorUtils.TeamFileName.FullFilePath().LoadFile().ConvertToTeamModel().FirstOrDefault(team => team.Team_Id == id);
        }

        public List<TeamModel> GetTeams_All()
        {
            return TextConnectorUtils.TeamFileName.FullFilePath().LoadFile().ConvertToTeamModel();
        }

        public List<TeamModel> GetTeams_Except(int id)
        {
            List<int> teamIds = TextConnectorUtils.PlayerTeamFileName.FullFilePath().LoadFile().GetAssocTable_First(id);
            return TextConnectorUtils.TeamFileName.FullFilePath().LoadFile().ConvertToTeamModel().Where(team => !teamIds.Contains(team.Team_Id)).ToList();
        }

        public List<TeamModel> GetTeams_Match(int id)
        {
            List<int> teamIds = TextConnectorUtils.TeamMatchFileName.FullFilePath().LoadFile().GetAssocTable_First(id);
            return TextConnectorUtils.TeamFileName.FullFilePath().LoadFile().ConvertToTeamModel().Where(team => teamIds.Contains(team.Team_Id)).ToList();
        }

        public List<TeamModel> GetTeams_MatchExcept(int tournament_id, int match_id)
        {
            List<int> matchIds = TextConnectorUtils.TournamentMatchFileName.FullFilePath().LoadFile().GetAssocTable_Second(tournament_id);
            List<int> teamIds = TextConnectorUtils.TeamMatchFileName.FullFilePath().LoadFile().GetAssocTable_First(match_id).Where(id => matchIds.Contains(id)).ToList();
            return TextConnectorUtils.TeamFileName.FullFilePath().LoadFile().ConvertToTeamModel().Where(team => !teamIds.Contains(team.Team_Id)).ToList();
        }

        public List<TeamModel> GetTeams_Player(int id)
        {
            List<int> teamIds = TextConnectorUtils.PlayerTeamFileName.FullFilePath().LoadFile().GetAssocTable_Second(id);
            return TextConnectorUtils.TeamFileName.FullFilePath().LoadFile().ConvertToTeamModel().Where(team => teamIds.Contains(team.Team_Id)).ToList();
        }

        public List<TeamModel> GetTeams_Tournament(int id)
        {
            List<int> teamIds = TextConnectorUtils.TournamentTeamFileName.FullFilePath().LoadFile().GetAssocTable_Second(id);
            return TextConnectorUtils.TeamFileName.FullFilePath().LoadFile().ConvertToTeamModel().Where((team) => teamIds.Contains(team.Team_Id)).ToList();
        }

        public void JoinTournament(int team_id, int tournament_id)
        {
            TextConnectorUtils.TournamentTeamFileName.InsertAssocTable(tournament_id, team_id);
        }

        public int KickPlayer(int player_id, int team_id)
        {
            List<TeamModel> teams = TextConnectorUtils.TeamFileName.FullFilePath().LoadFile().ConvertToTeamModel();

            List<int[]> ids = TextConnectorUtils.PlayerTeamFileName.FullFilePath().LoadFile().GetAssocTable_All();

            int index = -1;
            for (int i = 0; i < ids.Count; i++)
            {
                if (ids[i][0] == player_id && ids[i][1] == team_id) {
                    index = i;
                    break;
                }
            }

            if(index != -1)
                ids.RemoveAt(index);

            ids.RewriteAssocTable(TextConnectorUtils.PlayerTeamFileName);

            TeamModel team = teams.Where(team => team.Team_Id == team_id).First();
            team.Players = GlobalConfig.Connection.PlayerDao.GetPlayers_Team(team_id);


            int rating = team.Players.Sum(player => player.Rating) / team.Players.Count();
            teams.Where(team => team.Team_Id == team_id).ToList().ForEach(team => team.Rating = rating);

            return rating;
        }

        public void UpdateTeam(TeamModel tm)
        {
            List<TeamModel> teams = TextConnectorUtils.TeamFileName.FullFilePath().LoadFile().ConvertToTeamModel();
            teams.ForEach(t => t = t.Team_Id == tm.Team_Id ? tm : t);
            teams.SaveToFile(TextConnectorUtils.TeamFileName);
        }
    }
}

using DataLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess.DataAccessObjects.Text
{
    internal class PlayerTextDao : IPlayerDao
    {
        public PlayerModel CreatePlayer(PlayerModel pm)
        {
            List<PlayerModel> players = TextConnectorUtils.PlayerFileName.FullFilePath().LoadFile().ConvertToPlayerModel();
            pm.Player_Id = players.Count > 0 ? (players.Last().Player_Id + 1) : 1;

            players.Add(pm);
            players.SaveToFile(TextConnectorUtils.PlayerFileName);

            return pm;
        }

        public PlayerModel GetById(int id)
        {
            return TextConnectorUtils.PlayerFileName.FullFilePath().LoadFile().ConvertToPlayerModel().Where(player => player.Player_Id == id).First();
        }

        public List<PlayerModel> GetPlayers_All()
        {
            return TextConnectorUtils.PlayerFileName.FullFilePath().LoadFile().ConvertToPlayerModel();
        }

        public List<PlayerModel> GetPlayers_Team(int id)
        {
            List<int> playerIds = TextConnectorUtils.PlayerTeamFileName.FullFilePath().LoadFile().GetAssocTable_First(id);
            return TextConnectorUtils.PlayerFileName.FullFilePath().LoadFile().ConvertToPlayerModel().Where(player => playerIds.Contains(player.Player_Id)).ToList();
        }

        public int JoinTeam(int team_id)
        {
            TextConnectorUtils.PlayerTeamFileName.InsertAssocTable(GlobalConfig.CurrentPlayer.Player_Id, team_id);
            List<TeamModel> teams = TextConnectorUtils.TeamFileName.FullFilePath().LoadFile().ConvertToTeamModel();

            TeamModel team = teams.Where(team => team.Team_Id == team_id).First();
            team.Captain = GlobalConfig.Connection.PlayerDao.GetById(team_id);
            team.Players = GlobalConfig.Connection.PlayerDao.GetPlayers_Team(team_id);
 

            int rating = team.Players.Sum(player => player.Rating) / team.Players.Count();
            teams.ForEach(t => t.Rating = t.Team_Id == team_id ? rating : t.Rating);
            teams.SaveToFile(TextConnectorUtils.TeamFileName);

            return rating;
        }

        public PlayerModel Login(string username, string password)
        {
            PlayerModel pm = TextConnectorUtils.PlayerFileName.FullFilePath().LoadFile().ConvertToPlayerModel()
                .Where(player => player.NickName == username && player.Password == password).FirstOrDefault();

            if (pm != null)
            {
                pm.Teams = GlobalConfig.Connection.TeamDao.GetTeams_Player(pm.Player_Id);
                pm.Teams.ForEach(team => team.Captain = GlobalConfig.Connection.PlayerDao.GetById(team.Captain.Player_Id));
            }
            return pm;
        }

        public void UpdatePlayer()
        {
            List<PlayerModel> players = TextConnectorUtils.PlayerFileName.FullFilePath().LoadFile().ConvertToPlayerModel();
            players.ForEach(p => p = p.Player_Id == GlobalConfig.CurrentPlayer.Player_Id ? GlobalConfig.CurrentPlayer : p);
            players.SaveToFile(TextConnectorUtils.PlayerFileName);
        }
    }
}

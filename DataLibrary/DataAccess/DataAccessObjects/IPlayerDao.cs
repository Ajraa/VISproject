using System;
using DataLibrary.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess.DataAccessObjects
{
    public interface IPlayerDao
    {
        PlayerModel CreatePlayer(PlayerModel pm);
        PlayerModel GetById(int v);
        List<PlayerModel> GetPlayers_All();
        List<PlayerModel> GetPlayers_Team(int id);
        PlayerModel Login(string username, string password);
        int JoinTeam(int team_id);
        void UpdatePlayer();
        string GenerateXml(int player_id, int? team_id);
    }
}

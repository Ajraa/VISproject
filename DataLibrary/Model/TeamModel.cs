using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Model
{
    public class TeamModel : ICSV
    {
        public int Team_Id { get; set; }
        public string Team_Name { get; set; }
        public string Tag { get; set; }
        public int Rating { get; set; }
        public List<PlayerModel> Players { get; set; }
        public PlayerModel Captain { get; set; }

        public TeamModel()
        {
            this.Players = new List<PlayerModel>();
        }
        public TeamModel(int id, string name, string tag, List<PlayerModel> players, PlayerModel captain, int rating)
        {
            Team_Id = id;
            Team_Name = name;
            Tag = tag;
            Players = players;
            Captain = captain;
            Rating = rating;
        }

        public override string ToString()
        {
            return Team_Id.ToString() + " " + Team_Name + " " + Tag + " " + Rating + " " + Captain.NickName;
        }

        public string CSV()
        {
            return Team_Id + "," + Team_Name + "," + Tag + "," + Rating + "," + Captain.Player_Id;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is TeamModel))
                return false;
            return Team_Id == ((TeamModel)obj).Team_Id;
        }
    }
}

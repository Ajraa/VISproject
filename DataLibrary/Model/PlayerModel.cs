using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Model
{
    public class PlayerModel : ICSV
    {
        public int Player_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public int Rating { get; set; }
        public List<TeamModel> Teams { get; set; }

        public PlayerModel()
        {
            this.Teams = new List<TeamModel>();
        }
        public PlayerModel(int id, string name, string lastName, string nickName, int rating, List<TeamModel> teams, string password)
        {
            Player_Id = id;
            First_Name = name;
            Last_Name = lastName;
            NickName = nickName;
            Rating = rating;
            Teams = teams;
            Password = password;
        }

        public override string ToString()
        {
            return $"{this.Player_Id} {this.First_Name} {this.NickName} {this.Last_Name} {this.Rating}";
        }

        public string CSV()
        {
            return Player_Id + "," + First_Name + "," + Last_Name + "," + NickName + "," + Password + "," + Rating;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is PlayerModel))
                return false;
            return Player_Id == ((PlayerModel)obj).Player_Id;
        }
    }
}

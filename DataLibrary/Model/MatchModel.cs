using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Model
{
    public class MatchModel : ICSV
    {
        public int Match_Id { get; set; }
        public string Match_State { get; set; }
        public TournamentModel Tournament { get; set; }
        public List<TeamModel> Teams { get; set; }
        public TeamModel Winner { get; set; }

        public string CSV()
        {
            string output = Match_Id + "," + Match_State + "," + Tournament.Tournament_Id;
            if (Winner != null)
                output += "," + Winner.Team_Id;
            else
                output += "," + 0;
            return output;
        }

        public override string ToString()
        {
            string ret = "ID: " + Match_Id + ", Status: " + Match_State + ", Turnaj: " + Tournament.Name;
            if (Winner != null)
                ret += ", Vítěz: " + Winner.Team_Name;
            return ret;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is MatchModel))
                return false;
            return Match_Id == ((MatchModel)obj).Match_Id;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Model
{
    public class TournamentModel : ICSV
    {
        public int Tournament_Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Prize { get; set; }
        public int Min_Rating { get; set; }
        public int Max_Rating { get; set; }
        public FounderModel Founder { get; set; }
        public List<TeamModel> Teams { get; set; }
        public TeamModel winner { get; set; }
        public List<MatchModel> Matches { get; set; }

        public TournamentModel()
        {
            this.Teams = new List<TeamModel>();        
            this.Matches = new List<MatchModel>();
        }
        public TournamentModel (int id, string name, int price, int prize, int minRating, int maxRating, FounderModel founder, List<TeamModel> teams, TeamModel winner)
        {
            Tournament_Id = id;
            Name = name;
            Price = price;
            Prize = prize;
            Min_Rating = minRating;
            Max_Rating = maxRating;
            Founder = founder;
            Teams = teams;
            this.winner = winner;
        }

        public override string ToString()
        {
            string wname = "Není";
            if (winner != null)
                wname = winner.Team_Name;
            string output = "Jméno: " + Name + ", Vstupné: "  + Price + ", Výhra: " + Prize + ", Minimální rating: " + Min_Rating + ",Maximální rating: " + Max_Rating + ", Zakladatel " + Founder.Nickname + ", Vítěz: " + wname;
            
            return output;
        }

        public string CSV()
        {
            string output = Tournament_Id + "," + Name + "," + Price + "," + Prize + "," + Min_Rating + "," + Max_Rating + "," + Founder.Founder_Id;
            if (winner != null)
                output += "," + winner.Team_Id;
            else
                output += "," + 0;
            return output;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is TournamentModel))
                return false;
            return Tournament_Id == ((TournamentModel)obj).Tournament_Id;
        }
    }
}

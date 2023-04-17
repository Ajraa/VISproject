using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Model
{
    public class FounderModel : ICSV
    {
        public int Founder_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public List<TournamentModel> TournamentList { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }


        public override string ToString()
        {
            return First_Name + " " + Nickname + " " + Last_Name;
        }

        public string CSV() 
        {
            return Founder_Id + "," + First_Name + "," + Last_Name + "," + Nickname + "," + Password;
        }

        public override bool Equals(object? obj)
        {
            if(!(obj is FounderModel))
                return false;
            return Founder_Id == ((FounderModel)obj).Founder_Id;
        }
    }
}

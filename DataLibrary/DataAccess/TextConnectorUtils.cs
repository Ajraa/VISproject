using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Model;

namespace DataLibrary.DataAccess
{
    public static class TextConnectorUtils
    {
        public static string PlayerFileName = "Players.csv";
        public static string TeamFileName = "Teams.csv";
        public static string TournamentFileName = "Tournaments.csv";
        public static string FounderFileName = "Founders.csv";
        public static string MatchFileName = "Matches.csv";
        public static string PlayerTeamFileName = "Player_team.csv";
        public static string TournamentMatchFileName = "Tournament_match.csv";
        public static string TeamMatchFileName = "Team_match.csv";
        public static string TournamentTeamFileName = "Tournament_team.csv";

        public static char MainSeperator = ',';
        public static char MainIndexSeparator = '|';

        public static string FullFilePath(this string fileName)
        {
            return $"{ ConfigurationManager.AppSettings["textFilePath"] }\\{ fileName }";
        }

        public static IEnumerable<string> LoadFile(this string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
                return new List<string>();
            }

            return File.ReadAllLines(filePath);
        }

        public static void SaveToFile(this IEnumerable<ICSV> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (var model in models)
            {
                lines.Add(model.CSV());
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static List<PlayerModel> ConvertToPlayerModel(this IEnumerable<string> lines)
        {
            List<PlayerModel> result = new List<PlayerModel>();
           

            foreach (string line in lines)
            { 
                PlayerModel model = new PlayerModel();

                string[] cols = line.Split(MainSeperator);

                model.Player_Id = int.Parse(cols[0]);
                model.First_Name = cols[1];
                model.Last_Name = cols[2];
                model.NickName = cols[3];
                model.Password = cols[4];
                model.Rating = int.Parse(cols[5]);

                result.Add(model);
            }

            return result;
        }

        public static List<TeamModel> ConvertToTeamModel(this IEnumerable<string> lines)
        {
            List<TeamModel> result = new List<TeamModel>();

            foreach (string line in lines)
            { 
                TeamModel model = new TeamModel();

                string[] cols = line.Split(MainSeperator);

                model.Team_Id = int.Parse(cols[0]);
                model.Team_Name = cols[1];
                model.Tag = cols[2];
                model.Rating = int.Parse(cols[3]);
                model.Captain = new PlayerModel();
                model.Captain.Player_Id = int.Parse(cols[4]);

                result.Add(model);
            }

            return result;
        }

        public static List<FounderModel> ConvertToFounderModel(this IEnumerable<string> lines)
        { 
            List<FounderModel> result = new List<FounderModel>();
            foreach (string line in lines)
            {
                string[] cols = line.Split(MainSeperator);

                FounderModel model = new FounderModel();

                model.Founder_Id = int.Parse(cols[0]);
                model.First_Name = cols[1];
                model.Last_Name = cols[2];
                model.Nickname = cols[3];
                model.Password = cols[4];

                result.Add(model);
            }

            return result;
        }

        public static List<TournamentModel> ConvertToTournamentModel(this IEnumerable<string> lines)
        {
            List<TournamentModel> result = new List<TournamentModel>();
            foreach (string line in lines)
            {
                string[] cols = line.Split(MainSeperator);
                TournamentModel model = new TournamentModel();

                model.Tournament_Id = int.Parse(cols[0]);
                model.Name = cols[1];
                model.Price = int.Parse(cols[2]);
                model.Prize = int.Parse(cols[3]);
                model.Min_Rating = int.Parse(cols[4]);
                model.Max_Rating = int.Parse(cols[5]);
                model.Founder = new FounderModel();
                model.Founder.Founder_Id = int.Parse(cols[6]);
                model.winner = new TeamModel();
                model.winner.Team_Id = int.Parse(cols[7]);

                result.Add(model);
            }
            return result;

        }

        public static List<MatchModel> ConvertToMatchModel(this IEnumerable<string> lines)
        { 
            List<MatchModel> result = new List<MatchModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(MainSeperator);
                MatchModel model = new MatchModel();

                model.Match_Id = int.Parse(cols[0]);
                model.Match_State = cols[1];
                model.Tournament = new TournamentModel();
                model.Tournament.Tournament_Id = int.Parse(cols[2]);
                model.Winner = new TeamModel();
                model.Winner.Team_Id= int.Parse(cols[3]);

                result.Add(model);
            }

            return result;
        }

        public static List<int> GetAssocTable_First(this IEnumerable<string> lines, int id)
        { 
            List<int> result = new List<int>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(MainSeperator);
                if(int.Parse(cols[1]) == id)
                   result.Add(int.Parse(cols[0]));
            }
            return result;
        }

        public static List<int> GetAssocTable_Second(this IEnumerable<string> lines, int id)
        {
            List<int> result = new List<int>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(MainSeperator);
                if (int.Parse(cols[0]) == id)
                    result.Add(int.Parse(cols[1]));
            }
            return result;
        }

        public static List<int[]> GetAssocTable_All(this IEnumerable<string> lines)
        {
            List<int[]> result = new List<int[]>();
            foreach (string line in lines)
            {
                string[] cols = line.Split(MainSeperator);
                int[] ids = {int.Parse(cols[0]), int.Parse(cols[1])};
                result.Add(ids);
            }

            return result;
        }

        public static void InsertAssocTable(this string fileName, int id_first, int id_second)
        {
            File.AppendAllText(fileName.FullFilePath(), id_first + "," + id_second + Environment.NewLine);
        }

        public static void RewriteAssocTable(this List<int[]> ids, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (var id in ids)
            {
                lines.Add(id[0] + "," + id[1]);
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
    
    }
}

using DataLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess.DataAccessObjects.Text
{
    internal class MatchTextDao : IMatchDao
    {
        public void addTeam(int team_id, int match_id)
        {
            TextConnectorUtils.TeamMatchFileName.InsertAssocTable(team_id, match_id);
        }

        public int CreateMatch(MatchModel mm)
        {
            List<MatchModel> matches = TextConnectorUtils.MatchFileName.FullFilePath().LoadFile().ConvertToMatchModel();
            mm.Match_Id = matches.Count() > 0 ? (matches.Last().Match_Id + 1) : 0;
            matches.Add(mm);
            matches.SaveToFile(TextConnectorUtils.MatchFileName);
            return mm.Match_Id;
        }

        public List<MatchModel> GetMatches_All()
        {
            return TextConnectorUtils.MatchFileName.FullFilePath().LoadFile().ConvertToMatchModel();
        }

        public List<MatchModel> GetMatches_Tournament(int tournament_id)
        {
            return TextConnectorUtils.MatchFileName.FullFilePath().LoadFile().ConvertToMatchModel().ToList().Where(m => m.Tournament.Tournament_Id == tournament_id).ToList();
        }

        public void setWinner(int team_id, int match_id)
        {
            List<MatchModel> matches = TextConnectorUtils.MatchFileName.FullFilePath().LoadFile().ConvertToMatchModel();
            matches.ForEach(m => m.Match_Id =  m.Match_Id == match_id ? m.Winner.Team_Id = team_id : m.Winner.Team_Id = m.Winner.Team_Id);
            matches.SaveToFile(TextConnectorUtils.MatchFileName);
        }

        public void UpdateMatch(MatchModel mm)
        {
            
            List<MatchModel> matches = TextConnectorUtils.MatchFileName.FullFilePath().LoadFile().ConvertToMatchModel();
            matches.ForEach(m => m = m.Match_Id == mm.Match_Id ? m = mm : m = m);
            matches.SaveToFile(TextConnectorUtils.MatchFileName);
        }
    }
}

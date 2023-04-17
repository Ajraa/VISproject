using DataLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess.DataAccessObjects.Text
{
    internal class TournamentTextDao : ITournamentDao
    {
        public TournamentModel CreateTournament(TournamentModel tm)
        {
            List<TournamentModel> tms = TextConnectorUtils.TournamentFileName.FullFilePath().LoadFile().ConvertToTournamentModel();
            tm.Tournament_Id = tms.Count() > 0 ? tms.Last().Tournament_Id : 1;
            tms.Add(tm);
            tms.SaveToFile(TextConnectorUtils.TournamentFileName);
            return tm;
        }

        public TournamentModel GetById(int id)
        {
            return TextConnectorUtils.TournamentFileName.FullFilePath().LoadFile().ConvertToTournamentModel().Where(tournament => tournament.Tournament_Id == id).First();
        }

        public List<TournamentModel> GetTournaments_All()
        {
            return TextConnectorUtils.TournamentFileName.FullFilePath().LoadFile().ConvertToTournamentModel();
        }

        public List<TournamentModel> GetTournaments_Except(int id, int rating)
        {
            List<int> tournamentIds = TextConnectorUtils.TournamentTeamFileName.FullFilePath().LoadFile().GetAssocTable_First(id).ToList();
            List<TournamentModel> tournaments = TextConnectorUtils.TournamentFileName.FullFilePath().LoadFile().ConvertToTournamentModel()
                .Where(tournament => tournament.Min_Rating <= rating && !tournamentIds.Contains(tournament.Tournament_Id)).ToList();
            tournaments.ForEach(tournament => tournament.Founder = GlobalConfig.Connection.FounderDao.GetById(tournament.Founder.Founder_Id));
            return tournaments;
        }

        public List<TournamentModel> GetTournaments_Founder(int id)
        {
           return TextConnectorUtils.TournamentFileName.FullFilePath().LoadFile().ConvertToTournamentModel().Where(tournament => tournament.Founder.Founder_Id == id).ToList();
        }

        public void removeMatch(int match_id)
        {
            List<MatchModel> matches = TextConnectorUtils.MatchFileName.FullFilePath().LoadFile().ConvertToMatchModel();
            matches.Where(match => match.Match_Id != match_id).ToList().SaveToFile(TextConnectorUtils.MatchFileName);
        }

        public void removeTeam(int tournament_id, int team_id)
        {
            List<int> tournament_ids = TextConnectorUtils.TournamentTeamFileName.FullFilePath().LoadFile().GetAssocTable_First(tournament_id);
            List<int> team_ids = TextConnectorUtils.TournamentTeamFileName.FullFilePath().LoadFile().GetAssocTable_Second(team_id);

            for (int i = 0; i < tournament_ids.Count(); i++)
            {
                if (tournament_ids[i] == tournament_id && team_ids[i] == team_id)
                {
                    tournament_ids.RemoveAt(i);
                    team_ids.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < tournament_ids.Count(); i++)
                TextConnectorUtils.PlayerTeamFileName.InsertAssocTable(tournament_ids[i], team_ids[i]);
        }

        public void setWinner(int tournament_id, int team_id)
        {
            List<TournamentModel> tournaments = TextConnectorUtils.TournamentFileName.FullFilePath().LoadFile().ConvertToTournamentModel();
            tournaments.ForEach(tournament => tournament.winner.Team_Id = tournament.Tournament_Id == tournament_id ? team_id : tournament.winner.Team_Id);
        }

        public void UpdateTournament(TournamentModel tm)
        {
            throw new NotImplementedException();
        }
    }
}

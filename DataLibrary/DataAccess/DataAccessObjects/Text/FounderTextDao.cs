using DataLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess.DataAccessObjects.Text
{
    internal class FounderTextDao : IFounderDao
    {
        public FounderModel CreateFounder(FounderModel fm)
        {
            List<FounderModel> founders = TextConnectorUtils.FounderFileName.FullFilePath().LoadFile().ConvertToFounderModel();

            fm.Founder_Id = founders.Count() > 0 ? (founders.Last().Founder_Id + 1) : 1;

            founders.Add(fm);
            founders.SaveToFile(TextConnectorUtils.FounderFileName);
            return fm;
        }

        public FounderModel GetById(int id)
        {
            return TextConnectorUtils.FounderFileName.FullFilePath().LoadFile().ConvertToFounderModel().Where(founder => founder.Founder_Id == id).FirstOrDefault();
        }

        public FounderModel Login(string username, string password)
        {
            FounderModel founder = TextConnectorUtils.FounderFileName.FullFilePath().LoadFile().ConvertToFounderModel().Where((founder) => founder.Nickname == username && founder.Password == password).FirstOrDefault();

            founder.TournamentList = TextConnectorUtils.TournamentFileName.FullFilePath().LoadFile().ConvertToTournamentModel().Where(tournament => tournament.Founder.Founder_Id == founder.Founder_Id).ToList();
            founder.TournamentList.ForEach(tournament => tournament.Founder = founder);
            return founder;
        }

        public void UpdateFounder()
        {
            List<FounderModel> founders = TextConnectorUtils.FounderFileName.FullFilePath().LoadFile().ConvertToFounderModel();
            founders.ForEach(f => f = f.Founder_Id == GlobalConfig.CurrentFounder.Founder_Id ? GlobalConfig.CurrentFounder : f);
            founders.SaveToFile(TextConnectorUtils.FounderFileName);
        }
    }
}

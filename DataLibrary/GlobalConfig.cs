using System;
using System.Collections.Generic;
using DataLibrary.DataAccess;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Model;
using System.Configuration;

namespace DataLibrary
{
    public class GlobalConfig
    {
        public static IDataConnection Connection { get; set; } = new TextConnector();
        public static PlayerModel CurrentPlayer { get; set; } = new PlayerModel();
        public static FounderModel CurrentFounder { get; set; } = new FounderModel();

        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}

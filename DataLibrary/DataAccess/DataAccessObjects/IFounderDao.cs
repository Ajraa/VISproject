using System;
using DataLibrary.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess.DataAccessObjects
{
    public interface IFounderDao
    {
        FounderModel CreateFounder(FounderModel fm);
        FounderModel Login(string username, string password);
        FounderModel GetById(int id);
        void UpdateFounder();
    }
}

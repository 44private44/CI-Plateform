using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.MissionModel
{
    public class SPFormViewModel
    {
        public SPDemo Submission { get; set; } = new SPDemo();
        public List<City> Cities { get; set; } = new List<City>();

        public List<Country> Countries  { get; set; } = new List<Country>();


    }
}

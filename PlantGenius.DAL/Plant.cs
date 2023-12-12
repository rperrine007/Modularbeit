using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PlantGenius.DAL
{
    public class Plant
    {
        public Plant(string plantName, DateTime waterLastTime, int waterRequirement)
        {
            PlantName = plantName;
            PlantWaterLastTime = waterLastTime;
            PlantWaterRequirement = waterRequirement;
        }

        public int PlantID { get; set; }
        public string PlantName { get; set; }
        public string PlantNameScientific { get; set; }
        public int PlantRoom {  get; set; }
        public int PlantSort { get; set; }        
        public int PlantWaterRequirement { get; set; }
        public DateTime PlantWaterLastTime { get; set; }

        public override string ToString()
        {
            return $"Pflanzenname: {PlantName}, Zuletzt gewässert {PlantWaterLastTime}, Giessintervall {PlantWaterRequirement} Tage";
        }
    }
}
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PlantGenius.GUI
{
    public class Plant
    {
        public Plant(string plantName, DateTime waterLastTime, int waterRequirement)
        {
            PlantName = plantName;
            WaterLastTime = waterLastTime;
            WaterRequirement = waterRequirement;
        }

        public string PlantName { get; set; }
        public DateTime WaterLastTime { get; set; }
        public int WaterRequirement { get; set; }
        // public Room Room { get; set; }

        public override string ToString()
        {
            return $"Pflanzenname: {PlantName}, Zuletzt gewässert {WaterLastTime}, Giessintervall {WaterRequirement} Tage";
        }
    }
}
 
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
        public Plant(string PlantName, DateTime WaterLastTime, int WaterRequirement)
        {
            this.PlantName = PlantName;
            this.WaterLastTime = WaterLastTime;
            this.WaterRequirement = WaterRequirement;
        }

        public string PlantName { get; set; }
        public DateTime WaterLastTime { get; set; }
        public int WaterRequirement { get; set; }
        // public Room Room { get; set; }
    }
}

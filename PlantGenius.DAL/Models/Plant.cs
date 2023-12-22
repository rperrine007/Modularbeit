using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PlantGenius.DAL.Models
{
    public class Plant
    {

        // EF needs this constructor even though it is never called. Else the "No suitable constructor found exception" is thrown.
        public Plant() { }

        public Plant(string plantName, DateTime waterLastTime, int waterRequirement)
        {
            PlantName = plantName;
            PlantWaterLastTime = waterLastTime;
            PlantWaterRequirement = waterRequirement;
        }

        public int PlantID { get; set; }

        [Required]
        [MaxLength(30)]
        public string PlantName { get; set; }

        [MaxLength(50)]
        public string PlantNameScientific { get; set; }

        [Required]
        public int PlantRoom { get; set; }
        [ForeignKey("PlantRoom")]
        public Room Room { get; set; }

        public int PlantSort { get; set; }

        [Required]
        public int PlantWaterRequirement { get; set; }

        // Calculation for next watering time
        public DateTime PlantWaterLastTime { get; set; }
                 
        private DateTime CalculatePlantWaterNextTime()
        {
            return PlantWaterLastTime.AddDays(PlantWaterRequirement);
        }

        public string PlantWaterNextTime
        {
            get
            {
                return CalculatePlantWaterNextTime().ToString("dd.MM.yyyy");
            }
        }
                

        public override string ToString()
        {
            return $"Pflanzenname: {PlantName}, Zuletzt gewässert {PlantWaterLastTime}, Giessintervall {PlantWaterRequirement} Tage";
        }


    }
}

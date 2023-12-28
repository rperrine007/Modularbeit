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
    /// <summary>
    /// A class in which a Plant is defined. 
    /// </summary>
    public class Plant
    {
        /// <summary>
        /// Constructor: EF needs this constructor even though it is never called. Else the "No suitable constructor found exception" is thrown.
        /// </summary>
        public Plant() { }

        /// <summary>
        /// Second Constructor with input values.
        /// </summary>
        /// <param name="plantName"></param>
        /// <param name="waterLastTime"></param>
        /// <param name="waterRequirement"></param>
        public Plant(string plantName, DateTime waterLastTime, int waterRequirement)
        {
            PlantName = plantName;
            PlantWaterLastTime = waterLastTime;
            PlantWaterRequirement = waterRequirement;
            UpdatePlantSort();
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

        //PlantSort is calculated based on the amount of days until the next watering.
        public int PlantSort{ get; private set; }

        [Required]
        public int PlantWaterRequirement { get; set; }

        // Calculation for next watering time
        public DateTime PlantWaterLastTime { get; set; }
       
        /// <summary>
        /// Date of next needed watering is calculated.
        /// </summary>
        /// <returns></returns>
        private DateTime CalculatePlantWaterNextTime()
        {
            return PlantWaterLastTime.AddDays(PlantWaterRequirement);
        }

        /// <summary>
        /// The DateTime of the Function CalculatePlantWaterNextTime is parsed to a string with the format: "dd.MM.yyyy".
        /// </summary>
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

        /// <summary>
        /// The path for the watering can is set depending on the date.
        /// </summary>
        public string WaterIconPath
        {
            get
            {
                var nextWaterTime = CalculatePlantWaterNextTime();
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);

                if (nextWaterTime.Date == today || nextWaterTime.Date == tomorrow)
                {
                    return "watering_icon_orange.svg"; // Path to the icon -> today and tomorrow
                }
                else if (nextWaterTime.Date < today)
                {
                    return "watering_icon_red.svg"; // Path to the icon -> past
                }
                else
                {
                    return "watering_icon_green.svg"; // Path to the icon -> future
                }
            }
        }

        public void UpdatePlantSort()
        {
            PlantSort = (CalculatePlantWaterNextTime() - DateTime.Today).Days;
        }
    }
}

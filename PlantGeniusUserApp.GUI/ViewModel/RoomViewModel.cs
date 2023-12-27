using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Collections;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Utilities;
using PlantGenius.DAL.Models;
using PlantGeniusUserApp.GUI.Views;

namespace PlantGeniusUserApp.GUI.ViewModel
{
    /// <summary>
    /// RoomViewModel is a child-class of PlantsViewModel as it is basically the same but in the childclass, the plant get also sorted after there room property.
    /// </summary>
    public class RoomViewModel : PlantsViewModel
    {
        //This is a special Typ of the CommunityToolkit.Mvvm.Collections which merges the functionality GroupedCollection with Observable Collection.
        public ObservableGroupedCollection<string, Plant> groupedPlants { get; set; } = new ObservableGroupedCollection<string, Plant>();


        public RoomViewModel()
        {
            // contstructor of PlantsViewmodel is called automatically as it has no parameters.
        }


        /// <summary>
        /// Override the function "LoadPlants" so that the groupedPlants Collection is also created.
        /// </summary>
        /// <returns></returns>
        protected override async Task LoadPlants()
        {
            Plants.Clear();
            await base.LoadPlants();
            groupedPlants.Clear();


            // Group the plants by RoomName and then add them to the groupedPlants collection.
            var grouped = Plants.GroupBy(p => p.Room.RoomName)
                                 //Sort the groups by there PlantSort
                                 .Select(group => new
                                 {
                                     RoomName = group.Key,
                                     Plants = group.OrderBy(p => p.PlantSort)
                                 });

            //Creating ObservableGroups and Adding to groupedPlants
            foreach (var group in grouped)
            {
                var groupKey = group.RoomName;
                var plantsInGroup = new ObservableCollection<Plant>(group.Plants);

                groupedPlants.Add(new ObservableGroup<string, Plant>(groupKey, plantsInGroup));
            }
        }

        /// <summary>
        /// Override function so it can be called easily in the .xaml file.
        /// </summary>
        protected override async void UpdatePlantList()
        {
            await LoadPlants();
        }


    }
}

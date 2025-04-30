// Source/System/LocationManager.cs
using System.Collections.Generic;

namespace PickyMoo.Source.System
{
    public class LocationManager
    {
        public static string CurrentLocation { get; private set; }
        public Dictionary<string, TerrainComponent> Locations = new();

        public LocationManager(string start)
        {
            CurrentLocation = start;
        }

        public void Register(string name, TerrainComponent map)
        {
            Locations[name] = map;
        }

        public void SwitchTo(string name)
        {
            if (!Locations.ContainsKey(name)) return;
            CurrentLocation = name;
        }

        public TerrainComponent GetCurrentTerrain() =>
            Locations.TryGetValue(CurrentLocation, out var terrain) ? terrain : null;
    }
}

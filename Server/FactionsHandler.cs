using System;
using System.Collections.Generic;
using System.IO;

namespace DarkMultiPlayerServer
{
    public static class FactionsHandler
    {
        public static Dictionary<string, Faction> Factions = new Dictionary<string, Faction>();
        public static void Reset()
        {
            Factions.Clear();
        }
        public static void LoadFactions()
        {
            string directory = Path.Combine(Server.universeDirectory, "Factions");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            foreach (string file in Directory.EnumerateFiles(directory))
            {
                string FactionId = Path.GetFileNameWithoutExtension(file);
                var faction = Faction.LoadFaction(FactionId);
                Factions.Add(FactionId,faction);
            }
        }
        public static void SavePermissions()
        {
            string directory = Path.Combine(Server.universeDirectory, "Factions");
            foreach (Faction perm in Factions.Values)
            {
                Faction.SaveFaction(perm);
            }
        }
    }
}

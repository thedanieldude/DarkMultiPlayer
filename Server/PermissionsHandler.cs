using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace DarkMultiPlayerServer
{
    public static class PermissionsHandler
    {
        public static Dictionary<string, VesselPermissions> vesselperms=new Dictionary<string,VesselPermissions>();
        public static void Reset()
        {
            vesselperms = new Dictionary<string, VesselPermissions>();
        }
        public static void DeleteVesselessPermissions()
        {
            string Vesseldirectory = Path.Combine(Server.universeDirectory, "Vessels");
            string Permdirectory = Path.Combine(Server.universeDirectory, "Vessels", "Permissions");
            if (!Directory.Exists(Permdirectory))
            {
                Directory.CreateDirectory(Permdirectory);
            }
            foreach (string file in Directory.EnumerateFiles(Permdirectory))
            {
                string VesselID = Path.GetFileNameWithoutExtension(file);
                if (!File.Exists(Path.Combine(Vesseldirectory, VesselID + ".txt")))
                {
                    File.Delete(file);
                }

            }
        }
        public static void GeneratePermissionsForVesselsWithoutPermissions()
        {
            string Vesseldirectory = Path.Combine(Server.universeDirectory, "Vessels");
            string Permdirectory = Path.Combine(Server.universeDirectory, "Vessels", "Permissions");
            if (!Directory.Exists(Permdirectory))
            {
                Directory.CreateDirectory(Permdirectory);
            }
            foreach (string file in Directory.EnumerateFiles(Vesseldirectory))
            {
                string VesselID = Path.GetFileNameWithoutExtension(file);
                if (File.Exists(Path.Combine(Permdirectory, VesselID + ".json"))) continue;
                VesselPermissions output = VesselPermissions.Default;
                output.VesselID = VesselID;
                VesselPermissions.SaveVesselPermissions(output);
                
            }
        }
        public static void LoadPermissions()
        {
            string directory = Path.Combine(Server.universeDirectory, "Vessels", "Permissions");
            foreach (string file in Directory.EnumerateFiles(directory))
            {
                string VesselID = Path.GetFileNameWithoutExtension(file);
                var perm = VesselPermissions.LoadVesselPermissions(VesselID);
                vesselperms.Add(VesselID, perm);
            }
        }
        public static void SavePermissions()
        {
            string directory = Path.Combine(Server.universeDirectory, "Vessels", "Permissions");
            foreach (VesselPermissions perm in vesselperms.Values)
            {
                VesselPermissions.SaveVesselPermissions(perm);
            }
        }
    }
}

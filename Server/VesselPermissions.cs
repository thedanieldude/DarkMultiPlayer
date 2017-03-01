using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace DarkMultiPlayerServer
{
    [DataContract]
    public class VesselPermissions
    {

        [DataMember]
        public string VesselID;
        [DataMember]
        public string OwnerName;
        [DataMember]
        public bool OwnerIsFaction;
        [DataMember]
        public List<String> CanEditPermissions;
        [DataMember]
        public List<String> CanControl;
        public static VesselPermissions Default
        {
            get
            {
                return new VesselPermissions() { VesselID = "-1", OwnerName = "-1", OwnerIsFaction = false, CanEditPermissions = new List<String>() { "<everyone>" }, CanControl = new List<String>() { "<everyone>" } };
            }
        }
        public static VesselPermissions LoadVesselPermissions(string id) {
            string directory = Path.Combine(Server.universeDirectory, "Vessels", "Permissions");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string path = Path.Combine(directory, id + ".json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                var ser = new DataContractJsonSerializer(typeof(VesselPermissions));
                return (VesselPermissions)(ser.ReadObject(stream));
            }
        }
        public static void SaveVesselPermissions(VesselPermissions perm)
        {
            string directory = Path.Combine(Server.universeDirectory, "Vessels", "Permissions");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string path = Path.Combine(directory, perm.VesselID + ".json");
            if (File.Exists(path))
            {
                File.WriteAllText(path, String.Empty);
            }
            using (FileStream stream = File.Open(path, FileMode.OpenOrCreate))
            {
                
                var ser = new DataContractJsonSerializer(typeof(VesselPermissions));
                ser.WriteObject(stream,perm);
                
            }
        }
    }
}

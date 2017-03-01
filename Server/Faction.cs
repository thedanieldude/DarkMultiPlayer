using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Linq;
namespace DarkMultiPlayerServer
{
    [DataContract]
    public class Faction
    {
        [DataMember]
        public String FactionID;
        [DataMember]
        public String FactionName;
        [DataMember]
        public String OwnerName;
        [DataMember]
        public List<FactionPermission> Members;
        [DataMember]
        bool PublicFaction;

        public static Faction LoadFaction(string id)
        {
            string directory = Path.Combine(Server.universeDirectory, "Factions");
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
                return (Faction)(ser.ReadObject(stream));
            }
        }
        public static void SaveFaction(Faction faction)
        {
            string directory = Path.Combine(Server.universeDirectory, "Factions");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string path = Path.Combine(directory, faction.FactionID + ".json");
            if (File.Exists(path))
            {
                File.WriteAllText(path, String.Empty);
            }
            using (FileStream stream = File.Open(path, FileMode.OpenOrCreate))
            {

                var ser = new DataContractJsonSerializer(typeof(VesselPermissions));
                ser.WriteObject(stream, faction);

            }
        }
        public static String GenerateFactionID()
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz1234567890";
            return new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
    [DataContract]
    public struct FactionPermission {
        [DataMember]
        public String Player;
        [DataMember]
        public FactionRank rank;
        public FactionPermission(String player,FactionRank rank)
        {
            Player = player;this.rank = rank;
        }
    }
    public enum FactionRank
    {
        NA = -1,
        Member=0,
        Officer=1,
        Owner=2
    }
}

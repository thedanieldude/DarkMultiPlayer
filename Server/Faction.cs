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
        public Dictionary<string,int> Members;
        [DataMember]
        public bool PublicFaction;
        
        public static Faction LoadFaction(string plainid)
        {
            if(plainid.Contains("<")|| plainid.Contains(">"))
            {
                plainid = GetPlainID(plainid);
            }
            string directory = Path.Combine(Server.universeDirectory, "Factions");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string path = Path.Combine(directory, plainid + ".json");
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
            string path = Path.Combine(directory, GetPlainID(faction.FactionID) + ".json");
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
        public FactionRank RankOf(string id)
        {
            if (!IsInFaction(id))
            {
                return FactionRank.NA;
            }
            return OwnerName==id ? FactionRank.Owner :(FactionRank)Members[id];
        }
        public bool IsInFaction(string Name)
        {
            int output;
            return Members.TryGetValue(Name, out output);

        }
        public static String GenerateFactionID()
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz1234567890";
            return "<" + new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray())+ ">";
        }
        public static string GetPlainID(string FactionID)
        {
            if (FactionID == null) return null;
            if (FactionID == String.Empty) return "";
            if(!(FactionID.Contains("<") || FactionID.Contains(">")))
            {
                return FactionID;
            }
            var chararray = FactionID.ToCharArray().ToList();
            chararray.RemoveAt(chararray.Count - 1);
            chararray.RemoveAt(0);
            return new string(chararray.ToArray());
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

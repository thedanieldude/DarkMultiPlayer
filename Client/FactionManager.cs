using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkMultiPlayer
{
    public static class FactionManager
    {
        public static Dictionary<string, Faction> Factions = new Dictionary<string, Faction>();
        public static string MyFactionID="";
        public static void Reset()
        {
            Factions.Clear();
            MyFactionID = "";
        }
    }
    public class Faction
    {
        public String FactionID;

        public String FactionName;

        public String OwnerName;

        public Dictionary<string,int> Members;

        public bool PublicFaction;

        public bool AmOwner()
        {
            return PlayerStatusWorker.fetch.myPlayerStatus.playerName == OwnerName;
        }
        public int MyRank()
        {
            
            return AmOwner() ? 2 : (int)Members.First(x => x.Key == PlayerStatusWorker.fetch.myPlayerStatus.playerName).Value;
        }
        public bool IsInFaction(string Name)
        {
            return Members.Where(x => x.Key == Name).Count() > 0;
                
        }
    }
    public struct FactionPermission
    {
        public String Player;
        public FactionRank rank;
        public FactionPermission(String player, FactionRank rank)
        {
            Player = player; this.rank = rank;
        }
    }
    public enum FactionRank
    {
        NA = -1,
        Member = 0,
        Officer = 1,
        Owner = 2
    }
}

using System;
using System.Collections.Generic;
using DarkMultiPlayerCommon;
namespace DarkMultiPlayer
{
    public static class PermissionsManager
    {
        public static Dictionary<string, VesselPermissions> VesselPerms = new Dictionary<string, VesselPermissions>();
        public static List<string> MyVessels = new List<String>();
        public static void Reset()
        {
            VesselPerms.Clear();
            MyVessels.Clear();
        }
        public static void FindMyVessels()
        {
            MyVessels.Clear();
            foreach(KeyValuePair<string,VesselPermissions> v in VesselPerms)
            {
                if (v.Value.OwnerName == PlayerStatusWorker.fetch.myPlayerStatus.playerName)
                {
                    MyVessels.Add(v.Key);
                }
            }
        }
        
    }
    public class VesselPermissions
    {
        public string VesselID;
        public string OwnerName;
        public Dictionary<String, int> Permissions;
        public static VesselPermissions Default
        {
            get
            {
                return new VesselPermissions() { VesselID = "-1", OwnerName = "", Permissions = new Dictionary<string, int>() { { "<everyone>", (int)VesselPlayerPerms.All } } };
            }
        }
        public VesselPermissions()
        {
            Permissions = new Dictionary<string, int>();
        }
    }
}

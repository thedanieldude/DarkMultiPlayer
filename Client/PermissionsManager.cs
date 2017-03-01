using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public bool OwnerIsFaction;
        public List<String> CanEditPermissions;
        public List<String> CanControl;
        public static VesselPermissions Default
        {
            get
            {
                return new VesselPermissions() { VesselID = "-1", OwnerName = "-1", OwnerIsFaction = false, CanEditPermissions = new List<String>() { "<everyone>" }, CanControl = new List<String>() { "<everyone>" } };
            }
        }
    }
}

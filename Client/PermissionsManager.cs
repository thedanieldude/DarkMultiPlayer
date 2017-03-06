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
        public static VesselPermissions GenerateAsteroidPermissions(ProtoVessel vessel)
        {
            VesselPermissions output = VesselPermissions.Default;
            output.VesselID = vessel.vesselID.ToString();
            return output;
            
        }
        public static bool CanControl(string vesselID,string Name)
        {
            if (!VesselPerms.ContainsKey(vesselID))
            {
                return true;
            }
            else {
                return VesselPerms[vesselID].CanPlayerControl(Name);
            }
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
        public bool CanPlayerEditPermissions(string Name)
        {
            if (IsOwner(Name)) return true;
            if (Permissions.ContainsKey(Name))
            {
                return (Permissions[Name] & (int)VesselPlayerPerms.CanEditPermission) > 0;



            }
            else if (Permissions.ContainsKey("<everyone>"))
            {
                return (Permissions["<everyone>"] & (int)VesselPlayerPerms.CanEditPermission) > 0;
            }
            else
            {
                return false;
            }
        }
        public bool CanPlayerControl(string Name)
        {
            if (CanPlayerEditPermissions(Name)) return true;


            if (Permissions.ContainsKey(Name))
            {
                return (Permissions[Name] & (int)VesselPlayerPerms.CanControl) > 0;



            }
            else if (Permissions.ContainsKey("<everyone>"))
            {
                return (Permissions["<everyone>"] & (int)VesselPlayerPerms.CanControl) > 0;
            }
            else
            {
                return false;
            }
        }
        public bool IsOwner(string Name)
        {
            return OwnerName == Name;
        }
    }
}

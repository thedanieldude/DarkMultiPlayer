using System;
using System.Collections.Generic;
using System.Linq;
using DarkMultiPlayerCommon;
using MessageStream2;
namespace DarkMultiPlayerServer.Messages
{
    public static class PermissionsUpdate
    {
        public static void SendPermissionsUpdate(ClientObject client, VesselPermissions perm)
        {
            ServerMessage message = new ServerMessage();
            message.type = ServerMessageType.PERMISSIONS_UPDATE;
            using(MessageWriter mw = new MessageWriter())
            {
                mw.Write<string>(perm.VesselID);
                mw.Write<string>(perm.OwnerName);
                mw.Write<string[]>(perm.Permissions.Keys.ToArray());
                mw.Write<int[]>(perm.Permissions.Values.ToArray());
                message.data = mw.GetMessageBytes();
            }
            ClientHandler.SendToAll(null, message, false);
        }
        public static void HandlePermissionsUpdate(ClientObject client, byte[] messageData)
        {
            DarkLog.Debug("Recieved Vessel Permissions");
            VesselPermissions perm = new VesselPermissions();
            using (MessageReader mr = new MessageReader(messageData))
            {
                perm.VesselID = mr.Read<string>();
                perm.OwnerName = mr.Read<string>();
                var players = mr.Read<string[]>().ToList();
                var permissions = mr.Read<int[]>().ToList();
                perm.Permissions = Common.ListsToDictionary(players, permissions);
            }

            if (PermissionsHandler.vesselperms.ContainsKey(perm.VesselID))
            { 
                if (PermissionsHandler.vesselperms[perm.VesselID].OwnerName == perm.OwnerName)
                {
                    PermissionsHandler.vesselperms[perm.VesselID] = perm;
                    VesselPermissions.SaveVesselPermissions(perm);
                    SendPermissionsUpdate(client, perm);

                }

                }
                else
                {
                    PermissionsHandler.vesselperms.Add(perm.VesselID, perm);
                    VesselPermissions.SaveVesselPermissions(perm);
                    SendPermissionsUpdate(client, perm);

                }
            
            }
           
            
        }   
    }


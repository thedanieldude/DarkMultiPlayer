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
                mw.Write<bool>(perm.OwnerIsFaction);
                mw.Write<string[]>(perm.CanEditPermissions.ToArray());
                mw.Write<string[]>(perm.CanControl.ToArray());
                message.data = mw.GetMessageBytes();
            }
            ClientHandler.SendToAll(client, message, false);
        }
        public static void HandlePermissionsUpdate(ClientObject client, byte[] messageData)
        {
            VesselPermissions perm = new VesselPermissions();
            using (MessageReader mr = new MessageReader(messageData))
            {
                perm.VesselID = mr.Read<string>();
                perm.OwnerName = mr.Read<string>();
                perm.OwnerIsFaction = mr.Read<bool>();
                perm.CanEditPermissions = mr.Read<string[]>().ToList();
                perm.CanControl = mr.Read<string[]>().ToList();
            }

            if (PermissionsHandler.vesselperms.ContainsKey(perm.VesselID))
            {
                if (PermissionsHandler.vesselperms[perm.VesselID].OwnerName == perm.OwnerName||PermissionsHandler.vesselperms[perm.VesselID].CanEditPermissions.Contains("<everyone>"))
                {
                    PermissionsHandler.vesselperms[perm.VesselID] = perm;
                    SendPermissionsUpdate(client, perm);
                }

                }
                else
                {
                    PermissionsHandler.vesselperms.Add(perm.VesselID, perm);
                    SendPermissionsUpdate(client, perm);
                }
            }
           
            
        }   
    }


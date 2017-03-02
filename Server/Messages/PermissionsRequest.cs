using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageStream2;
using DarkMultiPlayerCommon;
namespace DarkMultiPlayerServer.Messages
{
    public static class PermissionsRequest
    {
        // Sends a list of all permissions currently loaded on the server
        public static void HandlePermissionsRequest(ClientObject client,byte[] messageData)
        {
            Console.WriteLine("Sending Permissions to " + client.playerName);
            ServerMessage message = new ServerMessage();
            message.type = ServerMessageType.PERMISSIONS_REQUEST;
            using (MessageWriter mw = new MessageWriter())
            {
                mw.Write<int>(PermissionsHandler.vesselperms.Count);
                foreach (var x in PermissionsHandler.vesselperms)
                {
                    var perm = x.Value;
                    mw.Write<string>(perm.VesselID);
                    mw.Write<string>(perm.OwnerName);
                    mw.Write<string[]>(perm.Permissions.Keys.ToArray());
                    mw.Write<int[]>(perm.Permissions.Values.ToArray());

                }
                message.data = mw.GetMessageBytes();
            }
            
            ClientHandler.SendToClient(client, message, false);
            PermissionsComplete(client);
        }
        public static void PermissionsComplete(ClientObject client)
        {
            ServerMessage message = new ServerMessage();
            message.type = ServerMessageType.PERMISSIONS_COMPLETE;
            message.data = new byte[0];
            ClientHandler.SendToClient(client,message,false);
        }
    }
}

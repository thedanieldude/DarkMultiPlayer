using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DarkMultiPlayerCommon;
using MessageStream2;
namespace DarkMultiPlayerServer.Messages
{
    public static class FactionRequest
    {
        public static void SendFactionList(ClientObject client)
        {
            ServerMessage message = new ServerMessage();
            message.type = ServerMessageType.FACTION_REQUEST;
            using(MessageWriter mw = new MessageWriter())
            {
                mw.Write<string>(client.faction);
                mw.Write<int>(FactionsHandler.Factions.Count);
                foreach(var x in FactionsHandler.Factions)
                {
                    var faction = x.Value;
                    mw.Write<string>(faction.FactionID);
                    mw.Write<string>(faction.FactionName);
                    mw.Write<string>(faction.OwnerName);
                    var members = new string[faction.Members.Count];
                    var ranks = new int[faction.Members.Count];
                    int i = 0;
                    foreach (var v in faction.Members)
                    {
                        members[i] = v.Key;
                        ranks[i++] = v.Value;
                    }
                    mw.Write<string[]>(members);
                    mw.Write<int[]>(ranks);
                    mw.Write<bool>(faction.PublicFaction);
                }
                message.data = mw.GetMessageBytes();
            }
            ClientHandler.SendToClient(client, message, false);
            PermissionsRequest.PermissionsComplete(client);
        }
    }
}

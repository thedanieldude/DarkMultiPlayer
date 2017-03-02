using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageStream2;
using DarkMultiPlayerCommon;
namespace DarkMultiPlayerServer.Messages
{
    public static class FactionUpdate
    {
        public static void HandleFactionUpdate(ClientObject client, byte[] messageData)
        {
            Faction faction = new DarkMultiPlayerServer.Faction();
            using(MessageReader mr= new MessageReader(messageData))
            {
                faction.FactionID = mr.Read<string>();
                faction.FactionName = mr.Read<string>();
                faction.OwnerName = mr.Read<string>();
                var members = mr.Read<string[]>();
                var ranks = mr.Read<int[]>();
                faction.Members = new Dictionary<string, int>();
                for(int i = 0; i < members.Length; i++)
                {
                    faction.Members.Add(members[i], ranks[i]);
                }
                faction.PublicFaction = mr.Read<bool>();
            }
            if (FactionsHandler.Factions.ContainsKey(faction.FactionID))
            {
                if ((int)FactionsHandler.Factions[faction.FactionID].RankOf(client.playerName) >= 1)
                {
                    FactionsHandler.Factions[faction.FactionID] = faction;
                    SendFactionUpdate(client, faction);
                }
            }
            else
            {
                FactionsHandler.Factions.Add(faction.FactionID, faction);
                SendFactionUpdate(client, faction);
            }
        }
        public static void SendFactionUpdate(ClientObject client,Faction faction)
        {
            ServerMessage message = new ServerMessage();
            message.type = ServerMessageType.FACTION_UPDATE;
            using(MessageWriter mw = new MessageWriter())
            {
                mw.Write<string>(faction.FactionID);
                mw.Write<string>(faction.FactionName);
                mw.Write<string>(faction.OwnerName);
                var members = new string[faction.Members.Count];
                var ranks = new int[faction.Members.Count];
                int i = 0;
                foreach(var v in faction.Members) { 
                    members[i] = v.Key;
                    ranks[i++] = v.Value;
                }    
                mw.Write<string[]>(members);
                mw.Write<int[]>(ranks);
                mw.Write<bool>(faction.PublicFaction);
                message.data = mw.GetMessageBytes();
            }
            ClientHandler.SendToAll(null, message, false);
        }
    }
}

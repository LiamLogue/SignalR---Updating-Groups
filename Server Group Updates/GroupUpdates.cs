using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Server_Group_Updates
{
    public class GroupUpdates : Hub
    {
        //Variables
        private static List<string> groupNames = new List<string>();
        private static int currentConnections = 0;
        private string groupName;

        public void SendGroupMessage(string pFrom, string pMessage, string pGroupName)
        {
            Clients.Group(pGroupName).broadcastMessage(pFrom, pGroupName, pMessage);
        }

        public void UpdateGroupName(string pGroupName)
        {
            Groups.Add(Context.ConnectionId, pGroupName);
            groupNames.Add(pGroupName);
            groupName = pGroupName;

            Clients.All.deleteAllGroups();

            SendAllGroups();

            Clients.All.broadcastMessage("ADMIN", "ALL", pGroupName + " has joined.");
        }

        public void AddGroupToList(string pGroupName)
        {
            Clients.All.addGroupToList(pGroupName);
        }

        public void SendAllGroups()
        {
            for (int i = 1; i <= groupNames.Count(); i++)
            {
                AddGroupToList(groupNames[i-1]);
            }
        }

        public void SendGroupsData()
        {
            for (int i = 0; i <= 9; i++)
            {
                for (int x = 1; x <= groupNames.Count(); x++)
                {
                    Clients.Group(groupNames[x-1]).broadcastMessage("ADMIN", groupNames[x-1], "Message " + i.ToString() + " from Admin");
                }
            }
        }

        public void ReturnValue(string pValue)
        {
            Clients.Caller.returnValue(pValue);
        }

        public override Task OnConnected()
        {
            currentConnections++;

            Clients.All.updateConnectionCount(currentConnections);

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            if (groupName != null)
            {
                Groups.Remove(Context.ConnectionId, groupName);
                currentConnections--;
            }

            Clients.All.updateConnectionCount(currentConnections);

            return base.OnDisconnected();
        }
    }
}
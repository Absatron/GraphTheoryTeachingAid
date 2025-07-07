using System;
using System.Collections.Generic;
using Avalonia;

namespace TeachingAidMac.Models
{
    [Serializable]
    public class NodeData
    {
        public string Name { get; set; } = "";
        public List<ConnectionData> Connections { get; set; } = new List<ConnectionData>();
        public Point LocationOnGraph { get; set; }

        public void SetName(string name)
        {
            Name = name;
        }

        public void AddConnection(ConnectionData connection)
        {
            Connections.Add(connection);
        }

        public void AddLocation(Point location)
        {
            LocationOnGraph = location;
        }

        public string GetName()
        {
            return Name;
        }

        public List<ConnectionData> GetConnections()
        {
            return Connections;
        }

        public Point GetLocation()
        {
            return LocationOnGraph;
        }
    }
}

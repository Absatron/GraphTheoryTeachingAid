using System;
using System.Collections.Generic;

namespace TeachingAidMac.Models
{
    [Serializable]
    public class GraphData
    {
        public List<NodeData> Nodes { get; set; } = new List<NodeData>();
        public string SourceNodeLetter { get; set; } = "";
        public string TargetNodeLetter { get; set; } = "";

        public void AddNode(NodeData addedNode)
        {
            Nodes.Add(addedNode);
        }

        public void SetSourceNodeLetter(string sourceNodeLetter)
        {
            SourceNodeLetter = sourceNodeLetter;
        }

        public void SetTargetNodeLetter(string targetNodeLetter)
        {
            TargetNodeLetter = targetNodeLetter;
        }

        public List<NodeData> GetGraphNodes()
        {
            return Nodes;
        }

        public void ClearGraph()
        {
            Nodes.Clear();
        }

        public string GetSourceNodeLetter()
        {
            return SourceNodeLetter;
        }

        public string GetTargetNodeLetter()
        {
            return TargetNodeLetter;
        }
    }
}

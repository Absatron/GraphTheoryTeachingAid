using System;

namespace TeachingAidMac.Models
{
    [Serializable]
    public class ConnectionData
    {
        public string StartNodeLetter { get; set; } = "";
        public string EndNodeLetter { get; set; } = "";
        public int EdgeNumber { get; set; }
        public int PreviousEdgeNumber { get; set; }
        public bool PixelMode { get; set; }

        public void SetStartNodeLetter(string startNodeLetter)
        {
            StartNodeLetter = startNodeLetter;
        }

        public void SetEndNodeLetter(string endNodeLetter)
        {
            EndNodeLetter = endNodeLetter;
        }

        public void SetEdgeNumber(int edgeNumber)
        {
            EdgeNumber = edgeNumber;
        }

        public void SetPreviousEdgeNumber(int previousEdgeNumber)
        {
            PreviousEdgeNumber = previousEdgeNumber;
        }

        public void SetPixelMode(bool pixelMode)
        {
            PixelMode = pixelMode;
        }

        public string GetStartNodeLetter()
        {
            return StartNodeLetter;
        }

        public string GetEndNodeLetter()
        {
            return EndNodeLetter;
        }

        public int GetEdgeNumber()
        {
            return EdgeNumber;
        }

        public int GetPreviousEdgeNumber()
        {
            return PreviousEdgeNumber;
        }

        public bool GetPixelMode()
        {
            return PixelMode;
        }
    }
}

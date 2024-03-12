using UnityEngine;

namespace Deckfense
{
    public enum TooltipPosition
    {
        left, right, top, bottom
    }

    public enum TooltipType 
    { 
        Default,
    }

    public class TooltipData
    {
        private Vector3 position;
        public Vector3 Position { get => position; set { position = value; } }

        private Vector3 pivot;
        public Vector3 Pivot { get => pivot; set { pivot = value; } }

        private TooltipPosition tooltipPosition;
        public TooltipPosition TooltipPosition { get => tooltipPosition; set { tooltipPosition = value; } }

        private string header;
        public string Header { get => header; set {  header = value; } }

        private string content;
        public string Content { get => content; set { content = value; } }
    }
}

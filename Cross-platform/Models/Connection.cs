using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Controls;

namespace TeachingAidMac.Models
{
    public class Connection
    {
        public enum ConnectionVisualState
        {
            Normal,
            Highlighted,      // Yellow - final path
            RedHighlighted,   // Red - being explored
            GreenHighlighted, // Green - best path
            OrangeHighlighted // Orange - alternative path
        }

        public Node Source { get; private set; }
        public Node Target { get; private set; }
        public int Weight { get; set; }
        public int PreviousWeight { get; set; }
        public bool PixelMode { get; set; }
        public ConnectionVisualState VisualState { get; set; } = ConnectionVisualState.Normal;

        private Point _startPoint;
        private Point _endPoint;
        private Canvas? _canvas;

        public Node StartNode => Source;
        public Node EndNode => Target;
        public int EdgeNumber => Weight;

        public Connection(Node source, Node target, int weight)
        {
            Source = source;
            Target = target;
            Weight = weight;
            PreviousWeight = 0;
            PixelMode = false;
        }

        public void SetPixelMode(bool pixelMode)
        {
            PixelMode = pixelMode;
            if (pixelMode)
            {
                PreviousWeight = Weight;
            }
            else
            {
                if (PreviousWeight != 0)
                {
                    Weight = PreviousWeight;
                }
            }
            UpdateEdgeNumber();
        }

        public void UpdateEdgeNumber()
        {
            if (PixelMode)
            {
                // Calculate distance between nodes in pixels
                var dx = Source.Bounds.Center.X - Target.Bounds.Center.X;
                var dy = Source.Bounds.Center.Y - Target.Bounds.Center.Y;
                var distance = Math.Sqrt(dx * dx + dy * dy);
                Weight = (int)Math.Round(distance / 20.0);
            }
        }

        public void DrawLine(Canvas? canvas = null)
        {
            _canvas = canvas;
            UpdatePoints();
            // Drawing will be handled by the GraphCanvas control
        }

        public void RemoveLine()
        {
            // Line removal will be handled by the GraphCanvas control
        }

        private void UpdatePoints()
        {
            var sourcePos = Source.GetPosition();
            var targetPos = Target.GetPosition();

            _startPoint = new Point(
                sourcePos.X + Source.Width / 2,
                sourcePos.Y + Source.Height / 2
            );
            _endPoint = new Point(
                targetPos.X + Target.Width / 2,
                targetPos.Y + Target.Height / 2
            );
        }

        public Point GetMidpoint()
        {
            UpdatePoints();
            return new Point(
                (_startPoint.X + _endPoint.X) / 2,
                (_startPoint.Y + _endPoint.Y) / 2
            );
        }

        public Point GetStartPoint()
        {
            UpdatePoints();
            return _startPoint;
        }

        public Point GetEndPoint()
        {
            UpdatePoints();
            return _endPoint;
        }

        public Node GetEndNode()
        {
            return Target;
        }

        public Node GetStartNode()
        {
            return Source;
        }

        public override string ToString()
        {
            return $"{Source.NodeName} -> {Target.NodeName} (Weight: {Weight})";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Connection other)
            {
                return (Source == other.Source && Target == other.Target) ||
                       (Source == other.Target && Target == other.Source);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Source, Target);
        }

        // Additional methods for VB compatibility
        public void DrawHighlightedLine()
        {
            Console.WriteLine($"DEBUG: DrawHighlightedLine called for {Source.NodeName}->{Target.NodeName}, Object ID: {this.GetHashCode()}");
            VisualState = ConnectionVisualState.Highlighted;
            Console.WriteLine($"DEBUG: VisualState set to {VisualState}");

            // For bidirectional graphs, also highlight the reverse connection
            var reverseConnection = Target.GetConnectionTo(Source);
            if (reverseConnection != null && reverseConnection != this)
            {
                Console.WriteLine($"DEBUG: Also highlighting reverse connection {Target.NodeName}->{Source.NodeName}, Object ID: {reverseConnection.GetHashCode()}");
                reverseConnection.VisualState = ConnectionVisualState.Highlighted;
            }

            // Trigger graph redraw if available
            if (Source.CurrentGraph != null)
            {
                Console.WriteLine($"DEBUG: Calling TriggerRedraw for {Source.NodeName}->{Target.NodeName}");
                Source.CurrentGraph.TriggerRedraw();
            }
            else
            {
                Console.WriteLine($"DEBUG: Source.CurrentGraph is null for {Source.NodeName}->{Target.NodeName}");
            }
        }

        public void DrawRedHighlightedLine()
        {
            VisualState = ConnectionVisualState.RedHighlighted;

            // For bidirectional graphs, also highlight the reverse connection
            var reverseConnection = Target.GetConnectionTo(Source);
            if (reverseConnection != null && reverseConnection != this)
            {
                reverseConnection.VisualState = ConnectionVisualState.RedHighlighted;
            }

            // Don't auto-trigger redraw to avoid performance issues during exploration
        }

        public void DrawGreenHighlightedLine()
        {
            VisualState = ConnectionVisualState.GreenHighlighted;

            // For bidirectional graphs, also highlight the reverse connection
            var reverseConnection = Target.GetConnectionTo(Source);
            if (reverseConnection != null && reverseConnection != this)
            {
                reverseConnection.VisualState = ConnectionVisualState.GreenHighlighted;
            }

            // Don't auto-trigger redraw to avoid performance issues
        }

        public void DrawOrangeHighlightedLine()
        {
            VisualState = ConnectionVisualState.OrangeHighlighted;

            // For bidirectional graphs, also highlight the reverse connection
            var reverseConnection = Target.GetConnectionTo(Source);
            if (reverseConnection != null && reverseConnection != this)
            {
                reverseConnection.VisualState = ConnectionVisualState.OrangeHighlighted;
            }

            // Don't auto-trigger redraw to avoid performance issues
        }

        public void ResetHighlighting()
        {
            VisualState = ConnectionVisualState.Normal;

            // For bidirectional graphs, also reset the reverse connection
            var reverseConnection = Target.GetConnectionTo(Source);
            if (reverseConnection != null && reverseConnection != this)
            {
                reverseConnection.VisualState = ConnectionVisualState.Normal;
            }

            // Don't trigger redraw here to avoid circular dependency
        }
    }
}

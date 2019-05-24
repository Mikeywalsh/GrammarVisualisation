using System.Windows.Input;
using TreeVisualisation.Core;

namespace TreeVisualisation.Visualisation
{
    internal class NodeMouseButtonEventArgs : MouseButtonEventArgs
    {
        public ITreeNode ClickedNode;

        public NodeMouseButtonEventArgs(MouseDevice mouse, int timestamp, MouseButton button, ITreeNode clickedNode) : base(mouse, timestamp, button)
        {
            ClickedNode = clickedNode;
        }

        public NodeMouseButtonEventArgs(MouseDevice mouse, int timestamp, MouseButton button, StylusDevice stylusDevice, ITreeNode clickedNode) : base(mouse, timestamp, button, stylusDevice)
        {
            ClickedNode = clickedNode;
        }
    }
}

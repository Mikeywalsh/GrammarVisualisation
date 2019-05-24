using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TreeVisualisation.Implementations.Grammar;

namespace TreeVisualisation.Visualisation
{
    /// <summary>
    /// Interaction logic for TreeVisualiser.xaml
    /// </summary>
    public partial class TreeVisualiser : Window
    {
        private Point _last;
        private bool isDragged;

        private GrammarTree VisualisedTree;

        public TreeVisualiser()
        {
            InitializeComponent();

            GenerateTree();
        }

        private void GenerateTree()
        {
            // TEMP - Create a test tree
            VisualisedTree = new GrammarTree("sampleGrammar.txt", new Vector2(100, 100), new Vector2(150, 100));

            // Get all elements of the tree
            var elements = new List<UIElement>();
            var lineConnections = new List<UIElement>();

            foreach (var node in VisualisedTree.AllNodes)
            {
                elements.AddRange(TreeNodeDrawingFactory.GenerateNodeDrawing(node,null));
                var connection = TreeNodeDrawingFactory.GenerateNodeConnection(node);
                if (connection != null)
                {
                    lineConnections.Add(connection);
                }
            }

            // Actually draw the tree to the canvas
            AddToCanvas(lineConnections);
            AddToCanvas(elements);

            // Show stats about the tree on the canvas
            treeIndexText.Text = "Tree 1 of 1";
            nonTerminalsText.Text = $"Non-Terminals: {VisualisedTree.AllNodes.Count(n => n.Data.NodeType == GrammarNodeType.NONTERMINAL)}";
            terminalsText.Text = $"Terminals: {VisualisedTree.AllNodes.Count(n => n.Data.NodeType == GrammarNodeType.TERMINAL)}";
            errorsText.Text = $"Errors: {VisualisedTree.AllNodes.Count(n => n.Data.NodeType == GrammarNodeType.ERROR)}";
        }

        private void Node_MouseDown(object sender, NodeMouseButtonEventArgs e)
        {
            Console.WriteLine(value: $"Clicked node: {e.ClickedNode}");
        }

        /// <summary>
        /// Generates a line to draw, given coordinates, thickness and color
        /// </summary>
        private static Line GenerateLine(Vector2 from, Vector2 to, int thickness, Color color)
        {
            return new Line()
            {
                X1 = from.X,
                X2 = to.X,
                Y1 = from.Y,
                Y2 = to.Y,
                StrokeThickness = thickness,
                Stroke = new SolidColorBrush(color)
            };
        }

        /// <summary>
        /// Adds a UI element to the canvas so that it is visible
        /// </summary>
        private void AddToCanvas(UIElement element)
        {
            mainCanvas.Children.Add(element);
        }

        /// <summary>
        /// Adds a range of UI elements to the canvas, so that they can all be visible
        /// </summary>
        private void AddToCanvas(IEnumerable<UIElement> elements)
        {
            foreach (var element in elements)
            {
                AddToCanvas(element);
            }
        }

        /// <summary>
        /// Enables the ability to move the canvas when the mouse button is pressed down
        /// </summary>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            CaptureMouse();
            _last = e.GetPosition(this);

            isDragged = true;
        }

        /// <summary>
        /// Stops the ability to move the canvas when the mouse button is released
        /// </summary>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            ReleaseMouseCapture();
            isDragged = false;
        }

        /// <summary>
        /// When dragged, moves the perspective of the canvas
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isDragged == false)
                return;

            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
            {
                var pos = e.GetPosition(this);
                var matrix = mt.Matrix; // it's a struct
                matrix.Translate(pos.X - _last.X, pos.Y - _last.Y);
                mt.Matrix = matrix;
                _last = pos;
            }
        }

        /// <summary>
        /// Allows zooming in/out of the visualisation
        /// </summary>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var scrollingUp = e.Delta > 0;
            var matrix = mt.Matrix;

            var widthScalingOffset = ActualWidth * 0.5f;
            var heightScalingOffset = ActualHeight * 0.5f;

            if (scrollingUp)
            {
                matrix.Translate(-widthScalingOffset, -heightScalingOffset);
                matrix.Scale(1.02f, 1.02f);
                matrix.Translate(widthScalingOffset, heightScalingOffset);
            }
            else
            {
                matrix.Translate(-widthScalingOffset, -heightScalingOffset);
                matrix.Scale(0.98f, 0.98f);
                matrix.Translate(widthScalingOffset, heightScalingOffset);
            }

            mt.Matrix = matrix;
        }
    }
}

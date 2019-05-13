using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TreeVisualisation.Implementations.Grammar;

namespace WpfTreeVisualisation
{
    /// <summary>
    /// Interaction logic for TreeVisualiser.xaml
    /// </summary>
    public partial class TreeVisualiser : Window
    {
        private Point _last;
        private bool isDragged;

        public TreeVisualiser()
        {
            InitializeComponent();

            GenerateTree();
        }

        private void GenerateTree()
        {
            // TEMP - Create a test tree
            var tree = new GrammarTree("sampleGrammar.txt", new Vector2(100, 100), new Vector2(150, 100));

            // Get all elements of the tree
            var elements = new List<UIElement>();
            var lineConnections = new List<UIElement>();

            foreach (var node in tree.AllNodes)
            {
                elements.AddRange(TreeNodeDrawingFactory.GenerateNodeDrawing(node));
                var connection = TreeNodeDrawingFactory.GenerateNodeConnection(node);
                if (connection != null)
                {
                    lineConnections.Add(connection);
                }
            }
           
            var lines = new List<Line>();

            for (int i = 0; i < 1000; i+= 25)
            {
                var from = new Vector2(i, 50);
                var to = new Vector2(i, 100);

                lines.Add(GenerateLine(from, to, 5, Color.FromRgb(0,0,0)));
            }

            AddToCanvas(lineConnections);
            AddToCanvas(elements);
        }

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

        private void AddToCanvas(UIElement element)
        {
            mainCanvas.Children.Add(element);
        }

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

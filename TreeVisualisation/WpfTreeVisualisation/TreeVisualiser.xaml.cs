using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TreeVisualisation.Core;

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
            var numTree = new Tree<string>(new Vector2D(10, 5));
            var root = numTree.SetRoot("TestNode1");
            var child1 = numTree.Add(root, "TestNode2");
            var child1A = numTree.Add(child1, "TestNode3");
            var child1B = numTree.Add(child1, "TestNode4");
            var child1C = numTree.Add(child1, "TestNode5");
            var child2 = numTree.Add(root, "TestNode6");
            numTree.PositionNodes();

            // Draw the root node
            var elements = TreeNodeDrawingFactory.GenerateNodeDrawing(numTree.Root);
           
            var lines = new List<Line>();

            for (int i = 0; i < 1000; i+= 25)
            {
                var from = new Vector2D(i, 50);
                var to = new Vector2D(i, 100);

                lines.Add(GenerateLine(from, to, 5, Color.FromRgb(0,0,0)));
            }

            AddToCanvas(lines);
            AddToCanvas(elements);
        }

        private static Line GenerateLine(Vector2D from, Vector2D to, int thickness, Color color)
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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            CaptureMouse();
            _last = e.GetPosition(this);

            isDragged = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            ReleaseMouseCapture();
            isDragged = false;
        }

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
    }
}

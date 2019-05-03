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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TreeVisualisation.Core;
using TreeVisualisation.Implementations.Grammar;

namespace WpfTreeVisualisation
{
    internal static class TreeNodeDrawingFactory
    {
        private static Brush c_LineBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private static Brush c_HeaderFillBrush = new SolidColorBrush(Color.FromRgb(255, 180, 180));
        private static Brush c_BodyFillBrush = new SolidColorBrush(Color.FromRgb(253, 253, 150));
        private static FontFamily c_MainFont = new FontFamily("Courier New");

        public static List<UIElement> GenerateNodeDrawing<T>(TreeNode<T> node)
        {
            // Initialise a collection of UIElements to return
            var elements = new List<UIElement>();

            // TEMP - Generate point from position of node
            var pos = new Point(node.Position.X, node.Position.Y);

            // Draw the outline box
            var outlineBox = new Rectangle
            {
                Width = 150,
                Height = 100,
                StrokeThickness = 5,
                Stroke = c_LineBrush,
                Fill = c_BodyFillBrush
            };
            elements.Add(outlineBox);

            // Draw the header box
            var headerBox = new Rectangle
            {
                Width = 150,
                Height = 25,
                StrokeThickness = 5,
                Stroke = c_LineBrush,
                Fill = c_HeaderFillBrush
            };
            elements.Add(headerBox);

            // Draw the header text
            var headerText = new TextBlock
            {
                Text = GetNodeText(node).Item1,
                FlowDirection = FlowDirection.LeftToRight,
                FontFamily = c_MainFont
            };
            Canvas.SetLeft(headerText, headerBox.Width / 2 - ((headerText.FontSize * headerText.Text.Length) / 4));
            Canvas.SetTop(headerText, (headerBox.Height / 2) - (headerText.FontSize / 2));
            elements.Add(headerText);

            // Draw the body text
            var bodyText = new TextBlock
            {
                Text = GetNodeText(node).Item2,
                FlowDirection = FlowDirection.LeftToRight,
                FontSize = 20,
                FontFamily = c_MainFont
            };
            Canvas.SetLeft(bodyText, outlineBox.Width / 2 - ((bodyText.FontSize * bodyText.Text.Length) / 4));
            Canvas.SetTop(bodyText, (outlineBox.Height / 2) - (bodyText.FontSize / 2));
            elements.Add(bodyText);

            // Return all elements generated
            return elements;
        }

        private static (string, string) GetNodeText<T>(TreeNode<T> node)
        {
            if (typeof(T) == typeof(GrammarData))
            {
                GrammarData data = node.Data as GrammarData;
                return (data.NodeType.ToString(), data.NodeText);
            }

            // If not a grammar node, just return string representation of data
            return (node.Data.ToString(), "...");
        }
    }
}

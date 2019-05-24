using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TreeVisualisation.Core;
using TreeVisualisation.Implementations.Grammar;

namespace TreeVisualisation.Visualisation
{
    internal static class TreeNodeDrawingFactory
    {
        private static Brush c_LineBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private static Brush c_NonterminalHeaderFillBrush = new SolidColorBrush(Color.FromRgb(255, 180, 180));
        private static Brush c_ErrorHeaderFillBrush = new SolidColorBrush(Color.FromRgb(255, 20, 20));
        private static Brush c_TerminalHeaderFillBrush = new SolidColorBrush(Color.FromRgb(100,190,100));
        private static Brush c_DefaultHeaderFillBrush = new SolidColorBrush(Color.FromRgb(100,100,190));
        private static Brush c_BodyFillBrush = new SolidColorBrush(Color.FromRgb(253, 253, 150));
        private static FontFamily c_MainFont = new FontFamily("Courier New");

        public static List<UIElement> GenerateNodeDrawing<T>(TreeNode<T> node, MouseButtonEventHandler nodeMouseDownCallback)
        {
            // Initialise a collection of UIElements to return
            var elements = new List<UIElement>();

            var pos = node.Position;

            // Draw the outline box
            var outlineBox = new Rectangle
            {
                Width = 150,
                Height = 100,
                StrokeThickness = 5,
                Stroke = c_LineBrush,
                Fill = c_BodyFillBrush
            };
            //outlineBox.MouseDown += nodeMouseDownCallback;
            Canvas.SetLeft(outlineBox, pos.X);
            Canvas.SetTop(outlineBox, pos.Y);
            elements.Add(outlineBox);

            // Draw the header box
            var headerBox = new Rectangle
            {
                Width = 150,
                Height = 25,
                StrokeThickness = 5,
                Stroke = c_LineBrush,
                Fill = GetNodeHeaderBrush(node)
            };
            Canvas.SetLeft(headerBox, pos.X);
            Canvas.SetTop(headerBox, pos.Y);
            elements.Add(headerBox);

            // Draw the header text
            var headerText = new TextBlock
            {
                Text = GetNodeText(node).Item1,
                FlowDirection = FlowDirection.LeftToRight,
                FontFamily = c_MainFont
            };
            Canvas.SetLeft(headerText, pos.X + (headerBox.Width / 2) - ((headerText.FontSize * headerText.Text.Length) / 4));
            Canvas.SetTop(headerText, pos.Y + (headerBox.Height / 2) - (headerText.FontSize / 2));
            elements.Add(headerText);

            // Draw the body text
            var bodyText = new TextBlock
            {
                Text = GetNodeText(node).Item2,
                FlowDirection = FlowDirection.LeftToRight,
                FontSize = 20,
                FontFamily = c_MainFont
            };
            Canvas.SetLeft(bodyText, pos.X + outlineBox.Width / 2 - ((bodyText.FontSize * bodyText.Text.Length) / 4));
            Canvas.SetTop(bodyText, pos.Y + (outlineBox.Height / 2) - (bodyText.FontSize / 2));
            elements.Add(bodyText);

            // Return all elements generated
            return elements;
        }

        public static Line GenerateNodeConnection<T>(TreeNode<T> node)
        {
            if (node.Parent == null)
            {
                return null;
            }

            // TEMP - Generate point from position of node
            var pos = new Point(node.Position.X, node.Position.Y);

            // Return a line from this node to its parent
            return new Line
            {
                X1 = node.Position.X + 75,
                Y1 = node.Position.Y + 50,
                X2 = node.Parent.Position.X + 75,
                Y2 = node.Parent.Position.Y + 50,
                StrokeThickness = 5,
                Stroke = c_LineBrush
            };
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

        private static Brush GetNodeHeaderBrush<T>(TreeNode<T> node)
        {
            if (typeof(T) == typeof(GrammarData))
            {
                GrammarData data = node.Data as GrammarData;
                switch (data.NodeType)
                {
                    case GrammarNodeType.TERMINAL:
                        return c_TerminalHeaderFillBrush;
                    case GrammarNodeType.NONTERMINAL:
                        return c_NonterminalHeaderFillBrush;
                    case GrammarNodeType.ERROR:
                        return c_ErrorHeaderFillBrush;
                }

            }

            return c_DefaultHeaderFillBrush;
        }
    }
}

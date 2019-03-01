using System;
using TreeVisualisation.Core;
using NUnit.Framework;

namespace TreeVisualisationTest
{
	internal class TreeTest
	{
		private Tree<int> numTree;

		[SetUp]
		public void CreateTree()
		{
			numTree = new Tree<int>(new Vector2D(10, 5));
		}

		[Test]
		public void ConstructorTest()
		{
			Assert.NotNull(numTree);
			Assert.IsEmpty(numTree.AllNodes);
		}

		[Test]
		public void SetRootTest()
		{
			var root = numTree.SetRoot(5);

			Assert.AreEqual(root, numTree.Root);
			Assert.AreEqual(5, numTree.Root.Data);
			Assert.AreEqual(1, numTree.AllNodes.Count);
			Assert.AreEqual(0, numTree.Root.Depth);
		}

		[Test]
		public void SetRootTwiceShouldFailTest()
		{
			numTree.SetRoot(1);

			Assert.Throws<Exception>(() => numTree.SetRoot(2));
		}

		[Test]
		public void AddSingleNodeTest()
		{
			var root = numTree.SetRoot(9);

			var newNode = numTree.Add(root, 1000);

			Assert.AreEqual(2, numTree.AllNodes.Count);
			Assert.AreEqual(1, numTree.MaxDepth);
			Assert.AreEqual(9, numTree.Root.Data);
			Assert.AreEqual(1, numTree.Root.Children.Count);
			Assert.AreEqual(1000, newNode.Data);
			Assert.AreEqual(newNode, numTree.Root.Children[0]);
			Assert.AreEqual(1, newNode.Depth);
		}

		[Test]
		public void AddMultipleNodesTest()
		{
			var root = numTree.SetRoot(1);

			var childNode = numTree.Add(root, 10);

			var grandchildNode = numTree.Add(childNode, 100);

			Assert.AreEqual(3, numTree.AllNodes.Count);
			Assert.AreEqual(2, numTree.MaxDepth);

			// Check child
			Assert.AreEqual(1, numTree.Root.Children.Count);
			Assert.AreEqual(10, childNode.Data);
			Assert.AreEqual(childNode, numTree.Root.Children[0]);
			Assert.AreEqual(1, childNode.Depth);

			// Check grandchild
			Assert.AreEqual(1, numTree.Root.Children[0].Children.Count);
			Assert.AreEqual(100, grandchildNode.Data);
			Assert.AreEqual(grandchildNode, numTree.Root.Children[0].Children[0]);
			Assert.AreEqual(2, grandchildNode.Depth);
		}

		[Test]
		public void AddNodeInvalidParentShouldFailTest()
		{
			numTree.SetRoot(999);

			Assert.Throws<Exception>(() => numTree.Add(new TreeNode<int>(4), 546));
		}

		[Test]
		public void GetLeafNodesWithRoot()
		{
			numTree.SetRoot(6);

			Assert.AreEqual(1, numTree.GetLeafNodes().Count);
		}

		[Test]
		public void GetLeafNodesWithSingleChildNode()
		{
			var root = numTree.SetRoot(1);

			numTree.Add(root, 2);

			Assert.AreEqual(1, numTree.GetLeafNodes().Count);
		}

		[Test]
		public void GetLeafNodesWithMultipleChildNodes()
		{
			var root = numTree.SetRoot(1);

			numTree.Add(root, 2);
			numTree.Add(root, 3);

			Assert.AreEqual(2, numTree.GetLeafNodes().Count);
		}

		[Test]
		public void PositionNodesTestRootNode()
		{
			var root = numTree.SetRoot(5);
			numTree.PositionNodes();

			var expectedPosition = new Vector2D(0, 0);
			Assert.AreEqual(expectedPosition, root.Position);
		}

		[Test]
		public void PositionNodesTestSingleChild()
		{
			var root = numTree.SetRoot(5);
			var child = numTree.Add(root, 3);
			numTree.PositionNodes();

			var expectedPosition = new Vector2D(0, 0);
			Assert.AreEqual(expectedPosition, root.Position);

			expectedPosition = new Vector2D(0, -5);
			Assert.AreEqual(expectedPosition, child.Position);
		}

		[Test]
		public void PositionNodesTestMultipleChildrenDepth1()
		{
			var root = numTree.SetRoot(5);
			var child1 = numTree.Add(root, 3);
			var child2 = numTree.Add(root, 6);
			numTree.PositionNodes();

			var expectedPosition = new Vector2D(5, 0);
			Assert.AreEqual(expectedPosition, root.Position);

			expectedPosition = new Vector2D(0, -5);
			Assert.AreEqual(expectedPosition, child1.Position);

			expectedPosition = new Vector2D(10, -5);
			Assert.AreEqual(expectedPosition, child2.Position);
		}

		[Test]
		public void PositionNodesTestMultipleChildrenDepth2()
		{
			var root = numTree.SetRoot(5);
			var child1 = numTree.Add(root, 3);
			var child1A = numTree.Add(child1, 1);
			var child1B = numTree.Add(child1, 2);
			var child1C = numTree.Add(child1, 4);
			var child2 = numTree.Add(root, 6);
			numTree.PositionNodes();

			Assert.AreEqual(4, numTree.GetLeafNodes().Count);

			var expectedPosition = new Vector2D(22.5f, 0);
			Assert.AreEqual(expectedPosition, root.Position);

			expectedPosition = new Vector2D(10, -5);
			Assert.AreEqual(expectedPosition, child1.Position);

			expectedPosition = new Vector2D(0, -10);
			Assert.AreEqual(expectedPosition, child1A.Position);

			expectedPosition = new Vector2D(10, -10);
			Assert.AreEqual(expectedPosition, child1B.Position);

			expectedPosition = new Vector2D(20, -10);
			Assert.AreEqual(expectedPosition, child1C.Position);

			expectedPosition = new Vector2D(35, -5);
			Assert.AreEqual(expectedPosition, child2.Position);
		}

        [Test]
        public void PositionNodesWithWidthAndHeight()
        {
            var treeWithDimensionalNodes = new Tree<string>(new Vector2D(2, 3), new Vector2D(10, 5));

            var root = treeWithDimensionalNodes.SetRoot("foo");
            var child1 = treeWithDimensionalNodes.Add(root, "bar");
            var child2 = treeWithDimensionalNodes.Add(root, "blah");

            treeWithDimensionalNodes.PositionNodes();

            var expectedPosition = new Vector2D(6, 0);
            Assert.AreEqual(expectedPosition, root.Position);

            expectedPosition = new Vector2D(0, -8);
            Assert.AreEqual(expectedPosition, child1.Position);

            expectedPosition = new Vector2D(12,-8);
            Assert.AreEqual(expectedPosition, child2.Position);
        }
	}
}

using NUnit.Framework;
using TreeVisualisation.Core;

namespace TreeVisualisationTest
{
	internal class TreeNodeTest
	{
		[Test]
		public void CreateRootTest()
		{
			var root = new TreeNode<string>("hmm");
			
			Assert.AreEqual("hmm", root.Data);
		}

		[Test]
		public void CreateChildTest()
		{
			var root = new TreeNode<byte>(1);
			var child = new TreeNode<byte>(root, data: 4);

			Assert.AreEqual(1, root.Data);
			Assert.AreEqual(4, child.Data);
			Assert.AreEqual(1, root.Children.Count);
			Assert.AreEqual(child, root.Children[0]);
		}

		[Test]
		public void IsSiblingOfTest()
		{
			var root = new TreeNode<byte>(1);
			var child1 = new TreeNode<byte>(root, data: 2);
			var child2 = new TreeNode<byte>(root, data: 3);

			Assert.True(child1.IsSiblingOf(child2));
			Assert.True(child2.IsSiblingOf(child1));

			Assert.AreEqual(0, root.SiblingCount());
			Assert.AreEqual(1, child1.SiblingCount());
			Assert.AreEqual(1, child2.SiblingCount());
		}

		[Test]
		public void IsNotSiblingOfTest()
		{
			var root = new TreeNode<byte>(1);
			var child1 = new TreeNode<byte>(root, data: 2);
			var child2 = new TreeNode<byte>(root, data: 3);
			var child1A = new TreeNode<byte>(child1, data: 4);
			var child2A = new TreeNode<byte>(child2, data: 5);

			Assert.AreEqual(0, root.SiblingCount());
			Assert.AreEqual(1, child1.SiblingCount());
			Assert.AreEqual(1, child2.SiblingCount());
			Assert.AreEqual(0, child1A.SiblingCount());
			Assert.AreEqual(0, child2A.SiblingCount());

			Assert.True(child1.IsSiblingOf(child2));
			Assert.True(child2.IsSiblingOf(child1));

			Assert.False(child1A.IsSiblingOf(child2A));
			Assert.False(child2A.IsSiblingOf(child1A));

			Assert.False(child1.IsSiblingOf(root));
			Assert.False(root.IsSiblingOf(child1));

			Assert.False(child2.IsSiblingOf(root));
			Assert.False(root.IsSiblingOf(child2));

			Assert.False(child1A.IsSiblingOf(root));
			Assert.False(root.IsSiblingOf(child1A));

			Assert.False(child2A.IsSiblingOf(root));
			Assert.False(root.IsSiblingOf(child2A));
		}

	}
}

using GrammarTree.Core;
using NUnit.Framework;

namespace GrammarTreeTest
{
	[TestFixture]
	internal class Vector2DTest
	{
		private Vector2D vecA;
		private Vector2D vecB;
		private Vector2D vecC;

		[OneTimeSetUp]
		public void CreateTestVectors()
		{
			vecA = new Vector2D(53.5f, 718.2f);
			vecB = new Vector2D(12, 34.93f);
			vecC = new Vector2D(53.5f, 718.2f);
		}
		[Test]
		public void Vector2DCreatedProperlyTest()
		{
			var vector = new Vector2D(53.5f, 718.2f);

			Assert.AreEqual(53.5f, vector.X);
			Assert.AreEqual(718.2f, vector.Y);
		}

		[Test]
		public void EqualityMethodTest()
		{
			Assert.False(vecA.Equals(vecB));
			Assert.False(vecB.Equals(vecA));
			Assert.True(vecA.Equals(vecC));
			Assert.True(vecC.Equals(vecA));
		}

		[Test]
		public void EqualityOperatorTest()
		{
			Assert.True(vecA == vecC);
			Assert.True(vecC == vecA);
		}

		[Test]
		public void InequalityOperatorTest()
		{
			Assert.True(vecA != vecB);
			Assert.True(vecB != vecA);
		}

		[Test]
		public void HashCodeTest()
		{
			Assert.AreNotEqual(vecA.GetHashCode(), vecB.GetHashCode());
			Assert.AreEqual(vecA.GetHashCode(), vecC.GetHashCode());
		}
	}
}

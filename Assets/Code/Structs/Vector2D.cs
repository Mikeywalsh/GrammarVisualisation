using System;

/// <summary>
/// A vector representing a 2D position in space
/// </summary>
public struct Vector2D
{
	/// <summary>
	/// The X position of this vector
	/// </summary>
	public readonly float X;

	/// <summary>
	/// The Y position of this vector
	/// </summary>
	public readonly float Y;

	/// <summary>
	/// Creates a vector representing a 2D position
	/// </summary>
	/// <param name="x">The X position of this vector</param>
	/// <param name="y">The Y position of this vector</param>
	public Vector2D(float x, float y)
	{
		X = x;
		Y = y;
	}

	public override bool Equals(object obj)
	{
		return obj is Vector2D vec &&
		       Math.Abs(vec.X - X) < float.Epsilon &&
		       Math.Abs(vec.Y - Y) < float.Epsilon;
	}

	public override int GetHashCode()
	{
		return X.GetHashCode() ^ Y.GetHashCode();
	}

	public static bool operator ==(Vector2D a, Vector2D b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(Vector2D a, Vector2D b)
	{
		return !(a == b);
	}
}

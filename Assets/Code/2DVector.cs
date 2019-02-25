using System;

/// <summary>
/// A vector representing a 2D position in space
/// </summary>
public struct Vector2D
{
	/// <summary>
	/// The X position of this vector
	/// </summary>
	public readonly float XPos;

	/// <summary>
	/// The Y position of this vector
	/// </summary>
	public readonly float YPos;

	/// <summary>
	/// Creates a vector representing a 2D position
	/// </summary>
	/// <param name="xPos">The X position of this vector</param>
	/// <param name="yPos">The Y position of this vector</param>
	public Vector2D(float xPos, float yPos)
	{
		XPos = xPos;
		YPos = yPos;
	}

	public override bool Equals(object obj)
	{
		return obj is Vector2D vec &&
		       Math.Abs(vec.XPos - XPos) < float.Epsilon &&
		       Math.Abs(vec.YPos - YPos) < float.Epsilon;
	}

	public override int GetHashCode()
	{
		return XPos.GetHashCode() ^ YPos.GetHashCode();
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

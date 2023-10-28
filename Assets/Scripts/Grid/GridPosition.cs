

using System;

public struct GridPosition : IEquatable<GridPosition>
{
    public int X; public int Z;

    public GridPosition(int x, int z)
    {
        this.X = x; this.Z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               X == position.X &&
               Z == position.Z;
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Z);
    }

    public override string ToString()
    {
        return $"x: {X}, z: {Z}";
    }

    public static bool operator ==(GridPosition lhs, GridPosition rhs)
    {
        return lhs.X == rhs.X && lhs.Z == rhs.Z;
    }

    public static bool operator !=(GridPosition lhs, GridPosition rhs) 
    {  
        return !(lhs == rhs);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.X + b.X, a.Z + b.Z);
    }
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.X - b.X, a.Z - b.Z);
    }

}

using System;
using UnityEngine;

public struct Coordinates : IEquatable<Coordinates>
{
    public readonly int x, y;

    public Coordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString() => $"({x},{y})";

    public static Coordinates Zero = new Coordinates(0, 0);

    public void Deconstruct(out int x, out int y)
    {
        x = this.x;
        y = this.y;
    }

    public bool Equals(Coordinates other) => this.x == other.x && this.y == other.y;
    public override bool Equals(object obj) => this.Equals((Coordinates)obj);
    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(Coordinates a, Coordinates b) => a.Equals(b);
    public static bool operator !=(Coordinates a, Coordinates b) => !a.Equals(b);

    public static implicit operator Coordinates((int, int) t) => new Coordinates(t.Item1, t.Item2);

    public static implicit operator Vector2(Coordinates coords) => new Vector2(coords.x, coords.y);
    public static implicit operator Vector3(Coordinates coords) => new Vector3(coords.x, coords.y);
}
using System;

namespace ConsoleApp
{
    public class Coords
    {
        public Coords((int x, int y) coords) : this(coords.x, coords.y)
        {
        }

        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public static Coords operator +(Coords lCoords, (int x, int y) rCoords)
        {
            return new Coords(lCoords.X + rCoords.x, lCoords.Y + rCoords.y);
        }

        public static Coords operator -(Coords lCoords, (int x, int y) rCoords)
        {
            return new Coords(lCoords.X - rCoords.x, lCoords.Y - rCoords.y);
        }

        public static bool operator ==(Coords lCoords, (int x, int y) rCoords)
        {
            return lCoords is not null && lCoords.X == rCoords.x && lCoords.Y == rCoords.y;
        }

        public static bool operator !=(Coords lCoords, (int x, int y) rCoords)
        {
            return !(lCoords == rCoords);
        }

        public static bool operator ==(Coords lCoords, Coords rCoords)
        {
            return lCoords is not null && rCoords is not null &&
                   lCoords.X == rCoords.X && lCoords.Y == rCoords.Y;
        }

        public static bool operator !=(Coords lCoords, Coords rCoords)
        {
            return !(lCoords == rCoords);
        }

        private bool Equals(Coords other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Coords) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
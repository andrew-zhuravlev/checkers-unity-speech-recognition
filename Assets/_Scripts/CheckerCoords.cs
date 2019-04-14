using System;

[Serializable]
public struct CheckerCoords {
    public int Y, X;

    public CheckerCoords(int y, int x) {
        Y = y;
        X = x;
    }

    public static CheckerCoords operator +(CheckerCoords one, CheckerCoords other) {
        return new CheckerCoords(one.Y + other.Y, one.X + other.X);
    }
}

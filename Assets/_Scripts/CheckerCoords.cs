using System;

[Serializable]
public struct CheckerCoords {
    public int Y, X;

    public CheckerCoords(int y, int x) {

        if(y < 1 || y > 8)
            throw new ArgumentOutOfRangeException();

        if(x < 1 || x > 8)
            throw new ArgumentOutOfRangeException();

        int yRemainder = y % 2, xRemainder = x % 2;

        if(yRemainder != xRemainder)
            throw new ArgumentException("Could not have white location");

        Y = y;
        X = x;
    }
}

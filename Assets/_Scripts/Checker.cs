using UnityEngine;

public class Checker : MonoBehaviour
{
    [SerializeField] Sprite _whiteCheckerSprite;
    [SerializeField] Sprite _blackCheckerSprite;

    CheckerCoords _coords;
    public bool IsWhite { get; private set; }

    public void Init(CheckerCoords checkerCoords, bool isWhite) {
        _coords = checkerCoords;
        IsWhite = isWhite;

        GetComponent<SpriteRenderer>().sprite = isWhite ? _whiteCheckerSprite : _blackCheckerSprite;
    }

    public bool IsValidMove(CheckerCoords newCoords) {
        var deltaY = newCoords.Y - _coords.Y;
        var deltaX = newCoords.X - _coords.X;
        var absoluteDeltaX = Mathf.Abs(deltaX);

        switch (deltaY) {
            case 1:
                return absoluteDeltaX == 1
                       && CheckerBoard.SharedInstance.CheckerAt(newCoords) == null;
            case 2:
                if (absoluteDeltaX == 2)
                    if (CheckerBoard.SharedInstance.CheckerAt(newCoords) == null) {
                        var killCoords = new CheckerCoords(_coords.Y + deltaY / 2, _coords.X + deltaX / 2);

                        var killChecker = CheckerBoard.SharedInstance.CheckerAt(killCoords);
                        return killChecker != null && killChecker.IsWhite != IsWhite;
                    }
                return false;

            default:
                return false;
        }
    }
}

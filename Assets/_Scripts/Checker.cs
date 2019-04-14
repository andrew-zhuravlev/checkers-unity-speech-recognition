using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    [SerializeField] Sprite _whiteCheckerSprite;
    [SerializeField] Sprite _blackCheckerSprite;
    [SerializeField] Sprite _whiteQueenSprite;
    [SerializeField] Sprite _blackQueenSprite;
    [SerializeField] SpriteRenderer _spriteRenderer;

    CheckerCoords _coords;
    public bool IsWhite { get; private set; }
    public bool IsQueen { get; private set; }

    public void BecomeQueen() {
        IsQueen = true;

        _spriteRenderer.sprite = IsWhite ? _whiteQueenSprite : _blackQueenSprite;
    }

    public void SetCoords(CheckerCoords newCoords) {
        _coords = newCoords;
        transform.position = CheckerBoard.SharedInstance.CheckerToWorldCoords(newCoords);
    }

    public void Init(CheckerCoords checkerCoords, bool isWhite) {
        _coords = checkerCoords;
        IsWhite = isWhite;

        _spriteRenderer.sprite = isWhite ? _whiteCheckerSprite : _blackCheckerSprite;
    }

    public static bool IsValidMove(CheckerCoords newCoords, Checker checker) {

        if (newCoords.Y < 1 || newCoords.Y > 8 || newCoords.X < 1 || newCoords.X > 8 
            || CheckerBoard.SharedInstance.isWhiteMove != checker.IsWhite)
            return false;
        //int yRemainder = newCoords.Y % 2, xRemainder = newCoords.X % 2;
        //if (yRemainder != xRemainder)
        //    return false;

        var deltaY = newCoords.Y - checker._coords.Y;
        var deltaX = newCoords.X - checker._coords.X;
        var absoluteDeltaX = Mathf.Abs(deltaX);
        var absoluteDeltaY = Mathf.Abs(deltaY);

        switch (absoluteDeltaY) {
            case 1:
                //TODO:
                return absoluteDeltaX == 1
                       && CheckerBoard.SharedInstance.CheckerAt(newCoords) == null;
            case 2:
                return IsKillMoveValid(absoluteDeltaY, absoluteDeltaX, newCoords, deltaY, deltaX, checker);

            default:
                return false;
        }
    }

    static bool IsKillMoveValid(int absoluteDeltaY, int absoluteDeltaX, CheckerCoords newCoords, 
                                                    int deltaY, int deltaX, Checker checker) {

        if (absoluteDeltaX == 2 && absoluteDeltaY == 2)
            if (CheckerBoard.SharedInstance.CheckerAt(newCoords) == null) {
                var killCoords = checker._coords + new CheckerCoords(deltaY / 2, deltaX / 2);

                var killChecker = CheckerBoard.SharedInstance.CheckerAt(killCoords);
                return killChecker != null && killChecker.IsWhite != checker.IsWhite;
            }

        return false;
    }

    public static List<CheckerMove> GetValidMoves(Checker checker) {
        var moves = new List<CheckerMove>();

        var ySign = checker.IsWhite ? 1 : -1;

        var leftDiagonalCell = new CheckerCoords(ySign, -1);
        var rightDiagonalCell = new CheckerCoords(ySign, 1);

        var oneLeftMove = checker._coords + leftDiagonalCell;
        var twoLeftMove = oneLeftMove + leftDiagonalCell;

        var oneRightMove = checker._coords + rightDiagonalCell;
        var twoRightMove = oneRightMove + rightDiagonalCell;

        if (IsValidMove(oneLeftMove, checker))
            moves.Add(new CheckerMove{ destinationCoords = oneLeftMove, oldCoords = checker._coords });
        else if (IsValidMove(twoLeftMove, checker))
            moves.Add(new CheckerMove() {
                destinationCoords = twoLeftMove, killCoords = oneLeftMove, isKill = true, oldCoords = checker._coords
            });

        if (IsValidMove(oneRightMove, checker))
            moves.Add(new CheckerMove { destinationCoords = oneRightMove, oldCoords = checker._coords});
        else if (IsValidMove(twoRightMove, checker))
            moves.Add(new CheckerMove() {
                destinationCoords = twoRightMove, killCoords = oneRightMove, isKill = true, oldCoords = checker._coords
            });

        return moves;
    }

    public static List<CheckerMove> GetChainedValidMoves(CheckerMove move, Checker checker) {
        var result = new List<CheckerMove>();
        var topLeft = move.destinationCoords + new CheckerCoords(2, -2);
        var topRight = move.destinationCoords + new CheckerCoords(2, 2);
        var bottomRight = move.destinationCoords + new CheckerCoords(-2, 2);
        var bottomLeft = move.destinationCoords + new CheckerCoords(-2, -2);

        // TODO: Make * operator
        if (IsValidMove(topLeft, checker)) {
            result.Add(new CheckerMove {
                destinationCoords = topLeft,
                isKill = true,
                killCoords = move.destinationCoords + new CheckerCoords(1, -1),
                oldCoords = move.destinationCoords
            });
        }

        if (IsValidMove(topRight, checker)) {
            result.Add(new CheckerMove {
                destinationCoords = topRight,
                isKill = true,
                killCoords = move.destinationCoords + new CheckerCoords(1, 1),
                oldCoords = move.destinationCoords
            });
        }

        if (IsValidMove(bottomRight, checker)) {
            result.Add(new CheckerMove {
                destinationCoords =  bottomRight,
                isKill = true,
                killCoords = move.destinationCoords + new CheckerCoords(-1, 1),
                oldCoords = move.destinationCoords
            });
        }

        if (IsValidMove(bottomLeft, checker)) {
            result.Add(new CheckerMove {
                destinationCoords = bottomLeft,
                isKill = true,
                killCoords = move.destinationCoords + new CheckerCoords(-1,-1),
                oldCoords = move.destinationCoords
            });
        }

        return result;
    }

    public void Die() {
        gameObject.SetActive(false);
    }
}

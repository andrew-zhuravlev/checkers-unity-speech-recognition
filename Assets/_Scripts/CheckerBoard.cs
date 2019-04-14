using UnityEngine;
using System.Collections.Generic;

public class CheckerBoard : MonoBehaviour
{
    [SerializeField] Checker _checkerPrefab;
    [SerializeField] Transform _dummyChecker;
    [SerializeField] float CellSize = 2.57f;

    public bool isWhiteMove = true;

    public List<Checker> WhiteCheckers { get; } = new List<Checker>();
    public List<Checker> BlackCheckers { get; } = new List<Checker>();

    readonly Dictionary<CheckerCoords, Checker> _allCheckers = new Dictionary<CheckerCoords, Checker>();

    public Checker CheckerAt(CheckerCoords coords) {
        return !_allCheckers.ContainsKey(coords) ? null : _allCheckers[coords];
    }

    public static CheckerBoard SharedInstance { get; private set; }
    
    void Awake() {
        if (SharedInstance == null) {
            SharedInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Debug.LogWarning("Destroyed Copy of CheckerBoard!");
            DestroyImmediate(gameObject);
        }


        for (int y = 1; y <= 3; ++y)
            CreateCheckerRow(y, true);

        for (int y = 6; y <= 8; ++y)
            CreateCheckerRow(y, false);
    }

    void CreateCheckerRow(int y, bool isWhite) {
        int beginCell = y % 2 == 0 ? 2 : 1;
        for (int x = beginCell; x <= beginCell + 6; x += 2) {
            var createdChecker = Instantiate(_checkerPrefab, CheckerToWorldCoords(y, x), 
                Quaternion.identity, transform);

            var checkerCoords = new CheckerCoords(y, x);
            createdChecker.Init(checkerCoords, isWhite);

            if (isWhite)
                WhiteCheckers.Add(createdChecker);
            else
                BlackCheckers.Add(createdChecker);

            _allCheckers.Add(checkerCoords, createdChecker);
        }
    }

    public Vector2 CheckerToWorldCoords(CheckerCoords coords) {
        return CheckerToWorldCoords(coords.Y, coords.X);
    }

    public Vector2 CheckerToWorldCoords(int y, int x) {
        return (Vector2) _dummyChecker.position
            + Vector2.up * (y - 1) * CellSize + Vector2.right * (x - 1) * CellSize;
    }

    public bool TryMove(CheckerMove move, bool end = true) {

        bool success = false;

        if (_allCheckers.ContainsKey(move.oldCoords)) {

            var checker = _allCheckers[move.oldCoords];
            bool isValidMove = Checker.IsValidMove(move.destinationCoords, checker);
            bool isValidKill = !move.isKill || _allCheckers.ContainsKey(move.killCoords);

            success = isValidMove && isValidKill;

            if (success) {
                if (move.isKill) {
                    var killedChecker = _allCheckers[move.killCoords];
                    _allCheckers.Remove(move.killCoords);

                    bool isKilledWhite = killedChecker.IsWhite;
                    if (isKilledWhite)
                        WhiteCheckers.Remove(killedChecker);
                    else
                        BlackCheckers.Remove(killedChecker);
                    killedChecker.Die();
                }

                checker.SetCoords(move.destinationCoords);

                _allCheckers.Remove(move.oldCoords);
                _allCheckers.Add(move.destinationCoords, checker);
            }
        }

        if (success && end)
            isWhiteMove = !isWhiteMove;
        return success;
    }

    public void EndMove() {
        isWhiteMove = !isWhiteMove;
    }
}

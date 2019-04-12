using UnityEngine;
using System.Collections.Generic;

public class CheckerBoard : MonoBehaviour
{
    [SerializeField] Checker _checkerPrefab;
    [SerializeField] Transform _dummyChecker;
    [SerializeField] float _cellSize = 2.57f;

    Dictionary<CheckerCoords, Checker> _whiteCheckers = new Dictionary<CheckerCoords, Checker>();
    Dictionary<CheckerCoords, Checker> _blackCheckers = new Dictionary<CheckerCoords, Checker>();

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
            var createdChecker = Instantiate(_checkerPrefab, (Vector2)_dummyChecker.position
                + Vector2.up * (y - 1) * _cellSize + Vector2.right * (x - 1) * _cellSize, Quaternion.identity, transform);

            var checkerCoords = new CheckerCoords(y, x);
            createdChecker.Init(checkerCoords, isWhite);

            if (isWhite)
                _whiteCheckers.Add(checkerCoords, createdChecker);
            else
                _blackCheckers.Add(checkerCoords, createdChecker);

            _allCheckers.Add(checkerCoords, createdChecker);
        }
    }
}

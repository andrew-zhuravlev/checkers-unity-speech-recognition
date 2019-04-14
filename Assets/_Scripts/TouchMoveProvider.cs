using System.Collections.Generic;
using UnityEngine;

class TouchMoveProvider : MonoBehaviour {

    [SerializeField] Camera _mainCamera;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] CheckerBoard _board;

    Checker _grabbedChecker;
    bool _locked;

    ObjectPooler _objectPooler;

    readonly Dictionary<GameObject, CheckerMove> _ghosts = new Dictionary<GameObject, CheckerMove>();

    void Start() {
        _objectPooler = ObjectPooler.Instance;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var mousePosition = Input.mousePosition;
            var hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(mousePosition), Vector3.zero, 
                0, _layerMask);

            if (hit.collider != null) {
                var checker = hit.collider.GetComponent<Checker>();

                if (checker == null) {
                    var move = _ghosts[hit.collider.gameObject];
                    var moveResult = _board.TryMove(_ghosts[hit.collider.gameObject], !move.isKill);

                    if (move.isKill) {
                        var moves = _grabbedChecker.GetValidMoves(false);

                        _locked = moves.Count > 0;

                        if (!_locked) {
                            ClearAvailableMoves();
                            _board.EndMove();
                        }
                        else
                            ShowAvailableMoves(moves);
                    }
                    else
                        ClearAvailableMoves();

                    Debug.Log(moveResult ? "Move was successful" : "Move was NOT successful");
                }
                else if (!_locked) {
                    _grabbedChecker = checker;

                    var moves = checker.GetValidMoves(true);
                    ShowAvailableMoves(moves);
                }
            }
        }
    }

    void HideAvailableMoves() {
        foreach (var ghost in _ghosts) {
            ghost.Key.SetActive(false);
        }
    }

    void ClearAvailableMoves() {
        HideAvailableMoves();
        _ghosts.Clear();
    }

    void ShowAvailableMoves(List<CheckerMove> moves) {

        ClearAvailableMoves();

        foreach (var move in moves) {
            var ghost = _objectPooler.GetPooledObject(0);
            ghost.transform.position = _board.CheckerToWorldCoords(move.destinationCoords);
            ghost.SetActive(true);

            _ghosts.Add(ghost, move);
        }
    }
}

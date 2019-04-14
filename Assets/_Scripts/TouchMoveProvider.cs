using System.Collections.Generic;
using UnityEngine;

class TouchMoveProvider : MonoBehaviour {

    [SerializeField] Camera _mainCamera;
    [SerializeField] GameObject _checkerGhost;
    [SerializeField] LayerMask _layerMask;

    Checker _grabbed;
    bool _locked = false;

    readonly Dictionary<GameObject, CheckerMove> _ghosts = new Dictionary<GameObject, CheckerMove>();

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var mousePosition = Input.mousePosition;
            var hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(mousePosition), Vector3.zero, 
                0, _layerMask);

            if (hit.collider != null) {
                var checker = hit.collider.GetComponent<Checker>();

                if (checker == null) {
                    var move = _ghosts[hit.collider.gameObject];
                    var moveResult = CheckerBoard.SharedInstance.TryMove(_ghosts[hit.collider.gameObject], !move.isKill);

                    if (move.isKill) {
                        var moves = Checker.GetChainedValidMoves(move, _grabbed);

                        _locked = moves.Count > 0;

                        if(!_locked) {
                            CheckerBoard.SharedInstance.EndMove();
                        }
                        ShowAvailableMoves(moves);
                    }
                    else
                        ClearAvailableMoves();

                    Debug.Log(moveResult ? "Move was successful" : "Move was NOT successful");
                }
                else if (!_locked) {
                    Debug.Log("Checker");

                    _grabbed = checker;

                    var moves = Checker.GetValidMoves(checker);
                    ShowAvailableMoves(moves);
                }
            }
        }
    }

    void ClearAvailableMoves() {
        foreach (var ghost in _ghosts) {
            ghost.Key.SetActive(false);
        }
        _ghosts.Clear();
    }

    void ShowAvailableMoves(List<CheckerMove> moves) {

        ClearAvailableMoves();

        foreach (var move in moves) {
            var ghost = ObjectPooler.Instance.GetPooledObject(0);
            ghost.transform.position = CheckerBoard.SharedInstance.CheckerToWorldCoords(move.destinationCoords);
            ghost.SetActive(true);

            _ghosts.Add(ghost, move);
        }
    }
}

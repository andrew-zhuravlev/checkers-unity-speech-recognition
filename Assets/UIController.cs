using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] PanelView _panelView;

    void Start()
    {
        CheckerBoard.OnGameEnd += isWhite => {
            _panelView.Init(isWhite);
            _panelView.gameObject.SetActive(true);
        };
    }
}

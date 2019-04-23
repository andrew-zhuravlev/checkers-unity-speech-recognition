using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelView : MonoBehaviour
{
    [SerializeField] Button replayButton;
    [SerializeField] Text _playerWinText;

    public void Awake() {

        Debug.Log("Awake!");

        replayButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    internal void Init(bool isWhite) {
        if (isWhite) {
            _playerWinText.text = "White wins!";
        }
        else
            _playerWinText.text = "Black wins!";
    }
}

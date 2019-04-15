using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechController : MonoBehaviour
{
    public Button applyButton;
    public InputField output;
    [SerializeField] CheckerBoard _board;

    private void Start() {
        applyButton.onClick.AddListener(CheckOutput);
    }

    private void CheckOutput() {
        var t = output.text;

        List<CheckerCoords> coords = new List<CheckerCoords>();
        var split = t.Split(' ');

        var checker = _board.CheckerAt(TextToCoords(t));

        for (int i = 1; i < split.Length; ++i) {
            
        }

        _board.TryMove();
    }

    private static CheckerCoords TextToCoords(string text) {
        if(text.Length == 2) {

        }
        throw new System.Exception();
    }
}

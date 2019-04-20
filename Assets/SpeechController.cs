using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechController : MonoBehaviour
{
    public Button applyButton;
    public InputField output;
    [SerializeField] CheckerBoard _board;
    [SerializeField] AndroidDebug androidDebug;
    [SerializeField] TouchMoveProvider touchMoveProvider;
    [SerializeField] Camera _camera;

    void Start() {
        applyButton.onClick.AddListener(CheckOutput);
    }

    void CheckOutput() {

        try {
            var t = output.text;
            var split = t.Split(new [] { " " }, StringSplitOptions.RemoveEmptyEntries);


            for (int i = 0; i < split.Length; ++i) {
                CheckerCoords coords = TextToCoords(split[i]);

                touchMoveProvider.Raycast(_board.CheckerToWorldCoords(coords));
            }
        }
        catch (Exception e) {
            androidDebug.AddLog(e.ToString());
        }
    }

    static Dictionary<char, int> charsToInt = new Dictionary<char, int> {
            {'A', 1 },
            {'B', 2 },
            {'C', 3 },
            {'D', 4 },
            {'E', 5 },
            {'F', 6 },
            {'G', 7 },
            {'H', 8 }
        };

    static Dictionary<char, int> charsToIntRu = new Dictionary<char, int> {
            {'А', 1 },
            {'Б', 2 },
            {'В', 3 },
            {'Г', 4 },
            {'Д', 5 },
            {'Е', 6 },
            {'Ж', 7 },
            {'З', 8 }
        };

    static CheckerCoords TextToCoords(string text) {
        if (text.Length == 2) {
            int x;
            var tmp = char.ToUpper(text[0]);
            if (SampleSpeechToText.I.txtLocale.text.Contains("ru"))
                x = charsToIntRu[tmp];
            else
                x = charsToInt[tmp];

            int y = text[1] - '0';

            return new CheckerCoords(y, x);
        }
        throw new Exception("Length is not 2");
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField, NotNull] private TMP_Text _text;

    private void OnValidate()
    {
        _text ??= GetComponent<TMP_Text>();
    }

    public void OnDisplay(int score)
    {
        _text.text = $"{score}";
    }
}

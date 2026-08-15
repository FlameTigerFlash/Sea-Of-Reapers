using TMPro;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField, NotNull] private TMP_Text _text;

    private void OnValidate()
    {
        _text ??= GetComponent<TMP_Text>();
    }

    public void OnDisplay(float health)
    {
        _text.text = $"{(int)health}";
    }
}

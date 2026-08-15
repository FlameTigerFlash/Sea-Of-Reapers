using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class AimingDisplay : MonoBehaviour
{
    [SerializeField, NotNull] private Image AimingArea;
    [SerializeField, NotNull] private Image AimingStick;

    private Vector2 _areaSize = Vector2.zero;

    private bool _currentlyEnabled = false;

    private void Awake()
    {
        _areaSize = AimingArea.rectTransform.rect.size;
    }

    public void Display(Vector2 pos)
    {
        pos = Vector2.ClampMagnitude(pos, 1f);
        AimingStick.rectTransform.localPosition = Vector2.Scale(pos, _areaSize / 2);

        if (_currentlyEnabled == false)
        {
            AimingArea.enabled = true;
            AimingStick.enabled = true;
            _currentlyEnabled = true;
        }
    }

    public void Hide()
    {
        if (!_currentlyEnabled)
        {
            return;
        }
        AimingArea.enabled = false;
        AimingStick.enabled = false;
        _currentlyEnabled = false;
    }
}

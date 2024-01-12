using UniRx;
using UnityEngine;

public class Button : MonoBehaviour
{
    public ReactiveProperty<GameObject> Click1
    {
        get => _click;
    }

    private ReactiveProperty<GameObject> _click;

    private void Awake()
    {
        _click = new();
    }

    public void OnClick()
    {
        _click.Value = gameObject;
    }
}

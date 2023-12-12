using UniRx;
using UnityEngine;

public class Button : MonoBehaviour
{
    public ReactiveProperty<bool> Click1
    {
        get => _click;
    }

    private ReactiveProperty<bool> _click = new ReactiveProperty<bool>(false);
    public void OnClick()
    {
        _click.Value = true;
    }
}

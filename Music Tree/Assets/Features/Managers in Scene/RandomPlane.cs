using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RandomPlane : MonoBehaviour
{
    private Material _material;
    [SerializeField] 
    private GameObject mainPlane;

    private float _timeAccumulator = 0;

    private ReactiveProperty<bool> _isTriggered = new(false);

    public ReactiveProperty<bool> isTriggered => _isTriggered;
    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<Renderer>().material;
        var material = mainPlane.GetComponent<Renderer>().material;

        PlayerController.Instance.isRandomGitarTriggered.Skip(1)
            .Subscribe(_ =>
        {
            Debug.Log(material.color);
            material.color = _material.color;
            isTriggered.Value = !isTriggered.Value;
        }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        _timeAccumulator += Time.deltaTime;
        if (_timeAccumulator > 1)
        {
            _material.color = Random.ColorHSV();
            _timeAccumulator = 0;
        }
    }
}

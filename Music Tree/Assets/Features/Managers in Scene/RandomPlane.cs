using UnityEngine;

public class RandomPlane : MonoBehaviour
{
    private Material _material;

    private float _timeAccumulator = 0;
    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<Renderer>().material;
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

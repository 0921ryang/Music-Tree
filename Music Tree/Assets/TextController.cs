using UnityEngine;

public class TextController : MonoBehaviour
{
    private GameObject _gameObject;
    void Start()
    {
        _gameObject = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_gameObject.transform);
    }
}

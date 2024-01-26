using UniRx;
using UnityEngine;

public class Audience : MonoBehaviour
{
    private Quaternion _rotation;
    private bool _shouldLook = false;
    private Transform _player;
    private Vector3 _direction;
    
    // Start is called before the first frame update
    void Start()
    {
        _rotation = transform.rotation;
        _player = PlayerController.Instance.transform;
        _direction = _player.position - transform.position;

        PlayerController.Instance.IsOnStage.Subscribe(isOnStage =>
        {
            if (isOnStage)
            {
                _shouldLook = true;
            }
            else
            {
                transform.rotation = _rotation;
                _shouldLook = false;
            }
        }).AddTo(this);
    }

    private void Update()
    {
        var dir = _player.position - transform.position;
        if (_shouldLook && Vector3.Dot(dir, transform.forward) > 0.9)
        {
            Quaternion rotation = Quaternion.LookRotation(_player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 4*Time.deltaTime);
        }
        else if (transform.rotation != _rotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _rotation, 4*Time.deltaTime);
        }
    }
}

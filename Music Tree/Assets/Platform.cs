using UnityEngine;
using UnityEngine.Serialization;

public class Platform : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _isEnter = false;
    public AudioClip audioClip;
    public Transform transformObject;
    public bool isMove = false;
    private AudioSource _audioSource;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = audioClip;
        transformObject = transform;
    }

    private void Update()
    {
        if (isMove)
        {
            var _pos = transformObject.position;// target
            var horizontalMove = new Vector3(_pos.x - transform.position.x, 
                0, _pos.z - transform.position.z);
            horizontalMove.Normalize();
            
            var position = _rigidbody.position;
            bool x1 = position.x >= _pos.x + 1;
            bool y1 = position.x <= _pos.x - 1;
            bool x2 = position.z >= _pos.z + 1;
            bool y2 = position.z <= _pos.z - 1;
            if (_rigidbody.position.y <= 25 && (x1 || y1 || x2 || y2))
            {
                _rigidbody.velocity = Vector3.up * 3;
            }
            else if (x1 || y1 || x2 || y2)
            {
                _rigidbody.velocity = horizontalMove * 5f;
            }
            else
            {
                if (position.y >= _pos.y + 1)
                {
                    _rigidbody.velocity = Vector3.down * 5f;
                }
                else
                {
                    isMove = false;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isEnter == false)
        {
            Debug.Log("music play");
            _audioSource.Play();
            _isEnter = true;
        }
    }
}

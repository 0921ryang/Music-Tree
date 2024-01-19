using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Platform : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Transform transformObject;
    public bool isMove = false;
    [SerializeField] private String room;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        transformObject = transform;
    }

    private void Update()
    {
        /*
        if (isMove)
        {
            var pos = transformObject.position;// target
            var horizontalMove = new Vector3(pos.x - transform.position.x, 
                0, pos.z - transform.position.z);
            horizontalMove.Normalize();
            
            var position = _rigidbody.position;
            bool x1 = position.x >= pos.x + 1;
            bool y1 = position.x <= pos.x - 1;
            bool x2 = position.z >= pos.z + 1;
            bool y2 = position.z <= pos.z - 1;
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
                if (position.y >= pos.y + 1)
                {
                    _rigidbody.velocity = Vector3.down * 5f;
                }
                else
                {
                    isMove = false;
                }
            }
        }
        */
    }
}

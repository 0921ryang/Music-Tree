using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //_rigidbody.velocity = Vector3.left * 1000000;
        //transform.Translate(Vector3.left * (10000 * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision other)
    {
        _rigidbody.velocity = other.rigidbody.velocity;
    }

    private void OnCollisionStay(Collision other)
    {
        _rigidbody.velocity = other.rigidbody.velocity;
    }
}

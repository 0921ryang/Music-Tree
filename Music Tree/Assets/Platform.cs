using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool isEnter = false;
    public AudioClip audioClip;

    public AudioClip audioClip1
    {
        set => audioClip = value;
    }

    private AudioSource _audioSource;
    // Start is called before the first frame update
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = audioClip;
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (isEnter == false)
        {
            Debug.Log("music play");
            _audioSource.Play();
            isEnter = true;
        }
    }
}

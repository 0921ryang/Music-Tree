using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;
    private AudioSource _audioSource;
    [SerializeField] private RandomPlane _randomPlane;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _randomPlane.isTriggered.Subscribe(_ =>
        {
            _audioSource.Stop();
            PlayRandomSound();
        });
    }

    private void Update()
    {
        if (!_audioSource.isPlaying)
        {
            PlayRandomSound();
        } 
    }

    private void PlayRandomSound()
    {
        int soundID = Random.Range(0, audioClips.Count - 1);
        _audioSource.clip = audioClips[soundID];
        _audioSource.Play();
    }
}

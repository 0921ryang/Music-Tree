using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<AudioClip> music;
    public List<Button> buttons;
    public GameObject platform;
    
    void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var i1 = i;
            buttons[i].click1.Subscribe(click =>
            {
                if (click)
                {
                    platform.GetComponent<Platform>().audioClip1 = music[i1];
                    var plat = Instantiate(platform, new Vector3(3, 4, 5), Quaternion.identity);
                    Debug.Log("receive");
                }
            }).AddTo(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

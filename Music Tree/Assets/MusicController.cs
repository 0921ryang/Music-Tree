using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public List<AudioClip> music;
    public List<Button> buttons;
    public GameObject platform;// prefab
    public List<Platform> platforms = new List<Platform>();// intend to save all the plaforms
    
    void Start()
    {
        // subscribe buttons
        for (int i = 0; i < buttons.Count; i++)
        {
            var i1 = i;
            buttons[i].Click1.Subscribe(click =>
            {
                if (click)
                {
                    platform.GetComponent<Platform>().audioClip = music[i1];
                    var plat = Instantiate(platform, new Vector3(3, 4, 5), Quaternion.identity);
                    platforms.Add(plat.GetComponent<Platform>());
                    Debug.Log("receive");
                }
            }).AddTo(this);
        }
        
        // send signal to Platform to move
        platforms[1].transformObject = platforms[0].transform;
        platforms[1].isMove = true;
    }
    
    void Update()
    {
        
    }
}

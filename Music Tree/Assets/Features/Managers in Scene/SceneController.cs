using System.Collections;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetPos());
    }

    private IEnumerator SetPos()
    {
        yield return null;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CharacterController characterController = player.GetComponent<CharacterController>();
        characterController.transform.position = new Vector3(0, 2, 0);
    }
}

using UnityEngine;

public class ChooseUI : MonoBehaviour
{
    public PlayerControllerInput playerControllerInput;

    private void Update()
    {
        playerControllerInput.hasUI.Value = true;
    }
}

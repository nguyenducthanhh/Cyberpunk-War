using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidedPlayer : MonoBehaviour
{
    public GameObject playerObject;
    public float delayTime = 2f;

    void Start()
    {
        playerObject.SetActive(false);
        Invoke("ShowPlayer", delayTime);
       
    }

    void ShowPlayer()
    {
        if (playerObject != null)
        {
            playerObject.SetActive(true);
        }
    }
}
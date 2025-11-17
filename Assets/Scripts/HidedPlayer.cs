using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Kéo GameObject Player (đang bị tắt) vào ô này trong Inspector
    public GameObject playerObject;
    public float delayTime = 2f;

    void Start()
    {
        playerObject.SetActive(false);
        // Gọi hàm để bật Player sau 5 giây
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
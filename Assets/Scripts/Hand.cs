
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Hand : MonoBehaviour
{
   
    [SerializeField] private SpriteRenderer GunRotate;
    [SerializeField] private SpriteRenderer HandRenderer;
    [SerializeField] private SpriteRenderer PlayerRenderer;
    [SerializeField] private float rotateOffset = 180f;

    [SerializeField] private Vector3 rightLocalPosHand = new Vector3(-0.349f, 1.23786f, 0f);
    [SerializeField] private Vector3 leftLocalPosHand = new Vector3(0.343f, 1.184f, 0f);

    [Header("Fire sockets")]
    [SerializeField] private Transform firePoint;  
    [SerializeField] private Vector3 fireR = new Vector3(1.3954f, -0.0986f, 0f);     
    [SerializeField] private Vector3 fireL = new Vector3(1.399f, 0.101f, 0f);      

    void Awake()
    {
        if (!HandRenderer) HandRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        RotateHand();
    }

    void RotateHand()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width ||
            Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height) return;

        if (Camera.main == null) return;

        Vector3 mouseW = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)
        );

        Vector3 displacement = transform.position - mouseW;
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;

    
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);

     
        bool lookRight = (angle < -90f || angle > 90f);

        if (lookRight)
        {
         
            GunRotate.flipY = false;
            HandRenderer.flipY = false;
            PlayerRenderer.flipX = false;

            transform.localPosition = rightLocalPosHand; 
            firePoint.localPosition = fireR;
        }
        else
        {
          
            GunRotate.flipY = true;
            HandRenderer.flipY = true;
            PlayerRenderer.flipX = true;


            transform.localPosition = leftLocalPosHand; 
            firePoint.localPosition = fireL;
        }

    }
}
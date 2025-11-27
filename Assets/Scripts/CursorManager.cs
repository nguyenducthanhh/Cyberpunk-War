using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorNormal;
    [SerializeField] private Texture2D cursorMoving;

    [SerializeField] private Vector2 hotspotNormal = new Vector2(22, 48);

    [SerializeField] private Vector2 hotspotMoving = new Vector2(22, 48);

    [SerializeField] private PlayerController player;

    private Texture2D currentCursor;

    void Start()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerController>();
        }

        SetCursor(cursorNormal, hotspotNormal);
    }

    void Update()
    {
        if (player == null) return;

        if (player.IsMoving())
        {
            SetCursor(cursorMoving, hotspotMoving);
        }
        else
        {
            SetCursor(cursorNormal, hotspotNormal);
        }
    }
    private void SetCursor(Texture2D newCursor, Vector2 newHotspot)
    {
        if (currentCursor != newCursor)
        {
            currentCursor = newCursor;
            Cursor.SetCursor(currentCursor, newHotspot, CursorMode.Auto);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Textures")]
    [SerializeField] private Texture2D cursorNormal; // Cursor khi đứng im
    [SerializeField] private Texture2D cursorMoving; // Cursor khi di chuyển

    [Header("Hotspots")]
    [Tooltip("Điểm 'nóng' (đầu click) của cursor đứng im")]
    [SerializeField] private Vector2 hotspotNormal = new Vector2(22, 48);
    [Tooltip("Điểm 'nóng' (đầu click) của cursor di chuyển")]
    [SerializeField] private Vector2 hotspotMoving = new Vector2(22, 48);

    [Header("Tham chiếu")]
    [SerializeField] private PlayerController player;

    private Texture2D currentCursor; // Dùng để lưu cursor hiện tại

    void Start()
    {
        // Tự động tìm Player nếu bạn quên kéo vào
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerController>();
        }

        // Đặt cursor ban đầu là 'Normal'
        SetCursor(cursorNormal, hotspotNormal);
    }

    void Update()
    {
        if (player == null) return; // Nếu không có Player, không làm gì cả

        // Kiểm tra Player có đang di chuyển không
        if (player.IsMoving())
        {
            // Player đang di chuyển
            SetCursor(cursorMoving, hotspotMoving);
        }
        else
        {
            // Player đang đứng im
            SetCursor(cursorNormal, hotspotNormal);
        }
    }

    /**
     * Hàm này chỉ đổi cursor NẾU cursor mới khác cursor cũ
     * (Để tối ưu hóa, không gọi Cursor.SetCursor mỗi frame)
     */
    private void SetCursor(Texture2D newCursor, Vector2 newHotspot)
    {
        if (currentCursor != newCursor)
        {
            currentCursor = newCursor;
            Cursor.SetCursor(currentCursor, newHotspot, CursorMode.Auto);
        }
    }
}
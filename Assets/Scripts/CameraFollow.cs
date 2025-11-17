//using UnityEngine;

//public class CameraFollow : MonoBehaviour
//{
//    [Tooltip("Kéo GameObject của Player vào đây")]
//    public Transform target; // Mục tiêu (Player)

//    [Tooltip("Tốc độ camera 'bắt kịp' Player. Số nhỏ hơn sẽ mượt hơn.")]
//    public float smoothSpeed = 0.125f;

//    [Tooltip("Khoảng cách giữ camera với Player (thường không cần cho 2D)")]
//    public Vector3 offset;

//    // Luôn luôn dùng LateUpdate cho Camera
//    // Nó chạy sau khi Player đã hoàn thành di chuyển trong FixedUpdate/Update
//    void LateUpdate()
//    {
//        // Nếu không có mục tiêu, không làm gì cả
//        if (target == null)
//        {
//            return;
//        }

//        // Vị trí mong muốn của camera
//        Vector3 desiredPosition = target.position + offset;

//        // Vị trí đã được làm mượt
//        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

//        // Cập nhật vị trí camera
//        // Chúng ta giữ nguyên trục Z của camera
//        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
//    }
//}

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Kéo GameObject của Player vào đây")]
    public Transform target; // Mục tiêu (Player)

    [Tooltip("Tốc độ camera 'bắt kịp' Player. Số nhỏ hơn sẽ mượt hơn.")]
    public float smoothSpeed = 0.125f;

    [Tooltip("Khoảng cách giữ camera với Player")]
    public Vector3 offset;

    // --- BIẾN MỚI ĐỂ GIỚI HẠN MAP ---
    [Header("Map Boundaries (Giới hạn bản đồ)")]
    public float minX = 11f; // Tọa độ X bên trái cùng
    public float maxX = 69f;  // Tọa độ X bên phải cùng
    // --- KẾT THÚC BIẾN MỚI ---

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // 1. Tính toán vị trí mong muốn của camera (như cũ)
        Vector3 desiredPosition = target.position + offset;

        // 2. Tính toán vị trí đã được làm mượt (như cũ)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // --- PHẦN GIỚI HẠN CAMERA ---
        // 3. "Kẹp" (Clamp) giá trị X của camera
        //    Không cho nó đi xa hơn minX hoặc maxX
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);

        // Bạn cũng có thể "kẹp" trục Y nếu muốn
        // smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);
        // --- KẾT THÚC PHẦN GIỚI HẠN ---

        // 4. Cập nhật vị trí camera (dùng vị trí đã "kẹp")
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
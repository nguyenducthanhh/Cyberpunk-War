using System.Collections; // Cần thiết cho Coroutine (IEnumerator)
using UnityEngine;
using UnityEngine.SceneManagement; // Cần thiết cho SceneManager

public class LoadSceneAfterDelay : MonoBehaviour
{
    [Header("Cài đặt")]
    [Tooltip("Thời gian (giây) chờ trước khi chuyển scene")]
    public float delayTime = 5f; // Bạn có thể chỉnh số này trong Inspector

    [Tooltip("Tên chính xác của Scene bạn muốn chuyển đến")]
    public string sceneNameToLoad;

    // Hàm Start() được gọi 1 lần khi game bắt đầu
    void Start()
    {
        // Kiểm tra để đảm bảo bạn không quên gán tên scene
        if (string.IsNullOrEmpty(sceneNameToLoad))
        {
            Debug.LogError("Bạn quên gán 'Scene Name To Load' trong Inspector!");
            return;
        }

        // Bắt đầu đếm ngược
        StartCoroutine(LoadSceneCoroutine());
    }

    // Đây là Coroutine (hàm đếm giờ)
    IEnumerator LoadSceneCoroutine()
    {
        // 1. Tạm dừng (yield)
        // Chờ 'delayTime' giây trôi qua
        yield return new WaitForSeconds(delayTime);

        // 2. Chuyển scene
        // Sau khi chờ xong, chạy dòng code này
        SceneManager.LoadScene(sceneNameToLoad);
    }
}
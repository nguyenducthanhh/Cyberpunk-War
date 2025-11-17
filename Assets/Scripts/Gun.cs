////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;
////using TMPro;
////public class Gun : MonoBehaviour
////{
////    [SerializeField] private AudioManager audioManager;
////    [SerializeField] private Transform firePos;
////    [SerializeField] private GameObject bulletPrefabs;
////    [SerializeField] private float shotDelay = 0.15f;
////    private float nextShot;
////    [SerializeField] private int maxAmmo = 30;
////    [SerializeField] private TextMeshProUGUI ammoText;
////    [SerializeField] GameObject fireFlash;
////    public int currentAmmo;

////    void Start()
////    {
////        currentAmmo = maxAmmo;
////        UpdateAmmoText();
////    }

////    void Update()
////    {

////        Shoot();
////        Reload();
////    }

////    void Shoot()
////    {
////        if (Input.GetMouseButton(0) && currentAmmo > 0 && Time.time > nextShot)
////        {
////            nextShot = Time.time + shotDelay;
////            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
////            currentAmmo--;
////            UpdateAmmoText();
////            audioManager.PlayShootSound();
////            GameObject flash = Instantiate(fireFlash, firePos.position, firePos.rotation, firePos);
////            Destroy(flash, 0.1f);
////        }
////    }

////     void Reload()
////    {
////        if (Input.GetMouseButtonDown(1) && currentAmmo < maxAmmo)
////        {
////            currentAmmo = maxAmmo;
////            UpdateAmmoText();
////            audioManager.PlayReloadSound();
////        }
////    }

////    private void UpdateAmmoText()
////    {
////        if (ammoText != null)
////        {
////            if (currentAmmo > 0)
////            {
////                ammoText.text = currentAmmo.ToString();
////            }
////            else
////            {
////                ammoText.text = "0";
////            }

////        }

////    }
////}

//using System.Collections; // <-- THÊM DÒNG NÀY
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class Gun : MonoBehaviour
//{
//    [SerializeField] private AudioManager audioManager;
//    [SerializeField] private Transform firePos;
//    [SerializeField] private GameObject bulletPrefabs;
//    [SerializeField] private float shotDelay = 0.15f;
//    private float nextShot;
//    [SerializeField] private int maxAmmo = 30;
//    [SerializeField] private TextMeshProUGUI ammoText;
//    [SerializeField] GameObject fireFlash;
//    public int currentAmmo;

//    // --- CÁC BIẾN MỚI CHO VIỆC NẠP ĐẠN ---
//    [Header("Reload Settings")]
//    [SerializeField] private float reloadTime = 1f; // Thời gian nạp đạn (1 giây)
//    private bool isReloading = false; // Cờ theo dõi
//    // --- KẾT THÚC BIẾN MỚI ---

//    void Start()
//    {
//        currentAmmo = maxAmmo;
//        UpdateAmmoText();
//    }

//    void Update()
//    {
//        // Nếu đang nạp đạn, không cho làm gì khác
//        if (isReloading)
//        {
//            return;
//        }

//        Shoot();
//        CheckForReload(); // Đã đổi tên hàm Reload()
//    }

//    void Shoot()
//    {
//        // Không cho bắn khi đang nạp đạn (đã kiểm tra ở Update)
//        if (Input.GetMouseButton(0) && currentAmmo > 0 && Time.time > nextShot)
//        {
//            nextShot = Time.time + shotDelay;
//            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
//            currentAmmo--;
//            UpdateAmmoText();
//            audioManager.PlayShootSound();
//            GameObject flash = Instantiate(fireFlash, firePos.position, firePos.rotation, firePos);
//            Destroy(flash, 0.1f);
//        }
//    }

//    // --- PHẦN NẠP ĐẠN ĐÃ SỬA ---

//    // Hàm này chỉ kiểm tra input
//    private void CheckForReload()
//    {
//        // Điều kiện: Bấm chuột phải, đạn chưa đầy, VÀ không đang nạp đạn
//        if (Input.GetMouseButtonDown(1) && currentAmmo < maxAmmo && !isReloading)
//        {
//            // Bắt đầu Coroutine nạp đạn
//            StartCoroutine(ReloadCoroutine());
//        }
//    }

//    // Coroutine (hàm chờ) để xử lý việc nạp đạn
//    private IEnumerator ReloadCoroutine()
//    {
//        isReloading = true; // Đặt cờ: đang nạp đạn
//        Debug.Log("Bắt đầu nạp đạn...");

//        audioManager.PlayReloadSound();

//        // Chờ (yield) 'reloadTime' giây (1 giây)
//        yield return new WaitForSeconds(reloadTime);

//        // Sau 1 giây, thực hiện nạp
//        currentAmmo = maxAmmo;
//        UpdateAmmoText();

//        Debug.Log("Nạp đạn xong!");
//        isReloading = false; // Hạ cờ: nạp xong
//    }

//    // --- KẾT THÚC PHẦN SỬA ---

//    private void UpdateAmmoText()
//    {
//        if (ammoText != null)
//        {
//            if (currentAmmo > 0)
//            {
//                ammoText.text = currentAmmo.ToString();
//            }
//            else
//            {
//                ammoText.text = "0";
//            }
//        }
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // <-- THÊM DÒNG NÀY ĐỂ DÙNG "Image"

public class Gun : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shotDelay = 0.15f;
    private float nextShot;
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] GameObject fireFlash;
    public int currentAmmo;

    [Header("UI Elements")] // <-- Tách ra cho gọn gàng
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image loadingImage; // <-- THÊM BIẾN NÀY

    [Header("Reload Settings")]
    [SerializeField] private float reloadTime = 1f; // Thời gian nạp đạn (1 giây)
    private bool isReloading = false; // Cờ theo dõi

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();

        // --- THÊM MỚI: Ẩn ảnh loading khi bắt đầu game ---
        if (loadingImage != null)
        {
            loadingImage.gameObject.SetActive(false);
        }
        // --- KẾT THÚC THÊM MỚI ---
    }

    void Update()
    {
        if (isReloading)
        {
            return;
        }

        Shoot();
        CheckForReload();
    }

    void Shoot()
    {
        if (Input.GetMouseButton(0) && currentAmmo > 0 && Time.time > nextShot)
        {
            nextShot = Time.time + shotDelay;
            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            currentAmmo--;
            UpdateAmmoText();
            audioManager.PlayShootSound();
            GameObject flash = Instantiate(fireFlash, firePos.position, firePos.rotation, firePos);
            Destroy(flash, 0.1f);
        }
    }

    private void CheckForReload()
    {
        if (Input.GetMouseButtonDown(1) && currentAmmo < maxAmmo && !isReloading)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    // Coroutine (hàm chờ) để xử lý việc nạp đạn
    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        Debug.Log("Bắt đầu nạp đạn...");

        audioManager.PlayReloadSound();

        // --- THÊM MỚI: TẮT TEXT, BẬT ẢNH LOADING ---
        if (ammoText != null) ammoText.gameObject.SetActive(false);
        if (loadingImage != null) loadingImage.gameObject.SetActive(true);
        // --- KẾT THÚC THÊM MỚI ---

        // Chờ 'reloadTime' giây (1 giây)
        yield return new WaitForSeconds(reloadTime);

        // Sau 1 giây, thực hiện nạp
        currentAmmo = maxAmmo;
        UpdateAmmoText(); // Cập nhật số đạn (dù text vẫn đang ẩn)

        // --- THÊM MỚI: BẬT TEXT, TẮT ẢNH LOADING ---
        if (ammoText != null) ammoText.gameObject.SetActive(true);
        if (loadingImage != null) loadingImage.gameObject.SetActive(false);
        // --- KẾT THÚC THÊM MỚI ---

        Debug.Log("Nạp đạn xong!");
        isReloading = false;
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            if (currentAmmo > 0)
            {
                ammoText.text = currentAmmo.ToString();
            }
            else
            {
                ammoText.text = "0";
            }
        }
    }
}
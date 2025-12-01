using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image loadingImage;

    [SerializeField] private float reloadTime = 1f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();

        if (loadingImage != null)
        {
            loadingImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isReloading)
        {
            return;
        }

        Shoot();
        CheckForReload();
        OutOfBullet();
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
    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        audioManager.PlayReloadSound();
        if (ammoText != null) ammoText.gameObject.SetActive(false);
        if (loadingImage != null) loadingImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        UpdateAmmoText();

        if (ammoText != null) ammoText.gameObject.SetActive(true);
        if (loadingImage != null) loadingImage.gameObject.SetActive(false);

        isReloading = false;
    }

    void OutOfBullet()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo == 0 && !isReloading)
        {
            audioManager.PlayOutOfBulletSound();
        }
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
                audioManager.PlayOutOfBulletSound();
                ammoText.text = "0";
               
            }
        }
    }
}
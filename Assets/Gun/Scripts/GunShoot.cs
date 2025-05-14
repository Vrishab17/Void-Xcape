using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunShoot : MonoBehaviour
{
    [Header("Shooting")]
    public float damage = 25f;
    public float range = 100f;
    public LayerMask hitMask;

    [Header("Ammo")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 1.5f;
    public bool isReloading = false;

    [Header("UI")]
    public TextMeshProUGUI ammoText;

    [Header("References")]
    public Camera fpsCam;
    public Image muzzleFlashImage;
    public Sprite[] flashes;
    public GameObject impactEffect;

    [Header("Audio")]
    [SerializeField] private AudioClip FIREClip;
    [SerializeField] private AudioSource gunAudioSource;
    [SerializeField] private float fireInterval = 0.1f; // Time between shots

    private float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (InventoryInput.BlockNextInput || InventoryInput.InventoryOpen || isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireInterval;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        StartCoroutine(MuzzleFlash());

        // Fire sound
        if (gunAudioSource && FIREClip)
            gunAudioSource.PlayOneShot(FIREClip);

        currentAmmo--;
        UpdateAmmoUI();

        Ray ray = new Ray(fpsCam.transform.position, fpsCam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask))
        {
            Debug.Log("Hit: " + hit.transform.name);

            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 1f);
            }
        }
    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlashImage.sprite = flashes[Random.Range(0, flashes.Length)];
        muzzleFlashImage.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        muzzleFlashImage.sprite = null;
        muzzleFlashImage.color = new Color(0, 0, 0, 0);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + maxAmmo;
        }
    }
}

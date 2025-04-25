using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunShoot : MonoBehaviour
{
    [Header("Shooting")]
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 10f;
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
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

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

    System.Collections.IEnumerator Reload()
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

using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public GunData gunData;

    public PlayerController playerController;
    public Transform cameraTransform;

    public Transform gunMuzzle;
    public GameObject bulletHolePrefab;
    public GameObject bulletHitParticlePrefab;

    private float currentAmmo = 0f;
    private float nextTimeToFire = 0f;

    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = gunData.magazineSize;

        playerController = transform.root.GetComponent<PlayerController>();
        cameraTransform = playerController._playerCamera.transform;
    }

    public virtual void Update()
    {
        playerController.ResetRecoil(gunData);
    }

    public void TryReload()
    {
        if (!isReloading && currentAmmo < gunData.magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log(gunData.gunName + " is reloading....");

        yield return new WaitForSeconds(gunData.reloadTime);

        currentAmmo = gunData.magazineSize;
        isReloading = false;

        Debug.Log(gunData.gunName + " is reloaded");
    }

    public void TryShoot()
    {
        if (isReloading)
        {
            Debug.Log(gunData.gunName + " is reloading....");
            return;
        }
        
        if(currentAmmo <= 0f)
        {
            Debug.Log(gunData.gunName + " has no bullets left, Please reload.");
            return ;
        }

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1 / gunData.fireRate);
            HandleShoot();
        }
    }

    private void HandleShoot()
    {
        currentAmmo--;
        Debug.Log(gunData.gunName + " Shot!, Bullets left: " + currentAmmo);
        Shoot();

        playerController.ApplyRecoil(gunData);
    }

    public abstract void Shoot();
}

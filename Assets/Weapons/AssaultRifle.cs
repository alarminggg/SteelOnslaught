using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.InputSystem;
using UnityEditor.Rendering.LookDev;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class AssaultRifle : Gun
{

    public InputAction shootAction;
    public InputAction reloadAction;

    private void Awake()
    {
        var input = FindAnyObjectByType<PlayerLocomotionInput>();
        if (input != null)
        {
            shootAction = input.PlayerControls.PlayerLocomotionMap.Shoot;
            reloadAction = input.PlayerControls.PlayerLocomotionMap.Reload;
        }
    }

    private void OnEnable()
    {
        if (shootAction != null && reloadAction != null)
        {
            shootAction.Enable();
            reloadAction.Enable();

            shootAction.performed += OnShoot;
            reloadAction.performed += OnReload;
        }
        else
        {
            Debug.LogWarning("Shoot or Reload action not assigned!");
        }
    }

    private void OnDisable()
    {
        if (shootAction != null && reloadAction != null)
        {
            shootAction.performed -= OnShoot;
            reloadAction.performed -= OnReload;

            shootAction.Disable();
            reloadAction.Disable();
        }
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            TryShoot();
        }
    }
    private void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryReload();
        }
    }

    public override void Update()
    {
        base.Update();

        if(shootAction != null && shootAction.ReadValue<float>() > 0f)
        {
            TryShoot();
        }
    }
    public override void Shoot()
    {
        RaycastHit hit;

        Vector3 origin = cameraTransform.position;

        Vector3 target = Vector3.zero;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, gunData.shootingRange, gunData.targetLayerMask))
        {
            Debug.Log(gunData.gunName + " hit" + hit.collider.name);
            Debug.DrawLine(origin, hit.point, Color.red, 1f);
            target = hit.point;
        }
        else
        {
            target = cameraTransform.position + cameraTransform.forward * gunData.shootingRange;
        }

        StartCoroutine(BulletFire(target, hit));
    }

    private IEnumerator BulletFire(Vector3 target, RaycastHit hit)
    {
        GameObject bulletTrail = Instantiate(gunData.bulletTrailPrefab, gunMuzzle.position, Quaternion.identity);

        while (bulletTrail != null && Vector3.Distance(bulletTrail.transform.position, target) > 0.1f)
        {
            bulletTrail.transform.position = Vector3.MoveTowards(bulletTrail.transform.position, target, Time.deltaTime * gunData.bulletSpeed);
            yield return null;
        }

        Destroy(bulletTrail);

        if (hit.collider != null)
        {
            BulletHitFX(hit);
        }
    }

    private void BulletHitFX(RaycastHit hit)
    {
        Vector3 hitPosition = hit.point + hit.normal * 0.01f;

        GameObject bulletHole = Instantiate(bulletHolePrefab, hitPosition, Quaternion.LookRotation(hit.normal));
        GameObject hitParticle = Instantiate(bulletHitParticlePrefab, hit.point, Quaternion.LookRotation(hit.normal));

        bulletHole.transform.parent = hit.collider.transform;
        hitParticle.transform.parent = hit.collider.transform;

        Destroy(bulletHole, 5f);
        Destroy(hitParticle, 0.1f);

        if (hit.collider.CompareTag("Enemy"))
        {
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(gunData.damage);
            }
        }
    }
}

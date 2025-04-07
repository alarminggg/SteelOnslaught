using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Pistol : Gun
{
    public override void Update()
    {
        base.Update();

        if(Input.GetButtonDown("Fire1"))
        {
            TryShoot();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }
    }
    public override void Shoot()
    {
        RaycastHit hit;

        Vector3 origin = cameraTransform.position;

        Vector3 target = Vector3.zero;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, gunData.shootingRange, gunData.targetLayerMask))
        {
            Debug.Log(gunData.gunName + " hit" +hit.collider.name);
            Debug.DrawLine(origin, hit.point, Color.red, 1f);
            target = hit.point;
        }
        else
        {
            target = cameraTransform.position * gunData.shootingRange;
        }

        StartCoroutine(BulletFire(target, hit));
    }

    private IEnumerator BulletFire(Vector3 target, RaycastHit hit)
    {
        GameObject bulletTrail = Instantiate(gunData.bulletTrailPrefab, gunMuzzle.position, Quaternion.identity);

        while(bulletTrail != null && Vector3.Distance(bulletTrail.transform.position, target) > 0.1f)
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
    }
}

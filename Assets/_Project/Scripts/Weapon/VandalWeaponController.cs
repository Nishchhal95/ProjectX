using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VandalWeaponController : WeaponController
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform gunTip;

    public override HitInfo Attack(Transform shootPoint, LayerMask hittableMask)
    {
        HitInfo hitInfo = base.Attack(shootPoint, hittableMask);
        if (CanShoot())
        {
            BulletTracers(hitInfo);
        }
        return hitInfo;
    }

    private void BulletTracers(HitInfo hitInfo)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, hitInfo == null || hitInfo.HitPoint == null ? gunTip.position + (gunTip.forward * 20f) : hitInfo.HitPoint);

        StartCoroutine(BulletTracersRoutine());
    }

    private IEnumerator BulletTracersRoutine()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSecondsRealtime(0.02f);
        lineRenderer.enabled = false;
    }
}

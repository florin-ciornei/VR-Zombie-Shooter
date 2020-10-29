using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform anchor;
    public bool right;
    public GameObject flash;
    public float flashDuration = 0.15f, shootCooldownSeconds = 0.4f;
    public LineRenderer tracerLine;
    public float tracerLineMaxLength = 200;
    public Transform tracerLineStartPoint;

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        transform.position = anchor.position;
        transform.rotation = anchor.rotation;
        if (right && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0)
        {
            Shoot();
        }

        if (!right && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0)
        {
            Shoot();
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
#endif
    }

    private bool canShoot = true;

    private void Shoot()
    {
        if (!canShoot)
            return;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit,
            Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Target"))
            {
                Destroy(hit.collider.gameObject);
            }

            StartCoroutine(DrawTraceLine(hit.point));
        }
        else
        {
            StartCoroutine(DrawTraceLine(transform.forward * tracerLineMaxLength));
        }

        canShoot = false;
        flash.SetActive(true);
        Invoke(nameof(DisableFlash), flashDuration);
        Invoke(nameof(EnableCanShoot), shootCooldownSeconds);
    }

    void DisableFlash()
    {
        flash.SetActive(false);
    }

    void EnableCanShoot()
    {
        canShoot = true;
    }

    IEnumerator DrawTraceLine(Vector3 targetPos)
    {
        tracerLine.gameObject.SetActive(true);
        tracerLine.SetPosition(0, tracerLineStartPoint.position);
        tracerLine.SetPosition(1, targetPos);
        yield return new WaitForSeconds(flashDuration);
        tracerLine.gameObject.SetActive(false);
    }
}
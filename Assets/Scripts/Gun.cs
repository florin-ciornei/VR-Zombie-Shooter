using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform anchor;
    public bool right;
    public GameObject flash;
    public float flashDuration = 0.15f, shootCooldownSeconds = 0.4f;

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
    }

    private bool canShoot = true;

    void Shoot()
    {
        if (!canShoot)
            return;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit,
            Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Target"))
                Destroy(hit.collider.gameObject);
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float fireRate;
    public Camera camera;
    
    private float nextFire;

    [Header("VFX")] public GameObject hitVFX;
    
    void Update()
    {
        if (nextFire > 0) nextFire -= Time.deltaTime;
        
        if (Input.GetButton("Fire1") && nextFire <= 0)
        {
            nextFire = 1 / fireRate;
            
            Fire();
        }
    }

    void Fire()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            GameObject vfx =Instantiate(hitVFX, hit.point, Quaternion.identity);
            Destroy(vfx, 1f);
            
            if (hit.transform.gameObject.GetComponent<Health>())
            {
                hit.transform.gameObject.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }
}

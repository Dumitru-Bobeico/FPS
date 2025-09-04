using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float fireRate;
    public Camera camera;
    
    private float nextFire;

    [Header("VFX")] public GameObject hitVFX;

    [Header("Ammo")]
    public int mag = 5;

    public int ammo = 30;
    public int magAmmo = 30;
    
    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    [Header("Animation")]
    public Animation animation;
    public AnimationClip reload;

    [SerializeField] private Recoil Recoil_Script;

    void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }
    
    void Update()
    {
        if (nextFire > 0) nextFire -= Time.deltaTime;
        
        if (Input.GetMouseButton(0) && nextFire <= 0 && ammo > 0 && animation.isPlaying == false)
        {
            nextFire = 1 / fireRate;

            ammo--;
            
            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo;
            
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && mag > 0)
        {
            Reload();
        }
    }

    void Fire()
    {
        Recoil_Script.RecoilFire();
        
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

    void Reload()
    {
        animation.Play(reload.name);
        
        if (mag > 0)
        {
            mag--;
            
            ammo = magAmmo;
        }
        
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }
}

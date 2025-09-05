using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float fireRate;
    public Camera camera;

    private float nextFire;

    [Header("VFX")]
    public GameObject hitVFX;
    public GameObject muzzleFlash;
    public Transform muzzleFlashPosition;

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

    [Header("Bobbing Settings")]
    public bool enableBobbing = true;
    public float bobFrequency = 1f;            // Base frequency of bobbing
    public float bobHorizontalAmplitude = 0.05f;
    public float bobVerticalAmplitude = 0.05f;
    public float walkSpeed = 4f;               // Reference walk speed to scale bobbing
    public float horizontalMultiplier = 2f;    // How fast horizontal bob oscillates
    public float verticalMultiplier = 4f;      // How fast vertical bob oscillates

    private float bobTimer = 0f;
    private Vector3 initialLocalPos;

    void Start()
    {
        initialLocalPos = transform.localPosition;
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
        GameObject Flash = Instantiate(muzzleFlash, muzzleFlashPosition.position, muzzleFlashPosition.rotation, muzzleFlashPosition);
        Destroy(Flash, 0.1f);

        
        
        Recoil_Script.RecoilFire();

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 100f))
        {
            GameObject vfx = Instantiate(hitVFX, hit.point, Quaternion.identity);
            Destroy(vfx, 1f);

            if (hit.transform.gameObject.GetComponent<Health>())
                hit.transform.gameObject.GetComponent<Health>().TakeDamage(damage);
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

    public void WeaponBobbing(Vector2 moveInput, bool grounded, float playerSpeed)
    {
        if (!enableBobbing) return;

        Vector3 targetPos = initialLocalPos;

        if (moveInput.magnitude > 0.01f && grounded)
        {
            float speedScale = playerSpeed / walkSpeed;
            bobTimer += Time.deltaTime * bobFrequency * speedScale;

            float horizontalOffset = Mathf.Cos(bobTimer * horizontalMultiplier) * bobHorizontalAmplitude * speedScale;
            float verticalOffset = Mathf.Sin(bobTimer * verticalMultiplier) * bobVerticalAmplitude * speedScale;

            targetPos += transform.right * horizontalOffset;
            targetPos += transform.up * verticalOffset;
        }
        else
        {
            bobTimer = 0f;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 10f);
    }
}

using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int damage;
    public float fireRate;
    public Camera camera;

    [Header("Shotgun Settings")]
    public int pelletsCount = 1;
    public float sprayMulitplier = 0f;

    private float nextFire;
    private bool isReloading = false;

    [Header("VFX")]
    public GameObject hitVFX;
    public GameObject muzzleFlash;
    public Transform muzzleFlashPosition;

    [Header("Decals")]
    public GameObject bulletHolePrefab;  
    public float decalLifetime = 30f;    

    [Header("Ammo")]
    public int mag = 5;        // number of spare mags
    public int ammo = 30;      // current ammo in mag
    public int magAmmo = 30;   // max ammo per mag

    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    [Header("Animation")]
    public Animation animation;
    public AnimationClip reload;

    [SerializeField] private Recoil Recoil_Script;

    [Header("Bobbing Settings")]
    public bool enableBobbing = true;
    public float bobFrequency = 1f;
    public float bobHorizontalAmplitude = 0.05f;
    public float bobVerticalAmplitude = 0.05f;
    public float walkSpeed = 4f;
    public float horizontalMultiplier = 2f;
    public float verticalMultiplier = 4f;

    private float bobTimer = 0f;
    private Vector3 initialLocalPos;

    [Header("Recoil Settings")]
    [Range(0f, 7f)] public float recoilAmountX;
    [Range(0f, 7f)] public float recoilAmountY;
    [Range(0f, 10f)] public float maxRecoilTime;
    private float timePressed;

    private Vector2 recoilOffset;     
    private Vector2 recoilVelocity;   

    [Header("SFX")]
    public int shootSFXIndex = 0;
    public int reloadSFXIndex = 0;
    public PlayerSoundManager playerSoundManager;

    void Start()
    {
        initialLocalPos = transform.localPosition;
        UpdateUI();
    }

    void Update()
    {
        if (nextFire > 0) nextFire -= Time.deltaTime;

        // Shoot
        if (Input.GetMouseButton(0) && nextFire <= 0 && ammo > 0 && !isReloading)
        {
            nextFire = 1 / fireRate;
            ammo--;

            UpdateUI();
            Fire();
        }
        else
        {
            timePressed = 0;
        }

        // Reload
        if (Input.GetKeyDown(KeyCode.R) && mag > 0 && ammo < magAmmo && !isReloading)
        {
            Reload();
        }

        ApplyRecoil();
    }

    void Fire()
    {
        timePressed += Time.deltaTime;
        timePressed = Mathf.Min(timePressed, maxRecoilTime);

        playerSoundManager.PlayShootSFX(shootSFXIndex);

        // Muzzle flash
        if (muzzleFlash != null && muzzleFlashPosition != null)
        {
            GameObject flash = Instantiate(muzzleFlash, muzzleFlashPosition);
            flash.transform.localPosition = Vector3.zero;
            flash.transform.localRotation = Quaternion.identity;
            Destroy(flash, 0.1f);
        }

        Recoil_Script?.RecoilFire();

        // Pellets loop (shotgun spread)
        for (int i = 0; i < Mathf.Max(1, pelletsCount); i++)
        {
            Vector3 direction = camera.transform.forward;
            direction.x += Random.Range(-sprayMulitplier, sprayMulitplier);
            direction.y += Random.Range(-sprayMulitplier, sprayMulitplier);

            Ray ray = new Ray(camera.transform.position, direction);
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 100f))
            {
                if (hitVFX != null)
                {
                    GameObject vfx = Instantiate(hitVFX, hit.point, Quaternion.identity);
                    Destroy(vfx, 1f);
                }

                if (bulletHolePrefab != null)
                {
                    Vector3 decalPosition = hit.point + hit.normal * 0.01f;
                    Quaternion decalRotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(0f, 180f, 0f);
                    GameObject decal = Instantiate(bulletHolePrefab, decalPosition, decalRotation);
                    Destroy(decal, decalLifetime);
                }

                Health health = hit.transform.GetComponent<Health>();
                if (health != null)
                    health.TakeDamage(damage);
            }
        }

        AddRecoil();
    }

    void AddRecoil()
    {
        float x = (Random.value - 0.5f) * recoilAmountX;
        float y = ((Random.value - 0.5f) * 0.2f + 1f) * (timePressed >= maxRecoilTime ? recoilAmountY / 4 : recoilAmountY);
        recoilOffset += new Vector2(x, y);
    }

    void ApplyRecoil()
    {
        recoilOffset = Vector2.SmoothDamp(recoilOffset, Vector2.zero, ref recoilVelocity, 0.2f);
        camera.transform.localRotation = Quaternion.Euler(-recoilOffset.y, recoilOffset.x, 0f);
    }

    void Reload()
    {
        isReloading = true;
        animation.Play(reload.name);
        playerSoundManager.PlayReloadSFX(reloadSFXIndex);

        // Call FinishReload after animation ends
        Invoke(nameof(FinishReload), reload.length);
    }

    void FinishReload()
    {
        if (mag > 0)
        {
            mag--;
            ammo = magAmmo;
        }

        UpdateUI();
        isReloading = false;
    }

    void UpdateUI()
    {
        if (magText != null) magText.text = mag.ToString();
        if (ammoText != null) ammoText.text = ammo + "/" + magAmmo;
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

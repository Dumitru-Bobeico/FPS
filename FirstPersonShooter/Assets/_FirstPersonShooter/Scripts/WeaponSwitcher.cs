using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public Animation _animation;
    public AnimationClip drawWeapon;

    private int selectedWeapon = 0;

    void Start()
    {
        SelectWeapon();
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        // Switch weapons by number keys
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedWeapon = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedWeapon = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) selectedWeapon = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) selectedWeapon = 4;

        // Switch weapons by mouse scroll
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            selectedWeapon = (selectedWeapon >= transform.childCount - 1) ? 0 : selectedWeapon + 1;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            selectedWeapon = (selectedWeapon <= 0) ? transform.childCount - 1 : selectedWeapon - 1;
        }

        // If weapon changed, select it
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        if (selectedWeapon >= transform.childCount)
            selectedWeapon = transform.childCount - 1;

        _animation.Stop();
        _animation.Play(drawWeapon.name);

        int i = 0;
        foreach (Transform _weapon in transform)
        {
            _weapon.gameObject.SetActive(i == selectedWeapon);
            i++;
        }
    }
}
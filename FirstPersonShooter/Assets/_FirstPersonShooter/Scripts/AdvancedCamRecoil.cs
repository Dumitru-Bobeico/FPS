using UnityEngine;
using Random = UnityEngine.Random;

public class AdvancedCamRecoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    public float rotationSpeed = 6f;
    public float returnSpeed = 25f;

    [Header("Hipfire Recoil")]
    public Vector3 RecoilRotation = new Vector3(2f, 2f, 2f);

    [Header("Aiming Recoil")]
    public Vector3 RecoilRotationAiming = new Vector3(0.5f, 0.5f, 1.5f);

    private Vector3 currentRotation;
    private Vector3 rot;

    private void FixedUpdate()
    {
        // Smoothly recover back to neutral rotation
        currentRotation = Vector3.Lerp(
            currentRotation,
            Vector3.zero,
            returnSpeed * Time.deltaTime * Time.deltaTime);

        rot = Vector3.Slerp(
            rot,
            currentRotation,
            rotationSpeed * Time.fixedDeltaTime);

        transform.localRotation = Quaternion.Euler(rot);
    }

    /// <summary>
    /// Apply recoil kick to the camera
    /// </summary>
    public void ApplyRecoil(bool aiming)
    {
        if (aiming)
        {
            currentRotation += new Vector3(
                -RecoilRotationAiming.x,
                Random.Range(-RecoilRotationAiming.y, RecoilRotationAiming.y),
                Random.Range(-RecoilRotationAiming.z, RecoilRotationAiming.z));
        }
        else
        {
            currentRotation += new Vector3(
                -RecoilRotation.x,
                Random.Range(-RecoilRotation.y, RecoilRotation.y),
                Random.Range(-RecoilRotation.z, RecoilRotation.z));
        }
    }
}

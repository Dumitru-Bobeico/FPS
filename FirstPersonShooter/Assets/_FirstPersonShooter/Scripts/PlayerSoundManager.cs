using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public AudioSource footstepSource;
    public AudioClip footstepSFX;

    public AudioSource gunShootSource;
    public AudioClip[] allGunShootSFX;

    public void PlayFootstepSFX()
    {
        footstepSource.clip = footstepSFX;

        footstepSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
        footstepSource.volume = UnityEngine.Random.Range(0.2f, 0.35f);
        
        footstepSource.Play();
    }

    public void PlayShootSFX(int index)
    {
        gunShootSource.clip = allGunShootSFX[index];

        gunShootSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
        gunShootSource.volume = UnityEngine.Random.Range(0.2f, 0.35f);

        gunShootSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public AudioSource footstepSource;
    public AudioClip footstepSFX;

    public AudioSource gunShootSource;
    public AudioClip[] allGunShootSFX;
    
    public AudioSource gunReloadSource;
    public AudioClip[] allReloadSFX;

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
        gunShootSource.volume = UnityEngine.Random.Range(0.2f, 0.4f);

        gunShootSource.Play();
    }
    
    public void PlayReloadSFX(int index)
    {
        if (allReloadSFX.Length == 0) return;
        if (index < 0 || index >= allReloadSFX.Length) index = 0;

        gunReloadSource.clip = allReloadSFX[index];
        gunReloadSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
        gunReloadSource.volume = UnityEngine.Random.Range(0.4f, 0.6f);
        gunReloadSource.Play();
    }
}

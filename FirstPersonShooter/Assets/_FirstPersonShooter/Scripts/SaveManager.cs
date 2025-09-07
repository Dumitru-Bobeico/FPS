using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region Keys
    private const string MasterVolumeKey = "MasterVolume";
    private const string AmmoKey = "Ammo";
    private const string MagKey = "Mag";
    private const string MedkitsKey = "Medkits";
    #endregion

    #region Volume
    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat(MasterVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
    }
    #endregion

    #region Ammo
    public void SetAmmo(int amount)
    {
        PlayerPrefs.SetInt(AmmoKey, amount);
        PlayerPrefs.Save();
    }

    public int GetAmmo()
    {
        return PlayerPrefs.GetInt(AmmoKey, 0);
    }
    #endregion

    #region Mags
    public void SetMags(int amount)
    {
        PlayerPrefs.SetInt(MagKey, amount);
        PlayerPrefs.Save();
    }

    public int GetMags()
    {
        return PlayerPrefs.GetInt(MagKey, 0);
    }
    #endregion

    #region Medkits
    public void SetMedkits(int amount)
    {
        PlayerPrefs.SetInt(MedkitsKey, amount);
        PlayerPrefs.Save();
    }

    public int GetMedkits()
    {
        return PlayerPrefs.GetInt(MedkitsKey, 0);
    }
    #endregion

    #region Reset
    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
    #endregion
}
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsManager : MonoBehaviour
{
    public static WindowsManager Layout;
    [SerializeField] private GameObject[] windows;
    [SerializeField] private string initialWindowName;

    private void Awake()
    {
        Layout = this;

        foreach (GameObject window in windows)
        {
            window.SetActive(false);
        }
    }

   

    public void OpenLayout(string windowName)
    {
        CloseAll();
        foreach (GameObject window in windows)
        {
            if (window.name == windowName)
            {
                window.SetActive(true);
                break;
            }
        }
    }

    
    public void CloseAll()
    {
        foreach (GameObject window in windows)
        {
            window.SetActive(false);
        }
    }
    

}

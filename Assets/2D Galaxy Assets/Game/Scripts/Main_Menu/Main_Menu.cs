using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public void LoadSinglePlayerGame()
    {
        LoadScene("Single_Player");
    }
    public void LoadCoOpGame()
    {
        LoadScene("Co-Op_Mode");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}


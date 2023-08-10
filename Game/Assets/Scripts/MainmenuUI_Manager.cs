using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuUI_Manager : MonoBehaviour
{
    public void Btn_Start()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Btn_Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}

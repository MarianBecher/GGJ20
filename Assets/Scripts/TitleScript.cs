using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    [SerializeField] private GameObject confirmationDialog;

    private bool quitting = false;
    void Update()
    {
        if (Input.GetButtonUp("Menu_Cancel"))
        {
            quitting = !quitting;
        }
        if (Input.GetButtonDown("Menu_Submit"))
        {
            if (quitting)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene("MainGame");
            }
        }
        confirmationDialog.SetActive(quitting);
    }
}

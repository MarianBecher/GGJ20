using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreScreenFunctionality : MonoBehaviour
{
    [SerializeField] private float graceTimer = 3;

    private bool inputAllowed = false;

    private void OnEnable()
    {
        StartCoroutine(GraceTimeCoroutine());
    }

    void Update()
    {
        if (inputAllowed && Input.anyKeyDown)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Title");
        }
    }

    private IEnumerator GraceTimeCoroutine()
    {
        yield return new WaitForSecondsRealtime(graceTimer);
        inputAllowed = true;
    }
}

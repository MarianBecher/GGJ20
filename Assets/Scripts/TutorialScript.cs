using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private float graceTimer = 3;

    private bool inputAllowed = false;

    private void Awake()
    {
        Time.timeScale = 0;
        StartCoroutine(GraceTimeCoroutine());
    }

    void Update()
    {
        if (inputAllowed && Input.anyKeyDown)
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }

    private IEnumerator GraceTimeCoroutine()
    {
        yield return new WaitForSecondsRealtime(graceTimer);
        inputAllowed = true;
    }
}

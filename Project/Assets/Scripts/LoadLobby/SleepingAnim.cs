using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SleepingAnim : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(Sleep());
    }

    private IEnumerator Sleep()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

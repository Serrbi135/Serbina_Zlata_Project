using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefabDie : MonoBehaviour
{
    void Start()
    {
        Die();
    }
    private IEnumerator Die()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}

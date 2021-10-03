using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyByTime : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float time;

    private IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    public void StartDestroy()
    {
        StartCoroutine(WaitDestroy());
    }
}

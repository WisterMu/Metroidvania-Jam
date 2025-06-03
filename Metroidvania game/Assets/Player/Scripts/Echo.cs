using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echo : MonoBehaviour
{
    private Transform pulseTransform;
    private float range;
    private float rangeMax;

    private void Awake()
    {
        pulseTransform = transform.Find("Circle");
        rangeMax = 60f;
    }
    
    private void Update()
    {
        float rangeSpeed = 20f;
        range += rangeSpeed * Time.deltaTime;
        if(range > rangeMax)
        {
            range = 0f;
        }
        pulseTransform.localScale = new Vector3(range, range);
    }
}

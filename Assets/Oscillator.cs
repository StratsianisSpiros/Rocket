using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 trap = new Vector3(10f, 10f, 10f);
    [Range(0,1)][SerializeField] float moveTrap;
    [SerializeField] float period = 10f;

    Vector3 startTrap;
  
    // Start is called before the first frame update
    void Start()
    {
        startTrap = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        round();
    }

    private void round()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycle = Time.time / period;
        float wave = Mathf.Sin(cycle * Mathf.PI * 2f);
        Vector3 offset = moveTrap * trap * wave;
        transform.position = startTrap + offset;
    }
}

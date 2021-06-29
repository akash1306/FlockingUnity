using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockParamLoader : MonoBehaviour
{

    public float maxSpeed=1;

    public int simDuration = 2000;
    public float desiredDistance = 20f;
    public float strengthPotential = 6f;
    public float steepnessPotential = 2f;
    public float range_multiplier = 3f;
    public float k1 = 0.5f;
    public float k2 = 0.2f;
    public float k3 = 0.1f;
    public float moveForward = 0.3f;
    public float interpolateCoeff = 0.95f;
    public float noise;
    public float maxRange;
    // Start is called before the first frame update
    void Start()
    {
        noise = desiredDistance / Mathf.Pow(2, 1 / steepnessPotential);
        maxRange = range_multiplier * desiredDistance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

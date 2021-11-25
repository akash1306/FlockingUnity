using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetricMeasure : MonoBehaviour
{

    public float headingSumCos;
    public float headingSumSin;

    [UPyPlot.UPyPlotController.UPyProbe]
    public float timeData;

    [UPyPlot.UPyPlotController.UPyProbe]
    public float psiMetric;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        headingSumCos = 0;
        headingSumSin = 0;
        psiMetric = 0;
        Flocking[] scripts = Object.FindObjectsOfType(typeof(Flocking)) as Flocking[];
        

        foreach (Flocking scriptName in scripts)
        {
            headingSumCos += Mathf.Cos(scriptName.headingToSend*Mathf.Deg2Rad);
            headingSumSin += Mathf.Sin(scriptName.headingToSend*Mathf.Deg2Rad);
        }
        psiMetric = Mathf.Sqrt(Mathf.Pow(headingSumCos , 2) + Mathf.Pow(headingSumSin , 2))/ scripts.Length;
        timeData = Time.time;
        Debug.Log(psiMetric);
    }


    // public string ToCSV()
    // {
    //     var sb= new StringBuilder(Time,Value);
    //     foreach(var frame in keyFrame)
    //     {

    //     }
    // }
}

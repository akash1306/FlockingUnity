using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour
{


    public GameObject FlockingController;

    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
       // MoveFunction(targetPosi, 3.14f);
        List<Transform> context = GetNearbyObjects();
        CalculateMove(context);
        
    }

    public void MoveFunction(Vector3 referencePosi, float heading, float speed) //Input Heading as radians
    {
        transform.position = Vector3.MoveTowards(transform.position, referencePosi, Time.deltaTime * Mathf.Min(FlockingController.GetComponent<FlockParamLoader>().maxSpeed,speed) );
        Quaternion q = Quaternion.Euler(0, heading, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, q* transform.rotation, FlockingController.GetComponent<FlockParamLoader>().interpolateCoeff);
        //Transform.forward*-1 is the heading direction
    }

    public double getProximalMagnitude(double range)
    {
        return (-4 * FlockingController.GetComponent<FlockParamLoader>().steepnessPotential* FlockingController.GetComponent<FlockParamLoader>().strengthPotential / range) *
            (2 * Mathf.Pow(FlockingController.GetComponent<FlockParamLoader>().noise / (float)range, 2 * FlockingController.GetComponent<FlockParamLoader>().steepnessPotential)
            - Mathf.Pow(FlockingController.GetComponent<FlockParamLoader>().noise / (float)range, FlockingController.GetComponent<FlockParamLoader>().steepnessPotential));
    }

    List<Transform> GetNearbyObjects()
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextCollider = Physics.OverlapSphere(transform.position, FlockingController.GetComponent<FlockParamLoader>().maxRange);
        foreach (Collider c in contextCollider)
        {
            if (c != GetComponent<Collider>())
            {
                context.Add(c.transform);
            }
        }
        
        return context;
    }

    public void CalculateMove(List<Transform> context)
    {
        double prox_vector_x = 0.0;
        double prox_vector_y = 0.0;
        double prox_vector_z = 0.0;
        double prox_magnitude;
        Vector3 moveCommand;
        
        if (context.Count > 0)
        {
            foreach (Transform item in context)
            {


                prox_magnitude = getProximalMagnitude(Vector3.Distance(item.position, transform.position));
                Debug.Log("Distance is "+Vector3.Distance(item.position, transform.position));
               // Debug.Log("ProxMag is"+prox_magnitude);
                Vector3 itemVector = item.position - transform.position;
                float inclination = Vector3.Angle(transform.up, itemVector); //Degrees
                float bearing = Vector3.SignedAngle(transform.forward , new Vector3(itemVector.x, 0, itemVector.z), Vector3.up); //Degrees
                // Debug.Log("Bearing is "+bearing);
                prox_vector_x += prox_magnitude * Mathf.Cos(bearing*Mathf.Deg2Rad) * Mathf.Sin(inclination*Mathf.Deg2Rad);// Converted to radians for the Cosine function
                prox_vector_y += prox_magnitude * Mathf.Sin(bearing*Mathf.Deg2Rad) * Mathf.Sin(inclination*Mathf.Deg2Rad);
                prox_vector_z += prox_magnitude * Mathf.Cos(inclination*Mathf.Deg2Rad);
                
            }
        }
        double flock_vector_x = prox_vector_x;
        double flock_vector_y = prox_vector_y;
        double flock_vector_z = prox_vector_z;

        double u = flock_vector_x * FlockingController.GetComponent<FlockParamLoader>().k1 + FlockingController.GetComponent<FlockParamLoader>().moveForward;
        double v = flock_vector_y * FlockingController.GetComponent<FlockParamLoader>().k2;
        double w = flock_vector_z * FlockingController.GetComponent<FlockParamLoader>().k3;
        Debug.Log(flock_vector_z );

        float headingtemp = UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).y;

        moveCommand.z = transform.position.z + (float)u * Mathf.Cos(headingtemp * Mathf.Deg2Rad);// Converted to radians for the Cosine function
        moveCommand.x = transform.position.x + (float)u * Mathf.Sin(headingtemp * Mathf.Deg2Rad);
        moveCommand.y = transform.position.y+ (float)w;

        //headingtemp += (float)v*Mathf.Rad2Deg;
        // Debug.Log(moveCommand);

        MoveFunction(moveCommand, ((float)v * Mathf.Rad2Deg)*0.05f, (float)u);

    }
}

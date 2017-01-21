using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    public float distance;
    public int zoomSpeed;

    Vector3 inspectPosition;
    Vector3 originalPosition;
    Quaternion originalRotation;
    Behaviour halo;
    float step;
    bool forward = false;

    void Start () {
        //Store initial postions
        inspectPosition = Camera.main.transform.position + Camera.main.transform.forward * distance;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (forward)
        {
            transform.Rotate(0, -20 * Time.deltaTime, 0);
        }
        inspect();
    }

    void OnMouseDown()
    {
        //When clicked, if object is not in front of camera, move it towards it
        if (transform.position == inspectPosition)
        {
            forward = false;
        }
        else
        {
            forward = true;
        }
    }

    void inspect()
    {
        step = zoomSpeed * Time.deltaTime;

        //Start moving object towards camera if user clicked to inspect it
        if (forward)
        {
            transform.position = Vector3.MoveTowards(transform.position, inspectPosition, step);
        }
        else
        {
            //Go back to orginal position next time object is clicked
            forward = false;
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, step);
            transform.rotation = originalRotation;
        }
    }
}

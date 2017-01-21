using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCamera : MonoBehaviour {

    public float panSpeed = 30f;
    Vector3 originalPosition;
    
    void Start()
    {
        originalPosition = transform.position;
    }

	void Update () {
        //Top
        if (Input.mousePosition.y >= Screen.height - 10f)
        {
            // Minimum x rotation is 350 (like -10). Other value is due to 'overflow' when you go past zero.
            if (transform.rotation.eulerAngles.x < 340 || transform.rotation.eulerAngles.x > 350)
            {
                gameObject.transform.Rotate(-20 * Time.deltaTime, 0, 0);
            }
        }
        //Bottom
        if (Input.mousePosition.y <= 10f)
        {
            // Max x rotation is 10. Other value is due to 'overflow' when you go past zero.
            if (transform.rotation.eulerAngles.x < 10 || transform.rotation.eulerAngles.x > 20)
            {
                gameObject.transform.Rotate(20 * Time.deltaTime, 0, 0);
            }
        }
        //Right
        if (Input.mousePosition.x >= Screen.width - 10f)
        {
            // Max y rotation is 10. Other value is due to 'overflow' when you go past zero.
            if (transform.rotation.eulerAngles.y < 10 || transform.rotation.eulerAngles.y > 20)
            {
                gameObject.transform.Rotate(0, 20 * Time.deltaTime, 0);
            }
        }
        //Left
        if (Input.mousePosition.x <= 10f)
        {
            // Minimum y rotation is 350 (like -10). Other value is due to 'overflow' when you go past zero.
            if (transform.rotation.eulerAngles.y < 340 || transform.rotation.eulerAngles.y > 350)
            {
                gameObject.transform.Rotate(0, 20 * Time.deltaTime, 0);
            }
        } 
    }
}

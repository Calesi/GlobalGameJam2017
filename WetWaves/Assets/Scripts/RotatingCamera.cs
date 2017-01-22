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
        if (Input.mousePosition.y >= Screen.height - 20f)
        {
            // Minimum x rotation is 340 (like -20). Other value is due to 'overflow' when you go past zero.
            if (transform.rotation.eulerAngles.x < 340 || transform.rotation.eulerAngles.x > 350)
            {
                gameObject.transform.Rotate(-20 * Time.deltaTime, 0, 0);
            }
        }
        //Bottom
        if (Input.mousePosition.y <= 20f)
        {
            // Max x rotation is 40. Other value is due to 'overflow' when you go past zero.
            if (transform.rotation.eulerAngles.x < 40 || transform.rotation.eulerAngles.x > 50)
            {
                gameObject.transform.Rotate(20 * Time.deltaTime, 0, 0);
            }
        }
        //Right
        if (Input.mousePosition.x >= Screen.width - 20f)
        {
            // Max y rotation is 20. Other value is due to 'overflow' when you go past zero.
            if (transform.rotation.eulerAngles.y < 5 || transform.rotation.eulerAngles.y > 70)
            {
                gameObject.transform.Rotate(0, 20 * Time.deltaTime, 0);
            }
        }
        //Left
        if (Input.mousePosition.x <= 20f)
        {
            // Minimum y rotation is 340 (like -20). Other value is due to 'overflow' when you go past zero.
            if (transform.rotation.eulerAngles.y < 335 || transform.rotation.eulerAngles.y > 348)
            {
                gameObject.transform.Rotate(0, -20 * Time.deltaTime, 0);
            }
        }
        Quaternion rot = Quaternion.Euler(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);
        gameObject.transform.rotation = rot;
    }
}

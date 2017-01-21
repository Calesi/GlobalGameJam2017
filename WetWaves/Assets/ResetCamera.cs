using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResetCamera : MonoBehaviour, IPointerClickHandler
{
    public GameObject camera;
    Vector3 originalPosition;

    public void OnPointerClick(PointerEventData eventData)
    {
        camera.transform.rotation = new Quaternion(0.2f, 0, 0, 1.0f);
    }
}

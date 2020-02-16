using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    private Transform xFormCamera;
    private Transform xFormParent;

    private Vector3 localRotation;
    private float cameraDist = 10f;

    public float mouseSensitivity = 4f;
    public float scrollSensitivity = 2f;
    public float orbitDampening = 10f;
    public float scrollDampening = 6f;
    public Vector2 scrollMinMax = new Vector2(-5, 30);

    public float panSpeed = 4.0f;

    private Vector3 mouseOrigin = Vector3.zero;
    public Vector3 mapSize = Vector3.one;

    private CamPan camPan;

    private void Start()
    {
        camPan = GetComponentInParent<CamPan>();

        this.xFormCamera = this.transform;
        this.xFormParent = this.transform.parent;
    }

    private void LateUpdate()
    {
        if (xFormCamera && xFormParent)
        {

            //middle click to move
            if (Input.GetMouseButtonDown(2))
            {
                mouseOrigin = Input.mousePosition;
            }

            if (Input.GetMouseButton(2))
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(mouseOrigin - Input.mousePosition);
                Vector3 move = new Vector3(pos.x, 0, pos.y);

                camPan.pan(move * panSpeed, mapSize);
            }

            //camera movement keys
            float xAxisVal = Input.GetAxis("Horizontal");
            float zAxisVal = Input.GetAxis("Vertical");
            camPan.pan(new Vector3(xAxisVal, 0, zAxisVal) * panSpeed, mapSize);

            //right click to rotate
            if (Input.GetMouseButton(1))
            {
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    localRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;
                    localRotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity;

                    //Clamp y between horizon and flipping
                    localRotation.y = Mathf.Clamp(localRotation.y, 0f, 90f);
                }
            }

            //scroll to zooms
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

                //scaled scroll speed based on distance
                scrollAmount *= 5;

                this.cameraDist += scrollAmount * -1f;

                //between 1m and 100m to prevent infinite scrolling
                this.cameraDist = Mathf.Clamp(this.cameraDist, scrollMinMax.x, scrollMinMax.y);
            }


            Quaternion qt = Quaternion.Euler(localRotation.y, localRotation.x, 0);
            this.xFormParent.rotation = Quaternion.Lerp(this.xFormParent.rotation, qt, Time.deltaTime * orbitDampening);

            if (this.xFormCamera.localPosition.z != this.cameraDist * -1f)
            {
                this.xFormCamera.localPosition = new Vector3(0, 0, Mathf.Lerp(this.xFormCamera.localPosition.z, this.cameraDist * -1f, Time.deltaTime * scrollDampening));
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPan : MonoBehaviour
{
    public float cameraPanBuffer = 5;

    public void pan(Vector3 move, Vector3 mapSize)
    {
        this.transform.Translate(move, Space.Self);

        //clamp borders of movement
        this.transform.localPosition = new Vector3(
            Mathf.Clamp(this.transform.localPosition.x, -mapSize.x / 2f - cameraPanBuffer, mapSize.x / 2f + cameraPanBuffer),
            7.5f,
            Mathf.Clamp(this.transform.localPosition.z, -mapSize.z / 2f - cameraPanBuffer, mapSize.z / 2f + cameraPanBuffer)
        );
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamClick : MonoBehaviour
{
    public LayerMask terrainMask;

    public GameObject testPrefab;
    //[SerializeField]
    private GameObject active;
    //[SerializeField]
    private GameObject ghostObject;
    public Material ghostMat;
    public GameObject parentObject;

    private void Start()
    {
        active = null;
    }

    private void Update()
    {
        //press 1 to set active, 0 to reset TEMP
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            active = testPrefab;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            active = null;
            DestroyGhost();
        }

        //spawn and move ghost objects
        if (active != null)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                GhostObject(active);
            }
        }

        //left click to spawn objects
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                InteractAtPoint(active);
            }
        }
    }

    private void GhostObject(GameObject active)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 offset = new Vector3(0, 0.5f, 0);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainMask))
        {
            if (ghostObject == null)
            {
                ghostObject = Instantiate(active);
                ghostObject.GetComponent<MeshRenderer>().material = ghostMat;
                ghostObject.transform.parent = parentObject.transform;
                ghostObject.name = "Ghost Object";
            }

            ghostObject.transform.position = hit.point + offset;
        }else{
            DestroyGhost();
        }
    }

    private void InteractAtPoint(GameObject active)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 offset = new Vector3(0, 0.5f, 0);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainMask))
        {
            GameObject go = Instantiate(active);

            go.transform.position = hit.point + offset;
            go.transform.parent = parentObject.transform;
        }
    }

    private void DestroyGhost()
    {
        if (ghostObject != null)
        {
            Destroy(ghostObject);
            ghostObject = null;
        }
    }
}

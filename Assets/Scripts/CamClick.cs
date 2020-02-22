using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class CamClick : MonoBehaviour
{
    public LayerMask terrainMask;

    public GameObject testPrefab;

    private GameObject ghostObject;
    private MeshRenderer ghostRenderer;
    public Material validMat;
    public Material invalidMat;
    public GameObject parentObject;

    private Spawnable active;
    private Spawnable spawnable;

    private void Start()
    {
        active = null;
        spawnable = new Spawnable(testPrefab, 0.5f, 1.2f, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), true, new Vector3(0, 3, 0));
    }

    private void Update()
    {
        //press 1 to set active, 0 to reset TEMP
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            active = spawnable;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            active = null;
            DestroyGhost();
        }

        if (active != null)
        {
            //spawn and move ghost objects
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                GhostObject();
            }

            //left click to spawn objects
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    InteractAtPoint();
                }
            }
        }
    }

    private void GhostObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainMask))
        {
            if (ghostObject == null)
            {
                ghostObject = Instantiate(active.getPrefab());
                ghostObject.transform.parent = parentObject.transform;
                ghostObject.name = "Ghost Object";
                ghostRenderer = ghostObject.GetComponentInChildren<MeshRenderer>();
            }
            else
            {
                if (ValidLocation(hit))
                {
                    ghostRenderer.material = validMat;
                }
                else
                {
                    ghostRenderer.material = invalidMat;
                }
            }

            ghostObject.transform.position = hit.point + active.getOffset();
        }
        else
        {
            DestroyGhost();
        }
    }

    private bool ValidLocation(RaycastHit hit)
    {
        if ((hit.normal == hit.transform.up && active.getTopValidSpawn()) || (hit.normal != hit.transform.up && !active.getTopValidSpawn()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void InteractAtPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainMask))
        {
            if (ValidLocation(hit))
            {
                GameObject go = Instantiate(active.getPrefab(), hit.point + active.getOffset(), spawnable.getRotation());
                go.transform.localScale = Vector3.one * spawnable.getSize();

                go.transform.parent = parentObject.transform;
            }
        }
    }

    private void DestroyGhost()
    {
        if (ghostObject != null)
        {
            ghostRenderer = null;
            Destroy(ghostObject);
            ghostObject = null;
        }
    }
}

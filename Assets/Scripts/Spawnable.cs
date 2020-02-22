using UnityEngine;
using UnityEngine.EventSystems;

public class Spawnable
{
    private GameObject prefab;
    private float minSize;
    private float maxSize;
    private Quaternion rotation;
    private bool topValidSpawn;

    public Spawnable(GameObject prefab, float minSize, float maxSize, Quaternion rotation, bool topValidSpawn){
        this.prefab = prefab;
        this.minSize = minSize;
        this.maxSize = maxSize;
        this.rotation = rotation;
        this.topValidSpawn = topValidSpawn;
    }

    public GameObject getPrefab(){
        return prefab;
    }

    public float getSize(){
        return Random.Range(minSize, maxSize);
    }

    public Quaternion getRotation(){
        return rotation;
    }

    public bool getTopValidSpawn(){
        return topValidSpawn;
    }
}
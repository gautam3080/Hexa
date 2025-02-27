using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class LevelEdit : MonoBehaviour
{ 


    public Action<LevelEdit> onclick;
    public levelEditMesh lm;


    void Start()
    {
        lm.onClick += () => { onclick?.Invoke(this); };
        originalRotation = transform.rotation;
        targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
        indesGridSphere1 = new Vector3(1, 2);
        indesGridSphere2 = new Vector3(1, 0);
    }


    private Quaternion originalRotation;
    private Quaternion targetRotation;
    public  bool isRotated = false;
    public  bool isRotating = false;
    public float rotationSpeed = 2.0f;
    public Transform sphere1;
    public Transform sphere2;
    public Vector2 indesGridSphere1;
    public Vector2 indesGridSphere2;
    public TileSpawnInfo hex1;
    public TileSpawnInfo hex2;
    public void SwapSphereIndex()
    {
        Vector2 temp = new Vector2(indesGridSphere1.x, indesGridSphere1.y);
        indesGridSphere1 = indesGridSphere2;
        indesGridSphere2 = temp;

        /*TileSpawnInfo tempHex = new TileSpawnInfo();
        tempHex = hex1;
        hex1 = hex2;
        hex2 = tempHex;*/

    }
   public System.Collections.IEnumerator Rotate()
    {
        if (hex1 != null && hex2 != null)
        {


            isRotating = true;
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = isRotated ? originalRotation : targetRotation;

            float elapsedTime = 0;

            while (elapsedTime < 1f / rotationSpeed)
            {
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime * rotationSpeed);
                elapsedTime += Time.deltaTime;
                var p1 = sphere1.GetComponent<MeshRenderer>().bounds.center;
                var p2 = sphere2.GetComponent<MeshRenderer>().bounds.center;
                hex1.transform.position = new Vector3(p1.x, hex1.transform.position.y, p1.z);
                hex2.transform.position = new Vector3(p2.x, hex2.transform.position.y, p2.z);
                yield return null;
            }
            SwapSphereIndex();
            transform.rotation = endRotation;
            isRotated = !isRotated;
            isRotating = false;
        }
        else
        {

         yield return null;
        }
    }
}


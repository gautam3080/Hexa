using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hexa : MonoBehaviour
{
    public TileSpawnInfo tileSpawnInfo;
    public System.Action onClick;
    public Color MyColor
    {
        set
        {

        }
    }
    private void OnMouseDown()
    {
       
           onClick?.Invoke();
    }
}

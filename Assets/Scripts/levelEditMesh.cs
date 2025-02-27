using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelEditMesh : MonoBehaviour
{
    public System.Action onClick;
    private void OnMouseDown()
    {

        onClick?.Invoke();
    }
}

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovableHex : MonoBehaviour
{
    public int ColumnIndex;
    public int ChildIndex;
    public ArrowDirection Direction;
    public Action<TileSpawnInfo> onHexClicked;
    public Hexa _hexa;
    public Color _green;
    public Color _yellow;
    public Color _blue;
    public Color _red;
    public Color _pink;
    public Color _black;
    public bool isMoving = false;
    public List<MeshRenderer> hexMeshes = new List<MeshRenderer>();
    public int columnIndex => ColumnIndex;
    public int childIndex => ChildIndex;
    public ArrowDirection direction => Direction;
    Grid grid;


    Vector3 p;
    float time;
    float z;
    float r;
    float h;

    float distanceZ;
    Vector3 initialPos;
    float rotFinal;
    bool moveSuccess;
    Vector3 destinationPos;
    float MoveForwardSpeed;
    float moveForwardInterp = 0;
    float delay = 0;
    float speed = 3.5f;
    public void MoveSuccess(Vector3 pos, Action<MovableHex> onComplete = null, int iterations = 1, int childIndex = 0)
    {

        isMoving = true;
        destinationPos = pos;
        time = iterations / speed;
        rotFinal = 180 * iterations;
        p = transform.position;
        z = transform.position.z;
        r = 0;
        h = transform.position.y;

        delay = childIndex /speed ;

        StartCoroutine(CallDelay(pos, onComplete));
       


        //MoveSuccesss(pos, onComplete, iterations = 1);
        //MoveSuccess(pos, onComplete, iterations = 1);
    }
    float fallY;
    void MakeItFall(float delay, Action<MovableHex> onComp)
    {
        Vector3 tempP = transform.position;
        transform.DOMove(transform.position + (Vector3.down), delay).SetEase(Ease.Linear).onComplete = () =>
        {
            onComp?.Invoke(this);
            Destroy(gameObject);
        };
       
        
    }

    IEnumerator CallDelay(Vector3 pos, Action<MovableHex> onComplete)
    {
        yield return new WaitForSeconds(delay);
        float tempR = r;
        DOTween.To(() => tempR, value => tempR = value, pos.z, time).onComplete = () => {
            MakeItFall(1, onComplete);
        };

        DOTween.To(() => p, value => p = value, pos, time).SetEase(Ease.Linear).onUpdate = () => { OnUpdateTweenMoveMent(); };

        DOTween.To(() => r, value => r = value, rotFinal, time).SetEase(Ease.Linear).onUpdate = () => { };

    }
    private void OnUpdateTweenMoveMent()
    {
        Transform t = transform;
        t.position = p;// new Vector3(hexMeshes[0].transform.position.x, hexMeshes[0].transform.position.y, z);
        // new Vector3(hexMeshes[0].transform.localEulerAngles.x, hexMeshes[0].transform.localRotation.y, r);
        //  t.Rotate(Vector3.up, -hexMeshes[0].transform.localEulerAngles.y + r);// new Vector3(hexMeshes[0].transform.localEulerAngles.x, hexMeshes[0].transform.localRotation.y, r);
        t.localEulerAngles = new Vector3(r - 90, 0, -60 - 30);

        var d = Vector3.Distance(transform.position, t.position);
        h = Mathf.Sin(r * Mathf.Deg2Rad) *  (1.9f * 0.045f / 2);
        t.position = new Vector3(t.position.x, Mathf.Abs(h), t.position.z);

    }
}

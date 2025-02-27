using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class TileSpawnInfo : MonoBehaviour
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

    public List<MeshRenderer> hexMeshes = new List<MeshRenderer>();
    public int columnIndex => ColumnIndex;
    public int childIndex => ChildIndex;
    public ArrowDirection direction => Direction;
    Grid grid;
    private void Start()
    {
        _hexa.onClick += () => { onHexClicked?.Invoke(this); };
    }
    public void SetInfo(int ColumnIndex, int ChildIndex, ArrowDirection Direction, Grid grid)
    {
        this.ColumnIndex = ColumnIndex;
        this.ChildIndex = ChildIndex;
        this.Direction = Direction;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, (int)Direction, transform.eulerAngles.z);
        this.grid = grid;
        ColorsTOInstantiatedHexa(Direction);
    }


    public void MoveSuccess(float distance, Action onComplete = null)
    {

        transform.DOLocalMoveZ(distance, 1f).onComplete = () => { onComplete?.Invoke(); };

    }
    private void Update()
    {

    }
    public void MoveSuccess(Vector3 pos, Action onComplete = null,  int iterations = 1, int child =0)
    {


        destinationPos = pos;
        time = iterations;
        rotFinal = 180 * time;
        p = transform.position;
        z = transform.position.z;
        r = 0;
        h = hexMeshes[2].transform.position.y;
        float tempR = r;
        DOTween.To(() => tempR, value => tempR = value, pos.z, time).onComplete = () => { onComplete?.Invoke(); };

        DOTween.To(() => p, value => p = value, pos, time).SetEase(Ease.Linear).onUpdate = () => { OnUpdateTweenMoveMent(); };

        DOTween.To(() => r, value => r = value, rotFinal, time).SetEase(Ease.Linear).onUpdate = () => { };


        //MoveSuccesss(pos, onComplete, iterations = 1);
        //MoveSuccess(pos, onComplete, iterations = 1);
    }


    public void MoveSuccesss(Vector3 targetPos, Action onComplete = null, int iterations = 1)
    {
        Vector3 movement = targetPos - transform.position;
        float targetAngle = Mathf.Atan2(movement.z, movement.x) * Mathf.Rad2Deg;
        float adjustedAngle = targetAngle + 90; 

        float delay = 0.3f;
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < hexMeshes.Count; i++)
        {
            var hexMesh = hexMeshes[i];
            sequence.AppendInterval(delay * i * iterations)
                .AppendCallback(() =>
                {
                    hexMesh.transform.DOMove(targetPos, 1f)
                        .SetEase(Ease.Linear)
                        .OnUpdate(() =>
                        {
                            hexMesh.transform.RotateAroundLocal(Vector3.right, 180 * Time.deltaTime);

                            // Handle Hexa height adjustment (if needed)
                            float h = Mathf.Sin(adjustedAngle * Mathf.Deg2Rad) * (grid._zOffset / 1.9f);
                            hexMesh.transform.position = new Vector3(hexMesh.transform.position.x, Mathf.Abs(h) + 0.01f, hexMesh.transform.position.z);
                        })
                        .OnComplete(() =>
                        {
                            onComplete?.Invoke();


                        });
                });
        }


    }

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



    private void OnUpdateTweenMoveMent()
    {
        Transform t = hexMeshes[0].transform;
        t.position = p;// new Vector3(hexMeshes[0].transform.position.x, hexMeshes[0].transform.position.y, z);
        // new Vector3(hexMeshes[0].transform.localEulerAngles.x, hexMeshes[0].transform.localRotation.y, r);
        //  t.Rotate(Vector3.up, -hexMeshes[0].transform.localEulerAngles.y + r);// new Vector3(hexMeshes[0].transform.localEulerAngles.x, hexMeshes[0].transform.localRotation.y, r);
        t.localEulerAngles = new Vector3(r - 90, 0, -60 - 30);

        var d = Vector3.Distance(transform.position, t.position);
        h = Mathf.Sin(r * Mathf.Deg2Rad) * ((grid._zOffset / 1.9f));
        t.position = new Vector3(t.position.x, Mathf.Abs(h) + 0.01f, t.position.z);

    }

    private void OnUpdateTweenRotation()
    {
        // new Vector3(hexMeshes[0].transform.position.x, hexMeshes[0].transform.position.y, z);
        // new Vector3(hexMeshes[0].transform.localEulerAngles.x, hexMeshes[0].transform.localRotation.y, r);

    }
    public void MoveFailure(Vector3 pos)
    {

    }
    private void OnDestroy()
    {
        StackSpawner.tileSpawnInfos.Remove(this);
        if (StackSpawner.tileSpawnInfos.Count <= 0)
        {
            for (int i = 0; i < grid.gameObject.transform.childCount; i++)
            {
                Destroy(grid.gameObject.transform.GetChild(i).gameObject);
            }
            grid.columns.Clear();
            grid.winPanel.SetActive(true);
            grid.winImage.transform.DOScale(1.2f, 5).SetEase(Ease.OutElastic);
            if (grid.isEasy == true)
            {

                grid.SpawnNinthLevelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Play Again";
            }
        }

    }
    public void ColorsTOInstantiatedHexa(ArrowDirection dir)
    {

        switch (dir)
        {
            case ArrowDirection.Up:
                foreach (var r in hexMeshes)
                {
                    r.material.color = _pink;
                }
                break;
            case ArrowDirection.UpperRight:
                foreach (var r in hexMeshes)
                {
                    r.material.color = _blue;
                }
                break;
            case ArrowDirection.DownRight:
                foreach (var r in hexMeshes)
                {
                    r.material.color = _green;
                }
                break;
            case ArrowDirection.Down:
                foreach (var r in hexMeshes)
                {
                    r.material.color = _yellow;
                }
                break;
            case ArrowDirection.DownLeft:
                foreach (var r in hexMeshes)
                {
                    r.material.color = _black;
                }
                break;
            case ArrowDirection.UpperLeft:
                foreach (var r in hexMeshes)
                {
                    r.material.color = _red;
                }
                break;
            default:
                break;
        }
    }
}




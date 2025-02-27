using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{

    [SerializeField] private GameObject hexaForGrid;
    [SerializeField] private int _width = 5;
    [SerializeField] private int _height = 5;
    public float _xOffset = 0.076f;
    public float _zOffset = 0.045f;
    public static List<IndexProperty> AllHexagonIndexes = new List<IndexProperty>();
    public List<TileSpawnInfo> SpawnInfos;
    public List<GameObject> AdditionalSpawnInfosGo;
    public Dictionary<int, List<GameObject>> columns = new Dictionary<int, List<GameObject>>();
    public Dictionary<int, List<GameObject>> level2Columns = new Dictionary<int, List<GameObject>>();

    public Button SpawnNinthLevelBtn;
    public Button retryBT;
    public int score = 28;
    public TMP_Text scoreText;
    public TMP_Text LevelText;
    public GameObject winPanel;
    public GameObject gameOverPanel;
    public bool isEasy;
    public LevelEdit levelE;
    public Camera cam;
    public Image winImage;
    public Image gameOverImage;

    [SerializeField] private StackSpawner ss;
    [SerializeField] private StartGameScript sgs;

    private void Awake()
    {

    }

    /*    private void OnDestroy()
        {
            SpawnNinthLevelBtn.onClick.RemoveListener(() => SpawnNinthLevel(Difficulty.Easy));

        }*/

    void Start()
    {


        GenerateGrid();
        //GenerateGrid2();

        SpawnNinthLevelBtn.onClick.AddListener(() => SpawnNinthLevel(Difficulty.Easy));
        retryBT.onClick.AddListener(() => GameRetryFun());

    }




    private void GameRetryFun()
    {
        if (isEasy == true)
        {
            columns.Clear();
            for (int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                Destroy(this.gameObject.transform.GetChild(i).gameObject);
            }
            foreach (var item in StackSpawner.tileSpawnInfos)
            {
                Destroy(item.gameObject);
            }
            StackSpawner.tileSpawnInfos.Clear();
            SpawnNinthLevel(Difficulty.Easy);
            gameOverPanel.SetActive(false);
        }
        else
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            gameOverPanel.SetActive(false);
     
        }

    }

    private void SpawnNinthLevel(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                /*
                    SpawnInfos = new List<TileSpawnInfo>
                         {

                        new TileSpawnInfo(0,1,ArrowDirection.UpperRight),

                        new TileSpawnInfo(0,2,ArrowDirection.Down),


                        new TileSpawnInfo(1,1,ArrowDirection.DownRight),

                        new TileSpawnInfo(1,2,ArrowDirection.DownRight),

                        new TileSpawnInfo(1,3,ArrowDirection.Up),


                        new TileSpawnInfo(2,1,ArrowDirection.UpperRight),


                        new TileSpawnInfo(3,0,ArrowDirection.Down),

                        new TileSpawnInfo(3,1,ArrowDirection.DownRight),

                        new TileSpawnInfo(3,2,ArrowDirection.UpperLeft),



                        new TileSpawnInfo(4,1,ArrowDirection.DownLeft),

                        new TileSpawnInfo(4,2,ArrowDirection.Up),

                         };*/
                winPanel.SetActive(false);

                score = 14;
                scoreText.text = score.ToString();
                LevelText.text = "Level 2";
                cam.transform.position = new Vector3(0.06f, 0.27f, -0.101f);
                GenerateGrid2();
                isEasy = true;
                ss.GenerateStackParent(ArrowDirection.UpperRight, 1, 1);
                ss.GenerateStackParent(ArrowDirection.UpperRight, 0, 1);
                ss.GenerateStackParent(ArrowDirection.UpperRight, 1, 0);
                ss.GenerateStackParent(ArrowDirection.DownLeft, 1, 2);
                ss.GenerateStackParent(ArrowDirection.DownLeft, 2, 0);

                break;
            default:
                throw new Exception("Diff does not exist");
        }
    }

    void GenerateGrid()
    {
        int[] colHeights = { 3, 4, 3, 4, 3 };

        for (int x = 0; x < colHeights.Length; x++)
        {
            columns.Add(x, new List<GameObject>());
            float baseZPos = 0f;

            for (int y = 0; y < colHeights[x]; y++)
            {
                float xPos = x * _xOffset;
                float zPos = y * _zOffset * 1.9f;

                if (x % 2 == 1)
                {
                    zPos += _zOffset;
                }

                if ((x == 1 || x == 3) && y == 0)
                {
                    zPos = -0.045f;
                    baseZPos = zPos;
                }
                else if ((x == 1 || x == 3) && y > 0)
                {
                    zPos = baseZPos + y * _zOffset * 1.9f;
                }

                var hexa = Instantiate(hexaForGrid, new Vector3(xPos, -0.0039f, zPos), Quaternion.identity);
                hexa.transform.SetParent(this.transform);
                columns[x].Add(hexa);
            }
        }
        isEasy = false;
        GenerateTileStack();
    }

    void GenerateTileStack()
    {

        ss.GenerateStackParent(ArrowDirection.UpperRight, 0, 0);
        ss.GenerateStackParent(ArrowDirection.Down, 0, 1);
        ss.GenerateStackParent(ArrowDirection.DownRight, 1, 1);
        ss.GenerateStackParent(ArrowDirection.DownRight, 1, 2);
        ss.GenerateStackParent(ArrowDirection.Up, 1, 3);
        ss.GenerateStackParent(ArrowDirection.UpperRight, 2, 1);
        ss.GenerateStackParent(ArrowDirection.Down, 3, 0);
        ss.GenerateStackParent(ArrowDirection.DownRight, 3, 1);
        ss.GenerateStackParent(ArrowDirection.UpperLeft, 3, 2);
        ss.GenerateStackParent(ArrowDirection.DownLeft, 4, 1);
        ss.GenerateStackParent(ArrowDirection.Up, 4, 2);
    }

    void GenerateGrid2()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            Destroy(this.gameObject.transform.GetChild(i).gameObject);
        }
        columns.Clear();
        int[] colHeights = { 2, 3, 2 };

        for (int x = 0; x < colHeights.Length; x++)
        {
            columns.Add(x, new List<GameObject>());
            float baseZPos = 0f;

            for (int y = 0; y < colHeights[x]; y++)
            {
                float xPos = x * _xOffset;
                float zPos = y * _zOffset * 1.9f;

                if (x % 2 == 1)
                {
                    zPos += _zOffset;
                }

                if ((x == 1 || x == 3) && y == 0)
                {
                    zPos = -0.045f;
                    baseZPos = zPos;
                }
                else if ((x == 1 || x == 3) && y > 0)
                {
                    zPos = baseZPos + y * _zOffset * 1.9f;
                }

                var hexa = Instantiate(hexaForGrid, new Vector3(xPos, -0.0039f, zPos), Quaternion.identity);
                hexa.transform.SetParent(this.transform);
                columns[x].Add(hexa);
            }
        }
    }

    public void OnClickHex(TileSpawnInfo hexInfo)
    {
        // Debug.Log(hexInfo.direction);

        switch (hexInfo.direction)
        {
            case ArrowDirection.Up:
                CheckMoveUP(hexInfo);
                break;
            case ArrowDirection.UpperRight:
                CheckMoveUpperRight(hexInfo);
                break;
            case ArrowDirection.DownRight:
                CheckMoveDownRight(hexInfo);
                break;
            case ArrowDirection.Down:
                CheckMoveDown(hexInfo);
                break;
            case ArrowDirection.DownLeft:
                CheckMoveDownLeft(hexInfo);
                break;
            case ArrowDirection.UpperLeft:
                CheckMoveUpperLeft(hexInfo);
                break;
            default:
                break;
        }

        score--;
        scoreText.text = score.ToString();
        if (score <= 0)
        {
            for (int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                Destroy(this.gameObject.transform.GetChild(i).gameObject);
            }
            columns.Clear();

            gameOverPanel.SetActive(true);
            gameOverImage.transform.DOScale(1.2f, 5).SetEase(Ease.OutElastic);

        }
    }


    [ContextMenu("Swap")]
    public void Swaaapy()
    {
        SwapHex(columns[0][0].GetComponent<TileSpawnInfo>(), columns[1][1].GetComponent<TileSpawnInfo>());
    }


    void SwapHex(TileSpawnInfo hexInfoA, TileSpawnInfo hexInfoB)
    {

        int columnA = hexInfoA.columnIndex;
        int columnB = hexInfoB.columnIndex;
        int indexA = StackSpawner.tileSpawnInfos.IndexOf(hexInfoA);
        int indexB = StackSpawner.tileSpawnInfos.IndexOf(hexInfoB);

        StackSpawner.tileSpawnInfos.Remove(hexInfoA);
        StackSpawner.tileSpawnInfos.Remove(hexInfoB);

        var c = hexInfoA.childIndex;
        var d = hexInfoB.childIndex;

        hexInfoA.ColumnIndex = columnB;
        hexInfoB.ColumnIndex = columnA;

        hexInfoA.ChildIndex = d;
        hexInfoB.ChildIndex = c;

        StackSpawner.tileSpawnInfos.Add(hexInfoB);
        StackSpawner.tileSpawnInfos.Add(hexInfoA);
    }

    TileSpawnInfo A;
    TileSpawnInfo B;

    public void LevelEditFun(LevelEdit le)
    {
        Debug.Log("LevelE");
        if (!le.isRotating)
        {
            
            foreach (var col in StackSpawner.tileSpawnInfos)
            {
                var tile = col.GetComponent<TileSpawnInfo>();
                if (tile.columnIndex == 1 && tile.childIndex == 0)
                {
                    A = tile;
                }
                if (tile.columnIndex == 1 && tile.childIndex == 2)
                {
                    B = tile;
                }
            }

            // Rotate if not already rotating
            if(A != null && B != null)
            {

            StartCoroutine(le.Rotate());
            SwapHex(A, B);

            }
        }

        // Swap the tiles

    }

    void CheckMoveUP(TileSpawnInfo tileInfo)
    {
        bool moveAllowed = true;
        Vector2 destinationIndex = new Vector2(tileInfo.columnIndex, tileInfo.childIndex + 1);
        int iterated = 1;
        for (int y = tileInfo.childIndex + 1; y < columns[tileInfo.columnIndex].Count; y++)
        {

            if (columns[tileInfo.columnIndex][y] == null)
            {
                //tileInfo.MoveSuccess(Vector2 destinationIndexInGrid) call here
                moveAllowed = true;
                break;
            }
            foreach (TileSpawnInfo spawnedHexInfo in StackSpawner.tileSpawnInfos)
            {
                if (spawnedHexInfo == null)
                {
                    moveAllowed = false;
                    break;
                }
                if (spawnedHexInfo.childIndex == y)
                {
                    moveAllowed = false;
                    break;
                }
            }
            if (!moveAllowed) break;
            iterated++;

        }
        float distance = _zOffset * 1.9f * iterated;

        if (moveAllowed)
        {
            var children = tileInfo.GetComponentsInChildren<MovableHex>();
            for (int t = 0; t < children.Length; t++)
            {
                if (children[t] != null)
                {
                    children[t].MoveSuccess(tileInfo.transform.position + distance * tileInfo.transform.forward, (i) =>
                    {
                        if (children.Last() == i)
                        {

                            Destroy(tileInfo.gameObject);
                        }

                    }, iterated, t);
                }

            }

        }
    }
    void CheckMoveDown(TileSpawnInfo tileInfo)
    {
        bool moveAllowed = true;
        Vector2 destinationIndex = new Vector2(tileInfo.columnIndex, tileInfo.childIndex - 1);
        int iterated = 1;
        for (int y = tileInfo.childIndex - 1; y >= 0; y--)
        {
            if (columns[tileInfo.columnIndex][y] == null)
            {
                //tileInfo.MoveSuccess(Vector2 destinationIndexInGrid) call here
                moveAllowed = true;
                break;
            }
            foreach (TileSpawnInfo spawnedHexInfo in StackSpawner.tileSpawnInfos)
            {
                if (spawnedHexInfo == null)
                {
                    moveAllowed = false;
                    break;
                }
                if (spawnedHexInfo.childIndex == y)
                {
                    moveAllowed = false;
                    break;
                }
            }
            if (!moveAllowed) break;
            iterated++;
        }
        float distance = _zOffset * 1.9f * iterated;
        if (moveAllowed)
        {
            var children = tileInfo.GetComponentsInChildren<MovableHex>();
            for (int t = 0; t < children.Length; t++)
            {
                if (children[t] != null)
                {
                    children[t].MoveSuccess(tileInfo.transform.position + distance * tileInfo.transform.forward, (i) =>
                    {
                        if (children.Last() == i)
                        {

                            Destroy(tileInfo.gameObject);
                        }

                    }, iterated, t);
                }

            }
        }
    }
    void CheckMoveUpperRight(TileSpawnInfo tileInfo)
    {
        int y = tileInfo.childIndex;
        int x = tileInfo.columnIndex;

        bool moveAllowed = true;
        Vector2 destinationIndex = new Vector2(tileInfo.columnIndex, tileInfo.childIndex - 1);
        int iterated = 1;

        //  while (!endChecking)
        {

            if (x + 1 < columns.Count)
            {

                if (columns[x].Count < columns[x + 1].Count)
                {
                    y++;
                }
            }
            x++;


            for (; x < columns.Count; x++)
            {

                if (!(y < columns[x].Count))
                {
                    moveAllowed = true;
                    break;
                }
                if (columns[x][y] == null)
                {
                    //tileInfo.MoveSuccess(Vector2 destinationIndexInGrid) call here
                    moveAllowed = true;
                    break;
                }
                foreach (TileSpawnInfo spawnedHexInfo in StackSpawner.tileSpawnInfos)
                {
                    if (spawnedHexInfo == null)
                    {
                        moveAllowed = false;
                        break;
                    }
                    if (spawnedHexInfo.childIndex == y && spawnedHexInfo.columnIndex == x)
                    {
                        moveAllowed = false;
                        break;
                    }
                }
                if (!moveAllowed) break;

                iterated++;

                if (x + 1 < columns.Count)
                {
                    if (columns[x].Count < columns[x + 1].Count)
                    {
                        y++;
                    }
                }

            }
            float distance = _zOffset * 1.9f * iterated;
            if (moveAllowed)
            {
                var children = tileInfo.GetComponentsInChildren<MovableHex>();
                for (int t = 0; t < children.Length; t++)
                {
                    if (children[t]!= null)
                    {
                        children[t].MoveSuccess(tileInfo.transform.position + distance * tileInfo.transform.forward, (i) =>
                        {
                            if (children.Last() == i)
                            {

                                Destroy(tileInfo.gameObject);
                            }

                        }, iterated, t);
                    }

                }
            }
        }

    }

    void CheckMoveDownRight(TileSpawnInfo tileInfo)
    {

        int y = tileInfo.childIndex;
        int x = tileInfo.columnIndex;

        bool moveAllowed = true;
        Vector2 destinationIndex = new Vector2(tileInfo.columnIndex, tileInfo.childIndex - 1);
        int iterated = 1;

        //  while (!endChecking)
        {

            if (x + 1 < columns.Count)
            {

                if (columns[x].Count > columns[x + 1].Count)
                {
                    y--;
                }
            }
            x++;


            for (; x < columns.Count; x++)
            {

                if (!(y >= 0))
                {
                    moveAllowed = true;
                    break;
                }
                if (columns[tileInfo.columnIndex][y] == null)
                {
                    //tileInfo.MoveSuccess(Vector2 destinationIndexInGrid) call here
                    moveAllowed = true;
                    break;
                }
                foreach (TileSpawnInfo spawnedHexInfo in StackSpawner.tileSpawnInfos)
                {
                    if (spawnedHexInfo == null)
                    {
                        moveAllowed = false;
                        break;
                    }
                    if (spawnedHexInfo.childIndex == y && spawnedHexInfo.columnIndex == x)
                    {
                        moveAllowed = false;
                        break;
                    }
                }
                if (!moveAllowed) break;

                iterated++;

                if (x + 1 < columns.Count)
                {
                    if (columns[x].Count > columns[x + 1].Count)
                    {
                        y--;
                    }
                }

            }
            float distance = _zOffset * 1.9f * iterated;
            if (moveAllowed)
            {
                var children = tileInfo.GetComponentsInChildren<MovableHex>();
                for (int t = 0; t < children.Length; t++)
                {
                    if (children[t] != null)
                    {
                        children[t].MoveSuccess(tileInfo.transform.position + distance * tileInfo.transform.forward, (i) =>
                        {
                            if (children.Last() == i)
                            {

                                Destroy(tileInfo.gameObject);
                            }

                        }, iterated, t);
                    }

                }
            }
        }

    }

    void CheckMoveDownLeft(TileSpawnInfo tileInfo)
    {

        int y = tileInfo.childIndex;
        int x = tileInfo.columnIndex;

        bool moveAllowed = true;
        Vector2 destinationIndex = new Vector2(tileInfo.columnIndex, tileInfo.childIndex - 1);
        int iterated = 1;

        //  while (!endChecking)
        {

            if (x - 1 >= 0)
            {

                if (columns[x].Count > columns[x - 1].Count)
                {
                    y--;
                }
            }
            x--;


            for (; x >= 0; x--)
            {

                if (!(y >= 0))
                {
                    moveAllowed = true;
                    break;
                }
                if (columns[tileInfo.columnIndex][y] == null)
                {
                    //tileInfo.MoveSuccess(Vector2 destinationIndexInGrid) call here
                    moveAllowed = true;
                    break;
                }
                foreach (TileSpawnInfo spawnedHexInfo in StackSpawner.tileSpawnInfos)
                {
                    if (spawnedHexInfo == null)
                    {
                        moveAllowed = false;
                        break;
                    }
                    if (spawnedHexInfo.childIndex == y && spawnedHexInfo.columnIndex == x)
                    {
                        moveAllowed = false;
                        break;
                    }
                }
                if (!moveAllowed) break;

                iterated++;

                if (x - 1 >= 0)
                {
                    if (columns[x].Count > columns[x - 1].Count)
                    {
                        y--;
                    }
                }

            }
            float distance = _zOffset * 1.9f * iterated;
            if (moveAllowed)
            {
                var children = tileInfo.GetComponentsInChildren<MovableHex>();
                for (int t = 0; t < children.Length; t++)
                {
                    if (children[t] != null)
                    {
                        children[t].MoveSuccess(tileInfo.transform.position + distance * tileInfo.transform.forward, (i) =>
                        {
                            if (children.Last() == i)
                            {

                                Destroy(tileInfo.gameObject);
                            }

                        }, iterated, t);
                    }

                }
            }
        }
    }
    void CheckMoveUpperLeft(TileSpawnInfo tileInfo)
    {
        int y = tileInfo.childIndex;
        int x = tileInfo.columnIndex;

        bool moveAllowed = true;
        Vector2 destinationIndex = new Vector2(tileInfo.columnIndex, tileInfo.childIndex - 1);
        int iterated = 1;

        //  while (!endChecking)
        {

            if (x - 1 >= 0)
            {

                if (columns[x].Count < columns[x - 1].Count)
                {
                    y++;
                }
            }
            x--;


            for (; x >= 0; x--)
            {

                if (!(y < columns.Count))
                {
                    moveAllowed = true;
                    break;
                }
                if (columns[tileInfo.columnIndex][y] == null)
                {
                    //tileInfo.MoveSuccess(Vector2 destinationIndexInGrid) call here
                    moveAllowed = true;
                    break;
                }
                foreach (TileSpawnInfo spawnedHexInfo in StackSpawner.tileSpawnInfos)
                {
                    if (spawnedHexInfo == null)
                    {
                        moveAllowed = false;
                        break;
                    }
                    if (spawnedHexInfo.childIndex == y && spawnedHexInfo.columnIndex == x)
                    {
                        moveAllowed = false;
                        break;
                    }
                }
                if (!moveAllowed) break;


                if (x - 1 >= 0)
                {
                    if (columns[x].Count < columns[x - 1].Count)
                    {
                        y++;
                    }
                    iterated++;
                }

            }
            float distance = _zOffset * 1.9f * iterated;
            if (moveAllowed)
            {
                var children = tileInfo.GetComponentsInChildren<MovableHex>();
                for (int t = 0; t < children.Length; t++)
                {
                    if (children[t] != null)
                    {
                        children[t].MoveSuccess(tileInfo.transform.position + distance * tileInfo.transform.forward, (i) =>
                        {
                            if (children.Last() == i)
                            {

                            Destroy(tileInfo.gameObject);
                            }

                        }, iterated,t);
                    }

                }
            }
        }

    }
}




[Serializable]
public class IndexProperty
{
    public int x;
    public int y;
    //The refrence to the attached tile

    public IndexProperty(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public enum ArrowDirection
{
    Up = 0,
    UpperRight = 60,
    DownRight = 120,
    Down = 180,
    DownLeft = 240,
    UpperLeft = 300
}

public enum Difficulty
{
    Easy,
    Hard
}



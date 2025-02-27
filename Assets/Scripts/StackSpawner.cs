using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StackSpawner : MonoBehaviour
{
   /* [SerializeField] private GameObject _hexaGreenStack;
    [SerializeField] private GameObject _hexaBlackStack;
    [SerializeField] private GameObject _hexaPinkStack;
    [SerializeField] private GameObject _hexaOrangeStack;
    [SerializeField] private GameObject _hexaYellowStack;*/
   // [SerializeField] private GameObject _hexaRedStack;
    [SerializeField] private TileSpawnInfo tileSpawnInfo;

    public List<GameObject> AdditionalSpawnInfosGo;
    public Grid grid;
    public static List<TileSpawnInfo> tileSpawnInfos = new();
    void Start()
    {

    }
LevelEdit levelEdit;
    public void GenerateStackParent(ArrowDirection Ad, int ColoumnInd, int ColChildInd)
    {
        if (grid.isEasy == true)
        {
            var column = grid.columns[ColoumnInd];
            if (ColoumnInd == 1 && ColChildInd == 1)
            {
                levelEdit = Instantiate(grid.levelE, new Vector3(column[ColChildInd].transform.position.x, 0.004f, column[ColChildInd].transform.position.z), Quaternion.identity);
                levelEdit.onclick += (s) => { grid.LevelEditFun(levelEdit); };
                levelEdit.transform.SetParent(grid.transform);

            }
            else
            {
                var hexxa = Instantiate(tileSpawnInfo, new Vector3(column[ColChildInd].transform.position.x, 0.004f, column[ColChildInd].transform.position.z), Quaternion.identity);
                hexxa.SetInfo(ColoumnInd, ColChildInd, Ad, grid);
                tileSpawnInfos.Add(hexxa);
                hexxa.onHexClicked += (s) => { grid.OnClickHex(s); };
                hexxa.transform.SetParent(grid.transform);
                if (ColoumnInd == 1 && ColChildInd == 2)
                {
                    if(levelEdit != null)
                    {
                        levelEdit.hex1 = hexxa;

                    }
                }
                else if(ColoumnInd == 1 && ColChildInd == 0)
                {
                    if(levelEdit != null)
                    {
                        levelEdit.hex2 = hexxa;

                    }
                }
                
            }
        }
        else
        {
            var column = grid.columns[ColoumnInd];
            var hexxa = Instantiate(tileSpawnInfo, new Vector3(column[ColChildInd].transform.position.x, 0.004f, column[ColChildInd].transform.position.z), Quaternion.identity);
            hexxa.SetInfo(ColoumnInd, ColChildInd, Ad, grid);
            tileSpawnInfos.Add(hexxa);
            hexxa.onHexClicked += (s) => { grid.OnClickHex(s); };
            hexxa.transform.SetParent(grid.transform);
        }


    }



}

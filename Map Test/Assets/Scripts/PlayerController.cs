using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TextAsset textAsset;
    private int mapCol, mapRow;
    private char[,] mapData;

    public GameObject startSquare;     // スタートマス
    public Vector3 offsetPos;       // プレーヤー位置調整
    private int xIndex, zIndex;     // mapDataにおけるプレイヤーの位置

    public float speed;
    private Vector3 current;    // 現在のマス
    private Vector3 target;     // 移動先のマス
    private bool move = false;  // 移動中:true, 停止中:false

    public GameObject refObj;

    void Start()
    {
        LoadMap();

        // プレーヤーの初期座標設定
        //startSquare = GameObject.Find("Squares/Square(4,7)");
        Vector3 startPos = startSquare.transform.position + offsetPos;
        this.transform.position = startPos;
        zIndex = (100 - (int)startPos.z) / 2;
        xIndex = (int)startPos.x / 2;
    }

    void Update()
    {
        // プレーヤーの移動
        if (!move)
        {
            // 移動方向を取得
            if (Input.GetKey("up") && existsSquare(xIndex, zIndex, "up"))
            {
                MapCreator MC = refObj.GetComponent<MapCreator>();
                current = this.transform.position;
                target = current + new Vector3(0.0f, 0.0f, MC.spaceScale.z + MC.pathScale.z);
                move = true;
                zIndex -= 2;
            }
            else if (Input.GetKey("down") && existsSquare(xIndex, zIndex, "down"))
            {
                MapCreator MC = refObj.GetComponent<MapCreator>();
                current = this.transform.position;
                target = current + new Vector3(0.0f, 0.0f, -(MC.spaceScale.z + MC.pathScale.z));
                move = true;
                zIndex += 2;
            }
            else if (Input.GetKey("right") && existsSquare(xIndex, zIndex, "right"))
            {
                MapCreator MC = refObj.GetComponent<MapCreator>();
                current = this.transform.position;
                target = current + new Vector3(MC.spaceScale.x + MC.pathScale.x, 0.0f, 0.0f);
                move = true;
                xIndex += 2;
            }
            else if (Input.GetKey("left") && existsSquare(xIndex, zIndex, "left"))
            {
                MapCreator MC = refObj.GetComponent<MapCreator>();
                current = this.transform.position;
                target = current + new Vector3(-(MC.spaceScale.x + MC.pathScale.x), 0.0f, 0.0f);
                move = true;
                xIndex -= 2;
            }
        }
        else
        {
            float step = speed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, step);

            // targetとの距離が小さければ移動終了
            if (Vector3.Distance(this.transform.position, target) < 0.0001)
            {
                move = false;
            }
        }
    }
    void LoadMap()
    {
        string mapTextData = textAsset.text;

        // 行列数カウント
        int row = 0, col = 0;
        bool readLine = true;
        int n = 0;
        foreach (var c in mapTextData)
        {
            if (c == '\n')
            {
                row++;
                readLine = false;
            }
            else if (c != '\r' && c != '\n' && readLine)
            {
                col++;
            }
            n++;
        }
        mapCol = col;
        mapRow = row + 1;

        // マップデータ格納
        mapData = new char[mapRow, mapCol];
        int x = 0, z = 0;
        foreach (var c in mapTextData)
        {
            if (c != '\r' && c != '\n')
            {
                mapData[z, x] = c;
                if (x < mapCol - 1)
                {
                    x++;
                }
                else if (z < mapRow - 1)
                {
                    x = 0;
                    z++;
                }
                else
                {
                    break;
                }
            }
        }
    }
    bool existsSquare(int xIndex, int zIndex, string direction)
    {
        switch (direction)
        {
            case "up":
                if (zIndex > 0 && mapData[zIndex - 1, xIndex] == '|')
                    return true;
                else
                    return false;
            case "down":
                if (zIndex < mapRow - 1 && mapData[zIndex + 1, xIndex] == '|')
                    return true;
                else
                    return false;
            case "right":
                if (xIndex < mapCol - 1 && mapData[zIndex, xIndex + 1] == '-')
                    return true;
                else
                    return false;
            case "left":
                if (xIndex > 0 && mapData[zIndex, xIndex - 1] == '-')
                    return true;
                else
                    return false;
            default:
                return false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapCreator : MonoBehaviour
{
    public bool laySquare;
    public bool layCard;

    // マップのテキストファイル
	public TextAsset textAsset;

    // 配置するオブジェクト
    public GameObject square;
    public GameObject cardSquare;
    public GameObject path;
    public GameObject card;

    // 親となるオブジェクト
    private GameObject squareParent;
    private GameObject pathParent;
    private GameObject cardParent;

    public Vector3 createPos;

    public Vector3 spaceScale = new Vector3(2.0f, 0.0f, 2.0f);
    public Vector3 pathScale = new Vector3(2.0f, 0.0f, 2.0f);

    void Start ()
    {
        CreateMap(createPos);
	}
    void CreateMap(Vector3 pos)
    {
        Vector3 originPos = pos;
        string mapTextData = textAsset.text;
        if (laySquare)
        {
            squareParent = new GameObject("Squares");
            pathParent = new GameObject("Path");
        }
        if (layCard)
        {
            cardParent = new GameObject("Cards");
        }
		int xIndex = 0, zIndex = 0;
        int col = 0, row = 0;
        foreach(char c in mapTextData)
        {
            GameObject obj = null;

            if (c == '\n')
            {
                pos.z -= spaceScale.z;
                pos.x = originPos.x;
                if (row % 2 == 1)
                {
                    zIndex++;
                }
                xIndex = 0;
                col = 0;
                row++;
            }

            // マス目の設置
            if ((col % 2 == 0 && row % 2 == 0) || (col % 2 == 1 && row % 2 == 1))
            {
                if (c == '0')
                {
                    pos.x += spaceScale.x;
                    xIndex++;
                    col++;
                }
                else if (c == '#')  // 通常マス
                {
                    if (laySquare)
                    {
                        obj = Instantiate(square, pos, Quaternion.identity, squareParent.transform) as GameObject;
                        obj.name = square.name + " (" + zIndex + "," + xIndex + ")";
                    }
                    pos.x += spaceScale.x;
                    xIndex++;
                    col++;
                }
                else if (c == '@')  // カードマス
                {
                    if (laySquare)
                    {
                        obj = Instantiate(cardSquare, pos, Quaternion.identity, squareParent.transform) as GameObject;
                        obj.name = square.name + " (" + zIndex + "," + xIndex + ")";
                    }
                    if(layCard)
                    {
                        obj = Instantiate(card, pos + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.Euler(60,180,0), cardParent.transform) as GameObject;
                        obj.name = card.name + " (" + zIndex + "," + xIndex + ")";
                    }
                    pos.x += spaceScale.x;
                    xIndex++;
                    col++;
                }

            }
            // 横のパスの設置
            else if(col % 2 == 1 && row % 2 == 0)
            {
                if (c == '0')
                {
                    pos.x += pathScale.x;
                    col++;
                }
                else if (c == '-')
                {
                    if (laySquare)
                    {
                        obj = Instantiate(path, pos, Quaternion.identity, pathParent.transform) as GameObject;
                        obj.name = path.name + "X (" + zIndex + "," + (xIndex - 1) + ") <-> (" + zIndex + "," + xIndex + ")";
                    }
                    pos.x += pathScale.x;
                    col++;
                }
            }
            // 縦のパスの設置
            else if(col % 2 == 0 && row % 2 == 1)
            {
                if (c == '0')
                {
                    pos.x += spaceScale.x;
                    col++;
                }
                else if(c == '|')
                {
                    if (laySquare)
                    {
                        obj = Instantiate(path, pos, Quaternion.Euler(0,90,0), pathParent.transform) as GameObject;
                        obj.name = path.name + "Z (" + zIndex + "," + xIndex + ") <-> (" + (zIndex + 1) + "," + xIndex + ")";
                    }
                    pos.x += spaceScale.x;
                    col++;
                }
            }

        }
    }
}

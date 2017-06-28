using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapCreator : MonoBehaviour
{
	public TextAsset textAsset;

    // 配置するオブジェクト
    public GameObject square;
    public GameObject cardSquare;
    public GameObject pathX;
    public GameObject pathZ;

    // 親となるオブジェクト
    public GameObject squareParent;
    public GameObject pathXParent;
    public GameObject pathZParent;

    public Vector3 createPos;

    public Vector3 spaceScale = new Vector3(2.0f, 0.0f, 2.0f);
    public Vector3 pathScale = new Vector3(2.0f, 0.0f, 2.0f);

    void Start ()
    {
        CreateMap(createPos);

        createPos = Vector3.zero;
	}
    void CreateMap(Vector3 pos)
    {
        Vector3 originPos = pos;
        string mapTextData = textAsset.text;

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
                else if (c == '#')
                {
                    obj = Instantiate(square, pos, Quaternion.identity, squareParent.transform) as GameObject;
                    obj.name = square.name + "(" + zIndex + "," + xIndex + ")";
                    pos.x += spaceScale.x;
                    xIndex++;
                    col++;
                }
                else if (c == '@')
                {
                    obj = Instantiate(cardSquare, pos, Quaternion.identity, squareParent.transform) as GameObject;
                    obj.name = square.name + "(" + zIndex + "," + xIndex + ")";
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
                    obj = Instantiate(pathX, pos, Quaternion.identity, pathXParent.transform) as GameObject;
                    obj.name = pathX.name + "(" + zIndex + "," + (xIndex - 1) + ") <-> (" + zIndex + "," + xIndex + ")";
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
                    obj = Instantiate(pathZ, pos, Quaternion.identity, pathZParent.transform) as GameObject;
                    obj.name = pathZ.name + "(" + zIndex + "," + xIndex + ") <-> (" + (zIndex + 1) + "," + xIndex + ")";
                    pos.x += spaceScale.x;
                    col++;
                }
            }

        }
    }
}

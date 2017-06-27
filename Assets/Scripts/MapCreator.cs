using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapCreator : MonoBehaviour
{
    // マップデータのテキストファイル
	public TextAsset textAsset;

    // 配置するオブジェクト
    public GameObject square;

    // 親となるオブジェクト
    public GameObject squareParent;

    public Vector3 createPos;

    public Vector3 spaceScale = new Vector3(2.0f, 0.0f, 2.0f);

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

        foreach(char c in mapTextData)
        {
            GameObject obj = null;

            if (c == '0')
            {
                pos.x += spaceScale.x;
                xIndex++;
            }
            else if (c == '1')
            {
                obj = Instantiate(square, pos, Quaternion.identity, squareParent.transform) as GameObject;
                obj.name = square.name + "(" + zIndex + "," + xIndex + ")";
                pos.x += spaceScale.x;
                xIndex++;
            }
            else if(c == '\n')
            {
                pos.z -= spaceScale.z;
                pos.x = originPos.x;
                xIndex = 0;
                zIndex++;
            }
        }
    }
}

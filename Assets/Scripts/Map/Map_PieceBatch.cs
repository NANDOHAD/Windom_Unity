using System.Collections.Generic;
using UnityEngine;

public class Map_PieceBatch
{
    private List<GameObject> objects = new List<GameObject>();

    // まとめてGameObjectを破棄
    public void destroyGameObjects()
    {
        foreach (var obj in objects)
        {
            if (obj != null)
            {
                GameObject.Destroy(obj);
            }
        }
        objects.Clear();
    }

    // まとめてGameObjectを生成（仮実装）
    public void generateGameObjects()
    {
        // ここは用途に応じて拡張してください
        // 例: ダミーオブジェクトを1つ生成
        GameObject go = new GameObject("BatchObject");
        objects.Add(go);
    }
} 
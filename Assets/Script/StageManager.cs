using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public string[] Alpha_Ary = new string[] {"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z" };

    public GameObject npcPrefab; // NPCのプレハブ
    public Vector2 spawnPosition = new Vector2(21.37f, -2.55f); // スポーンする位置
    public float spawnInterval = 5f; // NPCを生成する間隔（秒）

    private void Start()
    {
        // NPCを一定間隔で生成するコルーチンを開始
        StartCoroutine(SpawnNPCs());
    }

    private IEnumerator SpawnNPCs()
    {
        while (true)
        {
            // 一定時間待機
            yield return new WaitForSeconds(spawnInterval);

            // NPCを指定された位置に生成
            GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);

            // スクリプトを取得してNameフィールドに名前を設定
            Enemy_AI npcController = npc.GetComponent<Enemy_AI>();
            if (npcController != null)
            {
                npcController.Name += Alpha_Ary[Random.Range(0, 27)]; // ランダムな名前を設定
                Debug.Log(npcController.Name);
            }

            Debug.Log("NPCがスポーンしました: " + spawnPosition);


        }
    }
}

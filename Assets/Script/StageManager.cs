using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject npcPrefab; // NPC�̃v���n�u
    public Vector2 spawnPosition = new Vector2(21.37f, -2.55f); // �X�|�[������ʒu
    public float spawnInterval = 5f; // NPC�𐶐�����Ԋu�i�b�j

    private void Start()
    {
        // NPC�����Ԋu�Ő�������R���[�`�����J�n
        StartCoroutine(SpawnNPCs());
    }

    private IEnumerator SpawnNPCs()
    {
        while (true)
        {
            // ��莞�ԑҋ@
            yield return new WaitForSeconds(spawnInterval);

            // NPC���w�肳�ꂽ�ʒu�ɐ���
            GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("NPC���X�|�[�����܂���: " + spawnPosition);


        }
    }
}

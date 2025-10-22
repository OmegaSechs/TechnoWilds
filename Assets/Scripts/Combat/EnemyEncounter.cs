using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyEncounter : MonoBehaviour
{
    [Header("Dados do Encontro")]

    public CreatureData enemyData;

    [Header("Teste do Jogador")]

    public CreatureData playerCreatureForTest; 

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (playerCreatureForTest == null || enemyData == null)
            {
                Debug.LogError("Dados do jogador ou inimigo não configurados no EnemyEncounter!");
                return;
            }

            Debug.Log($"Iniciando batalha contra {enemyData.creatureName}");
            GameManager.Instance.StartBattle(playerCreatureForTest, enemyData);
        }
    }
}
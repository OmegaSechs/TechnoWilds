using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ScriptableObjects de quem vai lutar
    public CreatureData playerCreatureData;
    public CreatureData enemyCreatureData;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartBattle(CreatureData player, CreatureData enemy)
    {
        playerCreatureData = player;
        enemyCreatureData = enemy;
        
        SceneManager.LoadScene("Battle"); 
    }
}
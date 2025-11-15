using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [Header("Referências Lógicas")]
    public Creature playerCreature;
    public Creature enemyCreature;
    public BattleAudioManager audioManager;

    [Header("Referências Visuais (Prefabs Instanciados)")]
    private GameObject playerPrefabInstance;
    private GameObject enemyPrefabInstance;

    [Header("Referências de UI (Controladores)")]
    public BattleUIManager uiManager;
    public BattleHUD jogadorHUD;
    public BattleHUD inimigoHUD;
    public TextMeshProUGUI logTexto;

    [Header("Pontos de Spawn")]
    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;

    [Header("Estado da Batalha")]
    public BattleState state;
    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

    [Header("Dados para Teste")]
    public bool usarDadosDeTeste = false;
    public CreatureData playerDataTeste;
    public CreatureData enemyDataTeste;

    IEnumerator Start()
    {
        yield return null;
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        CreatureData playerData;
        CreatureData enemyData;

        if (usarDadosDeTeste)
        {
            playerData = playerDataTeste;
            enemyData = enemyDataTeste;
            if (playerData == null || enemyData == null)
            {
                Debug.LogError("Configure os dados de teste (playerDataTeste e enemyDataTeste) no Inspector!");
                yield break;
            }
        }
        else
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager não encontrado!");
                yield break;
            }
            playerData = GameManager.Instance.playerCreatureData;
            enemyData = GameManager.Instance.enemyCreatureData;
            if (playerData == null || enemyData == null)
            {
                Debug.LogError("Dados das criaturas não foram configurados no GameManager!");
                yield break;
            }
        }

        if (playerData.battlePrefab != null)
        {
            playerPrefabInstance = Instantiate(playerData.battlePrefab, playerSpawnPoint);
        }
        if (enemyData.battlePrefab != null)
        {
            enemyPrefabInstance = Instantiate(enemyData.battlePrefab, enemySpawnPoint);
        }

        playerCreature = new FireCreature(playerData.creatureName, playerData.maxHealth, playerData.maxSP, playerData.baseAttack, playerData.baseDefense, playerData.baseLevel, playerData.attacks);
        enemyCreature = new FireCreature(enemyData.creatureName, enemyData.maxHealth, enemyData.maxSP, enemyData.baseAttack, enemyData.baseDefense, enemyData.baseLevel, enemyData.attacks);

        logTexto.text = $"Um {enemyCreature.Name} selvagem apareceu!";

        jogadorHUD.SetHUD(playerCreature);
        inimigoHUD.SetHUD(enemyCreature);

        if (audioManager != null)
        {
            audioManager.PlayBattleMusic();
        }

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }


    void PlayerTurn()
    {
        logTexto.text = "Sua vez! O que fazer?";
        uiManager.ShowMainActionPanel();
    }

    public void OnPlayerAttack(AttackData selectedAttack)
    {
        if (state != BattleState.PLAYERTURN)
            return;
        uiManager.HideAllActionPanels();
        StartCoroutine(PlayerAttackCoroutine(selectedAttack));
    }

    IEnumerator PlayerAttackCoroutine(AttackData selectedAttack)
    {
        if (playerCreature.CurrentSP < selectedAttack.spCost)
        {
            logTexto.text = "SP insuficiente!";
            if (audioManager != null) audioManager.PlayInsufficientSPSFX();
            yield return new WaitForSeconds(1.5f);
            PlayerTurn();
            yield break;
        }

        if (audioManager != null)
        {
            audioManager.PlaySFX(selectedAttack.attackSound);
        }

        playerCreature.AttackTarget(enemyCreature, selectedAttack);

        if (enemyPrefabInstance != null)
        {
            FeedbackDeDano feedback = enemyPrefabInstance.GetComponent<FeedbackDeDano>();
            if (feedback != null)
            {
                feedback.IniciarFeedbackDano();
            }
        }

        if (audioManager != null) audioManager.PlayDamageSFX();

        inimigoHUD.SetHP(enemyCreature.CurrentHP);
        jogadorHUD.SetSP(playerCreature.CurrentSP);
        logTexto.text = $"{playerCreature.Name} usou {selectedAttack.attackName}!";

        yield return new WaitForSeconds(1.5f);

        if (enemyCreature.CurrentHP <= 0)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        logTexto.text = $"{enemyCreature.Name} prepara um ataque...";
        yield return new WaitForSeconds(1.5f);

        if (enemyCreature.Attacks.Count > 0)
        {
            int attackIndex = Random.Range(0, enemyCreature.Attacks.Count);
            AttackData enemyAttack = enemyCreature.Attacks[attackIndex];

            if (enemyCreature.CurrentSP >= enemyAttack.spCost)
            {
                if (audioManager != null)
                {
                    audioManager.PlaySFX(enemyAttack.attackSound);
                }

                enemyCreature.AttackTarget(playerCreature, enemyAttack);

                if (playerPrefabInstance != null)
                {
                    FeedbackDeDano feedback = playerPrefabInstance.GetComponent<FeedbackDeDano>();
                    if (feedback != null)
                    {
                        feedback.IniciarFeedbackDano();
                    }
                }
                
                if (audioManager != null) audioManager.PlayDamageSFX();
                jogadorHUD.SetHP(playerCreature.CurrentHP);
                inimigoHUD.SetSP(enemyCreature.CurrentSP);
                logTexto.text = $"{enemyCreature.Name} usou {enemyAttack.attackName}!";
            }
            else
            {
                logTexto.text = $"{enemyCreature.Name} não tem SP!";
            }
        }
        else
        {
            logTexto.text = $"{enemyCreature.Name} não sabe nenhum ataque!";
        }

        yield return new WaitForSeconds(1.5f);

        if (playerCreature.CurrentHP <= 0)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EndBattle()
    {
        uiManager.HideAllActionPanels();

        if (state == BattleState.WON)
        {
            logTexto.text = "Você venceu a batalha!";
            if (audioManager != null) audioManager.PlayWinMusic();
            yield return new WaitForSeconds(5.0f);
            SceneManager.LoadScene("Creditos", LoadSceneMode.Single);
        }
        else if (state == BattleState.LOST)
        {
            logTexto.text = "Você foi derrotado.";
            if (audioManager != null) audioManager.PlayLoseMusic();
            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene("Creditos", LoadSceneMode.Single);
        }
    }
}
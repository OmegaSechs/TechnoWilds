using UnityEngine;
using System.Collections; 
using System.Collections.Generic;
using TMPro; 

public class BattleManager : MonoBehaviour
{
    [Header("Referências Lógicas")]
    public Creature playerCreature;
    public Creature enemyCreature;
    
    [Header("Dados das Criaturas (Arraste os ScriptableObjects aqui)")]
    public List<AttackData> playerAttacks;
    public List<AttackData> enemyAttacks;

    [Header("Referências de UI (Controladores)")]
    public BattleUIManager uiManager; 
    public BattleHUD jogadorHUD;
    public BattleHUD inimigoHUD;
    public TextMeshProUGUI logTexto;

    [Header("Estado da Batalha")]
    public BattleState state;
    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        playerCreature = new FireCreature("Pyro", 100, 50, 10, 5, playerAttacks);
        enemyCreature = new FireCreature("Inferno", 100, 50, 10, 5, enemyAttacks);

        logTexto.text = $"Um {enemyCreature.Name} selvagem apareceu!";

        jogadorHUD.SetHUD(playerCreature);
        inimigoHUD.SetHUD(enemyCreature);

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
        playerCreature.AttackTarget(enemyCreature, selectedAttack);
        
        inimigoHUD.SetHP(enemyCreature.CurrentHP);
        logTexto.text = $"{playerCreature.Name} usou {selectedAttack.attackName}!";

        yield return new WaitForSeconds(1.5f); 

        if (enemyCreature.CurrentHP <= 0)
        {
            state = BattleState.WON;
            EndBattle();
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
            AttackData enemyAttack = enemyCreature.Attacks[0];
            enemyCreature.AttackTarget(playerCreature, enemyAttack);
            jogadorHUD.SetHP(playerCreature.CurrentHP);
            logTexto.text = $"{enemyCreature.Name} usou {enemyAttack.attackName}!";
        }
        else
        {
            logTexto.text = $"{enemyCreature.Name} não tem ataques!";
        }
        
        yield return new WaitForSeconds(1.5f);
        
        if (playerCreature.CurrentHP <= 0)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        uiManager.HideAllActionPanels(); 

        if (state == BattleState.WON)
        {
            logTexto.text = "Você venceu a batalha!";
        }
        else if (state == BattleState.LOST)
        {
            logTexto.text = "Você foi derrotado.";
        }
    }
}
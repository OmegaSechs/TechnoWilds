using UnityEngine;
using System.Collections; 
using TMPro; 

public class BattleManager : MonoBehaviour
{
    public Creature playerCreature;
    public Creature enemyCreature;

    public BattleHUD jogadorHUD;
    public BattleHUD inimigoHUD;
    public TextMeshProUGUI logTexto;

    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
    public BattleState state;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        playerCreature = new FireCreature("Pyro", 100, 100, 5); 
        enemyCreature = new FireCreature("Inferno", 100, 100,5);

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
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        // Lógica de ataque
        playerCreature.AttackTarget(enemyCreature);
        
        // Atualiza a UI do inimigo
        inimigoHUD.SetHP(enemyCreature.CurrentHP);
        logTexto.text = $"{playerCreature.Name} ataca!";

        yield return new WaitForSeconds(1f);

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
        logTexto.text = $"{enemyCreature.Name} ataca de volta!";
        yield return new WaitForSeconds(1f);

        enemyCreature.AttackTarget(playerCreature);
        jogadorHUD.SetHP(playerCreature.CurrentHP);
        
        yield return new WaitForSeconds(1f);
        
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
using UnityEngine;
using System.Collections; 
using System.Collections.Generic; // Necessário para List<>
using TMPro; // Necessário para TextMeshPro

public class BattleManager : MonoBehaviour
{
    [Header("Referências Lógicas")]
    public Creature playerCreature;
    public Creature enemyCreature;
    
    // As listas de ataque "playerAttacks" e "enemyAttacks" foram REMOVIDAS
    // pois agora lemos elas de dentro do CreatureData

    [Header("Referências de UI (Controladores)")]
    public BattleUIManager uiManager; 
    public BattleHUD jogadorHUD;
    public BattleHUD inimigoHUD;
    public TextMeshProUGUI logTexto;
    
    [Header("Pontos de Spawn")]
    public Transform playerSpawnPoint; // Onde o monstro do jogador aparece
    public Transform enemySpawnPoint;  // Onde o monstro inimigo aparece

    [Header("Estado da Batalha")]
    public BattleState state;
    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

    [Header("Dados para Teste")]
    public bool usarDadosDeTeste = true;
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

        // --- ETAPA 1: LER OS DADOS DO GAMEMANAGER OU USAR DADOS DE TESTE ---
        if (usarDadosDeTeste)
        {
            // Usa os dados configurados no Inspector para testes
            playerData = playerDataTeste;
            enemyData = enemyDataTeste;
            
            if (playerData == null || enemyData == null)
            {
                Debug.LogError("Configure os dados de teste (playerDataTeste e enemyDataTeste) no Inspector do BattleManager!");
                yield break;
            }
        }
        else
        {
            // Verifica se o GameManager existe
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager não encontrado! Certifique-se que existe um GameManager na cena.");
                yield break;
            }

            // Pega os "moldes" (ScriptableObjects) que o GameManager transportou
            playerData = GameManager.Instance.playerCreatureData;
            enemyData = GameManager.Instance.enemyCreatureData;

            // Verifica se os dados das criaturas existem
            if (playerData == null || enemyData == null)
            {
                Debug.LogError("Dados das criaturas não foram configurados no GameManager!");
                yield break;
            }
        }

        // --- ETAPA 2: INSTANCIAR (CRIAR) OS VISUAIS ---
        // Cria os prefabs visuais nos locais de spawn
        if (playerData.battlePrefab != null)
        {
            Instantiate(playerData.battlePrefab, playerSpawnPoint);
        }
        if (enemyData.battlePrefab != null)
        {
            Instantiate(enemyData.battlePrefab, enemySpawnPoint);
        }

        // --- ETAPA 3: CRIAR AS CRIATURAS LÓGICAS ---
        // Cria as instâncias "vivas" das criaturas para a batalha
        // (Nome, MaxHP, MaxSP, AttackBase, Level, ListaDeAtaques)
        playerCreature = new FireCreature(playerData.creatureName, playerData.maxHealth, playerData.maxSP, playerData.baseAttack, playerData.baseLevel, playerData.attacks);
        
        // TODO: Criar uma classe base "EnemyCreature" ou classes específicas
        // Por enquanto, usamos FireCreature para ambos para testar
        enemyCreature = new FireCreature(enemyData.creatureName, enemyData.maxHealth, enemyData.maxSP, enemyData.baseAttack, enemyData.baseLevel, enemyData.attacks);

        // --- ETAPA 4: CONFIGURAR A UI INICIAL ---
        logTexto.text = $"Um {enemyCreature.Name} selvagem apareceu!";

        jogadorHUD.SetHUD(playerCreature);
        inimigoHUD.SetHUD(enemyCreature);

        yield return new WaitForSeconds(2f); // Pausa para o jogador ler

        // --- ETAPA 5: INICIAR O PRIMEIRO TURNO ---
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    // --- Gerenciamento de Turnos ---

    void PlayerTurn()
    {
        logTexto.text = "Sua vez! O que fazer?";
        // Avisa o UIManager para mostrar o menu principal
        uiManager.ShowMainActionPanel(); 
    }

    // Esta função é PÚBLICA e é chamada pelo UIManager (quando o jogador clica num ataque)
    public void OnPlayerAttack(AttackData selectedAttack)
    {
        if (state != BattleState.PLAYERTURN)
            return; // Garante que o jogador só ataque no seu turno

        // Esconde os menus de ação assim que o ataque é escolhido
        uiManager.HideAllActionPanels();
        
        // Inicia a rotina de ataque
        StartCoroutine(PlayerAttackCoroutine(selectedAttack));
    }

    // Esta é a rotina PRIVADA que executa o ataque do jogador
    IEnumerator PlayerAttackCoroutine(AttackData selectedAttack)
    {
        // Verifica se o jogador tem SP suficiente
        if (playerCreature.CurrentSP < selectedAttack.spCost)
        {
            logTexto.text = "SP insuficiente!";
            yield return new WaitForSeconds(1.5f);
            PlayerTurn(); // Devolve o turno ao jogador
            yield break; // Para a execução desta rotina
        }
        
        // Lógica de ataque
        playerCreature.AttackTarget(enemyCreature, selectedAttack);
        
        // Atualiza os HUDs
        inimigoHUD.SetHP(enemyCreature.CurrentHP);
        jogadorHUD.SetSP(playerCreature.CurrentSP); // Atualiza o SP do jogador
        logTexto.text = $"{playerCreature.Name} usou {selectedAttack.attackName}!";

        yield return new WaitForSeconds(1.5f); // Tempo para o jogador ler

        // Verifica se o inimigo morreu
        if (enemyCreature.CurrentHP <= 0)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            // Passa o turno para o inimigo
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    // A rotina de ataque do inimigo
    IEnumerator EnemyTurn()
    {
        logTexto.text = $"{enemyCreature.Name} prepara um ataque...";
        yield return new WaitForSeconds(1.5f); // Tempo para o inimigo "pensar"

        // --- Lógica de IA (Inteligência Artificial) Simples ---
        if (enemyCreature.Attacks.Count > 0)
        {
            // IA simples: escolhe um ataque aleatório
            int attackIndex = Random.Range(0, enemyCreature.Attacks.Count);
            AttackData enemyAttack = enemyCreature.Attacks[attackIndex];

            // Verifica se o inimigo tem SP para este ataque
            if (enemyCreature.CurrentSP >= enemyAttack.spCost)
            {
                enemyCreature.AttackTarget(playerCreature, enemyAttack);
                jogadorHUD.SetHP(playerCreature.CurrentHP);
                inimigoHUD.SetSP(enemyCreature.CurrentSP); // Atualiza SP do inimigo
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

            // StartCoroutine(ReturnToMap());
        }
        else if (state == BattleState.LOST)
        {
            logTexto.text = "Você foi derrotado.";
        }
    }
}
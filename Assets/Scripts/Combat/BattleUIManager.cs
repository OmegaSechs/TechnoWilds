using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; 

public class BattleUIManager : MonoBehaviour
{
    public BattleManager battleManager;

    [Header("Painéis de Menu")]
    public GameObject painelAcoesPrincipal;
    public GameObject painelListaAtaques;

    [Header("Botões do Menu Principal")]
    public Button botaoAtacar;
    public Button botaoHabilidades;
    public Button botaoItens;
    public Button botaoFugir;

    [Header("Botões do Submenu de Ataques")]
    public Button botaoVoltar_Ataques;

    [Header("Prefabs e Parents Dinâmicos")]
    public GameObject attackButtonPrefab;
    public Transform attackButtonParent; 
    private List<GameObject> instantiatedAttackButtons = new List<GameObject>();

    void Start()
    {
        HideAllActionPanels();

        botaoAtacar.onClick.AddListener(OnAtacarButton);
        botaoHabilidades.onClick.AddListener(OnHabilidadesButton);
        botaoItens.onClick.AddListener(OnItensButton);
        botaoFugir.onClick.AddListener(OnFugirButton);

        botaoVoltar_Ataques.onClick.AddListener(OnVoltarButton);
        
    }


    public void ShowMainActionPanel()
    {

        painelAcoesPrincipal.SetActive(true);
        painelListaAtaques.SetActive(false);
    }

    public void HideAllActionPanels()
    {
        painelAcoesPrincipal.SetActive(false);
        painelListaAtaques.SetActive(false);
    }

    public void OnAtacarButton()
    {
        painelAcoesPrincipal.SetActive(false);
        painelListaAtaques.SetActive(true);

        ClearAttackButtons();

        List<AttackData> attacks = battleManager.playerCreature.Attacks;

        foreach (AttackData attack in attacks)
        {
            GameObject buttonGO = Instantiate(attackButtonPrefab, attackButtonParent);
            instantiatedAttackButtons.Add(buttonGO);

            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = attack.attackName;
            }

            Button button = buttonGO.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => {
                    OnAttackSelected(attack);
                });
            }
        }
    }

    public void OnVoltarButton()
    {
        ShowMainActionPanel();
        ClearAttackButtons();
    }

    public void OnAttackSelected(AttackData selectedAttack) 
    {
        HideAllActionPanels();
        ClearAttackButtons();
        
        battleManager.OnPlayerAttack(selectedAttack); 
    }

    void ClearAttackButtons()
    {
        foreach (GameObject button in instantiatedAttackButtons)
        {
            Destroy(button);
        }
        instantiatedAttackButtons.Clear(); 
    }

    public void OnHabilidadesButton() { Debug.Log("Abrir menu de Habilidades..."); }
    public void OnItensButton() { Debug.Log("Abrir menu de Itens..."); }
    public void OnFugirButton() { Debug.Log("Tentando fugir..."); }
}
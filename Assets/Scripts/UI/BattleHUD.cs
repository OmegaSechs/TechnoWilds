using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    // Deixe as referências públicas para que possamos arrastá-las no Inspector
    
    public TextMeshProUGUI nomeTexto;
    public TextMeshProUGUI nivelTexto;
    public Slider hpSlider;
    public Slider spSlider; 

    // Este método é chamado no início da batalha para configurar o HUD
    public void SetHUD(Creature creature)
    {
        
        if (nomeTexto != null)
            nomeTexto.text = creature.Name;

        if (nivelTexto != null)
            nivelTexto.text = "Lv. " + creature.Level;

        if (hpSlider != null)
        {
            hpSlider.maxValue = creature.MaxHP;
            hpSlider.value = creature.CurrentHP;
        }

        if (spSlider != null)
        {
            spSlider.maxValue = creature.MaxSP; 
            spSlider.value = creature.CurrentSP;
        }
    }

    public void SetHP(int hp)
    {
        if (hpSlider != null)
            hpSlider.value = hp;
    }

    public void SetSP(int sp)
    {
        if (spSlider != null)
            spSlider.value = sp;
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nomeTexto;
    public TextMeshProUGUI nivelTexto;
    public Slider hpSlider;

    //Método chamado no início da batalha para configurar o HUD com os dados do BioMon
    public void SetHUD(Creature creature)
    {
        nomeTexto.text = creature.Name;
        nivelTexto.text = "Lv. " + creature.Level;
        
        hpSlider.maxValue = creature.MaxHP; 
        hpSlider.value = creature.CurrentHP; 
    }
    // Método para atualizar a barra de HP durante a batalha    
    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    /*
    // Método para atualizar a barra de SP/Mana no futuro, se fromos fazer 
    public void SetSP(int sp)
    {
        spSlider.value = sp;
    }
    */
}
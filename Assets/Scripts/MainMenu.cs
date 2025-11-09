using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void IniciarJogo()
    {
        SceneManager.LoadScene("Map");
    }

    public void CarregarJogo()
    {
        Debug.Log("Função de carregar jogo ainda não implementada!");
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Saindo do jogo...");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    public void GoToBattle()
    {
        SceneManager.LoadScene("Battle", LoadSceneMode.Single);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
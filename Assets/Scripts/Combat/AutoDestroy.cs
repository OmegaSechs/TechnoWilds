// Arquivo: AutoDestroy.cs (Novo Script)
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Tooltip("Duração da animação em segundos. O objeto será destruído após esse tempo.")]
    public float delay = 1.0f;

    void Start()
    {
        // Destrói o GameObject (que contém esta animação) após 'delay' segundos.
        Destroy(gameObject, delay);
    }
}
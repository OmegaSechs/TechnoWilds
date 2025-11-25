using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Tooltip("Duração da animação em segundos. O objeto será destruído após esse tempo.")]
    public float delay = 1.0f;

    void Start()
    {
        Destroy(gameObject, delay);
    }
}
using System.Collections;
using UnityEngine;

public class FeedbackDeDano : MonoBehaviour
{
    [Header("Configuração do Feedback")]
    [SerializeField] private float duracaoShake = 0.15f;
    [SerializeField] private float intensidadeShake = 0.1f;
    [SerializeField] private float duracaoFlash = 0.15f;

    [Header("Configuração do Flash")]
    [Tooltip("Arraste o material 'M_SpriteFlash_White' para cá")]
    [SerializeField] private Material flashMaterial;

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Vector3 posicaoOriginal;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.material;
        }
    }

    public void IniciarFeedbackDano()
    {
        posicaoOriginal = transform.localPosition; 
        
        StartCoroutine(FlashCoroutine());
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        if (spriteRenderer == null || flashMaterial == null) 
            yield break;

        spriteRenderer.material = flashMaterial;

        yield return new WaitForSeconds(duracaoFlash);

        spriteRenderer.material = originalMaterial;
    }
    private IEnumerator ShakeCoroutine()
    {
        float tempoDecorrido = 0f;

        while (tempoDecorrido < duracaoShake)
        {
            Vector2 offset = Random.insideUnitCircle * intensidadeShake;

            transform.localPosition = posicaoOriginal + new Vector3(offset.x, offset.y, 0);

            tempoDecorrido += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = posicaoOriginal;
    }
}
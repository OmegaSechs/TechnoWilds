using System.Collections;
using UnityEngine;

public class FeedbackDeDano : MonoBehaviour
{
    // --- Variáveis para o Feedback (Ajustáveis no Inspector) ---
    [Header("Configuração do Feedback")]
    [SerializeField] private float duracaoShake = 0.15f;
    [SerializeField] private float intensidadeShake = 0.1f;
    [SerializeField] private float duracaoFlash = 0.15f;

    // --- MUDANÇA (Passo 2) ---
    [Header("Configuração do Flash")]
    [Tooltip("Arraste o material 'M_SpriteFlash_White' para cá")]
    [SerializeField] private Material flashMaterial;
    // --- FIM DA MUDANÇA ---

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial; // --- MUDANÇA: Vamos salvar o material, não a cor
    private Vector3 posicaoOriginal;

    // --- Setup Inicial ---
    void Awake()
    {
        // Pega o SpriteRenderer deste objeto
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // --- MUDANÇA: Salva o material original ---
            originalMaterial = spriteRenderer.material;
        }
    }

    // --- Função Pública para Iniciar o Efeito ---
    public void IniciarFeedbackDano()
    {
        // Salva a posição atual *antes* de tremer
        posicaoOriginal = transform.localPosition; 
        
        // Inicia as duas corrotinas (flash e shake) ao mesmo tempo
        StartCoroutine(FlashCoroutine());
        StartCoroutine(ShakeCoroutine());
    }

    // --- Corrotinas (O Efeito em si) ---

    /// <summary>
    /// Corrotina que faz o sprite piscar em branco (trocando o material).
    /// </summary>
    private IEnumerator FlashCoroutine()
    {
        // --- MUDANÇA: Verifica se temos o material de flash ---
        if (spriteRenderer == null || flashMaterial == null) 
            yield break; // Se não houver sprite ou material de flash, não faz nada

        // 1. Muda o material para o material de FLASH
        spriteRenderer.material = flashMaterial;

        // 2. Espera o tempo definido
        yield return new WaitForSeconds(duracaoFlash);

        // 3. Volta para o material original
        spriteRenderer.material = originalMaterial;
    }

    /// <summary>
    /// Corrotina que faz o sprite tremer.
    /// </summary>
    private IEnumerator ShakeCoroutine()
    {
        float tempoDecorrido = 0f;

        while (tempoDecorrido < duracaoShake)
        {
            // Gera um deslocamento aleatório
            Vector2 offset = Random.insideUnitCircle * intensidadeShake;

            // Aplica o deslocamento à posição original
            transform.localPosition = posicaoOriginal + new Vector3(offset.x, offset.y, 0);

            // Atualiza o tempo e espera o próximo frame
            tempoDecorrido += Time.deltaTime;
            yield return null; // Espera 1 frame
        }

        // 4. Garante que, ao final, ele volte exatamente para a posição original
        transform.localPosition = posicaoOriginal;
    }
}
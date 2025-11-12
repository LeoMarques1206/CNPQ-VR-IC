using UnityEngine;

public class IdleBreathing : MonoBehaviour
{
    [Header("Configuração da Respiração")]
    [Tooltip("Amplitude do movimento (0.001–0.05 é ideal)")]
    public float amplitude = 0.02f;
    [Tooltip("Velocidade da respiração (ciclos por segundo)")]
    public float frequency = 1.0f;
    [Tooltip("Define se o movimento é em escala ou posição vertical")]
    public bool useScale = false;

    [Header("Configuração do Perfil de Saúde")]
    [Tooltip("Fator de aumento da largura (X e Z) para o perfil obeso (1.0 = normal, >1.0 = maior)")]
    public float obeseScaleMultiplier = 1.15f;

    private Vector3 initialPosition;
    private Vector3 initialScale;
    private float offset;

    void Start()
    {
        // --- Guarda a posição e escala originais ---
        initialPosition = transform.localPosition;
        initialScale = transform.localScale;
        offset = Random.Range(0f, Mathf.PI * 2f); // respiração fora de sincronia

        // --- Recupera o perfil salvo no PlayerPrefs ---
        string healthProfile = PlayerPrefs.GetString("HealthName", "Padrão").ToLowerInvariant();

        // --- Ajusta apenas a largura (X e Z) se for obeso ---
        if (healthProfile.Contains("obeso") || healthProfile.Contains("obesa"))
        {
            transform.localScale = new Vector3(
                initialScale.x * obeseScaleMultiplier, // aumenta largura
                initialScale.y,                        // mantém altura
                initialScale.z * obeseScaleMultiplier   // aumenta profundidade
            );

            initialScale = transform.localScale; // redefine base da respiração
            Debug.Log($"🧍‍♂️ Escala ajustada para perfil obeso (largura +{(obeseScaleMultiplier - 1f) * 100f:0}%)");
        }
        else
        {
            Debug.Log($"🧍‍♀️ Escala normal aplicada para perfil '{healthProfile}'.");
        }
    }

    void Update()
    {
        float breathe = Mathf.Sin(Time.time * frequency * 2f * Mathf.PI + offset) * amplitude;

        if (useScale)
        {
            // Respiração expandindo levemente o tórax (X e Z)
            transform.localScale = new Vector3(
                initialScale.x + breathe,
                initialScale.y,
                initialScale.z + breathe
            );
        }
        else
        {
            // Respiração com leve movimento vertical
            transform.localPosition = new Vector3(
                initialPosition.x,
                initialPosition.y + breathe,
                initialPosition.z
            );
        }
    }
}

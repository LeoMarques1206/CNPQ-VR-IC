using UnityEngine;
using UnityEngine.UI;

public class FadeOutOnStart : MonoBehaviour
{
    [Header("Referência da Imagem")]
    public Image imageToFade;

    [Header("Configuração do Fade")]
    [Tooltip("Duração do fade em segundos")]
    public float fadeDuration = 2f;
    [Tooltip("Atraso antes de começar o fade")]
    public float startDelay = 0.5f;

    private Color initialColor;

    void Start()
    {
        if (imageToFade == null)
        {
            Debug.LogWarning("⚠️ Nenhuma imagem atribuída no FadeOutOnStart!");
            return;
        }

        initialColor = imageToFade.color;
        imageToFade.enabled = true;
        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(startDelay);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            imageToFade.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        imageToFade.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        imageToFade.enabled = false; // opcional, desativa a imagem após o fade
    }
}

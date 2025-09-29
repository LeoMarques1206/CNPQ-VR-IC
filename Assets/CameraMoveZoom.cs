using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraMoveZoom : MonoBehaviour
{
    [Header("Configuração do Movimento")]
    public Transform targetPoint; 
    public float duration = 2f;
    public float delay = 10f; 

    [Header("Configuração do Fade")]
    public Image fadeImage; 
    public float fadeDuration = 2f;

    private Vector3 startPos;
    private float elapsedTime = 0f;
    private bool moving = false;

    void Start()
    {
        startPos = transform.position;
        StartCoroutine(FadeOutEIniciar());
    }

    IEnumerator FadeOutEIniciar()
    {
        if (fadeImage != null)
        {
          
            fadeImage.color = new Color(0, 0, 0, 1);

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(1f, 0f, t));
                yield return null;
            }

          
            fadeImage.color = new Color(0, 0, 0, 0);
        }

      
        yield return new WaitForSeconds(delay);
        moving = true;
    }

    void Update()
    {
        if (moving && targetPoint != null)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, targetPoint.position, t);

            if (t >= 1f) moving = false;
        }
    }
}

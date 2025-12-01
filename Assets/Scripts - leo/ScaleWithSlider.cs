using UnityEngine;
using UnityEngine.UI;

public class ScaleWithSlider : MonoBehaviour
{
    [Header("Slider que controla a escala")]
    public Slider scaleSlider;

    [Header("Objetos que serão escalados")]
    public Transform[] scalableObjects;

    [Header("Multiplicação da escala")]
    public float scaleMultiplier = 1.0f;

    // Escalas originais
    private Vector3[] originalScales;

    void Start()
    {
        if (scaleSlider == null)
        {
            Debug.LogError("ScaleWithSlider: É necessário atribuir um Slider.");
            return;
        }

        // Salva a escala original de todos os objetos
        originalScales = new Vector3[scalableObjects.Length];
        for (int i = 0; i < scalableObjects.Length; i++)
        {
            if (scalableObjects[i] != null)
                originalScales[i] = scalableObjects[i].localScale;
        }

        // Toda vez que o slider mudar → chama UpdateScale
        scaleSlider.onValueChanged.AddListener(UpdateScale);
    }

    void UpdateScale(float value)
    {
        for (int i = 0; i < scalableObjects.Length; i++)
        {
            if (scalableObjects[i] != null)
            {
                scalableObjects[i].localScale =
                    originalScales[i] * (1 + value * scaleMultiplier);
            }
        }
    }
}

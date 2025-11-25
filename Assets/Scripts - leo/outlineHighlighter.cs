using UnityEngine;

[DisallowMultipleComponent]
public class OutlineHighlighter : MonoBehaviour
{
    private Renderer[] renderers;
    private Material outlineMaterial;

    public Color outlineColor = Color.cyan;
    public float outlineWidth = 4f;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        // Shader universal que existe em todos os pipelines
        outlineMaterial = new Material(Shader.Find("UI/Unlit/Transparent"));
        outlineMaterial.SetColor("_Color", outlineColor);
    }

    public void EnableOutline()
    {
        foreach (var rend in renderers)
        {
            var mats = rend.materials;
            System.Array.Resize(ref mats, mats.Length + 1);

            mats[mats.Length - 1] = outlineMaterial;

            rend.materials = mats;
        }
    }

    public void DisableOutline()
    {
        foreach (var rend in renderers)
        {
            var mats = rend.materials;

            if (mats.Length <= 1) continue;

            System.Array.Resize(ref mats, mats.Length - 1);

            rend.materials = mats;
        }
    }
}

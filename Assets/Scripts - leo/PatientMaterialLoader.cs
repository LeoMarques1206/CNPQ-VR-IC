using UnityEngine;

public class PatientMaterialLoader : MonoBehaviour
{
    [Header("Renderer do corpo do personagem")]
    public MeshRenderer bodyRenderer;

    [Header("Lista de materiais disponíveis (mesmos usados no Customizer)")]
    public Material[] availableSkins;

    void Start()
    {
        string savedMaterialName = PlayerPrefs.GetString("SkinName", "");

        if (string.IsNullOrEmpty(savedMaterialName))
        {
            Debug.LogWarning("⚠️ Nenhum material de pele salvo. Usando padrão.");
            return;
        }

        // Procura o material correspondente pelo nome
        Material foundMat = null;
        foreach (var mat in availableSkins)
        {
            if (mat.name == savedMaterialName)
            {
                foundMat = mat;
                break;
            }
        }

        if (foundMat != null && bodyRenderer != null)
        {
            var mats = bodyRenderer.sharedMaterials;
            if (mats.Length > 0)
            {
                mats[0] = foundMat;
                bodyRenderer.sharedMaterials = mats;
                Debug.Log($"✅ Material '{foundMat.name}' aplicado no personagem!");
            }
        }
        else
        {
            Debug.LogWarning($"❌ Material '{savedMaterialName}' não encontrado na lista disponível!");
        }
    }
}

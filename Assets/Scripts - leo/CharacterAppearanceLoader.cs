using UnityEngine;

public class CharacterAppearanceLoader : MonoBehaviour
{
    [Tooltip("Renderer do corpo para aplicar a pele")]
    public MeshRenderer bodyRenderer;
    [Tooltip("Materiais possíveis de pele (na mesma ordem do customizador)")]
    public Material[] skinMaterials;

    void Start()
    {
        LoadCustomization();
    }

    void LoadCustomization()
    {
            int skinIndex = PlayerPrefs.GetInt("SkinIndex", 0);
            int healthIndex = PlayerPrefs.GetInt("HealthIndex", 0);
            int behaviorIndex = PlayerPrefs.GetInt("BehaviorIndex", 0);
            int ageIndex = PlayerPrefs.GetInt("AgeIndex", 0);
            int childrenIndex = PlayerPrefs.GetInt("ChildrenIndex", 0);

            Debug.Log($"🧬 Carregando Personagem: Skin={skinIndex}, Saúde={healthIndex}, Comportamento={behaviorIndex}, Idade={ageIndex}, Filhos={childrenIndex}");

            // aplicar apenas o material da pele
            if (bodyRenderer && skinMaterials.Length > 0)
            {
                int i = Mathf.Clamp(skinIndex, 0, skinMaterials.Length - 1);
                var mats = bodyRenderer.sharedMaterials;
                mats[0] = skinMaterials[i];
                bodyRenderer.sharedMaterials = mats;
            }

            // Aqui pode usar healthIndex, ageIndex, etc. para alterar animações,
            // parâmetros de comportamento, aparência física etc.
    }
}

using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class VRCharacterCustomizer : MonoBehaviour
{
    [Header("Referências do Modelo")]
    [Tooltip("Renderer da pele do personagem (SkinnedMeshRenderer do corpo).")]
    public SkinnedMeshRenderer bodyRenderer;

    [Header("UI (TextMeshPro)")]
    public TMP_Text skinLabel;
    public TMP_Text healthLabel;
    public TMP_Text behaviorLabel;

    [System.Serializable]
    public class OptionGroup
    {
        [Tooltip("Nome exibido nas opções (ex.: Branco, Pardo, Negro).")]
        public string[] display;
        [Tooltip("Índice atual desse grupo.")]
        public int index;
    }

    [Header("Opções")]
    [Tooltip("Materiais de pele que casam com 'skin.display'.")]
    public Material[] skinMaterials;          // tamanho 3
    public OptionGroup skin;                  // skin.display tamanho 3
    public OptionGroup healthProfile;         // ex.: Padrão, Hipertensa, Gestante
    public OptionGroup behavior;              // ex.: Calma, Ansiosa, Agressiva

    [Header("Eventos (opcional)")]
    public UnityEvent<int, string> onSkinChanged;
    public UnityEvent<int, string> onHealthChanged;
    public UnityEvent<int, string> onBehaviorChanged;

    void Start()
    {
        // Garante que começamos mostrando o estado atual
        RefreshAll();
    }

    #region Botões (ligue nos OnClick das setas)
    public void NextSkin()     { Step(ref skin.index, skin.display.Length, +1);  ApplySkin(); }
    public void PrevSkin()     { Step(ref skin.index, skin.display.Length, -1);  ApplySkin(); }

    public void NextHealth()   { Step(ref healthProfile.index, healthProfile.display.Length, +1);  ApplyHealth(); }
    public void PrevHealth()   { Step(ref healthProfile.index, healthProfile.display.Length, -1);  ApplyHealth(); }

    public void NextBehavior() { Step(ref behavior.index, behavior.display.Length, +1);  ApplyBehavior(); }
    public void PrevBehavior() { Step(ref behavior.index, behavior.display.Length, -1);  ApplyBehavior(); }
    #endregion

    void Step(ref int index, int length, int dir)
    {
        if (length == 0) return;
        index = (index + dir) % length;
        if (index < 0) index += length;
    }

    void ApplySkin()
    {
        if (skinLabel) skinLabel.text = skin.display[Mathf.Clamp(skin.index, 0, skin.display.Length - 1)];
        if (bodyRenderer && skinMaterials != null && skinMaterials.Length > 0)
        {
            int i = Mathf.Clamp(skin.index, 0, skinMaterials.Length - 1);
            var mats = bodyRenderer.sharedMaterials;
            // assume que o 1º material é a pele; ajuste o slot conforme seu mesh
            if (mats.Length > 0) { mats[0] = skinMaterials[i]; bodyRenderer.sharedMaterials = mats; }
        }
        onSkinChanged?.Invoke(skin.index, skin.display[skin.index]);
    }

    void ApplyHealth()
    {
        if (healthLabel) healthLabel.text = healthProfile.display[Mathf.Clamp(healthProfile.index, 0, healthProfile.display.Length - 1)];
        onHealthChanged?.Invoke(healthProfile.index, healthProfile.display[healthProfile.index]);
    }

    void ApplyBehavior()
    {
        if (behaviorLabel) behaviorLabel.text = behavior.display[Mathf.Clamp(behavior.index, 0, behavior.display.Length - 1)];
        onBehaviorChanged?.Invoke(behavior.index, behavior.display[behavior.index]);
    }

    public void RefreshAll()
    {
        ApplySkin();
        ApplyHealth();
        ApplyBehavior();
    }

    // Helpers para pegar o estado atual em outros scripts
    public int    GetSkinIndex()     => skin.index;
    public string GetSkinName()      => skin.display[skin.index];
    public int    GetHealthIndex()   => healthProfile.index;
    public string GetHealthName()    => healthProfile.display[healthProfile.index];
    public int    GetBehaviorIndex() => behavior.index;
    public string GetBehaviorName()  => behavior.display[behavior.index];
}

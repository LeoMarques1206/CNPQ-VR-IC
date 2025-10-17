using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class VRCharacterCustomizer : MonoBehaviour
{
    [Header("Referências do Modelo")]
    [Tooltip("Renderer da pele do personagem (MeshRenderer do corpo).")]
    public MeshRenderer bodyRenderer;

    [Header("UI (TextMeshPro)")]
    public TMP_Text skinLabel;
    public TMP_Text healthLabel;
    public TMP_Text behaviorLabel;
    public TMP_Text ageLabel;
    public TMP_Text childrenLabel;

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
    public Material[] skinMaterials;
    public OptionGroup skin;
    public OptionGroup healthProfile;
    public OptionGroup behavior;
    public OptionGroup ageRange;
    public OptionGroup children;

    [Header("Eventos (opcional)")]
    public UnityEvent<int, string> onSkinChanged;
    public UnityEvent<int, string> onHealthChanged;
    public UnityEvent<int, string> onBehaviorChanged;
    public UnityEvent<int, string> onAgeChanged;
    public UnityEvent<int, string> onChildrenChanged;
    public UnityEvent onSaveCompleted; // opcional: evento disparado após salvar

    void Start()
    {
        RefreshAll();
    }

    #region Botões (ligue nos OnClick das setas)
    public void NextSkin()     { Step(ref skin.index, skin.display.Length, +1); ApplySkin(); }
    public void PrevSkin()     { Step(ref skin.index, skin.display.Length, -1); ApplySkin(); }

    public void NextHealth()   { Step(ref healthProfile.index, healthProfile.display.Length, +1); ApplyHealth(); }
    public void PrevHealth()   { Step(ref healthProfile.index, healthProfile.display.Length, -1); ApplyHealth(); }

    public void NextBehavior() { Step(ref behavior.index, behavior.display.Length, +1); ApplyBehavior(); }
    public void PrevBehavior() { Step(ref behavior.index, behavior.display.Length, -1); ApplyBehavior(); }

    public void NextAge()      { Step(ref ageRange.index, ageRange.display.Length, +1); ApplyAge(); }
    public void PrevAge()      { Step(ref ageRange.index, ageRange.display.Length, -1); ApplyAge(); }

    public void NextChildren() { Step(ref children.index, children.display.Length, +1); ApplyChildren(); }
    public void PrevChildren() { Step(ref children.index, children.display.Length, -1); ApplyChildren(); }
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
            if (mats.Length > 0)
            {
                mats[0] = skinMaterials[i];
                bodyRenderer.sharedMaterials = mats;
            }
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

    void ApplyAge()
    {
        if (ageLabel) ageLabel.text = ageRange.display[Mathf.Clamp(ageRange.index, 0, ageRange.display.Length - 1)];
        onAgeChanged?.Invoke(ageRange.index, ageRange.display[ageRange.index]);
    }

    void ApplyChildren()
    {
        if (childrenLabel) childrenLabel.text = children.display[Mathf.Clamp(children.index, 0, children.display.Length - 1)];
        onChildrenChanged?.Invoke(children.index, children.display[children.index]);
    }

    public void RefreshAll()
    {
        ApplySkin();
        ApplyHealth();
        ApplyBehavior();
        ApplyAge();
        ApplyChildren();
    }

    // ✅ BOTÃO SALVAR
    public void SaveSelection()
    {
        PlayerPrefs.SetInt("SkinIndex", skin.index);
        PlayerPrefs.SetInt("HealthIndex", healthProfile.index);
        PlayerPrefs.SetInt("BehaviorIndex", behavior.index);
        PlayerPrefs.SetInt("AgeIndex", ageRange.index);
        PlayerPrefs.SetInt("ChildrenIndex", children.index);
        PlayerPrefs.Save();

        Debug.Log("⚙️ Preferências de personagem salvas!");
        onSaveCompleted?.Invoke();
    }

    // Helpers para pegar o estado atual
    public int GetSkinIndex() => skin.index;
    public string GetSkinName() => skin.display[skin.index];
    public int GetHealthIndex() => healthProfile.index;
    public string GetHealthName() => healthProfile.display[healthProfile.index];
    public int GetBehaviorIndex() => behavior.index;
    public string GetBehaviorName() => behavior.display[behavior.index];
    public int GetAgeIndex() => ageRange.index;
    public string GetAgeName() => ageRange.display[ageRange.index];
    public int GetChildrenIndex() => children.index;
    public string GetChildrenName() => children.display[children.index];
}

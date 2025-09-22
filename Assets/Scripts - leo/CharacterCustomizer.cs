using UnityEngine;
using TMPro;  // se usar TextMeshPro para rótulos

public class CharacterCustomizer : MonoBehaviour
{
    [Header("Modelos estéticos (3 prefabs)")]
    public GameObject[] modelPrefabs; // arraste os 3 prefabs aqui, na ordem

    [Header("Anchors/Parents")]
    public Transform previewAnchor; // onde o modelo fica na tela de seleção

    [Header("Opções (apenas strings por enquanto)")]
    public string[] behaviorOptions = { "Calma", "Ansiosa", "Confiante" };
    public string[] healthOptions    = { "Padrão", "Atleta", "Sensível" };

    [Header("UI (opcional)")]
    public TMP_Text aestheticLabel;
    public TMP_Text behaviorLabel;
    public TMP_Text healthLabel;

    private int currentModelIndex = 0;
    private int currentBehaviorIndex = 0;
    private int currentHealthIndex = 0;

    private GameObject currentPreview;

    void Start()
    {
        // Inicia com o que estiver salvo (ou default)
        currentModelIndex    = Mathf.Clamp(SelectedCharacterState.ModelIndex, 0, modelPrefabs.Length - 1);
        currentBehaviorIndex = Mathf.Clamp(IndexOf(behaviorOptions, SelectedCharacterState.Behavior), 0, behaviorOptions.Length - 1);
        currentHealthIndex   = Mathf.Clamp(IndexOf(healthOptions, SelectedCharacterState.HealthProfile), 0, healthOptions.Length - 1);

        SpawnPreview();
        RefreshLabels();
    }

    // Troca de modelo (estético)
    public void NextModel()
    {
        currentModelIndex = (currentModelIndex + 1) % modelPrefabs.Length;
        SelectedCharacterState.ModelIndex = currentModelIndex;
        SpawnPreview();
        RefreshLabels();
    }

    public void PrevModel()
    {
        currentModelIndex = (currentModelIndex - 1 + modelPrefabs.Length) % modelPrefabs.Length;
        SelectedCharacterState.ModelIndex = currentModelIndex;
        SpawnPreview();
        RefreshLabels();
    }

    // Troca de comportamento
    public void NextBehavior()
    {
        currentBehaviorIndex = (currentBehaviorIndex + 1) % behaviorOptions.Length;
        SelectedCharacterState.Behavior = behaviorOptions[currentBehaviorIndex];
        RefreshLabels();
    }

    public void PrevBehavior()
    {
        currentBehaviorIndex = (currentBehaviorIndex - 1 + behaviorOptions.Length) % behaviorOptions.Length;
        SelectedCharacterState.Behavior = behaviorOptions[currentBehaviorIndex];
        RefreshLabels();
    }

    // Troca de perfil de saúde
    public void NextHealth()
    {
        currentHealthIndex = (currentHealthIndex + 1) % healthOptions.Length;
        SelectedCharacterState.HealthProfile = healthOptions[currentHealthIndex];
        RefreshLabels();
    }

    public void PrevHealth()
    {
        currentHealthIndex = (currentHealthIndex - 1 + healthOptions.Length) % healthOptions.Length;
        SelectedCharacterState.HealthProfile = healthOptions[currentHealthIndex];
        RefreshLabels();
    }

    // Randomizar tudo
    public void RandomizeAll()
    {
        currentModelIndex    = Random.Range(0, modelPrefabs.Length);
        currentBehaviorIndex = Random.Range(0, behaviorOptions.Length);
        currentHealthIndex   = Random.Range(0, healthOptions.Length);

        SelectedCharacterState.ModelIndex    = currentModelIndex;
        SelectedCharacterState.Behavior      = behaviorOptions[currentBehaviorIndex];
        SelectedCharacterState.HealthProfile = healthOptions[currentHealthIndex];

        SpawnPreview();
        RefreshLabels();
    }

    // Confirmar (geralmente trocar de cena)
    public void Confirm() //CHAMAR A CENA DO CONSULTORIO!!!
    {
        // Aqui normalmente você chamaria SceneManager.LoadScene("Gameplay");
        // A seleção já está salva em PlayerPrefs.
    }

    // ---------- Helpers ----------
    private void SpawnPreview()
    {
        if (currentPreview != null) Destroy(currentPreview);
        if (modelPrefabs == null || modelPrefabs.Length == 0) return;

        currentPreview = Instantiate(modelPrefabs[currentModelIndex], previewAnchor);
        currentPreview.transform.localPosition = Vector3.zero;
        currentPreview.transform.localRotation = Quaternion.identity;
        currentPreview.transform.localScale = Vector3.one;
        // Pode desligar scripts/AI no preview:
        // foreach(var c in currentPreview.GetComponentsInChildren<MonoBehaviour>()) c.enabled = false;
    }

    private void RefreshLabels()
    {
        if (aestheticLabel) aestheticLabel.text = $"Modelo {currentModelIndex + 1}";
        if (behaviorLabel)  behaviorLabel.text  = behaviorOptions[currentBehaviorIndex];
        if (healthLabel)    healthLabel.text    = healthOptions[currentHealthIndex];
    }

    private int IndexOf(string[] arr, string val)
    {
        for (int i = 0; i < arr.Length; i++)
            if (arr[i] == val) return i;
        return 0; // default
    }
}
using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
class DialogueLine
{
    public string speaker;
    [TextArea(2, 5)] public string text;
}

[System.Serializable]
class BehaviorDialogue
{
    public string type;
    public DialogueLine[] lines;
}

[System.Serializable]
class DialogueData
{
    public BehaviorDialogue[] behaviors;
}

public class DialogueManagerJSON : MonoBehaviour
{
    [Header("UI Referências")]
    public TMP_Text speakerText;
    public TMP_Text dialogueText;

    [Header("Configuração")]
    [Tooltip("Comportamento da paciente (Calma, Ansiosa, Agressiva, etc.)")]
    public string selectedBehavior = "Calma";
    [Tooltip("Tempo em segundos que cada fala fica na tela")]
    public float lineDelay = 2.5f;
    [Tooltip("Nome do arquivo JSON (sem extensão) na pasta Resources")]
    public string jsonFileName = "dialogue_data";

    private DialogueData dialogueData;
    private BehaviorDialogue currentDialogue;

    void Start()
    {
        LoadDialogueData();

        string savedBehavior = PlayerPrefs.GetString("BehaviorName", selectedBehavior);
        SetBehavior(savedBehavior);

        StartCoroutine(PlayDialogue());
    }

    void LoadDialogueData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);
        if (jsonFile == null)
        {
            Debug.LogError($"❌ Arquivo JSON '{jsonFileName}.json' não encontrado em Assets/Resources/");
            return;
        }

        dialogueData = JsonUtility.FromJson<DialogueData>(jsonFile.text);
        if (dialogueData == null)
            Debug.LogError("❌ Erro ao converter o arquivo JSON para DialogueData.");
    }

    public void SetBehavior(string behaviorType)
    {
        if (dialogueData == null || dialogueData.behaviors == null)
        {
            Debug.LogWarning("⚠️ Nenhum diálogo carregado ainda.");
            return;
        }

        foreach (var b in dialogueData.behaviors)
        {
            if (b.type == behaviorType)
            {
                currentDialogue = b;
                Debug.Log($"🧠 Carregado diálogo para comportamento: {behaviorType}");
                return;
            }
        }

        Debug.LogWarning($"⚠️ Nenhum diálogo encontrado para comportamento '{behaviorType}'. Usando padrão.");
        currentDialogue = dialogueData.behaviors.Length > 0 ? dialogueData.behaviors[0] : null;
    }

    IEnumerator PlayDialogue()
    {
        if (currentDialogue == null || currentDialogue.lines == null)
        {
            Debug.LogWarning("⚠️ Nenhum diálogo válido para exibir.");
            yield break;
        }

        foreach (var line in currentDialogue.lines)
        {
            speakerText.text = line.speaker;
            dialogueText.text = line.text;
            yield return new WaitForSeconds(lineDelay);
        }

        Debug.Log("🎬 Diálogo encerrado!");
    }
}

using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Necessário para RawImage

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    [TextArea(2, 5)] public string text;
}

[System.Serializable]
public class TipoDialogue
{
    public string tipo;
    public DialogueLine[] linhas;
}

[System.Serializable]
public class ProblemaDialogue
{
    public string problema;
    public TipoDialogue[] tipos;
}

[System.Serializable]
public class DialogueRoot
{
    public ProblemaDialogue[] conversas;
}

public class DialogueManagerJSON : MonoBehaviour
{
    [Header("UI Referências")]
    [Tooltip("Campo de texto onde o diálogo será exibido.")]
    public TMP_Text dialogueText;

    [Tooltip("Imagem de exibição do personagem atual (usando RenderTexture).")]
    public RawImage characterImage;

    [Header("RenderTextures dos personagens")]
    [Tooltip("RenderTexture usada pela câmera da doutora.")]
    public RenderTexture doctorRenderTexture;
    [Tooltip("RenderTexture usada pela câmera da paciente.")]
    public RenderTexture patientRenderTexture;

    [Header("Configuração")]
    [Tooltip("Tempo em segundos que cada fala fica na tela")]
    public float lineDelay = 2.5f;
    [Tooltip("Nome do arquivo JSON (sem extensão) na pasta Resources")]
    public string jsonFileName = "dialogue_data";

    private DialogueRoot dialogueRoot;
    private TipoDialogue currentDialogue;

    void Start()
    {
        LoadDialogueData();

        // ✅ Recupera automaticamente os valores salvos pelo VRCharacterCustomizer
        string savedProblem = PlayerPrefs.GetString("ProblemName", "Incontinência Urinária");
        string savedType = PlayerPrefs.GetString("BehaviorName", "Calma");

        Debug.Log($"🧩 Carregando diálogo: Problema='{savedProblem}', Tipo='{savedType}'");

        SetConversation(savedProblem, savedType);
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

        dialogueRoot = JsonUtility.FromJson<DialogueRoot>(jsonFile.text);
        if (dialogueRoot == null)
            Debug.LogError("❌ Erro ao converter o arquivo JSON para DialogueRoot.");
    }

    public void SetConversation(string problem, string type)
    {
        if (dialogueRoot == null || dialogueRoot.conversas == null)
        {
            Debug.LogWarning("⚠️ Nenhum diálogo carregado ainda.");
            return;
        }

        // 🔍 Encontra o problema correspondente
        ProblemaDialogue foundProblem = null;
        foreach (var p in dialogueRoot.conversas)
        {
            if (p.problema == problem)
            {
                foundProblem = p;
                break;
            }
        }

        if (foundProblem == null)
        {
            Debug.LogWarning($"⚠️ Problema '{problem}' não encontrado no JSON. Usando o primeiro disponível.");
            foundProblem = dialogueRoot.conversas.Length > 0 ? dialogueRoot.conversas[0] : null;
        }

        // 🔍 Encontra o tipo dentro do problema
        if (foundProblem != null && foundProblem.tipos != null)
        {
            foreach (var t in foundProblem.tipos)
            {
                if (t.tipo == type)
                {
                    currentDialogue = t;
                    Debug.Log($"🧠 Carregado diálogo para '{problem}' ({type})");
                    return;
                }
            }

            // Se não achar o tipo, usa o primeiro
            currentDialogue = foundProblem.tipos.Length > 0 ? foundProblem.tipos[0] : null;
            Debug.LogWarning($"⚠️ Tipo '{type}' não encontrado em '{problem}'. Usando tipo padrão.");
        }
    }

    IEnumerator PlayDialogue()
    {
        if (currentDialogue == null || currentDialogue.linhas == null)
        {
            Debug.LogWarning("⚠️ Nenhum diálogo válido para exibir.");
            yield break;
        }

        foreach (var line in currentDialogue.linhas)
        {
            // 🧍‍♀️ Alterna RenderTexture conforme quem fala
            if (characterImage != null)
            {
                if (line.speaker.ToLower().Contains("doutora"))
                {
                    characterImage.texture = doctorRenderTexture;
                }
                else if (line.speaker.ToLower().Contains("paciente"))
                {
                    characterImage.texture = patientRenderTexture;
                }
            }

            // Atualiza texto da fala
            dialogueText.text = line.text;

            yield return new WaitForSeconds(lineDelay);
        }

        Debug.Log("🎬 Diálogo encerrado!");
    }
}

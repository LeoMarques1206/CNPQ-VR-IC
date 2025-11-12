using System.Collections;
using System.Globalization;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable] public class DialogueLine { public string speaker; [TextArea(2, 5)] public string text; }
[System.Serializable] public class TipoDialogue { public string tipo; public DialogueLine[] linhas; }
[System.Serializable] public class ProblemaDialogue { public string problema; public TipoDialogue[] tipos; }
[System.Serializable] public class DialogueRoot { public ProblemaDialogue[] conversas; }

public class DialogueManagerJSON : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public RawImage characterImage;

    [Header("RenderTextures")]
    public RenderTexture doctorRenderTexture;
    public RenderTexture patientRenderTexture;

    [Header("Transição de Cena")]
    public CanvasGroup fadeCanvas;     // CanvasGroup preto cobrindo a tela
    public float fadeDuration = 1.5f;  // Duração do fade
    public string nextSceneName = "MainScene";

    [Header("Configuração")]
    public string jsonFileName = "dialogue_data";

    private DialogueRoot dialogueRoot;
    private TipoDialogue currentDialogue;
    private int currentLineIndex = 0;
    private bool dialogueActive = false;

    void Start()
    {
        LoadDialogueData();

        string savedProblem = PlayerPrefs.GetString("ProblemName", "Incontinência Urinária");
        string savedType = PlayerPrefs.GetString("BehaviorName", "Calma");

        Debug.Log($"🧩 Carregando diálogo: {savedProblem} ({savedType})");

        SetConversation(savedProblem, savedType);

        // Garante que o fade começa visível (tela preta)
        if (fadeCanvas != null) fadeCanvas.alpha = 1f;

        // Faz o fade-out no início da cena
        StartCoroutine(FadeOutStart());

        StartDialogue();
    }

    void Update()
    {
        if (!dialogueActive) return;

        if (Input.GetMouseButtonDown(0) ||
            OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            NextLine();
        }
    }

    IEnumerator FadeOutStart()
    {
        // Espera um pequeno delay antes de iniciar o fade (opcional)
        yield return new WaitForSeconds(0.3f);

        if (fadeCanvas != null)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadeCanvas.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
                yield return null;
            }

            fadeCanvas.alpha = 0f;
        }
    }

    void LoadDialogueData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);
        if (jsonFile == null)
        {
            Debug.LogError($"❌ JSON '{jsonFileName}.json' não encontrado em Assets/Resources/");
            return;
        }

        dialogueRoot = JsonUtility.FromJson<DialogueRoot>(jsonFile.text);
        if (dialogueRoot == null)
            Debug.LogError("❌ Erro ao converter JSON para DialogueRoot.");
    }

    public void SetConversation(string problem, string type)
    {
        if (dialogueRoot?.conversas == null || dialogueRoot.conversas.Length == 0)
        {
            Debug.LogWarning("⚠️ Nenhum diálogo carregado.");
            return;
        }

        ProblemaDialogue foundProblem = null;
        foreach (var p in dialogueRoot.conversas)
        {
            if (p != null && string.Equals(p.problema, problem, System.StringComparison.OrdinalIgnoreCase))
            {
                foundProblem = p;
                break;
            }
        }

        if (foundProblem == null)
        {
            Debug.LogWarning($"⚠️ Problema '{problem}' não encontrado. Usando o primeiro.");
            foundProblem = dialogueRoot.conversas[0];
        }

        foreach (var t in foundProblem.tipos)
        {
            if (t != null && string.Equals(t.tipo, type, System.StringComparison.OrdinalIgnoreCase))
            {
                currentDialogue = t;
                return;
            }
        }

        currentDialogue = foundProblem.tipos.Length > 0 ? foundProblem.tipos[0] : null;
    }

    public void StartDialogue()
    {
        if (currentDialogue == null || currentDialogue.linhas == null || currentDialogue.linhas.Length == 0)
        {
            Debug.LogWarning("⚠️ Nenhum diálogo válido para exibir.");
            return;
        }

        dialogueActive = true;
        currentLineIndex = 0;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        ShowCurrentLine();
    }

    void ShowCurrentLine()
    {
        if (currentDialogue == null || currentLineIndex >= currentDialogue.linhas.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.linhas[currentLineIndex];

        if (characterImage != null)
        {
            var who = Normalize(line.speaker);
            RenderTexture next = null;

            if (who.Contains("doutora") || who.Contains("medica") || who.Contains("médica"))
                next = doctorRenderTexture;
            else if (who.Contains("paciente"))
                next = patientRenderTexture;

            if (next != null)
                characterImage.texture = next;
        }

        if (dialogueText != null)
            dialogueText.text = line.text ?? "";
    }

    public void NextLine()
    {
        if (!dialogueActive) return;

        currentLineIndex++;

        if (currentLineIndex < currentDialogue.linhas.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        Debug.Log("🎬 Diálogo encerrado e iniciando fade...");

        StartCoroutine(FadeAndLoadScene());
    }

    IEnumerator FadeAndLoadScene()
    {
        yield return new WaitForSeconds(0.5f);

        if (fadeCanvas != null)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
                yield return null;
            }
        }

        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(nextSceneName);
    }

    private static string Normalize(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        string lower = s.ToLowerInvariant();
        var formD = lower.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var ch in formD)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != UnicodeCategory.NonSpacingMark) sb.Append(ch);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC).Trim();
    }
}

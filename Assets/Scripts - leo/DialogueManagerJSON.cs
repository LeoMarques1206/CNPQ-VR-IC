using System.Collections;
using System.Globalization;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

#region DATA STRUCTS

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

#endregion

public class DialogueManagerJSON : MonoBehaviour
{
    #region UI

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public RawImage characterImage;

    #endregion

    #region RenderTextures

    [Header("RenderTextures")]
    public RenderTexture doctorRenderTexture;
    public RenderTexture patientRenderTexture;

    #endregion

    #region Scene Transition

    [Header("Transição de Cena")]
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1.5f;
    public string nextSceneName = "IntoScene";

    #endregion

    #region INPUT XR (IGUAL AO SCRIPT DE GRAB)

    [Header("Input XR")]
    public InputActionProperty nextAction;
    [Range(0.1f, 1f)]
    public float pressThreshold = 0.7f;

    private bool pressedLastFrame = false;

    #endregion

    #region Config

    [Header("Configuração")]
    public string jsonFileName = "dialogue_data";

    #endregion

    private DialogueRoot dialogueRoot;
    private TipoDialogue currentDialogue;
    private int currentLineIndex;
    private bool dialogueActive;

    void OnEnable()
    {
        nextAction.action?.Enable();
    }

    void OnDisable()
    {
        nextAction.action?.Disable();
    }

    void Start()
    {
        LoadDialogueData();

        string savedProblem = PlayerPrefs.GetString("ProblemName", "Incontinência Urinária");
        string savedType = PlayerPrefs.GetString("BehaviorName", "Calma");

        SetConversation(savedProblem, savedType);

        if (fadeCanvas != null)
            fadeCanvas.alpha = 1f;

        StartCoroutine(FadeOutStart());
        StartDialogue();
    }

    void Update()
    {
        if (!dialogueActive) return;

        HandleInput();
    }

    // ───────────────────────────────────────────────
    // INPUT (MESMO PADRÃO DO OUTRO SCRIPT)
    void HandleInput()
    {
        float value = nextAction.action.ReadValue<float>();
        bool pressed = value > pressThreshold;

        if (pressed && !pressedLastFrame)
            NextLine();

        pressedLastFrame = pressed;
    }

    #region Fade

    IEnumerator FadeOutStart()
    {
        yield return new WaitForSeconds(0.3f);

        if (fadeCanvas == null) yield break;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = 0f;
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

    #endregion

    #region Dialogue

    void LoadDialogueData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);
        if (jsonFile == null)
        {
            Debug.LogError($"❌ JSON '{jsonFileName}.json' não encontrado em Assets/Resources/");
            return;
        }

        dialogueRoot = JsonUtility.FromJson<DialogueRoot>(jsonFile.text);
    }

    public void SetConversation(string problem, string type)
    {
        if (dialogueRoot?.conversas == null || dialogueRoot.conversas.Length == 0)
            return;

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
            foundProblem = dialogueRoot.conversas[0];

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
        if (currentDialogue == null || currentDialogue.linhas.Length == 0)
            return;

        dialogueActive = true;
        currentLineIndex = 0;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        ShowCurrentLine();
    }

    void ShowCurrentLine()
    {
        if (currentLineIndex >= currentDialogue.linhas.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.linhas[currentLineIndex];

        if (characterImage != null)
        {
            string who = Normalize(line.speaker);
            if (who.Contains("doutora"))
                characterImage.texture = doctorRenderTexture;
            else if (who.Contains("paciente"))
                characterImage.texture = patientRenderTexture;
        }

        if (dialogueText != null)
            dialogueText.text = line.text ?? "";
    }

    public void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < currentDialogue.linhas.Length)
            ShowCurrentLine();
        else
            EndDialogue();
    }

    void EndDialogue()
    {
        dialogueActive = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        StartCoroutine(FadeAndLoadScene());
    }

    #endregion

    #region Utils

    private static string Normalize(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";

        string lower = s.ToLowerInvariant();
        var formD = lower.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var ch in formD)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                sb.Append(ch);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC).Trim();
    }

    #endregion
}

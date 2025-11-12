using UnityEngine;
using System.Collections;

public class MiniGameLogicManager : MonoBehaviour
{
    [Header("Tempo de espera antes de iniciar o mini game")]
    public float startDelay = 3f;

    private string problemName;

    void Start()
    {
        // Recupera o problema salvo no PlayerPrefs
        problemName = PlayerPrefs.GetString("ProblemName", "Incontinência Urinária");

        Debug.Log($"🧩 Problema selecionado: {problemName}");

        // Inicia o mini game após alguns segundos
        StartCoroutine(StartMiniGameAfterDelay());
    }

    IEnumerator StartMiniGameAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartMiniGame();
    }

    void StartMiniGame()
    {
        switch (problemName)
        {
            case "Incontinência Urinária":
                StartIncontinenciaMiniGame();
                break;

            case "Vaginismo":
                StartVaginismoMiniGame();
                break;

            case "Dor Pélvica Crônica":
                StartDorPelvicaMiniGame();
                break;

            case "Dispareunia":
                StartDispareuniaMiniGame();
                break;

            default:
                Debug.LogWarning($"⚠️ Nenhuma lógica definida para o problema: {problemName}");
                break;
        }
    }

    // ------------------------------
    // 🧠 Lógicas específicas dos mini-games
    // ------------------------------

    void StartIncontinenciaMiniGame()
    {
        Debug.Log("🚽 Iniciando mini game de Incontinência Urinária...");
        // Exemplo: mostrar barra de força do assoalho pélvico, treino de contração etc.
        // Exemplo: Ativar objetos específicos
        // pelvicMuscleTrainer.SetActive(true);
    }

    void StartVaginismoMiniGame()
    {
        Debug.Log("🌸 Iniciando mini game de Vaginismo...");
        // Exemplo: simulação de relaxamento, controle de respiração
        // breathingExercise.SetActive(true);
    }

    void StartDorPelvicaMiniGame()
    {
        Debug.Log("💢 Iniciando mini game de Dor Pélvica Crônica...");
        // Exemplo: minigame de reconhecimento de dor e relaxamento muscular
        // painMappingExercise.SetActive(true);
    }

    void StartDispareuniaMiniGame()
    {
        Debug.Log("❤️‍🔥 Iniciando mini game de Dispareunia...");
        // Exemplo: minigame de alongamento e lubrificação gradual
        // touchSensitivityGame.SetActive(true);
    }
}

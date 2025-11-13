using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrocarCena : MonoBehaviour
{
    [Header("Nome da Cena para Carregar")]
    [Tooltip("Nome exato da cena (precisa estar adicionada em Build Settings).")]
    public string nomeCena;

    [Header("Botão que dispara a troca de cena")]
    public Button botao;

    [Header("Botão para sair do jogo")]
    public Button botaoSair;

    [Header("Tempo de atraso antes da troca (opcional)")]
    [Tooltip("Tempo em segundos antes de carregar a cena.")]
    public float atraso = 1f;

    void Start()
    {
        // Configura o botão de troca de cena
        if (botao != null)
            botao.onClick.AddListener(CarregarCena);
        else
            Debug.LogWarning("⚠️ Nenhum botão atribuído para trocar de cena!");

        // Configura o botão de sair
        if (botaoSair != null)
            botaoSair.onClick.AddListener(SairDoJogo);
        else
            Debug.LogWarning("⚠️ Nenhum botão atribuído para sair do jogo!");
    }

    public void CarregarCena()
    {
        if (string.IsNullOrEmpty(nomeCena))
        {
            Debug.LogWarning("⚠️ Nome da cena não foi definido no Inspector!");
            return;
        }

        // Aguarda o tempo de atraso antes de carregar
        Invoke(nameof(CarregarCenaDepoisInvoke), atraso);
    }

    private void CarregarCenaDepoisInvoke()
    {
        Debug.Log($"🎬 Carregando cena: {nomeCena}");
        SceneManager.LoadScene(nomeCena);
    }

    public void SairDoJogo()
    {
        Debug.Log("🚪 Saindo do jogo...");

        // No editor, apenas para visualização
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // No build (PC/VR)
        Application.Quit();
#endif
    }
}

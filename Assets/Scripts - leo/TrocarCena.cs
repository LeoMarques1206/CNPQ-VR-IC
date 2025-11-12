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

    [Header("Tempo de atraso antes da troca (opcional)")]
    [Tooltip("Tempo em segundos antes de carregar a cena.")]
    public float atraso = 1f;

    void Start()
    {
        if (botao != null)
        {
            botao.onClick.AddListener(CarregarCena);
        }
        else
        {
            Debug.LogWarning("⚠️ Nenhum botão atribuído no script TrocarCena!");
        }
    }

    public void CarregarCena()
    {
        if (string.IsNullOrEmpty(nomeCena))
        {
            Debug.LogWarning("⚠️ Nome da cena não foi definido no Inspector!");
            return;
        }

        // 🔄 Aguarda o tempo de atraso antes de carregar a cena
        Invoke(nameof(CarregarCenaDepoisInvoke), atraso);
    }

    private void CarregarCenaDepoisInvoke()
    {
        Debug.Log($"🎬 Carregando cena: {nomeCena}");
        SceneManager.LoadScene(nomeCena);
    }
}

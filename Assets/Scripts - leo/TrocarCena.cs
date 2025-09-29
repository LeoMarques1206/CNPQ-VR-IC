using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrocarCena : MonoBehaviour
{
    [Header("Nome da Cena para Carregar")]
    public string nomeCena;

    [Header("Botão que dispara a troca de cena")]
    public Button botao;

    void Start()
    {
        
        if (botao != null)
        {
            botao.onClick.AddListener(CarregarCena);
        }
        else
        {
            Debug.LogWarning("Nenhum botão atribuído no script TrocarCena!");
        }
    }

    public void CarregarCena()
    {
        if (!string.IsNullOrEmpty(nomeCena))
        {
            SceneManager.LoadScene(nomeCena);
        }
        else
        {
            Debug.LogWarning("Nome da cena não foi definido no Inspector!");
        }
    }
}

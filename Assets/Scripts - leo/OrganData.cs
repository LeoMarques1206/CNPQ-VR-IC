using UnityEngine;

[DisallowMultipleComponent]
public class OrganData : MonoBehaviour
{
    [Header("Dados do Órgão")]
    public string organName = "Vagina";

    [TextArea(3, 6)]
    public string description =
        "Canal muscular e elástico que conecta o colo do útero ao exterior. É o local de entrada do pênis e saída do fluxo menstrual, além de atuar no parto.";
}

using UnityEngine;

public class btnHide : MonoBehaviour
{
    [Header("Sistema de Esconder/Mostrar")]
    public GameObject[] ossos;
    public GameObject lanterna;
    private bool apareceOsso = true;

    [Header("Player que será movido")]
    public Transform player;

    [Header("Pontos de Movimento")]
    public Transform pontoIntermediario;
    public float travelSpeed = 1.5f;

    [Header("Teleportes")]
    public Transform teleportInicio; // Ponto A
    public Transform teleportFinal;  // Ponto C
    public Transform teleportVolta;  // Ponto D

    private bool isTraveling = false;

    //---------------------------------------
    // HIDE BONE
    //---------------------------------------
    public void HideBone()
    {
        apareceOsso = !apareceOsso;

        foreach (GameObject o in ossos)
        {
            if (o != null)
                o.SetActive(apareceOsso);
        }
    }

    //---------------------------------------
    void Update()
    {
        if (isTraveling)
            TravelStep();
    }

    //---------------------------------------
    // IDA: TP A → MOVE B → TP C
    //---------------------------------------
    public void Travel()
    {
        if (player == null || teleportInicio == null || pontoIntermediario == null)
        {
            Debug.LogWarning("Configuração incompleta no Inspector!");
            return;
        }

        // 1️⃣ TELEPORTA PARA O PONTO A
        player.position = teleportInicio.position;
        player.rotation = teleportInicio.rotation;

        // 2️⃣ INICIA MOVIMENTO ATÉ O PONTO B
        isTraveling = true;

        Debug.Log("TP A realizado → iniciando viagem até B");

        lanterna.SetActive(true);
    }

    //---------------------------------------
    // VOLTA: TP D
    //---------------------------------------
    public void TravelBack()
    {
        if (player == null || teleportVolta == null)
        {
            Debug.LogWarning("Configure o TELEPORT DE VOLTA (D)!");
            return;
        }

        isTraveling = false;

        player.position = teleportVolta.position;
        player.rotation = teleportVolta.rotation;

        lanterna.SetActive(false);
        

        Debug.Log("TP direto para o ponto D");
    }

    //---------------------------------------
    // MOVE ATÉ B, DEPOIS TP PARA C
    //---------------------------------------
    private void TravelStep()
    {
        player.position = Vector3.Slerp(
            player.position,
            pontoIntermediario.position,
            travelSpeed * Time.deltaTime
        );

        float distancia = Vector3.Distance(player.position, pontoIntermediario.position);

        if (distancia < 0.1f)
        {
            isTraveling = false;

            Debug.Log("Chegou no ponto B");

            // 3️⃣ TELEPORTA PARA O PONTO C
            if (teleportFinal != null)
            {
                player.position = teleportFinal.position;
                player.rotation = teleportFinal.rotation;

                Debug.Log("TP C realizado");
            }
        }
    }
}

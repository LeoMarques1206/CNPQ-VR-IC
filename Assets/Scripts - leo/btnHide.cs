using UnityEngine;

public class btnHide : MonoBehaviour
{
    [Header("Sistema de Esconder/Mostrar")]
    public GameObject[] ossos;   // ← AGORA É UM ARRAY!
    private bool apareceOsso = true;

    [Header("Player que será movido")]
    public Transform player;

    [Header("Pontos de Movimento (Travel)")]
    public Transform pontoInicial;
    public Transform pontoIntermediario;
    public Transform pontoFinal;
    public float travelSpeed = 1.5f;

    [Header("Teleporte ao Final")]
    public Transform teleportFinal;
    public Transform teleportInicial;

    private bool isTraveling = false;
    private Transform currentTarget;
    private bool travelingBack = false;

    //---------------------------------------
    //  HIDE BONE (ARRAY)
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
        {
            TravelStep();
        }
    }

    //---------------------------------------
    public void Travel()
    {
        if (player == null)
        {
            Debug.LogWarning("PLAYER não configurado no inspector!");
            return;
        }

        if (pontoIntermediario == null || pontoFinal == null)
        {
            Debug.LogWarning("Configure os pontos no inspector!");
            return;
        }

        travelingBack = false;
        currentTarget = pontoIntermediario;
        isTraveling = true;
    }

    //---------------------------------------
    public void TravelBack()
    {
        if (player == null)
        {
            Debug.LogWarning("PLAYER não configurado no inspector!");
            return;
        }

        if (teleportInicial == null)
        {
            Debug.LogWarning("Configure o TELEPORT INICIAL no inspector!");
            return;
        }

        // Teleporta imediatamente
        player.position = teleportInicial.position;

        Debug.Log("Teleportado de volta ao ponto inicial!");
    }

    //---------------------------------------
    private void TravelStep()
    {
        player.position = Vector3.Slerp(
            player.position,
            currentTarget.position,
            travelSpeed * Time.deltaTime
        );

        float distancia = Vector3.Distance(player.position, currentTarget.position);

        if (distancia < 0.1f)
        {
            if (!travelingBack)
            {
                if (currentTarget == pontoIntermediario)
                {
                    currentTarget = pontoFinal;
                }
                else
                {
                    isTraveling = false;
                    Debug.Log("Chegou ao destino final!");

                    if (teleportFinal != null)
                        player.position = teleportFinal.position;
                }
            }
            else
            {
                if (currentTarget == pontoIntermediario)
                {
                    currentTarget = pontoInicial;
                }
                else
                {
                    isTraveling = false;
                    Debug.Log("Voltou ao ponto inicial!");

                    if (teleportInicial != null)
                        player.position = teleportInicial.position;
                }
            }
        }
    }
}

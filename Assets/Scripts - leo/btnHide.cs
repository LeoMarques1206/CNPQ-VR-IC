using UnityEngine;

public class btnHide : MonoBehaviour
{
    [Header("Sistema de Esconder/Mostrar")]
    public GameObject osso;
    private bool apareceOsso = true;

    [Header("Player que será movido")]
    public Transform player;   

    [Header("Pontos de Movimento (Travel)")]
    public Transform pontoInicial;         
    public Transform pontoIntermediario;   
    public Transform pontoFinal;           
    public float travelSpeed = 1.5f;

    [Header("Teleporte ao Final")]
    public Transform teleportFinal;    // posição ao terminar a ida
    public Transform teleportInicial;  // posição ao terminar a volta

    private bool isTraveling = false;
    private Transform currentTarget;
    private bool travelingBack = false;

    //---------------------------------------
    //  HIDE BONE
    //---------------------------------------
    public void HideBone()
    {
        if (apareceOsso == true)
        {
            apareceOsso = false;
            osso.SetActive(false);
        }
        else
        {
            apareceOsso = true;
            osso.SetActive(true);
        }
    }

    //---------------------------------------
    //  UPDATE
    //---------------------------------------
    void Update()
    {
        if (isTraveling)
        {
            TravelStep();
        }
    }

    //---------------------------------------
    //  IR PARA O DESTINO FINAL
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
    //  VOLTAR PARA A POSIÇÃO INICIAL
    //---------------------------------------
    public void TravelBack()
    {
        if (player == null)
        {
            Debug.LogWarning("PLAYER não configurado no inspector!");
            return;
        }

        if (pontoIntermediario == null || pontoInicial == null)
        {
            Debug.LogWarning("Configure os pontos no inspector!");
            return;
        }

        travelingBack = true;
        currentTarget = pontoIntermediario;
        isTraveling = true;
    }

    //---------------------------------------
    //  MOVIMENTO SUAVE (SLERP)
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
                // Indo para frente
                if (currentTarget == pontoIntermediario)
                {
                    currentTarget = pontoFinal;
                }
                else
                {
                    isTraveling = false;
                    Debug.Log("Chegou ao destino final!");

                    // TELEPORTE FINAL
                    if (teleportFinal != null)
                    {
                        player.position = teleportFinal.position;
                    }
                }
            }
            else
            {
                // Voltando
                if (currentTarget == pontoIntermediario)
                {
                    currentTarget = pontoInicial;
                }
                else
                {
                    isTraveling = false;
                    Debug.Log("Voltou ao ponto inicial!");

                    // TELEPORTE INICIAL
                    if (teleportInicial != null)
                    {
                        player.position = teleportInicial.position;
                    }
                }
            }
        }
    }
}

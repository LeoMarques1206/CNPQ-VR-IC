using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteEntrando : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entrou no gatilho de: " + other.gameObject.name);
    }

    public void Interagiu()
    {
        Debug.Log("Interação detectada!");
    }

}

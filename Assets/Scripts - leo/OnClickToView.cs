using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickToView : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject uterusCanvas; 
    public RawImage uterusImage;       

    void Start()
    {
        if (uterusCanvas != null)
            uterusCanvas.SetActive(false);
    }

    
    public void SelectUterus()
    {
        if (uterusCanvas != null)
        {
            uterusCanvas.SetActive(true);
            Debug.Log("Visualização do útero ativada!");
        }
    }

    // Botão para fechar o canvas
    public void CloseUterusView()
    {
        if (uterusCanvas != null)
        {
            uterusCanvas.SetActive(false);
            Debug.Log("Visualização do útero encerrada.");
        }
    }
}

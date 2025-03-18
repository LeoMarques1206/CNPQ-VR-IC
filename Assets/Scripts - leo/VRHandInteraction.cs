using UnityEngine;
using UnityEngine.InputSystem; 

public class VRHandInteraction : MonoBehaviour
{
    private GrabLeg grabbedLeg;
    public InputActionProperty grabAction; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leg"))
        {
            grabbedLeg = other.GetComponent<GrabLeg>();
            Debug.Log("entrou na perna");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Leg") && grabbedLeg != null)
        {
            grabbedLeg.Release();
            grabbedLeg = null;
        }
    }

    void Update()
    {
        if (grabbedLeg != null)
        {
            if (grabAction.action.WasPressedThisFrame()) // Usa o novo Input System
            {
                grabbedLeg.Grab(transform);
            }

            if (grabAction.action.WasReleasedThisFrame())
            {
                grabbedLeg.Release();
            }
        }
    }
}

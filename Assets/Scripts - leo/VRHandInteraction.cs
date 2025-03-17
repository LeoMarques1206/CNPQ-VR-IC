using UnityEngine;

public class VRHandInteraction : MonoBehaviour
{
    private GrabLeg grabbedLeg;

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
       
            if (Input.GetButtonDown("Grab"))
            {
                grabbedLeg.Grab(transform);
            }


            if (Input.GetButtonUp("Grab"))
            {
                grabbedLeg.Release();
            }
        }
    }
}
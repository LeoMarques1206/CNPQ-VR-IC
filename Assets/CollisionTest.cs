using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entrou em: " + other.gameObject.name);
    }
}

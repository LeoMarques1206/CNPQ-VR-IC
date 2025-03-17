using UnityEngine;

public class GrabLeg : MonoBehaviour
{
    private Rigidbody legRigidbody;
    private bool isGrabbed = false;
    private Transform hand;

    void Start()
    {
        legRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isGrabbed && hand != null)
        {
            legRigidbody.MovePosition(hand.position);
        }
    }

    public void Grab(Transform handTransform)
    {
        isGrabbed = true;
        hand = handTransform;
        legRigidbody.isKinematic = false;
    }

    public void Release()
    {
        isGrabbed = false;
        hand = null;
        legRigidbody.isKinematic = true;
    }
}

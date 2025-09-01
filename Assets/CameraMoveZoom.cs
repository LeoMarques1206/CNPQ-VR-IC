using UnityEngine;

public class CameraMoveZoom : MonoBehaviour
{
    public Transform targetPoint; 
    public float duration = 2f;

    private Vector3 startPos;
    private float elapsedTime = 0f;
    private bool moving = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (moving && targetPoint != null)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, targetPoint.position, t);

            if (t >= 1f) moving = false;
        }
    }
}

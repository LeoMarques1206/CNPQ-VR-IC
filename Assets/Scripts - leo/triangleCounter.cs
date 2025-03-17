using UnityEngine;

public class triangleCounter : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Count Started");
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter && meshFilter.mesh)
        {
            int triangleCount = meshFilter.mesh.triangles.Length / 3;
            Debug.Log($"{gameObject.name} tem {triangleCount} triângulos.");
        }
    }
}
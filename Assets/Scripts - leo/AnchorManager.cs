using UnityEngine;

public class AnchorManagement : MonoBehaviour
{
    [Header("Referências")]
    public GameObject prefab;                // O objeto que será instanciado
    public Transform rightController;        // Referência ao controle direito (OVRControllerPrefab)

    [Header("Configurações")]
    public float rayLength = 5f;
    public Color laserColor = Color.green;   // Cor do feixe visual
    public bool onlyOnce = true;             // Se quiser permitir apenas uma instanciação

    private LineRenderer lineRenderer;
    private bool placed = false;

    void Start()
    {
        // --- LASER ---
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.005f;
        lineRenderer.endWidth = 0.002f;

        // Usa um shader seguro para build (substitui Unlit/Color, que pode dar rosa no Quest)
        Shader safeShader = Shader.Find("Sprites/Default");
        lineRenderer.material = new Material(safeShader);
        lineRenderer.material.color = laserColor;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        if ((onlyOnce && placed) || rightController == null)
            return;

        // Origem e direção do controle
        Vector3 origin = rightController.position;
        Vector3 direction = rightController.forward;
        Vector3 targetPoint = origin + direction * rayLength;

        // Atualiza o laser
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, targetPoint);

        // --- DETECÇÃO DE GATILHO (OVR) ---
        bool triggerPressed = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        // Instancia o prefab quando o gatilho é pressionado
        if (triggerPressed)
        {
            Debug.Log("🎯 Gatilho detectado! Instanciando prefab...");
            Instantiate(prefab, targetPoint, prefab.transform.rotation);

            if (onlyOnce)
            {
                lineRenderer.enabled = false;
                placed = true;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class VRRaycastOrganSelector : MonoBehaviour
{
    [Header("Referências da Mão / Ray")]
    public Transform handTransform;
    public float maxDistance = 10f;
    public LayerMask organLayer;

    [Header("UI de Informação (Painel FIXO em World Space)")]
    public GameObject infoPanel;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    [Header("Laser")]
    public LineRenderer laser;
    public float laserWidth = 0.01f;
    public float laserHitDistanceMin = 0.05f;

    [Header("Comportamento")]
    public bool showPanelOnlyOnHit = true;
    public Camera mainCamera;

    // Estado interno
    private OrganData currentOrgan = null;

    // Seleção e highlight
    private Transform selectedTransform;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public float moveSpeed = 1f;
    public float rotationSpeed = 50f;
    public bool isGrabbing = false;
    private OutlineHighlighter currentHighlight;
    public InputActionProperty rightGrip;
    void Awake()
    {
        if (laser == null)
        {
            laser = GetComponent<LineRenderer>();
            if (laser == null)
                laser = gameObject.AddComponent<LineRenderer>();
        }

        laser.positionCount = 2;
        laser.startWidth = laserWidth;
        laser.endWidth = laserWidth;

        if (infoPanel != null) infoPanel.SetActive(false);
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Update()
    {
        if (handTransform == null) return;

        Ray ray = new Ray(handTransform.position, handTransform.forward);
        RaycastHit hit;
        bool didHit = Physics.Raycast(ray, out hit, maxDistance, organLayer);

        // Laser
        Vector3 laserEnd = handTransform.position + handTransform.forward * maxDistance;
        if (didHit) laserEnd = hit.point;

        laser.SetPosition(0, handTransform.position);
        Vector3 toEnd = laserEnd - handTransform.position;
        if (toEnd.magnitude < laserHitDistanceMin)
            laserEnd = handTransform.position + handTransform.forward * laserHitDistanceMin;
        laser.SetPosition(1, laserEnd);

        // ------------------------------------------
        // Seleção do órgão
        // ------------------------------------------
        if (didHit)
        {
            OrganData od = hit.collider.GetComponentInParent<OrganData>();

            if (od != null)
            {
                if (currentOrgan != od)
                {
                    currentOrgan = od;

                    ReleaseObject();

                    // Remove highlight antigo
                    if (currentHighlight != null)
                        currentHighlight.DisableOutline();

                    // Novo highlight
                    currentHighlight = od.GetComponent<OutlineHighlighter>();
                    if (currentHighlight != null)
                        currentHighlight.EnableOutline();

                    // Salvar transform e mostrar info
                    selectedTransform = od.transform;
                    SaveOriginalTransforms();
                    ShowOrganInfo(od);
                }
            }
            else
            {
                ClearSelection();
            }
        }
        else
        {
            ClearSelection();
        }

        // -------------------------------------------
        // Controle de mover e girar objeto
        // -------------------------------------------
        if (isGrabbing && selectedTransform != null)
        {
            selectedTransform.position = Vector3.Lerp(
                selectedTransform.position,
                handTransform.position + handTransform.forward * 0.25f,
                Time.deltaTime * moveSpeed
            );

            selectedTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        // Controles G (placeholder VR)
        if (Input.GetKeyDown(KeyCode.G)) StartGrab();
        if (Input.GetKeyUp(KeyCode.G)) ReleaseObject();

        if (rightGrip.action.WasPressedThisFrame())
        {
            StartGrab();  
        }
          
        if (rightGrip.action.WasReleasedThisFrame())
        {
            ReleaseObject();
        }
            
    }

    // ---------------------------------------------------------------
    // Controle de pegar/soltar
    // ---------------------------------------------------------------
    void StartGrab()
    {
        if (selectedTransform == null) return;
        isGrabbing = true;
    }

    void SaveOriginalTransforms()
    {
        if (selectedTransform == null) return;

        originalPosition = selectedTransform.position;
        originalRotation = selectedTransform.rotation;
    }

    void ReleaseObject()
    {
        if (selectedTransform == null) return;

        isGrabbing = false;
        selectedTransform.position = originalPosition;
        selectedTransform.rotation = originalRotation;
    }

    // ---------------------------------------------------------------
    // Painel FIXO
    // ---------------------------------------------------------------
    void ShowOrganInfo(OrganData od)
    {
        if (infoPanel == null) return;

        if (showPanelOnlyOnHit)
            infoPanel.SetActive(true);

        if (nameText != null) nameText.text = od.organName;
        if (descriptionText != null) descriptionText.text = od.description;
    }

    void ClearSelection()
    {
        ReleaseObject();
        currentOrgan = null;

        if (infoPanel != null && showPanelOnlyOnHit)
            infoPanel.SetActive(false);

        if (currentHighlight != null)
        {
            currentHighlight.DisableOutline();
            currentHighlight = null;
        }
    }
}

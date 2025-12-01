using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class VRRaycastOrganSelector : MonoBehaviour
{
    [Header("Referências")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;
    public Transform handTransform;
    public Transform cameraTransform;

    [Header("UI de Informação (Painel FIXO)")]
    public GameObject infoPanel;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    [Header("Mover / Rotacionar")]
    public float moveSpeed = 8f;
    public float rotationSpeed = 120f;
    public float maxGrabDistance = 2f;

    [Header("Tempo Máximo de Movimento")]
    public float maxGrabTime = 1.2f;  
    private float grabTimer = 0f;

    private bool isGrabbing = false;

    [Header("Input (Input System)")]
    public InputActionProperty rightGrip;
    public InputActionProperty rightRotateAxis;
    public InputActionProperty leftGripRotate;

    private OrganData currentOrgan = null;
    private Transform selectedTransform;
    private OutlineHighlighter currentHighlight;

    private class SavedTransform
    {
        public Vector3 pos;
        public Quaternion rot;
        public Transform parent;
        public bool hadRigidbody;
    }

    private Dictionary<Transform, SavedTransform> saved = new Dictionary<Transform, SavedTransform>();


    void OnEnable()
    {
        rightGrip.action?.Enable();
        rightRotateAxis.action?.Enable();
        leftGripRotate.action?.Enable();
    }

    void OnDisable()
    {
        rightGrip.action?.Disable();
        rightRotateAxis.action?.Disable();
        leftGripRotate.action?.Disable();
    }


    void Update()
    {
        if (rayInteractor == null) return;

        // ───────────────────────────────────────────────
        // RAYCAST XR RAY INTERACTOR
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            OrganData od = hit.collider.GetComponentInParent<OrganData>();

            if (od != null && od != currentOrgan)
            {
                if (currentHighlight != null)
                    currentHighlight.DisableOutline();

                currentOrgan = od;

                currentHighlight = od.GetComponent<OutlineHighlighter>();
                if (currentHighlight != null)
                    currentHighlight.EnableOutline();

                selectedTransform = od.transform;

                if (!saved.ContainsKey(selectedTransform))
                {
                    SavedTransform s = new SavedTransform();
                    s.pos = selectedTransform.position;
                    s.rot = selectedTransform.rotation;
                    s.parent = selectedTransform.parent;
                    s.hadRigidbody = selectedTransform.GetComponent<Rigidbody>() != null;

                    saved[selectedTransform] = s;
                }

                ShowOrganInfo(od);
            }
        }


        // ───────────────────────────────────────────────
        // INPUT (PEGAR / SOLTAR)
        if (rightGrip.action.WasPressedThisFrame())
            StartGrab();

        if (rightGrip.action.WasReleasedThisFrame())
            ReleaseObject();


        // ───────────────────────────────────────────────
        // MOVIMENTO + ROTAÇÃO ENQUANTO SEGURA
        if (isGrabbing && selectedTransform != null)
        {
            // Atualiza timer
            grabTimer += Time.deltaTime;

            bool canStillMove = grabTimer < maxGrabTime;

            if (canStillMove)
            {
                // Movimento
                Vector3 desiredPos = handTransform.position + handTransform.forward * 0.25f;

                float distFromCamera = Vector3.Distance(desiredPos, cameraTransform.position);

                if (distFromCamera > maxGrabDistance)
                {
                    desiredPos =
                        cameraTransform.position +
                        (desiredPos - cameraTransform.position).normalized * maxGrabDistance;
                }

                selectedTransform.position = Vector3.Lerp(
                    selectedTransform.position,
                    desiredPos,
                    Time.deltaTime * moveSpeed
                );
            }

            // ROTAÇÃO (não tem limite de tempo)
            if (leftGripRotate.action.IsPressed())
            {
                selectedTransform.Rotate(
                    Vector3.up,
                    rotationSpeed * Time.deltaTime,
                    Space.World
                );
            }
        }
    }


    void StartGrab()
    {
        if (selectedTransform == null) return;

        isGrabbing = true;

        grabTimer = 0f;  // ← reset do tempo ao pegar

        Rigidbody rb = selectedTransform.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        selectedTransform.SetParent(null);
    }


    void ReleaseObject()
    {
        if (selectedTransform == null) return;

        isGrabbing = false;

        if (saved.TryGetValue(selectedTransform, out SavedTransform s))
        {
            selectedTransform.position = s.pos;
            selectedTransform.rotation = s.rot;
            selectedTransform.SetParent(s.parent);

            Rigidbody rb = selectedTransform.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = !s.hadRigidbody ? true : false;
        }

        if (currentHighlight != null)
        {
            currentHighlight.DisableOutline();
            currentHighlight = null;
        }

        currentOrgan = null;
        selectedTransform = null;
    }


    void ShowOrganInfo(OrganData od)
    {
        if (nameText != null) nameText.text = od.organName;
        if (descriptionText != null) descriptionText.text = od.description;
    }
}

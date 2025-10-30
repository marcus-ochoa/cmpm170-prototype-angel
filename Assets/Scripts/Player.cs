using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction attackAction;

    private Vector2 moveVector;
    private Vector2 deltaLook;

    private Vector3 rotation = Vector3.zero;

    [SerializeField] private float mouseSens = 40.0f;
    [SerializeField] private float forceScalar = 1.0f;

    private Rigidbody rb;
    private Camera cam;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        attackAction = InputSystem.actions.FindAction("Attack");

        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>();
        deltaLook = lookAction.ReadValue<Vector2>();

        // https://gist.github.com/KarlRamstedt/407d50725c7b6abeaf43aee802fdd88e
        
        rotation.x += deltaLook.x * mouseSens * Time.deltaTime;
		rotation.y += deltaLook.y * mouseSens * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, -90, 90);
        
		Quaternion xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
		Quaternion yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = xQuat * yQuat;

        if (attackAction.WasPressedThisFrame() && Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit))
        {
            if (hit.transform.gameObject.CompareTag("Grapple"))
            {
                // TODO: affect things
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 forceVector = new(moveVector.x, 0, moveVector.y);
        //rb.AddRelativeForce(forceScalar * Time.fixedDeltaTime * forceVector, ForceMode.Force);

        forceVector = transform.TransformVector(forceVector);

        rb.MovePosition(transform.position + forceScalar * Time.fixedDeltaTime * forceVector);
    }
}

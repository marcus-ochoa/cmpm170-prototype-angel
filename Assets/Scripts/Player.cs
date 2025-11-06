using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// [RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction attackAction;

    private Vector2 moveVector;
    private Vector2 deltaLook;

    private Vector2 rotation = Vector2.zero;

    private bool moveEnabled = true;

    [SerializeField] private float mouseSens = 40.0f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float hitRange = 5.0f;

    [SerializeField] private float animSpeed = 1.0f;

    // private Rigidbody rb;
    private CharacterController controller;
    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        attackAction = InputSystem.actions.FindAction("Attack");

        // rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();

        rotation.x = transform.localRotation.eulerAngles.y;
        rotation.y = -transform.localRotation.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {

        if (moveEnabled)
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

            if (attackAction.WasPressedThisFrame() && Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, hitRange))
            {
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();

                    if (enemy == null)
                    {
                        Debug.LogWarning("Object has tag enemy but does not have enemy component");
                    }
                    else
                    {
                        enemy.Found();
                    }
                }
            }

            Vector3 moveDirection = new(moveVector.x, 0, moveVector.y);
            moveDirection = transform.TransformVector(moveDirection);
            controller.Move(speed * Time.deltaTime * moveDirection);
        }
        
    }

    public void SetMoveEnabled(bool state)
    {
        moveEnabled = state;
    }

    public void MoveToAndLookAt(Vector3 targetPos, Vector3 targetLook)
    {
        StartCoroutine(MoveRoutine(targetPos, targetLook));
    }

    private IEnumerator MoveRoutine(Vector3 targetPos, Vector3 targetLook)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, animSpeed * Time.deltaTime);
            transform.LookAt(targetLook);
            yield return null;
        }
    }
}

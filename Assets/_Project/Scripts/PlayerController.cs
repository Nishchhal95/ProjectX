using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float xMouseSens = 2;
    [SerializeField] private float yMouseSens = 2;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerTransform;

    [Header("Ground Check")]
    [SerializeField] private float groundedOffset = .14f;
    [SerializeField] private float groundedRadius = .28f;
    [SerializeField] private LayerMask groundLayer;
    private bool _grounded;

    [Header("Gravity")]
    [SerializeField] private float gravity = -15f;
    [SerializeField] private float jumpHeight = 5f;
    private float _verticalVelocity;
    private bool _jump;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    private float _playerSpeed = 10f;
    private Vector3 _movement;
    private bool isWalking;

    [SerializeField] private CharacterController characterController;

    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        Jump();
        ApplyGravity();
        Move();
        HandlePlayerLook();
    }

    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x,
            transform.position.y - groundedOffset, transform.position.z);
        _grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayer, QueryTriggerInteraction.Ignore);
    }

    private void Jump()
    {
        _jump = inputManager.IsJumpPressed();
        if (_jump && _grounded)
        {
            _jump = false;
            // the square root of H * -2 * G = how much velocity needed to reach desired height
            _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ApplyGravity()
    {
        _verticalVelocity += gravity * Time.deltaTime;
        if (_grounded && _verticalVelocity < 0.0f)
        {
            _verticalVelocity = -2f;
        }
    }

    private void Move()
    {
        isWalking = inputManager.IsShiftHeld();
        _playerSpeed = isWalking ? walkSpeed : sprintSpeed;

        Vector2 inputVector = inputManager.GetPlayerInput();
        _movement = (cameraTransform.right * inputVector.x * _playerSpeed) +
                    (cameraTransform.forward * inputVector.y * _playerSpeed);
        _movement.y = _verticalVelocity;
        characterController.Move(_movement * Time.deltaTime);
    }

    private void HandlePlayerLook()
    {
        Vector2 mouseDelta = inputManager.GetMouseDelta();

        mouseDelta = new Vector2(mouseDelta.x * xMouseSens, mouseDelta.y * yMouseSens) * Time.deltaTime;
        Vector3 playerRotation = playerTransform.eulerAngles;
        Vector3 cameraRotation = cameraTransform.eulerAngles;

        Vector3 finalPlayerRotation = new Vector3(playerRotation.x, playerRotation.y + mouseDelta.x, 0);
        finalPlayerRotation = new Vector3(GameHelper.ClampAngle(finalPlayerRotation.x, -90f, 90f), finalPlayerRotation.y, finalPlayerRotation.z);

        Vector3 finalCameraRotation = new Vector3(cameraRotation.x - mouseDelta.y, cameraRotation.y, 0);
        finalCameraRotation = new Vector3(GameHelper.ClampAngle(finalCameraRotation.x, -90f, 90f), finalCameraRotation.y, finalCameraRotation.z);

        cameraTransform.rotation = Quaternion.Euler(finalCameraRotation);
        playerTransform.rotation = Quaternion.Euler(finalPlayerRotation);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (_grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x,
            transform.position.y - groundedOffset, transform.position.z), groundedRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    public Camera _playerCamera;

    public float runAcceleration = 0.25f;
    public float runSpeed = 4f;

    public float drag = 0.1f;
    public float inAirDrag = 5f;

    public float sprintAcceleration = 0.5f;
    public float sprintSpeed = 7f;

    public float gravity = 25f;
    public float jumpSpeed = 1.0f;

    public float lookSensH = 0.1f;
    public float lookSensV = 0.1f;
    public float lookLimitV = 89f;

    private Vector3 _currentVelocity = Vector3.zero;
    private PlayerLocomotionInput _playerLocomotionInput;
    private Vector2 _cameraRotation = Vector2.zero;
    private Vector2 _playerTargetRotation = Vector2.zero;

    private Vector3 targetRecoil = Vector3.zero;
    private Vector3 currentRecoil = Vector3.zero;

    private float verticalVelocity = 0f;

    private void Awake()
    {
        Cursor.visible = false;
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
    }

    private void Update()
    {
        //jumping
        bool isGrounded = IsGrounded();
        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = 0f;

        verticalVelocity -= gravity * Time.deltaTime;

        if(_playerLocomotionInput.JumpPressed && isGrounded )
        {
            verticalVelocity = Mathf.Sqrt(jumpSpeed * 3 * gravity);
        }

        //sprinting
        bool isSprinting = _playerLocomotionInput.SprintToggledOn;
        float lateralAcceleration = isSprinting ? sprintAcceleration : runAcceleration;
        float clampLateralMagnitude = isSprinting ? sprintSpeed : runSpeed;

        Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

        Vector3 movementDelta = movementDirection * lateralAcceleration;
        _currentVelocity.x += movementDelta.x * Time.deltaTime;
        _currentVelocity.z += movementDelta.z * Time.deltaTime;
        

        float dragMagnitude = isGrounded ? drag : inAirDrag;
        Vector3 horizontalVelocity = new Vector3(_currentVelocity.x,0f,_currentVelocity.z);
        Vector3 dragVector = horizontalVelocity * dragMagnitude * Time.deltaTime;
        horizontalVelocity = (horizontalVelocity.magnitude > dragMagnitude * Time.deltaTime) ? horizontalVelocity - dragVector : Vector3.zero;

        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, clampLateralMagnitude);

        _currentVelocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);

        _characterController.Move(_currentVelocity * Time.deltaTime);
    }

    private void LateUpdate()
    {
        _cameraRotation.x += lookSensH * _playerLocomotionInput.LookInput.x;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSensV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

        _playerTargetRotation.x += transform.eulerAngles.x + lookSensH * _playerLocomotionInput.LookInput.x;
        transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);

        _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y + currentRecoil.y, _cameraRotation.x + currentRecoil.x, 0f);
    }

    private bool IsGrounded()
    {
        return _characterController.isGrounded;
    }
    
    
    public void ApplyRecoil(GunData gunData)
    {
        float recoilX = Random.Range(-gunData.maxRecoil.x, gunData.maxRecoil.x) * gunData.recoilAmount;
        float recoilY = Random.Range(-gunData.maxRecoil.y, gunData.maxRecoil.y) * gunData.recoilAmount;

        targetRecoil += new Vector3(recoilX, recoilY, 0);

        currentRecoil = Vector3.MoveTowards(currentRecoil, targetRecoil, Time.deltaTime * gunData.recoilSpeed);
    }

    public void ResetRecoil(GunData gunData)
    {
        currentRecoil = Vector3.MoveTowards(currentRecoil, Vector3.zero, Time.deltaTime * gunData.resetRecoilSpeed);
        targetRecoil = Vector3.MoveTowards(targetRecoil, Vector3.zero, Time.deltaTime * gunData.resetRecoilSpeed);
    }
    
}

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    Vector3 currentInput;
    Vector2 moveInput;
    Vector2 mouseInput;

    // Mouse values
    float mouseX;
    float mouseY;
    float xRotation;
    float mouseSensivity = 100f;

    [Header("Player Floats")]
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpForce = 2f;
    [SerializeField] float crouchHeight = 1f;

    [Header("Player Bools")]
    [SerializeField] bool isJumping;
    [SerializeField] bool isRunning;
    [SerializeField] bool isCrouching;
    [SerializeField] bool onGround;

    float gravityForce = -14.81f;
    Vector3 gravityV3;

    [Header("Transforms")]
    [SerializeField] Transform groundChecker;
    [SerializeField] Transform cameraPosition;

    CharacterController _characterController;

    #region Unity Funcs

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        PlayerMovement();
        Jump();
        Gravity();
        SpeedChanger();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void LateUpdate()
    {
        PlayerLook();
    }

    #endregion

    #region Player Movements
    void PlayerMovement()
    {
        currentInput = transform.right * moveInput.x + transform.forward * moveInput.y;
        _characterController.Move(currentInput * playerSpeed * Time.deltaTime);
    }

    void Jump()
    {
        if (isJumping && onGround)
        {
            gravityV3.y = Mathf.Sqrt(jumpForce * -2f * gravityForce);
        }
    }

    public void Crouch()
    {
        isCrouching = !isCrouching;

        if (isCrouching && onGround)
        {
            _characterController.height = crouchHeight;
            _characterController.center = new Vector3(0, crouchHeight / 2f, 0);

        }
        else
        {
            _characterController.height = 2f;
            _characterController.center = new Vector3(0, 1f, 0);
        }
    }

    void PlayerLook()
    {
        mouseX = mouseInput.x * mouseSensivity * Time.deltaTime;
        mouseY = mouseInput.y * mouseSensivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        cameraPosition.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Gravity()
    {
        onGround = Physics.CheckSphere(groundChecker.position, 0.1f, LayerMask.GetMask("Ground"));

        gravityV3.y += gravityForce * Time.deltaTime;
        _characterController.Move(gravityV3 * Time.deltaTime);

        if (gravityV3.y < 0 && onGround)
        {
            gravityV3.y = -3f;
            SetJumpingValue(false);
        }
    }

    void SpeedChanger()
    {
        if (isRunning && onGround && !isCrouching && (currentInput != Vector3.zero))
        {
            playerSpeed = 5f;
        }
        else if (onGround && isCrouching)
        {
            playerSpeed = 1.5f;
        }
        else
        {
            playerSpeed = 3f;
        }
    }

    #endregion

    #region Set Funcs

    public void SetJumpingValue(bool isActive)
    {
        isJumping = isActive;
    }

    public void SetRunningValue(bool isActive)
    {
        isRunning = isActive;
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void SetMouseInput(Vector2 input)
    {
        mouseInput = input;
    }

    #endregion

    #region Get Funcs

    public bool GetJumpingValue() { return isJumping; }

    #endregion
}




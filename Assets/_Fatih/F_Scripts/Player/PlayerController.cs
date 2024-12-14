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
    [SerializeField] float normalHeight = 2f;
    [SerializeField] float crouchHeight = 1f;
    [SerializeField] float moveIndex;

    [Header("Player Bools")]
    [SerializeField] bool isMoving;
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
    Animator _playerAnimator;
    GameManager _gameManager;

    #region Unity Funcs

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerAnimator = GetComponentInChildren<Animator>();
        _gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        PlayerMovement();
        Jump();
        Gravity();
        SpeedChanger();
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
            _characterController.height = normalHeight;
            _characterController.center = new Vector3(0, normalHeight / 2, 0);
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
            moveIndex = 2;
            playerSpeed = 5f;
        }
        else if (onGround && isCrouching)
        {
            moveIndex = 1;
            playerSpeed = 1.5f;
        }
        else
        {
            moveIndex = 0;
            playerSpeed = 3f;
        }
    }

    void UpdateAnimationStates()
    {
        _playerAnimator.SetFloat("Horizontal", moveInput.x);
        _playerAnimator.SetFloat("Vertical", moveInput.y);
        _playerAnimator.SetBool("isMoving", isMoving);

        if (onGround)
        {
            _playerAnimator.SetBool("isJumping", false);

            if (GetJumpingValue())
                _playerAnimator.SetBool("isJumping", true);
        }

        _playerAnimator.SetFloat("MoveIndex", moveIndex);

    }

    #endregion

    public void Interact()
    {
        Debug.Log("basýldý");

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3f))
        {
            if (hit.collider.CompareTag("Door"))
            {
                Debug.Log("doðru kapý " + hit.collider.name);
            }

            if (hit.collider.CompareTag("WrongDoor"))
            {
                Debug.Log("yanlýþ kapý " + hit.collider.name);
            }
        }
    }

    public void SettingsPanel()
    {
        _gameManager.SettingsPanel();
    }

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

    public void SetMouseSensivity(float Sensivity)
    {
        mouseSensivity = Sensivity;
    }

    #endregion

    #region Get Funcs

    public bool GetJumpingValue() { return isJumping; }

    #endregion
}




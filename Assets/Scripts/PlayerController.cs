using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float groundDrag = 10f;
    [SerializeField] private float airDrag = 2f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform codeInputPanel;

    [Header("Footstep Sounds")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float baseStepInterval = 0.5f; 
    [SerializeField] private float footstepVolume = 0.5f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isSprinting;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private float footstepTimer = 0f;

    private float CurrentMaxSpeed => moveSpeed * (isSprinting ? sprintMultiplier : 1f);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (codeInputPanel != null && codeInputPanel.gameObject.activeSelf)
        {
            horizontalInput = 0;
            verticalInput = 0;
            isSprinting = false;
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        }

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.2f, groundLayer);

        SpeedControl();
        HandleFootsteps(); 

        rb.drag = isGrounded ? groundDrag : airDrag;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (horizontalInput != 0 || verticalInput != 0)
        {
            moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
            float maxSpeed = CurrentMaxSpeed;
            rb.AddForce(moveDirection.normalized * maxSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float maxSpeed = CurrentMaxSpeed;

        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void HandleFootsteps()
    {
        if (!IsMoving() || !isGrounded)
        {
            footstepTimer = 0f;
            return;
        }

        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;


        float speedRatio = currentSpeed / CurrentMaxSpeed;
        float adjustedInterval = baseStepInterval / Mathf.Max(speedRatio, 0.5f);

        footstepTimer += Time.deltaTime;

        if (footstepTimer >= adjustedInterval)
        {
            PlayFootstep();
            footstepTimer = 0f;
        }
    }

    bool IsMoving()
    {
        return Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;
    }

    void PlayFootstep()
    {
        if (footstepSounds == null || footstepSounds.Length == 0)
            return;

        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, transform.position, footstepVolume);
    }
}

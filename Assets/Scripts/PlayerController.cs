using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float groundDrag = 10f;
    [SerializeField] private float airDrag = 2f;
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform codeInputPanel; // UI 패널

    private Rigidbody rb;
    private bool isGrounded;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // UI가 활성화되면 입력 받지 않기
        if (codeInputPanel != null && codeInputPanel.gameObject.activeSelf)
        {
            horizontalInput = 0;
            verticalInput = 0;
        }
        else
        {
            // 입력 받기
            horizontalInput = Input.GetAxis("Horizontal"); // A, D
            verticalInput = Input.GetAxis("Vertical");     // W, S
        }

        // 지면 체크
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.2f, groundLayer);

        // 속도 제한
        SpeedControl();

        // Drag 조정
        rb.drag = isGrounded ? groundDrag : airDrag;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        // 입력이 있을 때만 이동
        if (horizontalInput != 0 || verticalInput != 0)
        {
            moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            // 입력이 없으면 즉시 멈추기
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
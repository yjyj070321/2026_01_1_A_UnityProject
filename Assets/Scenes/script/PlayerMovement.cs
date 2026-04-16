using UnityEngine;

public class PlayerMovemont : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float turnSpeed = 10.0f;

    [Header("점프 개선 설정")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrouned = true;

    [Header("글라이더 설정")]
    public GameObject gliderObject;
    public float gliderFalISpeed = 1.0f;
    public float gliderFMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimerLeft;
    public bool isGliding = false;

    public Rigidbody rb;

    public bool isGrounded = true;

    public int coinCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coyoteTimeCounter = 0;
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }
        gliderTimerLeft = gliderMaxTime;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGrounededState();

        //움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveHorizontal);

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.G) && !isGrounded && gliderTimerLeft > 0)
        {
            if (!isGliding)
            {
                EnableGlider();
            }
            gliderTimerLeft -= Time.deltaTime;

            if (gliderTimerLeft <= 0)
            {
                DisableGlider();
            }

        }
        else if (isGliding)
        {
            DisableGlider();
        }

        if (isGliding)
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);
        }
        else
        {
            rb.linearVelocity = new Vector3(moveHorizontal * moveSpeed, rb.linearVelocity.y, moveVertical * moveSpeed);


            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }

            else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            //점프 입력
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                realGrouned = false;
                coyoteTimeCounter = 0;
            }

            if (isGrounded)
            {
                if (isGliding)
                {
                    DisableGlider();
                }
                gliderTimerLeft = gliderMaxTime;
            }
        }

        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGrouned = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGrouned = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGrouned = false;
        }
    }

    void UpdateGrounededState()
    {
        if (realGrouned)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;
            }

            else
            {
                isGrounded = false;
            }

        }

    }

    void EnableGlider()
    {
        isGliding = true;

        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, -gliderFalISpeed, rb.linearVelocity.z);
    }

    void DisableGlider()
    {
        isGliding = false;
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
    }

    void ApplyGliderMovement(float horizontal, float vertical)
    {
        Vector3 gliderVelocity = new Vector3(horizontal * gliderFMoveSpeed, -gliderFalISpeed, vertical * gliderFMoveSpeed);
        rb.linearVelocity = gliderVelocity;
    }
}

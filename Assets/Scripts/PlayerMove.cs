using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;

    // gravité "jeu": -9.81 * 3 = -29.43 (fort), -20 est déjà fort.
    // Mets -12 à -18 pour commencer.
    public float gravity = -14f;

    public float jumpHeight = 1.5f;

    // réglages sol
    public float groundCheckDistance = 0.25f;
    public LayerMask groundMask = ~0; // tout par défaut

    // confort
    public float coyoteTime = 0.12f;
    public float terminalVelocity = -35f;

    CharacterController cc;
    Vector3 velocity;
    float lastGroundedTime;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // déplacement horizontal
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = (transform.right * x + transform.forward * z) * speed;
        cc.Move(move * Time.deltaTime);

        bool grounded = cc.isGrounded || GroundCheck();

        if (grounded)
        {
            lastGroundedTime = Time.time;

            // reset propre quand on touche le sol (évite l'accumulation négative)
            if (velocity.y < 0f)
                velocity.y = -2f;
        }

        // saut (Space)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool canJump = grounded || (Time.time - lastGroundedTime <= coyoteTime);
            if (canJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // gravité + limite de chute
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < terminalVelocity) velocity.y = terminalVelocity;

        cc.Move(velocity * Time.deltaTime);
    }

    bool GroundCheck()
    {
        // on part du centre, proche du bas du controller
        Vector3 origin = transform.position + Vector3.up * (cc.radius + 0.05f);
        float radius = cc.radius * 0.95f;

        return Physics.SphereCast(origin, radius, Vector3.down, out _, groundCheckDistance, groundMask, QueryTriggerInteraction.Ignore);
    }
}

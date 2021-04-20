using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;
    Vector2 velocity;
    private PlayerDeath fps;

    Animator m_Animator;
    Rigidbody m_Rigidbody;

    float horizontal;
    float vertical;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        GameObject player = GameObject.Find("/Player");
        fps = player.GetComponent<PlayerDeath>();
    }

    void Update()
    {
        if (fps.isDead == false)
        {
            horizontal = Input.GetAxis("Horizontal") * speed;
            vertical = Input.GetAxis("Vertical") * speed;
            velocity.y = vertical;
            velocity.x = horizontal;
            bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
            bool isWalking = hasHorizontalInput || hasVerticalInput;
            m_Animator.SetBool("IsWalking", isWalking);

        }
    }

    private void FixedUpdate()
    {
        if (fps.isDead == false)
        {
            m_Rigidbody.MovePosition(transform.position + Time.deltaTime * speed * transform.TransformDirection(horizontal, 0f, vertical));
        }
    }
}
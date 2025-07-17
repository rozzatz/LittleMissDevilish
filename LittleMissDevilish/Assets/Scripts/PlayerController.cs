using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private float lastDashTime;
    private bool isDashing;

    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (!isDashing)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            moveInput = new Vector3(horizontal, vertical, 0f).normalized;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.velocity = new Vector3(moveInput.x * moveSpeed, rb.velocity.y, 0f);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        Vector3 dashDirection = new Vector3(
            moveInput.x * dashSpeed,
            moveInput.y * (dashSpeed / 2f), // vertical dash is weaker
            0f
        );

        rb.velocity = dashDirection;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

}

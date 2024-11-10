using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    Rigidbody rb;

    public float moveSpeed;
    public float walkSpeed = 6.0f;
    public float sprintSpeed = 12.0f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    private bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        moveSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 directon = new Vector3(horizontal, 0f, vertical).normalized;

        if (directon.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(directon.x, directon.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && moveSpeed != sprintSpeed)
        {
            moveSpeed = sprintSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && moveSpeed != walkSpeed)
        {
            moveSpeed = walkSpeed;
        }
    }
}

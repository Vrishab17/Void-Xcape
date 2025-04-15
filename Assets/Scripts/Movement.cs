using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour {

    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 11f;
    Vector2 xInput;

    [SerializeField] float jumpHeight = 3.5f;
    bool jump;

    [SerializeField] float gravity = -30f; // -9.81
    Vector3 yVelocity = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;

    private void Update ()
        {
            isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);
            if (isGrounded) {
                yVelocity.y = 0;
            }

            Vector3 horizontalVelocity = (transform.right * xInput.x + transform.forward * xInput.y) * speed;
            controller.Move(horizontalVelocity * Time.deltaTime);

        // Jump: v = sqrt(-2 * jumpHeight * gravity)
            if (jump) {
                if (isGrounded) {
                    yVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
                }
                jump = false;
            }

            yVelocity.y += gravity * Time.deltaTime;
            controller.Move(yVelocity * Time.deltaTime);
        }

    public void ReceiveInput (Vector2 _xInput)
    {
        xInput = _xInput;
    }

    public void OnJumpPressed ()
    {
        jump = true;
    }

}

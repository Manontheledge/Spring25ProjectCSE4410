using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInout : MonoBehaviour
{
    public float speed = 3.0f;
    public const float baseSpeed = 3f;
    public float Grav = -9.8f;
    public float jumpforce = 2.0f;
    public float baseJump = 10f;

    private bool isGrounded;
    private float verticalSpeed;

    private CharacterController characterController;

    //private void OnEnable()
    //{
    //    Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    //}

    //private void OnDisable()
    //{
    //    Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    //}

    private void OnSpeedChanged(float value)
    {
        speed = baseSpeed * value;
        jumpforce = baseJump * value;
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;

        // Control movement along x and z axes
        float deltax = Input.GetAxis("Horizontal") * speed;
        float deltaz = Input.GetAxis("Vertical") * speed;

        // Initialize movement vector (without y for now)
        Vector3 move = new Vector3(deltax, 0, deltaz);

        // Handle jumping and gravity
        if (isGrounded)
        {
            // Prevent upward movement when grounded (slightly pulling player down)
            if (verticalSpeed < 0)
            {
                verticalSpeed = -1f;
            }

            if (Input.GetButtonDown("Jump"))
            {
                // Apply jump force only when grounded
                verticalSpeed = Mathf.Sqrt(jumpforce * -2f * Grav);
            }
        }
        else
        {
            // Apply gravity when not grounded
            verticalSpeed += Grav * Time.deltaTime;
        }

        // Apply vertical speed (gravity or jump) to the move vector
        move.y = verticalSpeed;

        // Apply the movement speed cap
        Vector3 moveclamp = Vector3.ClampMagnitude(new Vector3(move.x, 0, move.z), speed);

        move.x = moveclamp.x;
        move.z = moveclamp.z;

        // Convert movement from local to global coordinates
        move = transform.TransformDirection(move);

        // Move the character, using Time.deltaTime to scale the movement
        characterController.Move(move * Time.deltaTime);
    }
}

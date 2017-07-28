using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    private GameObject player;
    private GameObject DeathScreen;
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        DeathScreen = GameObject.FindGameObjectWithTag("DeathScreen");
        if (player = null)
        {
            if (DeathScreen.gameObject.activeInHierarchy == false)

            {
                DeathScreen.gameObject.SetActive(true);
                Time.timeScale = 0;
            }

            else
            {
                DeathScreen.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}

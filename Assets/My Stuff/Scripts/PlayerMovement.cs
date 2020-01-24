﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController character_Controller;
    private Vector3 move_Direction;

    public float speed = 5f;
    private float gravity = 20f;

    public float jump_Force = 10f;
    private float vertical_Velocity;

    void Awake() {
        character_Controller = GetComponent<CharacterController>();
    }
    

    // Update is called once per frame
    void Update() {
        MoveThePlayer();
    }

    void MoveThePlayer() {

        move_Direction = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f, Input.GetAxis(Axis.VERTICAL));
        move_Direction = transform.TransformDirection(move_Direction); //get direction from player to world space
        move_Direction *= speed * Time.deltaTime; //smooths movement in terms of framerate

        ApplyGravity();

        character_Controller.Move(move_Direction);
    }

    void ApplyGravity() {

        vertical_Velocity -= gravity * Time.deltaTime;

        PlayerJump();
        
        move_Direction.y = vertical_Velocity * Time.deltaTime;
    }

    void PlayerJump() {
        
        if(character_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            vertical_Velocity = jump_Force;
        }
        if(!character_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space)) 
        {
            vertical_Velocity = jump_Force;
        }

    }
}
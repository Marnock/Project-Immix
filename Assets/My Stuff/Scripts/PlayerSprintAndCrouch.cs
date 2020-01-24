using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{

    private PlayerMovement player_Movement;
    public float sprint_Speed = 10f;
    public float move_Speed = 5f;
    public float crouch_Speed = 2f;

    private Transform look_Root;
    private float stand_Height = 1.6f;
    private float crouch_Height = 1f;
    private bool is_Crouching;

    private float sprint_Volume = 0.8f;
    private float crouch_Volume = 0.1f;
    private float walk_Volume_Min = 0.2f, walk_Volume_Max = 0.4f;
    private float walk_Step_Distance = 0.4f;
    private float sprint_Step_Distance = 0.3f;
    private float crouch_Step_Distance = 0.5f;

    private float sprintValue = 100f;
    private float sprintThreshold = 5f;


    void Awake()  {

        player_Movement = GetComponent<PlayerMovement>();
        look_Root = transform.GetChild(0);

        
    }

    // Update is called once per frame
    void Update() {

        Sprint();
        Crouch();
        
    }

    void Sprint() 
    {
        if(sprintValue > 0f)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift) && !is_Crouching) 
            {
                player_Movement.speed = sprint_Speed;
          
            }
        }
        

        if(Input.GetKeyUp(KeyCode.LeftShift) && !is_Crouching) 
        {
           
            player_Movement.speed = move_Speed;
        }

        if(Input.GetKey(KeyCode.LeftShift) && !is_Crouching)
        {
            sprintValue -= sprintThreshold * Time.deltaTime;

            if(sprintValue <= 0f)
            {
               sprintValue = 0f;
               player_Movement.speed = move_Speed;
             
            }
        }
         else
            {
                if(sprintValue != 100f)
                {
                    sprintValue += (sprintThreshold * 2) * Time.deltaTime;

                    if(sprintValue > 100f)
                    {
                        sprintValue = 100f;
                    }
                }
            }
    }

    void Crouch() {

        if(Input.GetKeyDown(KeyCode.C)) {

            if(is_Crouching) {

                look_Root.localPosition = new Vector3(0f, stand_Height, 0f);
                player_Movement.speed = move_Speed;


                is_Crouching = false;
            } else {

                look_Root.localPosition = new Vector3(0f, crouch_Height, 0f);
                player_Movement.speed = crouch_Speed;


                is_Crouching = true;

            }
        }
    }
}

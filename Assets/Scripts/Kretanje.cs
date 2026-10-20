using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kretanje : MonoBehaviour
{
    private CharacterController character_controller;    //Komponenta od igraca
    private Vector3 move_direction;
    public float speed = 5f;     //Brzina hodanja
    private float gravity = 10f;    //Gravitacija
    public float jump_force = 5f;    //Jacina skoka
    private float vertical_velocity;    //Sila kojom gravitacija vuce igraca na dole
    public float sprint_speed = 10f;   //Brzina trcanja
    public float crouch_speed = 2f;   //Brzina sunjanja
    private Transform kamere;
    private float stand_height = 1.6f;   //Visina igraca kada stoji
    private float crouch_height = 1f;   //Visina igraca kada cuci
    private bool is_crouching;  //Da li cuci?


    private void Awake()
    {
        character_controller = GetComponent<CharacterController>();

        kamere = transform.GetChild(0);  
    }

    void Update()
    {
        MoveThePlayer();
        Sprint();
        Crouch();
    }

    void MoveThePlayer()
    {
        move_direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));   //Cita se koriscenje w,a,s,d dugmica
        move_direction = transform.TransformDirection(move_direction);   //Na osnovu dugmica menja se komponenta transformacija od igraca
        move_direction *= speed * Time.deltaTime;   //Primenjuje se i ova formule radi brzine kao i vremena radi prirodnijeg kretanja
        ApplyGravity();
        character_controller.Move(move_direction);  //Primenjuje se sve od gore i igrac se krece
    }

    void ApplyGravity()
    {
        vertical_velocity -= gravity * Time.deltaTime;  //Opsta formula za vertikalnu silu
        PlayerJump();
        move_direction.y = vertical_velocity * Time.deltaTime;  //Igrac ce se po y osi pomerati na osnovu vertikalne sile
    }

    void PlayerJump()
    {
        if(character_controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_velocity = jump_force;   //Ako je igrac na zemlji i pritisnemo Space on ce skociti
        }
    }

    void Sprint()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && !is_crouching)   //Ako drzimo levi shift i ne cucimo
        {
            speed = sprint_speed;

        }
        if(Input.GetKeyUp(KeyCode.LeftShift) && !is_crouching)   //Ako ne drzimo levi shift ine cucimo
        {
            speed = 5f;
        }
    }
    void Crouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))  //Ako pritisnemo levi ctrl
        {
            if(is_crouching)  //Ako smo vec cucali onda ustajemo
            {
                kamere.localPosition = new Vector3(0f, stand_height, 0f);  //Pozicija kamera
                speed = 5f;

                is_crouching = false;
            }
            else  //Ako smo stajali onda treba da cucnemo
            {
                kamere.localPosition = new Vector3(0f, crouch_height, 0f);
                speed = crouch_speed;

                is_crouching = true;
            }
        }
    }
}

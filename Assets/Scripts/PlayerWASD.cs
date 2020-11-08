using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class should only be for player movement - WASD style
public class PlayerWASD : MonoBehaviour {
    
    public GameObject sprite;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem spriteParticles;
    public float stepTime = 0.05f;
    private CharacterController controller;
    public float speed = 2f;

    void Start() {
        
        animator = sprite.GetComponent<Animator>();
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        spriteParticles = sprite.GetComponent<ParticleSystem>();
        controller = GetComponent<CharacterController>();
    }

    void Update() {

        stepTime -= Time.deltaTime;
        if (stepTime <= 0 && controller.velocity.magnitude > 0) {
            stepTime = 0.05f;
            spriteParticles.Play();
        }


        animator.SetFloat("speed", controller.velocity.magnitude / speed);
        bool isLeft;
        if (controller.velocity.x == 0) {
            isLeft = spriteRenderer.flipX;
        } else if (controller.velocity.x < 0) {
            isLeft = true;
        } else isLeft = false;
        spriteRenderer.flipX = isLeft;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), -2, Input.GetAxis("Vertical")).normalized * 2;
        Debug.DrawRay(gameObject.transform.position, move);
        int layerMask = 1 << 10;
        if (!Physics.Raycast(gameObject.transform.position, move, 5f, layerMask)) {
            controller.Move(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * Time.deltaTime * speed);
        } else Debug.Log("Hit wall");


        
        sprite.transform.forward = Camera.main.transform.forward;
    }
}

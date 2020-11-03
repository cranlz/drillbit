using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWASD : MonoBehaviour {
    public LayerMask whatCanBeClickedOn;
    public GameObject[] towers;
    public int towerCost = 5;
    private Transform buildPreview;
    public GameObject sprite;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem spriteParticles;
    public float stepTime = 0.05f;
    private CharacterController controller;
    public float speed = 2f;

    void Start() {
        buildPreview = transform.Find("BuildPreview");
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

        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButton(1)) {
            //rotate to mouse
            if (Physics.Raycast(myRay, out hit, 100, whatCanBeClickedOn)) {
                var lookPos = hit.point - transform.position;
                lookPos.y = 0;
                transform.rotation = Quaternion.LookRotation(lookPos);
            }
            if (Input.GetMouseButtonDown(0) && ConCollector.bank >= towerCost) {
                var newTower = Instantiate(towers[0], buildPreview.position, buildPreview.rotation);
                ConCollector.bank -= towerCost;
                Camera.main.GetComponent<CameraManager>().targets.Add(newTower.transform);
                Debug.Log("made tower");
            }
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), -2, Input.GetAxis("Vertical")).normalized * 2;
        Debug.DrawRay(gameObject.transform.position, move);
        int layerMask = 1 << 10;
        if (!Physics.Raycast(gameObject.transform.position, move, 5f, layerMask)) {
            controller.Move(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * Time.deltaTime * speed);
        } else Debug.Log("Hit wall");


        if (Input.GetMouseButtonDown(1)) {
            buildPreview.gameObject.SetActive(true);
        }
        if (Input.GetMouseButtonUp(1)) {
            buildPreview.gameObject.SetActive(false);
        }
        sprite.transform.forward = Camera.main.transform.forward;
    }
}

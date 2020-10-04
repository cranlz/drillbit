using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerController : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;
    IAstarAI ai;
    public GameObject[] towers;
    public int towerCost = 5;
    private Transform buildPreview;
    public GameObject sprite;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem spriteParticles;
    public float stepTime = 0.05f;
    void Start()
    {
        ai = GetComponent<IAstarAI>();
        if (ai != null) ai.onSearchPath += Update;
        buildPreview = transform.Find("BuildPreview");
        animator = sprite.GetComponent<Animator>();
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        spriteParticles = sprite.GetComponent<ParticleSystem>();
    }

    void Update() {

        stepTime -= Time.deltaTime;
        if (stepTime <= 0 && ai.velocity.magnitude > 0) {
            stepTime = 0.05f;
            spriteParticles.Play();
        }


        animator.SetFloat("speed", ai.velocity.magnitude / ai.maxSpeed);
        bool isLeft;
        if (ai.velocity.x == 0) {
            isLeft = spriteRenderer.flipX;
        } else if (ai.velocity.x < 0) {
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
            if (Input.GetMouseButtonDown(0) && BasicCollector.bank >= towerCost) {
                Instantiate(towers[0], buildPreview.position, buildPreview.rotation);
                BasicCollector.bank -= towerCost;
                Debug.Log("made tower");
            }
        }
        else if (Input.GetMouseButton(0)) {
            if (Physics.Raycast(myRay, out hit, 100, whatCanBeClickedOn)) {
                if (ai != null) ai.destination = hit.point;
            }
        }


        if (Input.GetMouseButtonDown(1)) {
            buildPreview.gameObject.SetActive(true);
        }
        if (Input.GetMouseButtonUp(1)) {
            buildPreview.gameObject.SetActive(false);
        }
        sprite.transform.forward = Camera.main.transform.forward;
    }
}

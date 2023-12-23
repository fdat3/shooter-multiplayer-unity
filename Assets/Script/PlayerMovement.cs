using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float rotateSpeed = 100.0f;
    private Rigidbody rb;
    private Animator anim;
    private Vector3 startPos;
    private bool respawned = false;
    private bool canJump = true;
    public bool isDead = false;
    public GameObject respawnPanel;
    public bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        startPos = transform.position;
        respawnPanel = GameObject.Find("Respawn_Panel");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead == false)
        {
            respawnPanel.SetActive(false);
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0,
            Input.GetAxis("Vertical")).normalized;
            Vector3 rotateY = new Vector3(0, Input.GetAxis("Mouse X") *
            rotateSpeed * Time.deltaTime, 0);
            if (movement != Vector3.zero)
            {
                rb.MoveRotation(rb.rotation * Quaternion.Euler(rotateY));
            }
            rb.MovePosition(rb.position + transform.forward * Input.GetAxis
            ("Vertical") * moveSpeed * Time.deltaTime + transform.right *
            Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime);
            anim.SetFloat("BlendV", Input.GetAxis("Vertical"));
            anim.SetFloat("BlendH", Input.GetAxis("Horizontal"));
        }
        if(isDead == true && respawned == false && gameOver == false)
        {
            respawned = true;
            respawnPanel.SetActive(true);
            respawnPanel.GetComponent<RespawnTimer>().enabled = true;
            StartCoroutine(RespawnWait());
        }
    }

    private void Update()
    {
        if (isDead == false)
        {
            if (Input.GetButtonDown("Jump") && canJump == true)
            {
                canJump = false;
                rb.AddForce(Vector3.up * 600 * Time.deltaTime,
                ForceMode.VelocityChange);
                StartCoroutine(JumpAgain());
            }
        }
    }
    IEnumerator JumpAgain()
    {
        yield return new WaitForSeconds(1);
        canJump = true;
    }

    IEnumerator RespawnWait()
    {
        yield return new WaitForSeconds(5);
        isDead = false;
        respawned = false;
        transform.position = startPos;
        GetComponent<DisplayColor>().Respawn(GetComponent<PhotonView>().Owner.NickName);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    private Rigidbody playerRb;
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround = true; // To check if the player is on the ground
    public bool gameOver = false;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround&& !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false; // Player is now in the air
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop(); // Stop the dirt particle effect when jumping
            playerAudio.PlayOneShot(jumpSound, 1.0f); // Play jump sound
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play(); // Play the dirt particle effect when landing
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            Debug.Log("Game Over!");
            playerAnim.SetBool("Death_b", true); // Trigger the death animation
            playerAnim.SetInteger("DeathType_int",1); // Randomly choose a death animation
            explosionParticle.Play(); // Play the explosion particle effect
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f); // Play crash sound
        }

    }
}
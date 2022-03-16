using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;

    private AudioSource audio;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        bool hasHorizontalInput = !Mathf.Approximately(x, 0f);
        bool hasVerticalInput = !Mathf.Approximately(z, 0f);

        bool isWalking = hasHorizontalInput || hasVerticalInput;

        if (isWalking)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }

        else
        {
            audio.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PistolaEscondida")
        {
            Destroy(other.gameObject);
            EventManagerLvl1.current.PickUpGunTrigger();
        }
    }
}

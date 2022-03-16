using UnityEngine;

public class FirstPersonPlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public GameEnding gameEnding;

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



    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "PistolaEscondida")
        {
            Destroy(collision.gameObject);
            EventManagerLvl1.current.PickUpGunTrigger();
        }

        if (collision.gameObject.tag == "Bullets")
        {
            Destroy(collision.gameObject);
            EventManagerLvl1.current.PickUpBulletsTrigger();
        }

        if(collision.gameObject.tag == "Enemy")
        {
            gameEnding.CaughtPlayer();
        }
    }
}

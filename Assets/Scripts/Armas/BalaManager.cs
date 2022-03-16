using UnityEngine;

public class BalaManager : MonoBehaviour
{
    Rigidbody rb;
    float speed = 50;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 30);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * speed;
    }
}

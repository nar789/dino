using UnityEngine;

public class BoltController : MonoBehaviour
{

    public float speed = 2f;
    private Rigidbody rigidbody;

    bool isHit = false;

    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isHit)
        {
            rigidbody.linearVelocity = Vector3.zero;
        }
        else
        {
            transform.Translate(-Vector3.up * speed * Time.deltaTime);
        }
    }
    void Update()
    {
        if (transform.position.x > 20 || transform.position.x < -20 || transform.position.z < -100 || transform.position.z > 100)
        {
            Destroy(gameObject);
        }

     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            isHit = true;
            other.GetComponent<EnemyController>().hitRemote(10);
            Destroy(gameObject);
        }
    }
}

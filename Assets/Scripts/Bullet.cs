using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{

    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float lifetime = 10f;

    [Server]
    public void Fire(Vector3 direction)
    {
        GetComponent<Rigidbody>().velocity = direction * speed;
    }

    private void Update()
    {
        if (isServer)
        {
            lifetime -= Time.deltaTime;
            if (lifetime < 0f)
            {
                NetworkServer.Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isServer)
        {
            if (collision.gameObject.GetComponent<Target>())
            {
                NetworkServer.Destroy(collision.gameObject);
            }

            NetworkServer.Destroy(gameObject);
        }
    }
}

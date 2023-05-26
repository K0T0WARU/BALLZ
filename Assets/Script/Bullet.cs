using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 75;
    private Transform target;
    private bool homing;

    private float rocketStrength = 15;

    private void Update()
    {
        if (homing && target != null)
        {
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            transform.LookAt(moveDirection);
            transform.position += moveDirection * speed * Time.deltaTime;
        }
        else
            Destroy(gameObject);
    }

    public void Fire(Transform newTraket)
    {
        target = newTraket;
        homing = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (target != null)
            if (collision.gameObject.CompareTag(target.tag))
            {
                Rigidbody targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 away = -collision.contacts[0].normal;
                targetRigidbody.AddForce(away * rocketStrength, ForceMode.Impulse);
                Destroy(gameObject);
            }
    }
}

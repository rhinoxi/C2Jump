using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RecursiveMovePlatform : MonoBehaviour
{
    public Vector2 direction;
    public float totalDistance;
    public float velocity;

    private Rigidbody2D rb;
    private float distance = 0;
    private bool wait;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (wait) return;
        if (distance >= totalDistance) {
            rb.velocity = Vector2.zero;
            distance = 0;
            direction *= -1;

            StartCoroutine(Wait(3));
            return;
        }

        rb.velocity = direction.normalized * velocity;
        distance += velocity * Time.fixedDeltaTime;
    }

    IEnumerator Wait(int seconds) {
        wait = true;
        yield return new WaitForSeconds(seconds);
        wait = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAccMovePlatform : MonoBehaviour
{
    public LayerMask triggerLayer;
    public Vector2 endPos;
    public float totalTime;
    public float relaxTime;
    private Rigidbody2D rb;
    private bool triggerable;
    private Vector2 startPos;
    private Vector2 currentPos;
    private Vector2 nextPos;
    private float returnTotalTime;
    private bool trigger;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        triggerable = true;
    }

    private void Update() {
        if (trigger && triggerable) {
            StartCoroutine(Move());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (1 << collision.gameObject.layer == triggerLayer) {
            trigger = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        trigger = false;
    }

    IEnumerator Move() {
        triggerable = false;

        yield return new WaitForSeconds(relaxTime);

        startPos = transform.position;
        currentPos = startPos;
        float timeCount = 0;
        while (Vector2.SqrMagnitude(currentPos - endPos) > Vector2.kEpsilon) {
            // SpeedUp
            nextPos = Vector2.Lerp(startPos, endPos, 1f + Mathf.Sin((timeCount / totalTime - 1f) * Mathf.PI / 2f));
            rb.velocity = (nextPos - currentPos) / Time.fixedDeltaTime;
            currentPos = nextPos;
            timeCount += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(relaxTime);

        timeCount = 0;
        returnTotalTime = 4 * totalTime;
        rb.velocity = (startPos - endPos) / returnTotalTime;
        while (!Mathf.Approximately(timeCount, returnTotalTime)) {
            timeCount += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.velocity = Vector2.zero;

        triggerable = true;
    }
}

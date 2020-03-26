using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster
{
    private float lifeTime;
    private Vector2 boostVelocity;
    private float lifeTimerX;
    private float lifeTimerY;
    private Vector2 lastVelocity;
    private float accX;
    private float accY;
    private float deltaTime;

    public Booster(float lt, float dt) {
        lifeTime = lt;
        lifeTimerX = 0;
        lifeTimerY = 0;
        deltaTime = dt;
    }

    public float X {
        get {
            return accX;
        }
    }

    public float Y {
        get {
            return accY;
        }
    }

    private void SetX(float v) {
        accX = v;
        lifeTimerX = lifeTime;
    }

    private void SetY(float v) {
        accY = v;
        lifeTimerY = lifeTime;
    }

    public void Update(Vector2 velocity) {
        float tempX = 0;
        float tempY = 0;

        tempX = CalcBoost(lastVelocity.x, velocity.x, deltaTime);
        if ((tempX * accX > 0 && Mathf.Abs(tempX) > Mathf.Abs(accX)) || tempX * accX < 0) {
            SetX(tempX);
        } else {
            SetX(tempX);
        }

        tempY = CalcBoost(lastVelocity.y, velocity.y, deltaTime);
        if ((tempY * accY > 0 && Mathf.Abs(tempY) > Mathf.Abs(accY)) || tempY * accY < 0) {
            SetY(tempY);
        } else {
            SetY(tempY);
        }

        lastVelocity = velocity;
    }

    public IEnumerator Run() {
        while (true) {
            if (lifeTimerX > 0) {
                lifeTimerX -= Time.deltaTime;
                BoostDecrease(accX, lifeTimerX);
            }

            if (lifeTimerY > 0) {
                lifeTimerY -= Time.deltaTime;
                BoostDecrease(accY, lifeTimerY);
            }
            yield return null;
        }
    }

    private float CalcBoost(float formerValue, float currentValue, float t) {
        if (formerValue * currentValue >= 0 && Mathf.Abs(currentValue) < Mathf.Abs(formerValue)) {
            return 0;
        }
        // Opposite direction
        return (currentValue - formerValue) / t;
    }

    private float BoostDecrease(float value, float timer) {
        return Mathf.Lerp(value, 0, Mathf.Cos(timer / lifeTime * Mathf.PI / 2));
    }
}

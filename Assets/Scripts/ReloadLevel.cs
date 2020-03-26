using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadLevel : MonoBehaviour
{
    private void Update() {
        if (transform.position.y < -16) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

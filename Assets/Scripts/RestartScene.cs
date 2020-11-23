     using UnityEngine;
     using UnityEngine.SceneManagement;
     using System.Collections;
     
     public class RestartScene : MonoBehaviour {
     
         public void RestartGame() {
             SceneManager.LoadScene(SceneManager.GetActiveScene().name);
             ConCollector.bank = 0;
         }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            RestartGame();
        }
    }

}
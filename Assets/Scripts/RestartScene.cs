﻿     using UnityEngine;
     using UnityEngine.SceneManagement;
     using System.Collections;
     
     public class RestartScene : MonoBehaviour {
     
         public void RestartGame() {
             SceneManager.LoadScene(SceneManager.GetActiveScene().name);
             BasicCollector.bank = 10;
         }
     
     }
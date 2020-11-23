using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameover : MonoBehaviour
{
    public GameObject mainRig;
    public Image blackScreen;
    public Text gameOverText;

    // Update is called once per frame
    void Update()
    {
        if (mainRig == null) {
            var screenColor = new Color();
            screenColor.a = Mathf.Lerp(blackScreen.color.a, 1, 0.05f);
            screenColor.r = blackScreen.color.r;
            screenColor.g = blackScreen.color.g;
            screenColor.b = blackScreen.color.b;
            blackScreen.color = screenColor;

            var textColor = new Color();
            textColor.a = Mathf.Lerp(gameOverText.color.a, 1, 0.05f);
            textColor.r = gameOverText.color.r;
            textColor.g = gameOverText.color.g;
            textColor.b = gameOverText.color.b;
            gameOverText.color = textColor;
        }
    }
}

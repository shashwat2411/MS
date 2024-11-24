using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenuUI : MonoBehaviour
{
    public Image title;
    public float size1;
    public float size2;

    public float speed = 0.4f;

    float counter = 0;
    bool switcher = false;
    private void FixedUpdate()
    {
        if (switcher == false)
        {
            counter += Time.deltaTime;

            if (counter < speed)
            {
                title.GetComponent<RectTransform>().localScale = Vector3.Lerp(title.GetComponent<RectTransform>().localScale, new Vector3(size1, size1, size1), 0.5f);
            }
            else if (counter >= speed) { counter = speed; switcher = true; }
        }
        else
        {
            counter -= Time.deltaTime;

            if (counter > 0f)
            {
                title.GetComponent<RectTransform>().localScale = Vector3.Lerp(title.GetComponent<RectTransform>().localScale, new Vector3(size2, size2, size2), 0.5f);
            }
            else if (counter <= 0f) { counter = 0f; switcher = false; }
        }
    }

    public void MainGame(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        FindFirstObjectByType<GameManager>().SceneChange();
    }
}

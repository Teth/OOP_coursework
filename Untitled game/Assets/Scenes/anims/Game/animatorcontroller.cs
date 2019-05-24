using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class animatorcontroller : MonoBehaviour
{
    TextMeshProUGUI tmp;
    bool paused;
    AMC_Factory menuFactory;
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    GameObject canvas;
    // Start is called before the first frame update

     
    void Start()
    {
        canvas.SetActive(false);
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = "Level " + PlayerPrefs.GetInt("LevelCleared");
        paused = false;
        menuFactory = new AMC_Factory(buttonPrefab, canvas.GetComponent<Canvas>());
        MenuElement ToMainMenu = new MenuElement("Main Menu", () =>
        {
            SceneManager.LoadScene("Menu");
            Time.timeScale = 1;
        });
        AbstractMenuComposite menuTop = menuFactory.createSubmenu("TopMenuItem", new List<AbstractMenuComposite> { ToMainMenu });
        menuTop.ClickOperation();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                canvas.SetActive(false);
                Time.timeScale = 1;
                paused = false;
            }
            else
            {
                canvas.SetActive(true);
                Time.timeScale = 0;
                paused = true;
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameoverscript : MonoBehaviour
{
    TextMeshProUGUI tmp;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponentsInChildren<TextMeshProUGUI>()[1];
        tmp.text = tmp.text = "You've reached " + PlayerPrefs.GetInt("LevelCleared") + " Level";
        PlayerPrefs.SetInt("LevelCleared", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

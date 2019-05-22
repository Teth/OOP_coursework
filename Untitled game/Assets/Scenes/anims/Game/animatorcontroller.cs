using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class animatorcontroller : MonoBehaviour
{
    TextMeshProUGUI tmp;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = "Level " + PlayerPrefs.GetInt("LevelCleared");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

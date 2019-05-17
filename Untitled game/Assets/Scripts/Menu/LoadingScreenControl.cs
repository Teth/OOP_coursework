using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenControl : MonoBehaviour
{
    public GameObject loadingScrObject;
    public Slider slider;

    AsyncOperation async;

    public void Start()
    {
        run();
    }

    void run()
    {
        StartCoroutine(LoadingScreen());
    }

    IEnumerator LoadingScreen()
    {
        loadingScrObject.SetActive(true);
        async = SceneManager.LoadSceneAsync("SampleScene");
        async.allowSceneActivation = false;

        while(async.isDone == false)
        {
            Debug.Log(async.progress);
            slider.value = async.progress;
            if(async.progress == 0.9f)
            {
                slider.value = 1f;
                async.allowSceneActivation = true;
            }
            yield return null; 
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

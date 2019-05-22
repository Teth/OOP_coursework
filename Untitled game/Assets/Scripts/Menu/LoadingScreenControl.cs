using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenControl : MonoBehaviour
{
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
        async = SceneManager.LoadSceneAsync("SampleScene");

        while (!async.isDone)
        { 
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//Debug.Log(async.progress);
//            slider.value = async.progress;
//            if(async.progress == 0.9f)
//            {
//                slider.value = 1f;
//                async.allowSceneActivation = true;
//            }
//            Debug.Log("Finished");
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
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadingScreen()
    {
        async = SceneManager.LoadSceneAsync("SampleScene");

        loadingScrObject.SetActive(true);

        while (!async.isDone)
        {
            float prog = Mathf.Clamp01(async.progress / 0.9f);

            slider.value = prog;

            yield return null;
        }
    }

    IEnumerator LoadScene()
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("SampleScene");
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                slider.value = asyncOperation.progress;
                //Wait to you press the space key to activate the Scene
                if (Input.GetKeyDown(KeyCode.Space))
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

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
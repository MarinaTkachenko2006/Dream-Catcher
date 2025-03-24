using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }
    public Animator transition;
    public float transitionTime = 1f;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadNamedLevel(levelName));
    }
    public void LoadLevelFast(string levelName)
    {
        StartCoroutine(LoadNamedLevelFast(levelName));
    }

    IEnumerator LoadNamedLevel(string levelName)
    {
        // Start transition animation
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);

        // End transition animation
        transition.SetTrigger("End");
    }

    IEnumerator LoadNamedLevelFast(string levelName)
    {
        transition.SetTrigger("Start");

        float minTransitionDuration = transitionTime * 0.75f;
        float startTime = Time.time;

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);
        operation.allowSceneActivation = false;

        while (Time.time - startTime < minTransitionDuration)
        {
            yield return null;
        }
        operation.allowSceneActivation = true;
        yield return new WaitForEndOfFrame();

        transition.SetTrigger("End");
    }

}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfterDelay : MonoBehaviour
{
    public float delayTime = 5f;

    public string sceneNameToLoad;

    void Start()
    {
        if (string.IsNullOrEmpty(sceneNameToLoad))
        {
            Debug.LogError("Bạn quên gán 'Scene Name To Load' trong Inspector!");
            return;
        }

        StartCoroutine(LoadSceneCoroutine());
    }
    IEnumerator LoadSceneCoroutine()
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(sceneNameToLoad);
    }
}
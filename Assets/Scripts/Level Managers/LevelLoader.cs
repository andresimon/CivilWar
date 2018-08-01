using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string levelToLoad;

    IEnumerator LoadLevelCo()
    {
        float fadeTime = GetComponent<Fading>().BeginFade(1);

        yield return new WaitForSeconds(fadeTime);

        SceneManager.LoadScene(levelToLoad);
    }

    public void LoadLevel()
    {
        StartCoroutine(LoadLevelCo());
    }

}

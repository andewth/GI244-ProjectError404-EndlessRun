using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public string sceneToLoad = "GameScene";
    public AudioSource audioSource;
    public AudioClip soundClip;

    public void LoadGameScene()
    {
        audioSource.PlayOneShot(soundClip);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

}
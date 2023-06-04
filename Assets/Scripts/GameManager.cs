using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    // UI element to display the highscore
    [SerializeField] TextMeshProUGUI highscoreText;

    // Input action to load the game
    [SerializeField] InputAction leftClick;

    [SerializeField] AudioMixer audioMixer;

    // En- / Disable input manager when object is disapled or not
    private void OnEnable() => leftClick.Enable();
    private void OnDisable() => leftClick.Disable();

    IEnumerator SceneChange()
    {
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "MasterVolume", 2f, 0f));
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Level");

    }
    // Start is called before the first frame update
    void Start(){
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "MasterVolume", 2f, 1f));
        highscoreText.text = $"{PlayerPrefs.GetInt("highscore")}";
    } 

    // Update is called once per frame
    void Update(){
         leftClick.performed += _ => StartCoroutine(nameof(SceneChange));
    }
}

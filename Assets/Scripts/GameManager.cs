using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // UI element to display the highscore
    [SerializeField] TextMeshProUGUI highscoreText;

    // Input action to load the game
    [SerializeField] InputAction leftClick;

    // En- / Disable input manager when object is disapled or not
    private void OnEnable() => leftClick.Enable();
    private void OnDisable() => leftClick.Disable();

    // Start is called before the first frame update
    void Start() => highscoreText.text = $"{PlayerPrefs.GetInt("highscore")}";

    // Update is called once per frame
    void Update() => leftClick.performed += _ => SceneManager.LoadScene("Level");
}

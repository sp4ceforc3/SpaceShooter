using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Scriptable object to store the highscore
    [SerializeField] HighScore highscore;

    // UI element to display the highscore
    [SerializeField] TextMeshProUGUI highscoreText;

    // Input action to load the game
    [SerializeField] InputAction leftClick;

    // En- / Disable input manager when object is disapled or not
    private void OnEnable() => leftClick.Enable();
    private void OnDisable() => leftClick.Disable();

    // Start is called before the first frame update
    void Start() => highscoreText.text = $"{highscore.completedWaves}";

    // Update is called once per frame
    void Update()
    {
        // When a left click is perfomed, load scene "Level"
        leftClick.performed += loadLevel => SceneManager.LoadScene("Level");
    }
}

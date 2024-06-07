using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class S_Menu : MonoBehaviour
{
    public S_GameManager gameManager;
    public AudioSource musicPlayer;
    public TextMeshProUGUI musicButtonText;
    public TMP_Dropdown colorCountDropdown;
    public TextMeshProUGUI startButtonText;
    public GameObject gui;

    private void Start()
    {
        musicPlayer.mute = false;
        UpdateButtonText();
    }

    public void ToggleMusic()
    {
        musicPlayer.mute = !musicPlayer.mute;
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        if (musicPlayer.mute) musicButtonText.text = "Music On";
        else musicButtonText.text = "Music Off";
    }

    public void StartButton()
    {
        gameObject.GetComponent<Canvas>().enabled = false;

        gui.GetComponent<Canvas>().enabled = true;

        colorCountDropdown.interactable = false;

        startButtonText.text = "Continue";

        gameManager.numberOfColorsUsed = colorCountDropdown.value + 1;
        if (gameManager.balls.Count <= 1) gameManager.SpawnBalls();
    }

    public void ExitButton()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OpenMenu()
    {
        gui.GetComponent<Canvas>().enabled = false;
        gameObject.GetComponent<Canvas>().enabled = true;
    }
}

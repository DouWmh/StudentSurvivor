using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] TMP_Text timeSurvived;
    public void OnContinueBtnClick()
    {
        SceneManager.LoadScene("Game");
    }
    public void OnQuitBtnClick()
    {
        SceneManager.LoadScene("Title");
    }
    private void Start()
    {
        timeSurvived.text = GameManager.timeText;
    }
}

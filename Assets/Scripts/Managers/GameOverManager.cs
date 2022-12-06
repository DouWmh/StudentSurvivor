using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] TMP_Text timeSurvived;
    [SerializeField] TMP_Text zomb;
    [SerializeField] TMP_Text eZomb;
    [SerializeField] TMP_Text merm;
    [SerializeField] TMP_Text eMerm;
    [SerializeField] TMP_Text rogue;
    [SerializeField] TMP_Text eRogue;
    [SerializeField] TMP_Text giant;
    [SerializeField] TMP_Text coin;
    [SerializeField] TMP_Text crystal;

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
        zomb.text = GameManager.Kills.ContainsKey("Zombie") ? GameManager.Kills["Zombie"].ToString() :"0" ;
        eZomb.text = GameManager.Kills.ContainsKey("Zombie Elite") ? GameManager.Kills["Zombie Elite"].ToString() : "0";
        merm.text = GameManager.Kills.ContainsKey("Merman") ? GameManager.Kills["Merman"].ToString() : "0";
        eMerm.text = GameManager.Kills.ContainsKey("Merman Elite") ? GameManager.Kills["Merman Elite"].ToString() : "0";
        rogue.text = GameManager.Kills.ContainsKey("Rogue") ? GameManager.Kills["Rogue"].ToString() : "0";
        eRogue.text = GameManager.Kills.ContainsKey("Rogue Elite") ? GameManager.Kills["Rogue Elite"].ToString() : "0";
        giant.text = GameManager.Kills.ContainsKey("Giant") ? GameManager.Kills["Giant"].ToString() : "0";
        coin.text = GameManager.Collected.ContainsKey("Coin") ? GameManager.Collected["Coin"].ToString() : "0";
        crystal.text = GameManager.Collected.ContainsKey("Crystal") ? GameManager.Collected["Crystal"].ToString() : "0";
        GameManager.Kills.Clear();
        GameManager.Collected.Clear();
    }
}

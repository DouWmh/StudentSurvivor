using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public static int currentPlayer = 1;//1 Julius  2 Edge
    public static int currentLevel = 1;
    public static bool URPenabled = true;
    public static SavedData saveData;
    string SavePath => Path.Combine(Application.persistentDataPath, "save.data");

    [SerializeField] GameObject upgradeMenu;
    [SerializeField] GameObject charSelMenu;
    [SerializeField] GameObject lvlSelMenu;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text atkLvlText;
    [SerializeField] TMP_Text hpLvlText;
    [SerializeField] TMP_Text postProcText;
    [SerializeField] GameObject lockImage;
    [SerializeField] GameObject lockImage2;

    private void Awake()
    {
        if (URPenabled)
        {
            postProcText.text = "Post-Processing\nON";
        }
        else
        {
            postProcText.text = "Post-Processing\nOFF";
        }
        if (saveData == null)
            Load();
        else
            Save();
        Debug.Log($"Deaths : {saveData.deathCount} Gold : {saveData.goldCoins} Edge {saveData.edgeUnlocked} Forest {saveData.levelUnlocked}");
    }
    private void Save()
    {
        FileStream file = null;
        try
        {
            if (!Directory.Exists(Application.persistentDataPath))
            {
                Directory.CreateDirectory(Application.persistentDataPath);
            }

            file = File.Create(SavePath);
            BinaryFormatter bf = new();
            bf.Serialize(file, saveData);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            if (file != null)
                file.Close();
        }
    }

    private void Load()
    {
        FileStream file = null;
        try
        {
            file = File.Open(SavePath, FileMode.Open);
            var bf = new BinaryFormatter();
            saveData = (SavedData)bf.Deserialize(file);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            saveData = new SavedData();
        }
        finally
        {
            if (file != null)
                file.Close();
        }
    }
    private void SelectCharacter()
    {
        lockImage2.SetActive(!saveData.edgeUnlocked);
        charSelMenu.SetActive(true);
    }
    public void OnStartBtnClick()
    {
        SelectCharacter();
    }
    public void OnUpgradeBtnClick()
    {

        upgradeMenu.SetActive(true);
        lockImage.SetActive(!saveData.edgeUnlocked);
        RefreshTexts();
    }
    public void OnQuitBtnClick()
    {
        Application.Quit();
    }
    public void OnAtkUpgradeClick()
    {
        if (saveData.goldCoins >= 5)
        {
            saveData.permAtkLvl++;
            saveData.goldCoins -= 5;
            RefreshTexts();
        }
    }
    public void OnHPUpgradeClick()
    {
        if (saveData.goldCoins >= 5)
        {
            saveData.permHpLvl++;
            saveData.goldCoins -= 5;
            RefreshTexts();
        }
    }
    public void OnReturnToMenuClick()
    {
        upgradeMenu.SetActive(false);
        charSelMenu.SetActive(false);
        lvlSelMenu.SetActive(false);
    }
    public void OnPostProcessingClick()
    {
        URPenabled = !URPenabled;
        postProcText.text = "Post-Processing\n" + (URPenabled ? "ON" : "OFF");
    }
    public void OnUnlockEdgeClick()
    {
        if (saveData.goldCoins >= 10 && saveData.edgeUnlocked == false)
        {
            saveData.goldCoins -= 10;
            saveData.edgeUnlocked = true;
            Save();
            lockImage.SetActive(!saveData.edgeUnlocked);
            RefreshTexts();
        }
    }
    public void OnJuliusClick()
    {
        TitleManager.currentPlayer = 1;
        CheckLevelSelection();
    }

    private void CheckLevelSelection()
    {
        if (!saveData.levelUnlocked)
            SceneManager.LoadScene(currentLevel);
        else
        {
            charSelMenu.SetActive(false);
            lvlSelMenu.SetActive(true);
        }
    }

    public void OnEdgeClick()
    {
        if (saveData.edgeUnlocked)
        {
            TitleManager.currentPlayer = 2;
            CheckLevelSelection();
        }
    }
    public void OnPlainClick()
    {
        currentLevel = 1;
        SceneManager.LoadScene(currentLevel);
    }
    public void OnForestClick()
    {
        currentLevel = 2;
        SceneManager.LoadScene(currentLevel);
    }
    private void RefreshTexts()
    {
        coinsText.text = $"Coins : {saveData.goldCoins}";
        atkLvlText.text = $"Lv. {saveData.permAtkLvl}";
        hpLvlText.text = $"Lv. {saveData.permHpLvl}";
    }
}

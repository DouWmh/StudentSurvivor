using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public static SavedData saveData;
    string SavePath => Path.Combine(Application.persistentDataPath, "save.data");

    [SerializeField] GameObject upgradeMenu;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text atkLvlText;
    [SerializeField] TMP_Text hpLvlText;

    private void Awake()
    {
        if (saveData == null)
            Load();
        else
            Save();
        Debug.Log($"Deaths : {saveData.deathCount} Gold : {saveData.goldCoins}");
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
            var bf = new BinaryFormatter();
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

    public void OnStartBtnClick()
    {
        SceneManager.LoadScene("Game");
    }
    public void OnUpgradeBtnClick()
    {

        upgradeMenu.SetActive(true);
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
    public void OnQuitUpgradeClick()
    {
        upgradeMenu.SetActive(false);
    }
    private void RefreshTexts()
    {
        coinsText.text = $"Coins : {saveData.goldCoins}";
        atkLvlText.text = $"Lv. {saveData.permAtkLvl}";
        hpLvlText.text = $"Lv. {saveData.permHpLvl}";

    }
}

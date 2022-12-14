using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] GameObject mainCharacter;
    [SerializeField] GameObject secondCharacter;
    public static GameObject player;
    Player playerScript;
    static bool MainPlayer = true;
    [SerializeField] PlayerCamera cam;

    [SerializeField] Canvas enemyHpCanvas;
    [SerializeField] float spawnDistance = 5f;
    public static Dictionary<string, int> Kills = new Dictionary<string, int>();
    public static Dictionary<string, int> Collected = new Dictionary<string, int>();

    [SerializeField] TMP_Text gameTimerText;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] float flashSpeed = 1f;

    [Header("LevelUp Menu")]
    [SerializeField] GameObject lvlUpMenu;
    [SerializeField] TMP_Text lvlOption1;
    [SerializeField] TMP_Text lvlOption2;
    [SerializeField] TMP_Text lvlOption3;
    [SerializeField] TMP_Text lvlOption4;
    [SerializeField] TMP_Text lvlDescription1;
    [SerializeField] TMP_Text lvlDescription2;
    [SerializeField] TMP_Text lvlDescription3;
    [SerializeField] TMP_Text lvlDescription4;
    [SerializeField] TMP_Text lvlUnlockText;

    public Dictionary<string, string> lvlUpDescription = new();
    public List<string> lvlUpOptions = new();

    int minutes = 0, seconds = 0;
    public static string timeText;
    public static bool isPaused = false;
    public static bool isLeveling = false;

    void Awake()
    {
        if (TitleManager.currentPlayer == 2)
        {
            Destroy(mainCharacter);
            player = secondCharacter;
        }
        else
        {
            Destroy(secondCharacter);
            player = mainCharacter;
        }
        player.gameObject.SetActive(true);
        if (TitleManager.saveData == null)
            TitleManager.saveData = new SavedData();
        //sounds = player.GetComponents<AudioSource>();
        if (lvlUpOptions.Count == 0)
        {
            lvlUpOptions.Add("KatanaSize");
            lvlUpOptions.Add("KatanaDamage");
            lvlUpOptions.Add("AxeSize");
            lvlUpOptions.Add("AxeDamage");
            lvlUpOptions.Add("ScytheDamage");
            lvlUpOptions.Add("ScytheNumber");
            lvlUpOptions.Add("EnergyDamage");
            lvlUpOptions.Add("EnergyRange");
            lvlUpOptions.Add("AttackPowerUp");
            lvlUpOptions.Add("HpPowerUp");
            lvlUpOptions.Add("Invincibility");

            lvlUpDescription.Add(lvlUpOptions[0], "Upgrades the size of the Katana");
            lvlUpDescription.Add(lvlUpOptions[1], "Upgrades the damage of the Katana");
            lvlUpDescription.Add(lvlUpOptions[2], "Upgrades the size of the Axe");
            lvlUpDescription.Add(lvlUpOptions[3], "Upgrades the damage of the Axe");
            lvlUpDescription.Add(lvlUpOptions[4], "Upgrades the damage of the Scythe");
            lvlUpDescription.Add(lvlUpOptions[5], "Upgrades the number of Scythes thrown");
            lvlUpDescription.Add(lvlUpOptions[6], "Upgrades the damage of the Energy Ball");
            lvlUpDescription.Add(lvlUpOptions[7], "Upgrades the range of the Energy Ball");
            lvlUpDescription.Add(lvlUpOptions[8], "Upgrades your raw Attack Power");
            lvlUpDescription.Add(lvlUpOptions[9], "Upgrades your raw Hit Points");
            lvlUpDescription.Add(lvlUpOptions[10], "Become Invincible for 5 Seconds");
        }
        playerScript = player.GetComponent<Player>();
        if (TitleManager.currentLevel == 2)
        {
            StartCoroutine(SpawnEnemiesCoroutineLevel2());
        }
        else
        {
            StartCoroutine(SpawnEnemiesCoroutineLevel1());
        }
    }
    private void Start()
    {
        if (lvlUnlockText != null)
            lvlUnlockText.gameObject.SetActive(false);
    }
    public void DisplayLevelUpOptions(List<int> rndOptions)
    {
        playerScript.PlayLevelUpSound();
        lvlUpMenu.SetActive(true);
        isLeveling = true;
        cam.PauseGame();
        lvlOption1.text = lvlUpOptions[rndOptions[0]];
        lvlOption2.text = lvlUpOptions[rndOptions[1]];
        lvlOption3.text = lvlUpOptions[rndOptions[2]];
        lvlOption4.text = lvlUpOptions[rndOptions[3]];
        lvlDescription1.text = lvlUpDescription[lvlOption1.text];
        lvlDescription2.text = lvlUpDescription[lvlOption2.text];
        lvlDescription3.text = lvlUpDescription[lvlOption3.text];
        lvlDescription4.text = lvlUpDescription[lvlOption4.text];
    }
    public void OnOption1Click()
    {
        playerScript.LevelUpSkill(lvlOption1.text);
        FinishLeveling();
    }
    public void OnOption2Click()
    {
        playerScript.LevelUpSkill(lvlOption2.text);
        FinishLeveling();
    }
    public void OnOption3Click()
    {
        playerScript.LevelUpSkill(lvlOption3.text);
        FinishLeveling();
    }
    public void OnOption4Click()
    {
        playerScript.LevelUpSkill(lvlOption4.text);
        FinishLeveling();
    }
    public void OnResumeClick()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseScreen.SetActive(false);
        cam.UnPauseGame();
    }
    public void OnMainMenuClick()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseScreen.SetActive(false);
        cam.UnPauseGame();
        SceneManager.LoadScene(0);
    }
    private void FinishLeveling()
    {
        Time.timeScale = 1f;
        lvlUpMenu.SetActive(false);
        isLeveling = false;
        cam.UnPauseGame();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isLeveling)
        {
            if (!isPaused)
            {
                Time.timeScale = 0;
                isPaused = true;
                pauseScreen.SetActive(true);
                cam.PauseGame();
            }
            else
            {
                Time.timeScale = 1f;
                isPaused = false;
                pauseScreen.SetActive(false);
                cam.UnPauseGame();
            }
        }
        if (player != null)
        {
            seconds = (int)Time.timeSinceLevelLoad - minutes * 60;
            minutes = (int)Time.timeSinceLevelLoad / 60;
        }
        timeText = $"{minutes} : {seconds:00}";
        gameTimerText.text = timeText;
    }
    public void ResetTimer()
    {
        seconds = 0;
        minutes = 0;
    }
    public void Spawn(string tag, int numEnemiesToSpawn, bool isChasing = true)
    {
        if (player != null)
        {
            Vector3 spawnPosition;
            for (int i = 0; i < numEnemiesToSpawn; i++)
            {
                Vector3 barOffset = new Vector3(0, 1f, 0);

                spawnPosition = UnityEngine.Random.insideUnitCircle.normalized * spawnDistance;
                spawnPosition += player.transform.position;
                GameObject go = ObjectPooler.Instance.SpawnFromPool(tag, spawnPosition, Quaternion.identity);
                Enemy enemy = go.GetComponent<Enemy>();
                enemy.SetupHealthBar(enemyHpCanvas);
                if (!isChasing)
                {
                    if (enemy.transform.position.x > player.transform.position.x)
                        enemy.movementDirection = Enemy.Direction.Left;
                    else
                        enemy.movementDirection = Enemy.Direction.Right;
                }

                else
                    enemy.movementDirection = Enemy.Direction.Chase;
            }
        }
    }
    IEnumerator SpawnEnemiesCoroutineLevel1()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 3; i++)
        {
            Spawn("Zombie", 2);
            Spawn("Merman", 2);
            Spawn("Giant", 1);
            yield return new WaitForSeconds(4);
            Spawn("Rogue", 3);
            yield return new WaitForSeconds(2);
            Spawn("Rogue", 10, isChasing: false);
            yield return new WaitForSeconds(4);
            Spawn("Merman", 2);
            Spawn("Zombie", 2);
            yield return new WaitForSeconds(4);
            Spawn("Rogue", 2);
            Spawn("Merman", 2);
            Spawn("Zombie", 4);
            yield return new WaitForSeconds(4);
            Spawn("Zombie", 15, isChasing: false);
        }


        yield return new WaitForSeconds(6f);
        if (!TitleManager.saveData.levelUnlocked)
        {
            StageUnlock();
        }
        yield return new WaitForSeconds(8f);
        Spawn("Slime", 1);
        Spawn("Giant", 6);

        // 1 minute
        Spawn("Merman", 30, isChasing: false);
        yield return new WaitForSeconds(6f);
        for (int i = 0; i < 2; i++)
        {
            Spawn("Merman", 6);
            Spawn("Rogue", 6);
            Spawn("Zombie", 6);
            yield return new WaitForSeconds(3f);
            Spawn("Rogue Elite", 5, false);
            yield return new WaitForSeconds(3f);
            Spawn("Merman Elite", 5, false);
            yield return new WaitForSeconds(3f);
            Spawn("Zombie Elite", 5, false);
            yield return new WaitForSeconds(3f);
            Spawn("Merman", 5);
            yield return new WaitForSeconds(3f);
            Spawn("Rogue", 6);
            yield return new WaitForSeconds(3f);
            Spawn("Zombie", 6);
            yield return new WaitForSeconds(3f);
            Spawn("Merman", 20);
            yield return new WaitForSeconds(3f);
            Spawn("Rogue", 8, false);
            Spawn("Merman", 12, false);
            yield return new WaitForSeconds(4f);
            Spawn("Rogue Elite", 12, false);
            Spawn("Merman Elite", 12, false);
            yield return new WaitForSeconds(3f);
            Spawn("Zombie", 20);
            yield return new WaitForSeconds(8f);
            Spawn("Rogue", 6);
            Spawn("Merman", 6);
            yield return new WaitForSeconds(3f);
        }
        Spawn("Giant", 10);
        yield return new WaitForSeconds(2f);

        //2minutes 30
        for (int i = 0; i < 3; i++)
        {
            Spawn("Zombie Elite", 3);
            Spawn("Merman Elite", 3);
            yield return new WaitForSeconds(4);
            Spawn("Rogue Elite", 5);
            yield return new WaitForSeconds(4);
            Spawn("Rogue Elite", 12, isChasing: false);
            Spawn("Zombie Elite", 12, isChasing: false);
            yield return new WaitForSeconds(6);
            Spawn("Merman Elite", 6);
            Spawn("Zombie Elite", 6);
            yield return new WaitForSeconds(4);
            Spawn("Rogue Elite", 3);
            Spawn("Merman Elite", 3);
            Spawn("Zombie Elite", 3);
            yield return new WaitForSeconds(3);
            Spawn("Giant", 10);
            Spawn("Merman Elite", 15, isChasing: false);
            yield return new WaitForSeconds(4);
        }
        if (!TitleManager.saveData.levelUnlocked)
        {
            StageUnlock();
        }
        //3 minutes 30
        yield return new WaitForSeconds(8f);
        Spawn("Slime", 1);
        for (int i = 0; i < 3; i++)
        {
            Spawn("Merman Elite", 6);
            Spawn("Rogue Elite", 6);
            Spawn("Zombie Elite", 6);
            yield return new WaitForSeconds(4f);
            Spawn("Rogue Elite", 10, false);
            yield return new WaitForSeconds(4f);
            Spawn("Merman Elite", 10, false);
            yield return new WaitForSeconds(4f);
            Spawn("Zombie Elite", 10, false);
            yield return new WaitForSeconds(4f);
            Spawn("Merman Elite", 5);
            yield return new WaitForSeconds(4f);
            Spawn("Rogue Elite", 6);
            yield return new WaitForSeconds(4f);
            Spawn("Zombie Elite", 6);
            yield return new WaitForSeconds(3f);
            Spawn("Merman Elite", 20);
            yield return new WaitForSeconds(3f);
            Spawn("Rogue Elite", 8, false);
            Spawn("Merman Elite", 12, false);
            yield return new WaitForSeconds(4f);
            Spawn("Rogue Elite", 20, false);
            Spawn("Merman Elite", 20, false);
            yield return new WaitForSeconds(3f);
            Spawn("Zombie Elite", 20);
            yield return new WaitForSeconds(7f);
            Spawn("Rogue", 6);
            Spawn("Merman", 6);
            yield return new WaitForSeconds(5f);
        }
        Spawn("Giant", 20);
        yield return new WaitForSeconds(5f);
        //5 min 30
        while (true)
        {
            yield return new WaitForSeconds(4f);
            Spawn("Rogue Elite", 20, false);
            Spawn("Merman Elite", 20, false);
            yield return new WaitForSeconds(5f);
            Spawn("Zombie Elite", 20, false);
            Spawn("Merman Elite", 20, false);
            yield return new WaitForSeconds(5f);
            Spawn("Zombie Elite", 20);
            yield return new WaitForSeconds(10f);
            Spawn("Rogue Elite", 6);
            Spawn("Merman Elite", 6);
            yield return new WaitForSeconds(5f);
            Spawn("Giant", 15);
        }
    }

    private void StageUnlock()
    {
        StartCoroutine(FlashUnlocked());
    }

    IEnumerator SpawnEnemiesCoroutineLevel2()
    {
        yield return new WaitForSeconds(2f);
        //Spawn("Slime", 1);
        for (int i = 0; i < 3; i++)
        {
            Spawn("Zombie", 10);
            Spawn("Merman", 10);
            Spawn("Giant", 2);
            yield return new WaitForSeconds(4);
            Spawn("Giant", 10);
            yield return new WaitForSeconds(2);
            Spawn("Zombie", 10, isChasing: false);
            yield return new WaitForSeconds(4);
            Spawn("Rogue", 6);
            Spawn("Giant", 2);
            yield return new WaitForSeconds(4);
            Spawn("Rogue", 8);
            Spawn("Merman", 8);
            Spawn("Zombie", 8);
            yield return new WaitForSeconds(4);
            Spawn("Giant", 15, isChasing: false);
        }

        yield return new WaitForSeconds(3f);
        Spawn("Giant", 8);
        yield return new WaitForSeconds(3f);
        Spawn("Zombie", 10);
        yield return new WaitForSeconds(0.5f);
        Spawn("Zombie", 15);
        yield return new WaitForSeconds(0.5f);
        Spawn("Zombie", 15);
        yield return new WaitForSeconds(3f);

        // 1 minute
        Spawn("Merman", 10);
        yield return new WaitForSeconds(6f);
        for (int i = 0; i < 2; i++)
        {
            Spawn("Rogue", 6);
            yield return new WaitForSeconds(1f);
            Spawn("Rogue Elite", 15, false);
            yield return new WaitForSeconds(1f);
            Spawn("Merman Elite", 15, false);
            yield return new WaitForSeconds(1f);
            Spawn("Zombie Elite", 10, false);
            yield return new WaitForSeconds(3f);
            Spawn("Merman", 5);
            yield return new WaitForSeconds(3f);
            Spawn("Rogue", 6);
            yield return new WaitForSeconds(3f);
            Spawn("Merman Elite", 6);
            yield return new WaitForSeconds(3f);
            Spawn("Merman", 30);
            yield return new WaitForSeconds(3f);
            Spawn("Rogue", 8, false);
            Spawn("Merman", 12, false);
            yield return new WaitForSeconds(4f);
            Spawn("Rogue Elite", 12, false);
            Spawn("Merman Elite", 12, false);
            yield return new WaitForSeconds(3f);
            Spawn("Zombie", 30);
            yield return new WaitForSeconds(8f);
            Spawn("Rogue", 8);
            Spawn("Merman", 8);
            yield return new WaitForSeconds(3f);
        }
        Spawn("Giant", 10);
        yield return new WaitForSeconds(2f);

        //2minutes 30
        for (int i = 0; i < 3; i++)
        {
            Spawn("Zombie Elite", 6);
            Spawn("Merman Elite", 1);
            yield return new WaitForSeconds(4);
            Spawn("Rogue Elite", 5);
            yield return new WaitForSeconds(2);
            Spawn("Rogue Elite", 12, isChasing: false);
            yield return new WaitForSeconds(1);
            Spawn("Rogue Elite", 12, isChasing: false);
            yield return new WaitForSeconds(1);
            Spawn("Rogue Elite", 12, isChasing: false);
            yield return new WaitForSeconds(1);
            Spawn("Zombie Elite", 12, isChasing: false);
            yield return new WaitForSeconds(6);
            Spawn("Merman Elite", 8);
            Spawn("Zombie Elite", 10);
            yield return new WaitForSeconds(4);
            Spawn("Rogue Elite", 5);
            Spawn("Merman Elite", 8);
            Spawn("Zombie Elite", 7);
            yield return new WaitForSeconds(3);
            Spawn("Giant", 3);
            Spawn("Merman Elite", 18, isChasing: false);
            yield return new WaitForSeconds(4);
        }
        Spawn("Slime", 1);
        yield return new WaitForSeconds(10);
        //3 minutes 30
        Spawn("Slime", 1);
        for (int i = 0; i < 3; i++)
        {
            Spawn("Merman Elite", 9);
            Spawn("Rogue Elite", 7);
            Spawn("Zombie Elite", 10);
            yield return new WaitForSeconds(4f);
            Spawn("Rogue Elite", 15, false);
            yield return new WaitForSeconds(4f);
            Spawn("Merman Elite", 15, false);
            yield return new WaitForSeconds(4f);
            Spawn("Zombie Elite", 15, false);
            yield return new WaitForSeconds(4f);
            Spawn("Merman Elite", 8);
            yield return new WaitForSeconds(4f);
            Spawn("Rogue Elite", 9);
            yield return new WaitForSeconds(4f);
            Spawn("Zombie Elite", 10);
            yield return new WaitForSeconds(3f);
            Spawn("Merman Elite", 15);
            yield return new WaitForSeconds(3f);
            Spawn("Rogue Elite", 10, false);
            Spawn("Merman Elite", 12, false);
            yield return new WaitForSeconds(4f);
            Spawn("Rogue Elite", 20, false);
            Spawn("Merman Elite", 20, false);
            yield return new WaitForSeconds(3f);
            Spawn("Zombie Elite", 20);
            yield return new WaitForSeconds(7f);
            Spawn("Rogue", 10);
            Spawn("Merman", 10);
            yield return new WaitForSeconds(5f);
        }
        Spawn("Giant", 12);
        yield return new WaitForSeconds(5f);
        //5 min 30
        while (true)
        {
            yield return new WaitForSeconds(4f);
            Spawn("Rogue Elite", 20, false);
            Spawn("Merman Elite", 20, false);
            yield return new WaitForSeconds(5f);
            Spawn("Zombie Elite", 20, false);
            Spawn("Merman Elite", 20, false);
            yield return new WaitForSeconds(5f);
            Spawn("Zombie Elite", 10);
            yield return new WaitForSeconds(1f);
            Spawn("Zombie Elite", 10);
            yield return new WaitForSeconds(1f);
            Spawn("Zombie Elite", 10);
            yield return new WaitForSeconds(10f);
            Spawn("Rogue Elite", 8);
            Spawn("Merman Elite", 6);
            yield return new WaitForSeconds(5f);
            Spawn("Giant", 15);
        }
    }
    IEnumerator FlashUnlocked()
    {
        lvlUnlockText.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            lvlUnlockText.CrossFadeAlpha(1f, flashSpeed, false);
            yield return new WaitForSeconds(flashSpeed);
            lvlUnlockText.CrossFadeAlpha(0f, flashSpeed, false);
            yield return new WaitForSeconds(flashSpeed);
        }
        TitleManager.saveData.levelUnlocked = true;
    }
}


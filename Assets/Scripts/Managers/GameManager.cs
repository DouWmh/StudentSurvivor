using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [Header("Characters")]
    //[SerializeField] GameObject boss;
    //[SerializeField] GameObject merfolk;
    //[SerializeField] GameObject merfolkV;
    //[SerializeField] GameObject zombie;
    //[SerializeField] GameObject zombieV;
    //[SerializeField] GameObject rogue;
    //[SerializeField] GameObject "Rogue Elite";
    [SerializeField] GameObject giant;
    [SerializeField] GameObject player;
    Player playerScript;
    static bool MainPlayer = true;
    [SerializeField] PlayerCamera cam;

    [SerializeField] Canvas enemyHpCanvas;
    [SerializeField] float spawnDistance = 5f;
    public static Dictionary<string, int> Kills = new Dictionary<string, int>();
    public static Dictionary<string, int> Collected = new Dictionary<string, int>();

    [SerializeField] TMP_Text gameTimerText;
    [SerializeField] GameObject pauseScreen;

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

    public Dictionary<string, string> lvlUpDescription = new();
    public List<string> lvlUpOptions = new();

    int minutes = 0, seconds = 0;
    public static string timeText;
    public static bool isPaused = false;
    public static bool isLeveling = false;

    void Start()
    {
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


        if (TitleManager.saveData == null)
            TitleManager.saveData = new SavedData();
        playerScript = player.GetComponent<Player>();
        StartCoroutine(SpawnEnemiesCoroutine());
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
    IEnumerator SpawnEnemiesCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            Spawn("Zombie", 2);
            Spawn("Merman", 2);
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
            Spawn("Merman Elite", 15, isChasing: false);
            yield return new WaitForSeconds(4);
        }
        //3 minutes 30
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
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    [Header("Stats")]
    [SerializeField] float speed = 1f;
    [SerializeField] float maxHp = 3;
    public float damageMultiplier = 1f;


    //Subject to change with persistance
    private int[] wpnLvls;
    //private int[] skillLvls;
    [SerializeField] TMP_Text hpDisplay;

    [Header("Weapons")]
    [SerializeField] BaseWeapon[] weapons;
    //0-Katana
    //1-ScytheSpawner
    //2-AxeSpawner
    //3-EnergyBallSpawner
    [SerializeField] TMP_Text[] wpnLvlsTxt;
    [SerializeField] Axe_Spawner axeSpawner;
    [SerializeField] EnergyBallSpawner energySpawner;
    [SerializeField] Scythe_Spawner scytheSpawner;
    [SerializeField] Katana katana;

    [SerializeField] GameObject xpCrystal;


    [SerializeField] HealthBar playerHpBar;
    bool isInvulnerable;
    bool isInvincible;

    private Enemy enemy;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Material flashMat;
    //private Material invincibleMat;
    private Color originalColor;
    private ParticleSystem starParticle;
    [SerializeField] PlayerCamera cam;

    #region Sounds
    [Header("Sound FX")]
    [SerializeField] AudioSource coinSound;
    public void PlayCoinSound()
    {
        coinSound.Play();
    }
    [SerializeField] AudioSource magnetSound;
    public void PlayMagnetSound()
    {
        magnetSound.Play();
    }
    [SerializeField] AudioSource lvlUpSound;
    public void PlayLevelUpSound()
    {
        lvlUpSound.Play();
    }
    [SerializeField] AudioSource[] deathSounds;
    public void PlayRandomDeathSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, deathSounds.Length);
        deathSounds[randomIndex].Play();
    }
    [SerializeField] AudioSource crystalSound;
    public void PlayCrystalSound()
    {
        crystalSound.Play();
    }
    [SerializeField] AudioSource foodSound;
    public void PlayFoodSound()
    {
        foodSound.Play();
    }
    #endregion

    internal int currentExp = 0;
    internal int expToLvl = 4;
    internal int currentLevel = 0;
    internal List<int> randomLvlOptions = new(4);
    internal Action<int, int> OnExpGained;

    private float currentHp;

    internal float CurrentHp { get => currentHp; set => currentHp = (value > maxHp) ? maxHp : (value < 0) ? 0 : value; }

    internal void AddExp()
    {
        currentExp++;
        if (currentExp >= expToLvl)
        {
            Time.timeScale = 0;
            int randomLvlOption;
            for (int i = 0; i < randomLvlOptions.Capacity; i++)
            {
                randomLvlOption = (int)UnityEngine.Random.Range(0, (float)gameManager.lvlUpOptions.Count);
                while (randomLvlOptions.Contains(randomLvlOption))
                    randomLvlOption = (int)UnityEngine.Random.Range(0, (float)gameManager.lvlUpOptions.Count);
                randomLvlOptions.Add(randomLvlOption);
            }

            gameManager.DisplayLevelUpOptions(randomLvlOptions);
            currentExp = 0;
            expToLvl += 4;
            currentLevel++;
            UpdateUIWeaponsLvl();
            randomLvlOptions.Clear();
        }
        OnExpGained(currentExp, expToLvl);
    }
    private void UpdateUIWeaponsLvl()
    {
        for (int i = 0; i < wpnLvlsTxt.Length - 1; i++)
            wpnLvlsTxt[i].text = $"LV.{wpnLvls[i]}";
        wpnLvlsTxt[4].text = $"X {damageMultiplier}";

    }
    public void LevelUpSkill(string skillName)
    {
        switch (skillName)
        {
            //("KatanaSize");
            //("KatanaDamage");
            //("AxeSize");
            //("AxeDamage");
            //("ScytheDamage");
            //("ScytheNumber");
            //("EnergyDamage");
            //("EnergyRange");
            //("AttackPowerUp");
            //("HpPowerUp");
            //0-Katana
            //1-ScytheSpawner
            //2-AxeSpawner
            //3-EnergyBallSpawner
            case "KatanaSize":
                katana.Size *= 1.3f;
                katana.LevelUp();
                wpnLvls[0]++;
                break;
            case "KatanaDamage":
                katana.Damage *= 1.3f;
                katana.LevelUp();
                wpnLvls[0]++;
                break;
            case "AxeSize":
                axeSpawner.Size *= 1.3f;
                axeSpawner.LevelUp();
                wpnLvls[2]++;
                break;
            case "AxeDamage":
                axeSpawner.DamageModifier *= 1.3f;
                axeSpawner.LevelUp();
                wpnLvls[2]++;
                break;
            case "ScytheDamage":
                scytheSpawner.DamageModifier *= 1.3f;
                scytheSpawner.LevelUp();
                wpnLvls[1]++;
                break;
            case "ScytheNumber":
                scytheSpawner.NumToSpawn++;
                scytheSpawner.LevelUp();
                wpnLvls[1]++;
                break;
            case "EnergyDamage":
                energySpawner.DamageModifier *= 1.3f;
                energySpawner.LevelUp();
                wpnLvls[3]++;
                break;
            case "EnergyRange":
                energySpawner.Size *= 1.3f;
                energySpawner.LevelUp();
                wpnLvls[3]++;
                break;
            case "AttackPowerUp":
                damageMultiplier += 0.2f;
                break;
            case "HpPowerUp":
                maxHp += 5;
                currentHp = maxHp;
                playerHpBar.SetMaxHealth(currentHp);
                hpDisplay.text = $"{CurrentHp} / {maxHp}";
                break;
            case "Invincibility":
                StartCoroutine(BecomeInvincible());
                break;
            default:
                break;
        }
        UpdateUIWeaponsLvl();
    }
    IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        starParticle.Play();
        flashMat.SetFloat("_Invincible", 1);
        yield return new WaitForSeconds(10f);
        isInvincible = false;
        starParticle.Stop();
        flashMat.SetFloat("_Invincible", 0);
    }
    IEnumerator Flash()
    {
        for (int i = 0; i < 4; i++)
        {
            flashMat.SetFloat("_Flash", 0.5f);
            yield return new WaitForSeconds(0.05f);
            flashMat.SetFloat("_Flash", 0);
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void Start()
    {
        starParticle = GetComponent<ParticleSystem>();
        starParticle.Stop();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        flashMat = spriteRenderer.material;
        damageMultiplier += (float)TitleManager.saveData.permAtkLvl / 10;
        weapons[0].LevelUp();
        wpnLvls = new int[4] { 1, 0, 0, 0 };
        UpdateUIWeaponsLvl();
        maxHp += TitleManager.saveData.permHpLvl * 2;
        CurrentHp = maxHp;
        animator = GetComponent<Animator>();
        playerHpBar.SetMaxHealth(maxHp);
        hpDisplay.text = $"{CurrentHp} / {maxHp}";
        flashMat.SetFloat("_Invincible", 0);
        //Application.targetFrameRate = 60;
    }
    public float GetHpRatio()
    {
        return (float)CurrentHp / maxHp;
    }
    public float GetHpRelative(float hpRatio)
    {
        return Mathf.Floor(hpRatio / 100 * maxHp);
    }
    public void OnHeal(float healRatio)
    {
        CurrentHp += GetHpRelative(healRatio);
        hpDisplay.text = $"{CurrentHp} / {maxHp}";
        playerHpBar.SetHealthUp(GetHpRatio());
    }
    public void OnDamage(float damage)
    {
        if (!isInvulnerable && !isInvincible)
        {
            StartCoroutine(cam.CameraShake());
            StartCoroutine(Flash());
            isInvulnerable = true;
            if (enemy != null)
                enemy.Dies();
            CurrentHp -= damage;
            if (CurrentHp <= 0)
            {
                StartCoroutine(cam.PlayerDeath());
            }
            playerHpBar.SetHealthDown(CurrentHp);
            Debug.Log($"HP = {CurrentHp}");
            hpDisplay.text = $"{CurrentHp} / {maxHp}";
        }
    }
    public void Death()
    {
        TitleManager.saveData.deathCount++;
        Destroy(gameObject);
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameOver");
    }
    private void Update()
    {
        isInvulnerable = false;
        // Control Movement
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        transform.position += speed * Time.deltaTime * new Vector3(inputX, inputY);
        // Transform direction
        if (inputX != 0)
        {
            transform.localScale = new Vector3(inputX > 0 ? -1 : 1, 1, 1);
        }
        //Animation Component
        bool isRunning = (inputX != 0 || inputY != 0);
        animator.SetBool("isRunning", isRunning);
    }
}

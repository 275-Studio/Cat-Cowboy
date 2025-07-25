using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject waveClearPanel;
    public GameObject inGameUI;
    public GameObject gameOverUI;
    public GameObject starterPanel;
    [Header("UI Containers")]
    public Transform bulletContainer;
    public Transform reloadSpeedContainer;
    public Transform bulletSpeedContainer;
    public Transform bulletCapacity;
    [Header("Prefabs & Sprites")]
    public GameObject bulletPrefab;
    public GameObject barPrefab;
    public Sprite filledSpriteBar;
    public Sprite emptySpriteBar;

    [Header("Wave Clear Elements")]
    public Button continueButton;
    public TextMeshProUGUI buildingHealthText;
    public WaveController waveController;
    [Header("Text")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI destroyedBarrelText;
    [Header("Data")]
    private PlayerStats playerStats;
    private HomeStats homeStats;
    private bool waitingForContinue = false;
    private void Awake()
    {
        waveClearPanel.SetActive(false);
        inGameUI.SetActive(true);
        gameOverUI.SetActive(false);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
    }
    private void Start()
    {
        playerStats = ItemStat.instance.stats.playerStats;
        homeStats = ItemStat.instance.stats.homeStats;

        GenerateBulletsUI();
        GenerateReloadSpeedUI();
        GenerateBulletSpeedUI();
        GenerateBulletCapacityUI();
    }
    private void Update()
    {
        setText();
    }
    public void GenerateBulletsUI()
    {
        foreach (Transform child in bulletContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < playerStats.maxBulletCapacity; i++)
        {
            Instantiate(bulletPrefab, bulletContainer);
        }
    }

    public void RemoveBulletUI()
    {
        if (bulletContainer.childCount > 0)
        {
            Destroy(bulletContainer.GetChild(bulletContainer.childCount - 1).gameObject);
        }
    }

    public void GenerateReloadSpeedUI()
    {
        GenerateBarUI(
            reloadSpeedContainer,
            maxValue: 2f,
            minValue: 0.5f,
            currentValue: playerStats.reloadTime,
            reverse: true
        );
    }

    public void GenerateBulletSpeedUI()
    {
        GenerateBarUI(
            bulletSpeedContainer,
            maxValue: 18f,
            minValue: 3f,
            currentValue: playerStats.bulletSpeed
        );
    }

    public void GenerateBulletCapacityUI()
    {
        int minCapacity = 10;
        int maxCapacity = minCapacity + (5 * 2);
        int totalBars = 5;
        float step = (maxCapacity - minCapacity) / (float)totalBars;

        int filledBars = Mathf.RoundToInt((playerStats.maxBulletCapacity - minCapacity) / step);

        foreach (Transform child in bulletCapacity)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < totalBars; i++)
        {
            GameObject bar = Instantiate(barPrefab, bulletCapacity);
            Image barImage = bar.GetComponent<Image>();
            if (barImage != null)
            {
                barImage.sprite = i < filledBars ? filledSpriteBar : emptySpriteBar;
            }
        }
    }

    private void GenerateBarUI(Transform container, float maxValue, float minValue, float currentValue, bool reverse = false)
    {
        int totalBars = 5;
        float step = (maxValue - minValue) / totalBars;
        int filledBars = Mathf.RoundToInt((currentValue - minValue) / step);

        if (reverse)
            filledBars = totalBars - filledBars;

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < totalBars; i++)
        {
            GameObject bar = Instantiate(barPrefab, container);
            Image barImage = bar.GetComponent<Image>();
            if (barImage != null)
            {
                barImage.sprite = i < filledBars ? filledSpriteBar : emptySpriteBar;
            }
        }
    }

    public void ShowWaveClear()
    {
        inGameUI.SetActive(false);
        waveClearPanel.SetActive(true);
        Time.timeScale = 0f;
        waitingForContinue = true;
    }

    public void HideWaveClear()
    {
        inGameUI.SetActive(true);
        waveClearPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public IEnumerator WaitForContinue()
    {
        yield return new WaitUntil(() => !waitingForContinue);
        HideWaveClear();
    }

    private void OnContinueButtonClicked()
    {
        waitingForContinue = false;
    }

    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnClick_TimeFreezeSkill()
    {
        var upgrade = ItemStat.instance.stats.itemUpgrade;

        if (upgrade.timeFrezee > 0f)
        {
            upgrade.timeFrezee -= 1f;
            waveController.ActivateTimeFreeze();
            Debug.Log("Skill Time Freeze digunakan. Sisa: ");
        }
        else
        {
            Debug.Log("Skill Time Freeze tidak tersedia.");
        }
    }
    public IEnumerator ShowStarterPanel()
    {
        starterPanel.SetActive(true);

        bool isStartClicked = false;
        UnityAction onStartClicked = () => isStartClicked = true;

        Button startButton = starterPanel.GetComponentInChildren<Button>();
        startButton.onClick.AddListener(onStartClicked);

        yield return new WaitUntil(() => isStartClicked);

        startButton.onClick.RemoveListener(onStartClicked);
        starterPanel.SetActive(false);
    }

    private void setText()
    {
        buildingHealthText.text = homeStats.currentHealthHome.ToString();
        coinText.text = playerStats.coin.ToString();
        scoreText.text = playerStats.score.ToString();
        destroyedBarrelText.text = playerStats.destroyedBarrel.ToString();
    }

    public void OnClick_BombSkill()
    {
        var upgrade = ItemStat.instance.stats.itemUpgrade;

        if (upgrade.bomb > 0)
        {
            upgrade.bomb -= 1;
            waveController.ActivateBombSkill();
        }
        else
        {
            Debug.Log("Bombnya ngga ada");
        }
    }
    
}

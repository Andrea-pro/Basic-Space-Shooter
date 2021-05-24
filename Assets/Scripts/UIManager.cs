using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Image _LivesImg;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Text _ammoText;
    [SerializeField] private Text _firewallText;
     
    private GameManager _gameManager;

    
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: " + 15 + " / " + 15; 
        _firewallText.text = _firewallText.text = "Firewall: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateAmmo(int ammoCount, int maxAmmo)
    {
        _ammoText.text = "Ammo: " + ammoCount + " / " + maxAmmo;

        if (ammoCount < 1)
        {
           _ammoText.color = Color.red;
            
        }
     }

    public void UpdateFirewallCounter(int firewallCount)
    {
        _firewallText.text = "Firewall: " + firewallCount + " (needs 3)";

        if (firewallCount == 3)
        {
            _firewallText.color = Color.green;
        }
        else 
        {
            _firewallText.color = Color.red;
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverBlinkRoutine());
    }

    IEnumerator GameOverBlinkRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);

        }
    }
   
    
}

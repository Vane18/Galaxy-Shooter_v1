using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isCoopMode = false;
    public bool gameOver = true;
    [SerializeField]
    private GameObject _player;
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _coopPlayers;
    [SerializeField]
    private GameObject _pauseMenuPanel;
    private void Start()
    {

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }
    void Update()
    {
        if(gameOver == true)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(isCoopMode == false)
                {
                    Instantiate(_player, Vector3.zero, Quaternion.identity);
                }
                else
                {
                    Instantiate(_coopPlayers, Vector3.zero, Quaternion.identity);
                }
                gameOver = false;
                _uiManager.HideTitleScreen();
                _spawnManager.StartSpawnRoutines();

            }
        else if (Input.GetKeyDown(KeyCode.Escape))

            {
                SceneManager.LoadScene("Main_Menu");
            }
        if (gameOver == false)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                _pauseMenuPanel.SetActive(true);
            }
        }
           
    }


}

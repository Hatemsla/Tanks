using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Race;
using Sumo;
using TankBattle;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public MenuUIManager menuUIManager;
    public SumoUIManager sumoUIManager;
    public BattleUIManager battleUIManager;
    public RaceUIManager raceUIManager;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        menuUIManager.battleBtn.onClick.AddListener(Battle);
        menuUIManager.sumoBtn.onClick.AddListener(Sumo);
        menuUIManager.exitBtn.onClick.AddListener(Exit);
        menuUIManager.raceBtn.onClick.AddListener(Race);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 2)
        {
            sumoUIManager = FindObjectOfType<SumoUIManager>();
            sumoUIManager.backBtn.onClick.AddListener(Back);
            sumoUIManager.exitBtn.onClick.AddListener(Exit);
        }
        else if (scene.buildIndex == 1)
        {
            battleUIManager = FindObjectOfType<BattleUIManager>();
            battleUIManager.backBtn.onClick.AddListener(Back);
            battleUIManager.exitBtn.onClick.AddListener(Exit);
        }
        else if (scene.buildIndex == 0)
        {
            var dontDestroyOnLoadObjects = FindObjectsOfType<MenuManager>();
            foreach (var obj in dontDestroyOnLoadObjects)
            {
                if (obj.transform.gameObject != transform.gameObject)
                    Destroy(obj);
            }

            menuUIManager = FindObjectOfType<MenuUIManager>();
            menuUIManager.battleBtn.onClick.AddListener(Battle);
            menuUIManager.sumoBtn.onClick.AddListener(Sumo);
            menuUIManager.raceBtn.onClick.AddListener(Race);
            menuUIManager.exitBtn.onClick.AddListener(Exit);
        }
        else if (scene.buildIndex == 3)
        {
            raceUIManager = FindObjectOfType<RaceUIManager>();
            raceUIManager.backBtn.onClick.AddListener(Back);
            raceUIManager.exitBtn.onClick.AddListener(Exit);
        }
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Race()
    {
        SceneManager.LoadScene(3);
    }

    private void Battle()
    {
        SceneManager.LoadScene(1);
    }

    private void Sumo()
    {
        SceneManager.LoadScene(2);
    }
}

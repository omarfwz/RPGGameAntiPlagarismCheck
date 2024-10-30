using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : Menu
{
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button loadGameButton;

    private void Start()
    {
        if (!DataPersistenceManager.instance.HasData())
        {
            continueGameButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }
    public void OnNewGameClicked()
    {
        saveSlotsMenu.ActivateMenu(false);
        DeactivateMenu();
    }

    public void OnLoadGameClicked()
    {
        saveSlotsMenu.ActivateMenu(true);
        DeactivateMenu();
    }

    public void OnContinueGameClicked()
    {
        DisableAllButtons();
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadSceneAsync("Gameplay");
        SceneManager.LoadSceneAsync("TempleLandingSpawn", LoadSceneMode.Additive);
    }


    private void DisableAllButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    } 
    public void ActivateMenu()
    {
        gameObject.SetActive(true);
    }
    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SaveSlotsMenu : Menu
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private Button backButton;
    [SerializeField] private ConfirmationMenu confirmationMenu;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;
       

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }

    public void ActivateMenu(bool isLoading)
    {
        gameObject.SetActive(true);
        backButton.interactable = true;
        isLoadingGame = isLoading;

        Dictionary<string, PlayerData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesData();
        Button firstSelected = backButton;
        foreach(SaveSlot saveSlot in saveSlots)
        {
            PlayerData profileData = null;
            profilesGameData.TryGetValue(saveSlot.getProfileID(), out profileData);
            saveSlot.SetData(profileData);
            if(isLoadingGame && profileData == null)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                if(profileData != null)
                {
                    saveSlot.hasData = true;
                }
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(backButton))
                {
                    firstSelected = saveSlot.gameObject.GetComponent<Button>();
                }
            }
        }
        SetFirstSelected(firstSelected);
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        DeactivateMenu();
    }
    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();
        if (!isLoadingGame && saveSlot.hasData)
        {
            confirmationMenu.ActivateMenu(saveSlot);
            DeactivateMenu();
            return;
        }
        DataPersistenceManager.instance.ChangeSelectedProfileID(saveSlot.getProfileID());
        if (!isLoadingGame)
        {
            DataPersistenceManager.instance.NewGame();
        } 
        SceneManager.LoadSceneAsync("Gameplay");
        SceneManager.LoadSceneAsync("TempleLandingSpawn", LoadSceneMode.Additive);
    }
    private void DisableMenuButtons()
    {
        foreach(SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}

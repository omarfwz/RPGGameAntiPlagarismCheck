using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfirmationMenu : Menu
{
    [SerializeField] SaveSlotsMenu saveSlotsMenu;
    [SerializeField] Button noButton;
    [SerializeField] Button yesButton;
    SaveSlot selectedSaveSlot;
    
    public void ActivateMenu(SaveSlot saveSlot)
    {
        gameObject.SetActive(true);
        selectedSaveSlot = saveSlot;
    }

    public void DeactivateMenu()
    {
        saveSlotsMenu.ActivateMenu(false);
        gameObject.SetActive(false);
    }

    public void NoClick()
    {
        DeactivateMenu();
    }

    public void YesClick()
    {
        DisableMenuButtons();
        DataPersistenceManager.instance.ChangeSelectedProfileID(selectedSaveSlot.getProfileID());
        DataPersistenceManager.instance.NewGame();
        DataPersistenceManager.instance.StartLoadedGame();
    }
    private void DisableMenuButtons()
    {
        noButton.interactable = false;
        yesButton.interactable = false;
    }
}

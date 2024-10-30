using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DialogueObject;
using TMPro;
using System;
using UnityEngine.InputSystem;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    //all parts of the dialouge to import
    [SerializeField] Transform parentOfResponses;
    [SerializeField] Button prefab_btnResponse;
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] GameObject picture;
    [SerializeField] GameObject titleBackground;

    //instance vars needed
 
    public GameObject dialougeBox;
    DialogueController controller;
    Logic managerScript;
    ItemManager inventory;
    playerControl inputManager;
    public float textSpeed;
    private Node currentNode;
    private int type;//1 = simple string, 2 = text file with multiple pages, 3 = string array of just thoughts, 4 is battle so it's called per turn unless there's a tag
    private string passed;

    //for array
    private string[] arr;
    private int arr_index;

    private bool avaliable = true;

    //turns off dialouge first frame
    private void Start()
    {
        dialougeBox.SetActive(false);
        controller = GetComponent<DialogueController>();
        inputManager = playerControl.instance;
        managerScript = Logic.instance;
        inventory = ItemManager.instance;
        controller.onEnteredNode += StartDialogue;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("multiple dialogue managers");
        }
        instance = this;
    }

    //the public call to create a dialouge, can be called anywhere
    public void CallDialogue(TextAsset newText)
    {
        if (!avaliable)
        {
            return;
        }
        avaliable = false;
        type = 2;
        SetUpDialogue();
        controller.InitializeDialogue(newText);
    }
    public void CallDialogue(string newText)
    {
        if (!avaliable)
        {
            return;
        }
        avaliable = false;
        type = 1;
        SetUpDialogue();
        passed = newText;
        textComponent.text = string.Empty;
        KillAllChildren(parentOfResponses);
        picture.SetActive(false);
        titleBackground.SetActive(false);
        StartCoroutine(TypeLine(passed));
        
    }

    public void CallDialogue(string[] newText)
    {
        if (!avaliable)
        {
            return;
        }
        avaliable = false;
        type = 3;
        arr = newText;
        arr_index = 0;
        SetUpDialogue();
        passed = newText[arr_index];
        textComponent.text = string.Empty;
        KillAllChildren(parentOfResponses);
        picture.SetActive(false);
        titleBackground.SetActive(false);
        StartCoroutine(TypeLine(passed));

    }
    public void EndDialogue()
    {
        StopAllCoroutines();
        dialougeBox.SetActive(false);
        managerScript.isAvaliable = true;
        inputManager.playerController.UI.Submit.performed -= ContinueText;
        inputManager.playerController.UI.Cancel.performed -= SkipText;
        inputManager.playerController.UI.Disable();
        inputManager.setUpControls();
        avaliable = true;
    }
    public void SetUpDialogue()
    {
        managerScript.isAvaliable = false;
        dialougeBox.SetActive(true);
        inputManager.DisableControls();
        inputManager.playerController.UI.Enable();
        inputManager.playerController.UI.Submit.performed += ContinueText;
        inputManager.playerController.UI.Cancel.performed += SkipText;
    }

    //starts the coroutine for onNodeEntered so the waiting works w/o extensions
    void StartDialogue(Node newNode)
    {
        currentNode = newNode;
        StartCoroutine(OnNodeEntered());
        
    }

    public void SkipText(InputAction.CallbackContext context)
    {
        if (type == 1 || type == 3) // if its a string
        {
            if (textComponent.text != passed)
            {
                StopAllCoroutines();
                textComponent.text = passed;
            }
        }

        if (type == 2 || type == 4) //if its a node
        {
            if (textComponent.text != currentNode.text)
            {
                StopAllCoroutines();
                textComponent.text = currentNode.text;
                SetChildren();
            }

        }
        
    }
    public void ContinueText(InputAction.CallbackContext context)
    {
        if (dialougeBox.activeInHierarchy)
        {
            if (type == 1) 
            {
                if (textComponent.text == passed)
                {
                    EndDialogue();
                }
            }
            if (type == 2) 
            {
                if (textComponent.text == currentNode.text && currentNode.responses.Count == 1 && !currentNode.ShowButton())
                {
                    StopAllCoroutines();
                    OnNodeSelected(0);
                }
                else if (currentNode.IsEndNode() && textComponent.text == currentNode.text)
                {
                    EndDialogue();
                }
            }
            if(type == 3)
            {
                if (textComponent.text == passed && arr_index == arr.Length - 1)
                {
                    EndDialogue();
                } else if (textComponent.text == passed)
                {
                    arr_index += 1;
                    passed = arr[arr_index];
                    textComponent.text = string.Empty;
                    StartCoroutine(TypeLine(passed));
                }
            }
            if(type == 4)
            {
                if(textComponent.text == currentNode.text)
                {
                    if (currentNode.IsSeries())
                    {
                        StopAllCoroutines();
                        OnNodeSelected(0);
                    } 
                    else
                    {
                        Invoke("EndBattleDialogue", 0.1F);
                    }
                    
                }
            }
        }
    }
    
    private void EndBattleDialogue()
    {
        GuiManager.instance.FinishDialogue();
    }
    //removes previous options
    public static void KillAllChildren(UnityEngine.Transform parent)
    {
        UnityEngine.Assertions.Assert.IsNotNull(parent);
        for (int childIndex = parent.childCount - 1; childIndex >= 0; childIndex--)
        {
            Destroy(parent.GetChild(childIndex).gameObject);
        }
    }

    //logs what you chose and takes you to correct node
    private void OnNodeSelected(int indexChosen)
    {
        controller.ChooseResponse(indexChosen);    
    }

    //constructs text and responses w/ wait time
    private IEnumerator OnNodeEntered()
    {
        //runs if the node will do something
        if (currentNode.isSpecialNode())
        {
            if (currentNode.IsCharacterNode())
            {
                picture.SetActive(true);
                titleBackground.SetActive(true);
                Debug.Log(CharacterObject.characters);
                for(int k = 0; k < CharacterObject.characters.Count; k++)
                {
                    if (CharacterObject.characters[k].Name == currentNode.CharacterType())
                    {
                        picture.GetComponent<Image>().sprite = CharacterObject.characters[k].Images[currentNode.StateType()];
                        
                        if (currentNode.IsUnknown())
                        {
                            titleBackground.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
                        }
                        else
                        {
                            titleBackground.GetComponentInChildren<TextMeshProUGUI>().text = currentNode.CharacterType();
                        }
                        
                    }
                }
            }
            else
            {
                picture.SetActive(false);
                titleBackground.SetActive(false);
            }
            if (currentNode.IsItemNode())
            {
                inventory.addToInventory(currentNode.getItemID());
            }
            if (currentNode.IsAtrNode())
            {
                managerScript.AddAttribute(currentNode.getAtrID());
            }
            int i = currentNode.CalcRelation();
            if (i != 0)
            {
                managerScript.EditRelation(i);
            }
            

            
        }
        KillAllChildren(parentOfResponses);
        textComponent.text = string.Empty;
        yield return StartCoroutine(TypeLine(currentNode.text));

        SetChildren();
    }

    //puts in all response options
    private void SetChildren()
    {
        if (type == 1 || (currentNode.responses.Count == 1 && !currentNode.ShowButton()))
        {
            return;
        }
        for (int i = currentNode.responses.Count - 1; i >= 0; i--)
        {
            int currentChoiceIndex = i;
            var response = currentNode.responses[i];
            var responseButton = Instantiate(prefab_btnResponse, parentOfResponses);
            responseButton.GetComponentInChildren<TextMeshProUGUI>().text = (response.displayText);
            
            responseButton.onClick.AddListener(delegate { OnNodeSelected(currentChoiceIndex); });
            if(i == currentNode.responses.Count - 1)
            {
                responseButton.Select();
            }
        }
    }

    //slowly types line
    IEnumerator TypeLine(string s)
    {
        foreach (char c in s.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }


    //battle specific dialogue stuff v


    public void InstantiateBattleDialogue(TextAsset newText)
    {
        type = 4;
        controller.InitializeDialogue(newText);
        dialougeBox.SetActive(true);
    }

    public void PauseDialogueCtrl()
    {
        inputManager.playerController.UI.Submit.performed -= ContinueText;
        inputManager.playerController.UI.Cancel.performed -= SkipText;
    }

    public void ResumeDialogueCtrl()
    {
        inputManager.playerController.UI.Submit.performed += ContinueText;
        inputManager.playerController.UI.Cancel.performed += SkipText;
    }

    public int GetRequiredHealth()
    {
        if (!currentNode.IsEndNode())
        {
            Node next = controller.GetNextNode(0);
            return next.RequiredHealth();
        }
        return -1;
    }
    public void AdvanceDialogue()
    {
        if (currentNode.IsEndNode())
        {
            dialougeBox.SetActive(false);
            GuiManager.instance.FinishDialogue();
            
        }
        else
        {
            dialougeBox.SetActive(true);
            OnNodeSelected(0);
        }
    }
    public void PauseDialogue()
    {
        dialougeBox.SetActive(false);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueObject;

public class DialogueController : MonoBehaviour
{

    Dialogue curDialogue;
    Node curNode;

    public delegate void NodeEnteredHandler(Node node);
    public event NodeEnteredHandler onEnteredNode;

    public Node GetCurrentNode()
    {
        return curNode;
    }

    public void InitializeDialogue(TextAsset newText)
    {
        curDialogue = new Dialogue(newText);
        curNode = curDialogue.GetStartNode();
        onEnteredNode(curNode);
    }

    public List<Response> GetCurrentResponses()
    {
        return curNode.responses;
    }

    public void ChooseResponse(int responseIndex)
    {
        string nextNodeID = curNode.responses[responseIndex].destinationNode;
        Node nextNode = curDialogue.GetNode(nextNodeID);
        curNode = nextNode;
        onEnteredNode(nextNode);
    }

    public Node GetNextNode(int responseIndex)
    {
        if (curNode.IsEndNode())
        {
            return null;
        }
        string nextNodeID = curNode.responses[responseIndex].destinationNode;
        Node nextNode = curDialogue.GetNode(nextNodeID);
        return nextNode;
    }
}
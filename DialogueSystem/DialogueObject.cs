using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueObject 
{
    private const string kStart = "START";
    private const string kEnd = "END";
    private const string kCharacter = "CHARACTER:";
    private const string kItem = "ITEM:";
    private const string kAtr = "ATTRIBUTE:";


    public struct Response
    {
        public string displayText;
        public string destinationNode;

        public Response(string display, string destination)
        {
            displayText = display;
            destinationNode = destination;
        }
    }

    public class Node
    {
        public string title;
        public string text;
        public List<string> tags;
        public List<Response> responses;

        internal bool IsEndNode()
        {
            return tags.Contains(kEnd);
        }
        internal bool IsSeries()
        {
            return tags.Contains("SERIES");
        }
        internal bool IsEndSeries()
        {
            return tags.Contains("END_SERIES");
        }
        internal bool isSpecialNode()
        {
            if(tags.Count == 0)
            {
                return false;
            }
            return true;
        }
        internal bool IsCharacterNode()
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Contains(kCharacter))
                {
                    return true;
                }
            }
            return false;
        }
        internal string CharacterType()
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Contains(kCharacter))
                {
                    return tags[i].Substring(10);
                }
            }
            return "error";
        }
        internal int StateType()
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Contains("STATE:"))
                {
                    return int.Parse(tags[i].Substring(6));
                }
            }
            return 0;
        }
        internal bool IsUnknown()
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Contains("UNKNOWN"))
                {
                    return true;
                }
            }
            return false;
        }
        internal bool ShowButton()
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Contains("SHOW"))
                {
                    return true;
                }
            }
            return false;
        }
        internal bool IsItemNode()
        {
            for(int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Contains(kItem)){
                    return true;
                }
            }
            return false;
        }
        internal int getItemID()
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Contains(kItem))
                {
                    return int.Parse(tags[i].Substring(5));
                }
            }
            return -1;
        }
        internal bool IsAtrNode()
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Contains(kAtr))
                {
                    return true;
                }
            }
            return false;
        }
        internal string getAtrID()
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Contains(kAtr))
                {
                    return tags[i].Substring(10);
                }
            }
            return "N/A";
        }
        internal int CalcRelation()
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Contains("RELATION:"))
                {
                    return int.Parse(tags[i].Substring(9));
                }
            }
            return 0;
        }

        internal int RequiredHealth()
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Contains("REQ_HEALTH:"))
                {
                    return int.Parse(tags[i].Substring(11));
                }
            }
            return -1;
        }

        // TODO proper override
        public string Print()
        {
            return "";//string.Format( "Node {  Title: '%s',  Tag: '%s',  Text: '%s'}", title, tag, text );
        }

    }

    public class Dialogue
    {
        string title;
        Dictionary<string, Node> nodes;
        string titleOfStartNode;

        public static string GetAfterTag(List<string> par, string val)
        {
            foreach(string str in par)
            {
                if (str.Contains(val))
                {
                    return str.Substring(val.Length);
                }
            }
            return "";
        }

        //constructor for dialogue class
        public Dialogue(TextAsset twineText)
        {
            nodes = new Dictionary<string, Node>();
            ParseTwineText(twineText.text);
        }

        //returns the node from node title
        public Node GetNode(string nodeTitle)
        {
            return nodes[nodeTitle];
        }

        //returns title of start node
        public Node GetStartNode()
        {
            UnityEngine.Assertions.Assert.IsNotNull(titleOfStartNode);
            return nodes[titleOfStartNode];
        }

        //extracts the text to turn into variables
        public void ParseTwineText(string twineText)
        {
            string[] nodeData = twineText.Split(new string[] { "::" }, StringSplitOptions.None);

            bool passedHeader = false;
            for (int i = 0; i < nodeData.Length; i++)
            {

                // The first node comes after the UserStylesheet node
                if (!passedHeader)
                {
                    if (nodeData[i].StartsWith(" UserStylesheet"))
                        passedHeader = true;

                    continue;
                }

                // Note: tags are optional
                // Normal Format: "NodeTitle [Tags, comma, seperated] \r\n Message Text \r\n [[Response One]] \r\n [[Response Two]]"
                // No-Tag Format: "NodeTitle \r\n Message Text \r\n [[Response One]] \r\n [[Response Two]]"
                string currLineText = nodeData[i];

                // Remove position data
                int posBegin = currLineText.IndexOf("{\"position");
                if (posBegin != -1)
                {
                    int posEnd = currLineText.IndexOf("}", posBegin);
                    currLineText = currLineText.Substring(0, posBegin) + currLineText.Substring(posEnd + 1);
                }

                bool tagsPresent = currLineText.IndexOf("[") < currLineText.IndexOf("\r\n");
                int endOfFirstLine = currLineText.IndexOf("\r\n");

                int startOfResponses = -1;
                int startOfResponseDestinations = currLineText.IndexOf("[[");
                bool lastNode = (startOfResponseDestinations == -1);
                if (lastNode)
                    startOfResponses = currLineText.Length;
                else
                {
                    // Last new line before "[["
                    startOfResponses = currLineText.Substring(0, startOfResponseDestinations).LastIndexOf("\r\n");
                }

                // Extract Title
                int titleStart = 0;
                int titleEnd = tagsPresent
                    ? currLineText.IndexOf("[")
                    : endOfFirstLine;
                UnityEngine.Assertions.Assert.IsTrue(titleEnd > 0, "Maybe you have a node with no responses?");
                string title = currLineText.Substring(titleStart, titleEnd).Trim();

                // Extract Tags (if any)
                string tags = tagsPresent
                    ? currLineText.Substring(titleEnd + 1, (endOfFirstLine - titleEnd) - 2)
                    : "";

                if (!string.IsNullOrEmpty(tags) && tags[tags.Length - 1] == ']')
                    tags = tags.Substring(0, tags.Length - 1);

                // Extract Message Text & Responses
                string messsageText = currLineText.Substring(endOfFirstLine, startOfResponses - endOfFirstLine).Trim();
                string responseText = currLineText.Substring(startOfResponses).Trim();

                Node curNode = new Node();
                curNode.title = title;
                curNode.text = messsageText;
                curNode.tags = new List<string>(tags.Split(new string[] { " " }, StringSplitOptions.None));

                if (curNode.tags.Contains(kStart))
                {
                    UnityEngine.Assertions.Assert.IsTrue(null == titleOfStartNode);
                    titleOfStartNode = curNode.title;
                }

                // Note: response messages are optional (if no message then destination is the message)
                // With Message Format: "\r\n Message[[Response One]]"
                // Message-less Format: "\r\n [[Response One]]"
                curNode.responses = new List<Response>();
                if (!lastNode)
                {
                    List<string> responseData = new List<string>(responseText.Split(new string[] { "\r\n" }, StringSplitOptions.None));
                    for (int k = responseData.Count - 1; k >= 0; k--)
                    {
                        string curResponseData = responseData[k];

                        if (string.IsNullOrEmpty(curResponseData))
                        {
                            responseData.RemoveAt(k);
                            continue;
                        }

                        bool addResponse = true;

                        Response curResponse = new Response();
                        int destinationStart = curResponseData.IndexOf("[[");
                        int destinationEnd = curResponseData.IndexOf("]]");
                        UnityEngine.Assertions.Assert.IsFalse(destinationStart == -1, "No destination around in node titled, '" + curNode.title + "'");
                        UnityEngine.Assertions.Assert.IsFalse(destinationEnd == -1, "No destination around in node titled, '" + curNode.title + "'");
                        string destination = curResponseData.Substring(destinationStart + 2, (destinationEnd - destinationStart) - 2);
                        curResponse.destinationNode = destination;
                        if (destinationStart == 0)
                            curResponse.displayText = ""; // If message-less, then message is an empty string
                        else
                            curResponse.displayText = curResponseData.Substring(0, destinationStart);
                        foreach(string tag in curNode.tags)
                        {
                            if (tag.Contains("GATE:"))
                            {
                                bool gateTrue = Logic.instance.HasAttribute(tag.Substring(5));
                                string des = GetAfterTag(curNode.tags, "DESTROY:");
                                string add = GetAfterTag(curNode.tags, "ADD:");
                                if (gateTrue)
                                {
                                    if (des == destination)
                                    {
                                        addResponse = false;
                                    }
                                }
                                else
                                {
                                    if (add == destination)
                                    {
                                        addResponse = false;
                                    }
                                }
                            }
                        }
                        if (addResponse)
                        {
                           curNode.responses.Add(curResponse);
                        }
                        
                    }
                }

                nodes[curNode.title] = curNode;
            }
        }
    }
}

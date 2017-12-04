using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickable : MonoBehaviour
{

    private int toggle = 0;
    private bool isOnTop = false, activeModel = false;
    private GameObject model;
    private Renderer renderer;
    private int nodeX, nodeY;
    Color hoverColorActive = new Color32(0x00, 0x99, 0x00, 0xFF); // RGBA
    Color hoverColorInactive = new Color32(0xCC, 0x00, 0x00, 0xFF); // RGBA
    Color startColor = new Color32(0xFF, 0xFF, 0xFF, 0x00); // RGBA
    Color nodeStartColor = new Color32(0x52, 0x52, 0x52, 0xFF); //RGBA
    private bool canActivate = true;

    // Use this for initialization
    void Start()
    {
        model = GameObject.FindGameObjectWithTag("Model");
        renderer = GetComponent<Renderer>();
    }


    void Update()
    {
        if (toggle == 1 && isOnTop == true)
        {
            renderer.material.color = hoverColorActive;
            AdjustNodes();
        }
        else if (toggle == 0 && isOnTop == false)
        {
            renderer.material.color = startColor;
            AdjustNodes();
        }
        else if (isOnTop == true)
        {
            if (canActivate == true)
                renderer.material.color = hoverColorActive;
            else
                renderer.material.color = hoverColorInactive;
        }
        else if (toggle == 1 && isOnTop == false)
        {
            renderer.material.color = hoverColorActive;
        }
    }


    void OnMouseEnter()
    {
        isOnTop = true;
    }


    void OnMouseDown()
    {
        activeModel = true;
        if (canActivate == true)
        {
            if (toggle == 0)
            {
                toggle = 1;
                //LegalMoves Go here...
            }
            else
                toggle = 0;
        }
    }

    void OnMouseExit()
    {
        isOnTop = false;
        renderer.material.color = startColor;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Nodes")
        {
            string nodeName = col.collider.name;
            string[] nodeLocation = nodeName.Split(' ');

            string[] nodeCoords = nodeLocation[1].Split(',');
            nodeX = int.Parse(nodeCoords[0]);
            nodeY = int.Parse(nodeCoords[1]);

        }
    }

    /*
     * Helps get the nodes adjacent to the selected minion. 
     */
    void AdjustNodes()
    {
        if (nodeX > 0 && nodeY > 0)
        {
    
            int newNodeX, newNodeY;

            newNodeX = nodeX - 1;                                                                       //Tweak these to get adjacent nodes.
            newNodeY = nodeY - 1;                                                                       //Tweak these to get adjacent nodes.
            if (newNodeX > 0 && newNodeY > 0 && newNodeX < 8 && newNodeY < 12)
            {
                ChangeNodeColor(newNodeX, newNodeY);
            }

            newNodeX = nodeX - 1;                                                                       //Tweak these to get adjacent nodes.
            newNodeY = nodeY;                                                                           //Tweak these to get adjacent nodes.
            if (newNodeX > 0 && newNodeY > 0 && newNodeX < 8 && newNodeY < 12)
            {
                ChangeNodeColor(newNodeX, newNodeY);
            }

            newNodeX = nodeX + 1;                                                                       //Tweak these to get adjacent nodes.
            newNodeY = nodeY;                                                                           //Tweak these to get adjacent nodes.
            if (newNodeX > 0 && newNodeY > 0 && newNodeX < 8 && newNodeY < 12)
            {
                ChangeNodeColor(newNodeX, newNodeY);
            }

            newNodeX = nodeX + 1;                                                                       //Tweak these to get adjacent nodes.
            newNodeY = nodeY - 1;                                                                           //Tweak these to get adjacent nodes.
            if (newNodeX > 0 && newNodeY > 0 && newNodeX < 8 && newNodeY < 12)
            {
                ChangeNodeColor(newNodeX, newNodeY);
            }

            newNodeX = nodeX + 1;                                                                       //Tweak these to get adjacent nodes.
            newNodeY = nodeY + 1;                                                                           //Tweak these to get adjacent nodes.
            if (newNodeX > 0 && newNodeY > 0 && newNodeX < 8 && newNodeY < 12)
            {
                ChangeNodeColor(newNodeX, newNodeY);
            }

            newNodeX = nodeX - 1;                                                                       //Tweak these to get adjacent nodes.
            newNodeY = nodeY + 1;                                                                           //Tweak these to get adjacent nodes.
            if (newNodeX > 0 && newNodeY > 0 && newNodeX < 8 && newNodeY < 12)
            {
                ChangeNodeColor(newNodeX, newNodeY);
            }

            newNodeX = nodeX;                                                                       //Tweak these to get adjacent nodes.
            newNodeY = nodeY - 1;                                                                           //Tweak these to get adjacent nodes.
            if (newNodeX > 0 && newNodeY > 0 && newNodeX < 8 && newNodeY < 12)
            {
                ChangeNodeColor(newNodeX, newNodeY);
            }

            newNodeX = nodeX;                                                                       //Tweak these to get adjacent nodes.
            newNodeY = nodeY + 1;                                                                           //Tweak these to get adjacent nodes.
            if (newNodeX > 0 && newNodeY > 0 && newNodeX < 8 && newNodeY < 12)
            {
                ChangeNodeColor(newNodeX, newNodeY);
            }

            newNodeX = nodeX - 2;                                                                       //Tweak these to get adjacent nodes.
            newNodeY = nodeY;                                                                           //Tweak these to get adjacent nodes.
            if (newNodeX > 0 && newNodeY > 0 && newNodeX < 8 && newNodeY < 12)
            {
                ChangeNodeColor(newNodeX, newNodeY);
            }

            newNodeX = nodeX + 2;                                                                       //Tweak these to get adjacent nodes.
            newNodeY = nodeY;                                                                           //Tweak these to get adjacent nodes.
            if (newNodeX > 0 && newNodeY > 0 && newNodeX < 8 && newNodeY < 12)
            {
                ChangeNodeColor(newNodeX, newNodeY);
            }

            newNodeX = nodeX;                                                                       //Tweak these to get adjacent nodes.
            newNodeY = nodeY + 2;                                                                           //Tweak these to get adjacent nodes.
            if (newNodeX > 0 && newNodeY > 0 && newNodeX < 8 && newNodeY < 12)
            {
                ChangeNodeColor(newNodeX, newNodeY);
            }

            newNodeX = nodeX;                                                                       //Tweak these to get adjacent nodes.
            newNodeY = nodeY - 2;                                                                           //Tweak these to get adjacent nodes.
            if (newNodeX > 0 && newNodeY > 0 && newNodeX < 8 && newNodeY < 12)
            {
                ChangeNodeColor(newNodeX, newNodeY);
            }
        }
    }

    void ChangeNodeColor(int newNodeX, int newNodeY)
    {
        string newNodeName = "node " + newNodeX.ToString() + "," + newNodeY.ToString();         //Set the new node name.
        GameObject newnode = GameObject.Find(newNodeName);                                      //Get the node GameObject.
        Renderer nodeRenderer = newnode.GetComponent<Renderer>();                               //Get the node Renderer.
        if (toggle == 1)
            nodeRenderer.material.color = hoverColorActive;
        else
            nodeRenderer.material.color = nodeStartColor;

    }
}

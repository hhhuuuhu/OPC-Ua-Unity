using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public TMP_InputField tmpInputField;
    public TMP_Dropdown tmpDropDown;
    Client client = new Client();
    ServerNode node;


    public void DisplayServerData()
    {
        int val = tmpDropDown.value;
        
        if(val == 0)
        {
            if (IsInvoking("Display"))
            {
                CancelInvoke("Display");
            }
            tmpInputField.text = "Please select a server";
        }
        if (val == 1)
        {
            InvokeRepeating("Display", 0.5f, 0.5f);
        }
        if (val == 2)
        {
            if (IsInvoking("Display"))
            {
                CancelInvoke("Display");
            }
            tmpInputField.text = "This server is not available now!";
        }
        if (val == 3)
        {
            if (IsInvoking("Display"))
            {
                CancelInvoke("Display");
            }
            tmpInputField.text = "This server is not available now!";
        }

    }
    async void Display()
    {

        string nodeId = "ns=2;i=2";
        node = await client.Fetchdata(nodeId);
        tmpInputField.text = node.Nodename.ToString()+ ": "+node.Nodevalue.ToString();
    }
}

using System.Data.Common;
// using System.Diagnostics;
// using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRx;
using ViewModel;
using System;
using Controllers;

namespace Components
{
    public class ToolsDisplay : MonoBehaviour
    {

        public CharacterTable characterTable;
        public CharacterTools characterTools;
        public Text textcontent;
        public GameObject ScrollViewContainer;
        public GameObject content;
        public List<Button> button_list = new List<Button>();
        public GameObject templateBtn;
        public Text templateBtnText;
        // public ScrollView scroll;
        // public Button button;


        public void Start()
        {
            try
            {
                characterTools.characterToolsView
                    .Subscribe(OnLoadTools)
                    .AddTo(this);
                
            }
            catch (NullReferenceException e)
            {
                 Debug.Log("Donothing : " + e);
            } 
            RemoveButton();
            button_list.Clear();  
        }

        public Button addButton(string permName, string type)
        {
            // if (permName != null){

            
                permName = permName.Replace(" ", "");
                permName = permName.Replace("\n", "");

                GameObject newButton = Instantiate(templateBtn);
                // GameObject newText = Instantiate(templateBtn);
                newButton.name = permName; 
                newButton.transform.localScale = new Vector3(1, 1, 1);
                newButton.transform.position = new Vector2(6, calculateYposition());
                // newButton.transform.position = new Vector2(6, 12);
                newButton.transform.SetParent(content.transform, false);
                newButton.SetActive(true);
                // Debug.Log("test = " + newButton.GetComponentInChildren<Text>().text);
                Button tempButton = newButton.GetComponent<Button>();
                // Text tempTextButton = tempButton.GetComponent<Text>();
                Debug.Log(permName);
                Debug.Log(newButton);
                Debug.Log(tempButton);
                // Text contentTxt = tempButton.GetComponent<Text>();

                // Text contentTxt = Instantiate(templateBtnText);
                Text contentTxt = newButton.GetComponentsInChildren<Text>()[0];


                Debug.Log(contentTxt);
                if (contentTxt != null){
                    contentTxt.text = permName;
                }
               
                // templateBtnText.text = newButton.name;

                if (type == "permanence")
                    tempButton.onClick.AddListener(() => PermanenceButtonClicked(permName));
                else if (type== "methode")
                    tempButton.onClick.AddListener(() => MethodesButtonClicked(permName));

                return tempButton;
            // }
            // return null;
        }

        int calculateYposition(){
            
            var y = 125;
            var ITEM_HEIGHT = 30;

            var size = button_list.Count;
            for (int i = 1; i<=size; i++){
                y -= ITEM_HEIGHT + 12;
            }

            return y;
        }

        void PermanenceButtonClicked(string permanence)
        {
            characterTable.permFilePath = permanence;
            characterTable.readPermanence = true;
            characterTable.lastIndex = 0;
            // Debug.Log ("Button clicked = " + permanence);
        }

        void MethodesButtonClicked(string methodes)
        {
            // characterTable.permFilePath = permanence;
            // characterTable.readPermanence = true;
            // characterTable.lastIndex = 0;
            Debug.Log ("Button clicked for methodes = " + methodes);
        }


        void RemoveButton(){
            foreach (Button item in button_list)
            {
                Destroy(item.gameObject);
            }
        }

        public void OnLoadTools(string data)
        {
            // public Button button;
            Debug.Log("OnLoadTools : " + data);
            var datasplit = data.Split("//");
            if (datasplit.Length == 2){
                var value = data.Split("//")[0];
                var type = data.Split("//")[1];
                Debug.Log("OnLoadTools : " + value);

                
                button_list.Add(addButton(value, type));
                // ScrollViewContainer.SetActive(true);
                
            }else{
                if (data == ""){
                    // textcontent.text = "";
                    RemoveButton();
                    button_list.Clear();  
                }else{
                    Debug.Log("ERROR : " + data);

                }
                
            }
            

            
        }
    }
}

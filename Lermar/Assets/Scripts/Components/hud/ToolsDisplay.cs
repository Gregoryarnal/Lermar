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

        // public CharacterTable characterTable;
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
                RemoveButton();
                button_list.Clear();  
            }
            catch (System.Exception)
            {
                 Debug.Log("Donothing : ");
            } 
        }

        public Button addButton(string permName)
        {
            permName = permName.Replace(" ", "");
            permName = permName.Replace("\n", "");

            GameObject newButton = Instantiate(templateBtn);
            newButton.name = permName; 
            newButton.transform.localScale = new Vector3(1, 1, 1);
            newButton.transform.position = new Vector2(6, calculateYposition());
            // newButton.transform.position = new Vector2(6, 12);
            newButton.transform.SetParent(content.transform, false);
            newButton.SetActive(true);
            // Debug.Log("test = " + newButton.GetComponentInChildren<Text>().text);
            Button tempButton = newButton.GetComponent<Button>();

            // Text contentTxt = tempButton.GetComponent<Text>();
            templateBtnText.text = permName;


            tempButton.onClick.AddListener(() => ButtonClicked(permName));

            return tempButton;
        }

        int calculateYposition(){
            
            var y = 0;
            var ITEM_HEIGHT = 30;

            var size = button_list.Count;
            for (int i = 1; i<=size; i++){
                y -= ITEM_HEIGHT + 12;
            }

            return y;
        }

        void ButtonClicked(string permanence)
        {
            Debug.Log ("Button clicked = " + permanence);
        }

        void RemoveButton(){
            foreach (Button item in button_list)
            {
                Destroy(item.gameObject);
            }
        }

        public void OnLoadTools(string value)
        {
            // public Button button;
            Debug.Log("OnLoadTools : " + value);
            if (value == ""){
                // textcontent.text = "";
                RemoveButton();
                button_list.Clear();  
            }else{
                button_list.Add(addButton(value));
            }

            
        }
    }
}

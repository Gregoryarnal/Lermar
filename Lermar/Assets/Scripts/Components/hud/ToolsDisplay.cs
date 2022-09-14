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
        public GameObject popUpView;
        public Button popUpBackgroundBtn;
        public GameObject parentPanel;
        public Text statcontent;
        
        public List<Button> button_list = new List<Button>();
        public GameObject templateBtn;
        public Text templateBtnText;


        public void Start()
        {
            try
            {
                characterTools.characterToolsView
                    .Subscribe(OnLoadTools)
                    .AddTo(this);

                characterTools.characterStatisticsView
                    .Subscribe(AddStatistics)
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
            permName = permName.Replace(" ", "");
            permName = permName.Replace("\n", "");

            GameObject newButton = Instantiate(templateBtn);
            newButton.name = permName; 
            newButton.transform.localScale = new Vector3(1, 1, 1);
            newButton.transform.position = new Vector2(6, calculateYposition());
            newButton.transform.SetParent(content.transform, false);
            newButton.SetActive(true);

            Button tempButton = newButton.GetComponent<Button>();
            Text contentTxt = newButton.GetComponentsInChildren<Text>()[0];


            Debug.Log(contentTxt);
            if (contentTxt != null){
                contentTxt.text = permName;
            }

            if (type == "permanence")
                tempButton.onClick.AddListener(() => PermanenceButtonClicked(permName));
            else if (type== "methode")
                tempButton.onClick.AddListener(() => MethodesButtonClicked(permName));

            return tempButton;
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
        }

        void MethodesButtonClicked(string methodes)
        {
            Debug.Log ("Button clicked for methodes = " + methodes);

            popUpView.SetActive(true);
            // Button backgroundBtn = popUpBackgroundBtn.GetComponent<Button>();
            popUpBackgroundBtn.onClick.AddListener(() => BackgroundButtonClicked());

            popUpBackgroundBtn.gameObject.SetActive(true);

        }

        void BackgroundButtonClicked(){
            Debug.Log ("BBackgroundButtonClicked " );

            popUpView.SetActive(false);
            popUpBackgroundBtn.gameObject.SetActive(false);
        }

        void RemoveButton(){
            foreach (Button item in button_list)
            {
                Destroy(item.gameObject);
            }
        }

        public void AddStatistics(string line){
            statcontent.text += line;
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

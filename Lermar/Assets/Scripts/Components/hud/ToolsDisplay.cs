// using System.Diagnostics;
using System.Transactions;
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
        public GameObject montantePopUpView;
        public GameObject methodePopUpView;
        public GameObject ScrollPopUpView;
        public GameObject TemplateBtnPopUp;
        public GameObject ContentPopUpScroll;
        public Text TemplateTextPopUp;
        public Button popUpBackgroundBtn;

        public GameObject toolNameTopPopUp;
        public GameObject permanenceNameTop;


        
        // public GameObject parentPanel;


        public Text statcontent;
        
        public List<Button> button_list = new List<Button>();
        public List<Button> button_list_popup = new List<Button>();
        public List<string> button_permanence_list = new List<string>();
        // public string[] button_permanence_list = new string[]();
        
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
            button_permanence_list.Clear(); 
            

        }

        public void addPopUpButton()
        {            
            


            // Button[] tempButton = content.GetComponentsInChildren<Button>();
            for (int i = 0; i < button_permanence_list.Count; i++)
            {
                var value = button_permanence_list[i];
                value = value.Replace(" ", "");
                value = value.Replace("\n", "");

                GameObject newButton = Instantiate(TemplateBtnPopUp);
                newButton.name = value; 
                newButton.transform.localScale = new Vector3(1, 1, 1);
                newButton.transform.position = new Vector2(6, calculatePopUpYposition(i));
                newButton.transform.SetParent(ContentPopUpScroll.transform, false);
                newButton.SetActive(true);

                Button tempPopUpButton = newButton.GetComponent<Button>();
                Text contentTxt = newButton.GetComponentsInChildren<Text>()[0];


                // Debug.Log(contentTxt);
                if (contentTxt != null){
                    contentTxt.text = value;
                }

                tempPopUpButton.onClick.AddListener(() => highLightButton(tempPopUpButton, new Color(0, 204, 102)));

                button_list_popup.Add(tempPopUpButton);
            }
            
        }

        void highLightButton(Button btn, Color color){

            foreach (Button item in button_list_popup)
            {
                if (item == btn){
                    btn.GetComponent<Image>().color = color;
                    permanenceNameTop.GetComponent<Text>().text =  btn.GetComponentsInChildren<Text>()[0].text;
                }else{
                    item.GetComponent<Image>().color = new Color(1, 1, 1);
                }
            }

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

            if (contentTxt != null){
                contentTxt.text = permName;
            }

            if (type == "permanence")
                tempButton.onClick.AddListener(() => PermanenceButtonClicked(permName));
            else if (type== "methode")
                tempButton.onClick.AddListener(() => MethodesButtonClicked(permName));
            else if (type== "montante")
                tempButton.onClick.AddListener(() => MontanteButtonClicked(permName));

            return tempButton;
        }

        int calculatePopUpYposition(int size){
            
            var y = 1;
            var ITEM_HEIGHT = 68;

            for (int i = 1; i<=size; i++){
                y -= ITEM_HEIGHT + 12;
            }

            return y;
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

        void LoadPermanencesPopUp(){

        }

        void MethodesButtonClicked(string methodes)
        {
            Debug.Log ("Button clicked for methodes = " + methodes);
            addPopUpButton();

            popUpView.SetActive(true);
            methodePopUpView.SetActive(true);
            montantePopUpView.SetActive(false);
            toolNameTopPopUp.GetComponent<Text>().text =  methodes;
            // Button backgroundBtn = popUpBackgroundBtn.GetComponent<Button>();
            popUpBackgroundBtn.onClick.AddListener(() => BackgroundButtonClicked());

            popUpBackgroundBtn.gameObject.SetActive(true);

        }

        void MontanteButtonClicked(string montante)
        {
            Debug.Log ("Button clicked for montante = " + montante);
            addPopUpButton();
            toolNameTopPopUp.GetComponent<Text>().text =  montante;

            popUpView.SetActive(true);
            methodePopUpView.SetActive(false);
            montantePopUpView.SetActive(true);
            Button backgroundBtn = popUpBackgroundBtn.GetComponent<Button>();
            popUpBackgroundBtn.onClick.AddListener(() => BackgroundButtonClicked());

            popUpBackgroundBtn.gameObject.SetActive(true);

        }

        void BackgroundButtonClicked(){
            Debug.Log ("BBackgroundButtonClicked " );
            popUpView.SetActive(false);
            montantePopUpView.SetActive(false);

            popUpBackgroundBtn.gameObject.SetActive(false);
        }

        void RemoveButton(){
            foreach (Button item in button_list)
            {
                Destroy(item.gameObject);
            }
            // foreach (Button item in button_permanence_list)
            // {
            //     Destroy(item.gameObject);
            // }
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


                Button btn = addButton(value, type);
                if (type=="permanence"){
                    // if (!button_permanence_list.Contains(btn)){
                    button_permanence_list.Add(value);
                    // }
                }
                // else{
                    button_list.Add(btn);

                // }                
            }else{
                if (data == ""){
                    // textcontent.text = "";
                    RemoveButton();
                    button_list.Clear();  
                    // button_permanence_list.Clear();  
                }else{
                    Debug.Log("ERROR : " + data);

                }
                
            }
            

            
        }
    }
}

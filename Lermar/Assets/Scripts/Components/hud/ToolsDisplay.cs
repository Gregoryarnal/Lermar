// using System.Reflection.PortableExecutable;
// using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
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
using Montante;

namespace Components
{
    public class ToolsDisplay : MonoBehaviour
    {

        public CharacterTable characterTable;
        public CharacterTools characterTools;
        // public GameCmdFactory gameCmdFactory;
        
        public Text textcontent;
        public GameObject ScrollViewContainer;
        public GameObject content;


        public GameObject popUpView;

        public GameObject montantePopUpView;
        public GameObject palierView;


        public GameObject methodePopUpView;
        public GameObject ScrollPopUpView;
        public GameObject TemplateBtnPopUp;
        public GameObject ContentPopUpScroll;
        public Text TemplateTextPopUp;
        public Button popUpBackgroundBtn;

        public GameObject toolNameTopPopUp;
        public GameObject permanenceNameTop;


        // template line stats
        public GameObject template;
        public GameObject statcontent;
        
        public List<Button> button_list = new List<Button>();
        public List<Button> button_list_popup = new List<Button>();
        public List<string> button_permanence_list = new List<string>();
        List<GameObject> lineGameObject = new List<GameObject>();
        // public string[] button_permanence_list = new string[]();
        
        public GameObject templateBtn;
        public Text templateBtnText;


        public GameObject toBall;


        public Button ExecuteButton;



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
            lineGameObject.Clear();



        }

        public void addPopUpButton()
        {            
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
                    int last = readPermanenceFile(btn.GetComponentsInChildren<Text>()[0].text, -1);
                    toBall.GetComponent<InputField>().text = last.ToString();
                }else{
                    item.GetComponent<Image>().color = new Color(1, 1, 1);
                }
            }

        }

        private int readPermanenceFile(string nameFile, int index){
            var permanencePath = "/Users/gregoryarnal/dev/FreeLance/Lermar/Lermar/permanences/MC/" + nameFile;
            string[] lines = System.IO.File.ReadAllLines(permanencePath);
            if (index == -1){
                return lines.Length;
            }
            return Int32.Parse(lines[index]);
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
            
            // var y = 125;
            var y = 0;
            var ITEM_HEIGHT = 30;

            var size = button_list.Count;
            for (int i = 1; i<=size; i++){
                y -= ITEM_HEIGHT + 30;
            }

            return y;
        }

        int calculateYposition(int index){
            
            var y = -24;
            var ITEM_HEIGHT = 30;

            y -= ITEM_HEIGHT*index+50;

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
            addPopUpButton();

            popUpView.SetActive(true);
            methodePopUpView.SetActive(true);
            montantePopUpView.SetActive(false);
            toolNameTopPopUp.GetComponent<Text>().text =  methodes;

            popUpBackgroundBtn.onClick.AddListener(() => BackgroundButtonClicked());

            popUpBackgroundBtn.gameObject.SetActive(true);

        }

        void MontanteButtonClicked(string montante)
        {
            addPopUpButton();
            toolNameTopPopUp.GetComponent<Text>().text =  montante;
            
            switch (montante)
            {
                case "Apaliers":
                    palierView.SetActive(true);
                    // ExecuteButton.onClick.AddListener(() => characterTools.APalier());
                    // APalierCmd sn = gameObject.GetComponent<APalierCmd>();
                    // sn.DoSomething();
                    // ExecuteButton.onClick.AddListener(() => new APalierCmd());
                    break;
                default:
                    Debug.Log("palierView.SetActive(false);");
                    break;

            }

            popUpView.SetActive(true);
            methodePopUpView.SetActive(false);
            montantePopUpView.SetActive(true);
            Button backgroundBtn = popUpBackgroundBtn.GetComponent<Button>();
            popUpBackgroundBtn.onClick.AddListener(() => BackgroundButtonClicked());

            popUpBackgroundBtn.gameObject.SetActive(true);

        }

        void BackgroundButtonClicked(){
            popUpView.SetActive(false);
            montantePopUpView.SetActive(false);

            popUpBackgroundBtn.gameObject.SetActive(false);
        }

        void RemoveButton(){
            foreach (Button item in button_list)
            {
                Destroy(item.gameObject);
            }
        }

        public void AddStatistics(string valuestat){
            // GameObject template;
 
            // Debug.Log("AddStatistics");
            // Debug.Log(valuestat);
            string[] data = valuestat.Split("//");
            string coup = data[0];
            string value = data[1];
            string mise = data[2];
            string result = data[3];
            string bilan = data[4];

            if (coup!="0"){
                GameObject newLine = Instantiate(template);

                newLine.transform.localScale = new Vector3(1, 1, 1);
                newLine.transform.position = new Vector2(0, calculateYposition(lineGameObject.Count));
                newLine.transform.SetParent(statcontent.transform, false);
                newLine.SetActive(true);

                Text[] tempText = newLine.GetComponentsInChildren<Text>();
                
                // tempText[0].text = (index+1).ToString();
                tempText[0].text = coup.ToString();
                tempText[1].text = value.ToString();
                tempText[2].text = mise.ToString();
                tempText[3].text = result.ToString();
                tempText[4].text = bilan.ToString(); 

                lineGameObject.Add(newLine);
            }
            
        }

        public void OnLoadTools(string data)
        {
            // Debug.Log("OnLoadTools : " + data);
            var datasplit = data.Split("//");
            if (datasplit.Length == 2){
                var value = data.Split("//")[0];
                var type = data.Split("//")[1];

                Button btn = addButton(value, type);
                if (type=="permanence"){
                    button_permanence_list.Add(value);
                }
                button_list.Add(btn);
            }else{
                if (data == ""){
                    RemoveButton();
                    button_list.Clear();  
                }else{
                    Debug.Log("ERROR : " + data);
                }
            }
        }
    }
}

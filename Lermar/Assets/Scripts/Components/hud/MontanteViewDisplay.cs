
// using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRx;
using ViewModel;
using System;
using Controllers;
using Montante;
using System.Threading;

namespace Components
{
    public class MontanteViewDisplay : MonoBehaviour
    {

        public CharacterTools characterTools;
        public GameObject popUpView;
        public GameObject montantePopUpView;
        public Button popUpBackgroundBtn;

        //common
        public GameObject fromBall;
        public GameObject toBall;
        public GameObject fileName;
        public GameObject coinValue;
        public GameObject maxMise;

        //palier 
        public Dropdown chanceGame;
        public Dropdown Attaque;
        public Dropdown maxReach;
        public Dropdown ifMaxPalier;

        public Dropdown SecurityInput;
        public GameObject securityValueInput;
        public Dropdown gainOrLoos;

        public GameObject gainResearch;
        public GameObject nbPalier;
        public GameObject timePalier;
        

        public Button ExecuteButton;
        public Button AllExecuteButton;
        public Button CancelButton;
        public Button StatButton;
        // public Button popUpBackgroundBtn;


        public GameObject permanenceSelected;
        public GameObject montanteSelected;

        // public GameObject resultTxt;
        public GameObject resultView;
        public GameObject resultContent;
        public GameObject rightView;
        public GameObject leftView;

        // template
        public GameObject templateLineRed;
        public GameObject templateLineBlack;
        public GameObject templateLineRedFictive;
        public GameObject templateLineBlackFictive;

        public GameObject templateLineRedLoose;
        public GameObject templateLineBlackLoose;
        public GameObject templateLineRedFictiveLoose;
        public GameObject templateLineBlackFictiveLoose;

        public GameObject header;
        public GameObject headerFictive;
        public GameObject headerContent;
        public GameObject templateLineStat;

        public GameObject AttaqueNameUI;
        public GameObject ChanceNameUI;

        //stat
        public GameObject bilanInputStat;
        public GameObject gameInputStat;
        public GameObject miseInputStat;
        public GameObject decouvertInputStat;

       //sauteuse
        public GameObject sauteuseView;

        // fibonaci
        public Dropdown fiboStartValue;
        public Dropdown fiboScheme;


        public string maxReachTxt = "";

        Montantes montanteManager;

        public int[] redValue = new int[] {1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36};
        public int[] blackValue = new int[] {2,4,6,8,10,11,13,15,17,20,22,24,26,28,29,31,33,35};

        public List<GameObject> lineGameObject = new List<GameObject>();

        public List<string> sauteuseValue = null;

        bool lauchGame = true;

        public string SavePath = "";

        public void changeHeaderFictive(){
            header.SetActive(false);
            headerContent.SetActive(true);
            headerFictive.SetActive(true);
        }

        public void changeHeader(){
            headerContent.SetActive(true);
            header.SetActive(true);
            headerFictive.SetActive(false);
        }

        public void Start()
        {
            reset();
            ExecuteButton.onClick.AddListener(() => run(true));
            popUpBackgroundBtn.onClick.AddListener(() => setUpParamsView());

            AllExecuteButton.gameObject.SetActive(false);

            characterTools.characterSavePath
                .Subscribe(AddSavePath)
                .AddTo(this);
        }

        public void AddSavePath(string file){
            SavePath = file;

        }

        public void reset(){
            foreach (GameObject item in lineGameObject)
            {
                Destroy(item);
            }
            sauteuseValue = null;
            lineGameObject.Clear();
            resetStat();
        }
        
        void resetStat(){
            miseInputStat.GetComponent<Text>().text = "0";
            bilanInputStat.GetComponent<Text>().text = "0";
            gameInputStat.GetComponent<Text>().text = "0";
            decouvertInputStat.GetComponent<Text>().text =  "0";
        }
        void setUpStat(int bilan, int parti, int mise, int coinValueInt){

            var Oldmise = Int32.Parse(miseInputStat.GetComponent<Text>().text);
            if (mise>Oldmise){
                miseInputStat.GetComponent<Text>().text = (mise).ToString();
            }
            if (parti<0){
                var OldDecouvert = Int32.Parse(decouvertInputStat.GetComponent<Text>().text);
                if (parti<OldDecouvert){
                    decouvertInputStat.GetComponent<Text>().text = parti.ToString();
                }
            }

            bilanInputStat.GetComponent<Text>().text = bilan.ToString();
            gameInputStat.GetComponent<Text>().text = parti.ToString();
        }

        void LoadStat(string[,] result){
            
        }

        void run(bool first){
            setUpParamsView();
            var fromBallInt = Int32.Parse(fromBall.GetComponent<InputField>().text);
            var toBallInt = Int32.Parse(toBall.GetComponent<InputField>().text);
            // var fileNameTxt = fileName.GetComponent<InputField>().text;
            var fileNameTxt = "";
            var coinValueInt = Int32.Parse(coinValue.GetComponent<InputField>().text);
            var maxMiseInt = Int32.Parse(maxMise.GetComponent<InputField>().text);
            var permanenceSelectedTxt = permanenceSelected.GetComponent<Text>().text;
            var montanteSelectedTxt = montanteSelected.GetComponent<Text>().text;

            var gainResearchInt = Int32.Parse(gainResearch.GetComponent<InputField>().text);
            lauchGame = true;

            var nbPalierInt = Int32.Parse(nbPalier.GetComponent<InputField>().text);
            var  timePalierInt = Int32.Parse(timePalier.GetComponent<InputField>().text);
            var  ifMaxPalierTxt= ifMaxPalier.options[ifMaxPalier.value].text;
            var  maxReachTxt= maxReach.options[maxReach.value].text;
            var chanceTxt = chanceGame.options[chanceGame.value].text;
            var attaqueTxt = Attaque.options[Attaque.value].text;
            var security = false;

            AttaqueNameUI.GetComponent<Text>().text = attaqueTxt;
            ChanceNameUI.GetComponent<Text>().text = chanceTxt;
            
            if (SecurityInput.options[SecurityInput.value].text == "Oui"){
                security = true;
            }else{
                security =false;
            }
            var securityValue = Int32.Parse(securityValueInput.GetComponent<InputField>().text);
            var typeOfMise = gainOrLoos.options[gainOrLoos.value].text;



            switch (montanteSelectedTxt)
            {
                case "Apaliers":
                    setUpResultView();
                    
                    if (!first){
                        sauteuseValue= null;
                    }

                    if (attaqueTxt=="Sauteuse"){
                        if(sauteuseValue==null){
                            getSauteuseValue(fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
                            lauchGame = false;
                        }
                    }

                    if (lauchGame){
                        APalierCmd palier = new APalierCmd(  nbPalierInt,  timePalierInt,  ifMaxPalierTxt,  gainResearchInt,  maxReachTxt,  chanceTxt,  attaqueTxt,  fromBallInt,  toBallInt,  fileNameTxt,  coinValueInt,  maxMiseInt, permanenceSelectedTxt, sauteuseValue, security, securityValue, typeOfMise);
                        
                        montanteManager = palier.getMontanteManager();
                        palier.run();
                        // setUpResult(montanteManager,toBallInt);
                    }

                    break;
                case "D'Alembert":
                    setUpResultView();
                    
                    if (!first){
                        sauteuseValue= null;
                    }

                    if (attaqueTxt=="Sauteuse"){
                        if(sauteuseValue==null){
                            getSauteuseValue(fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
                            lauchGame = false;
                        }
                    }

                    if (lauchGame){
                        AlembertCmd alembert = new AlembertCmd(  nbPalierInt,  timePalierInt,  ifMaxPalierTxt,  gainResearchInt,  maxReachTxt,  chanceTxt,  attaqueTxt,  fromBallInt,  toBallInt,  fileNameTxt,  coinValueInt,  maxMiseInt, permanenceSelectedTxt, sauteuseValue, security,securityValue, typeOfMise);
                        montanteManager = alembert.getMontanteManager();
                        alembert.run();
                        // setUpResult(montanteManager,toBallInt);
                        
                    }
                    break;
                
                case "Fibonaci":
                    setUpResultView();
                    
                    if (!first){
                        sauteuseValue= null;
                    }

                    if (attaqueTxt=="Sauteuse"){
                        if(sauteuseValue==null){
                            getSauteuseValue(fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
                            lauchGame = false;
                        }
                    }
                    var fiboStartValueTxt = fiboStartValue.options[fiboStartValue.value].text;
                    var fiboSchemeTxt = fiboScheme.options[fiboScheme.value].text;
                    // fiboStartValue
                    // fiboScheme

                    if (lauchGame){
                        FibonaciCmd fibo = new FibonaciCmd( fiboStartValueTxt, fiboSchemeTxt, nbPalierInt,  timePalierInt,  ifMaxPalierTxt,  gainResearchInt,  maxReachTxt,  chanceTxt,  attaqueTxt,  fromBallInt,  toBallInt,  fileNameTxt,  coinValueInt,  maxMiseInt, permanenceSelectedTxt, sauteuseValue, security,securityValue, typeOfMise);
                        montanteManager = fibo.getMontanteManager();
                        fibo.run();
                        // setUpResult(montanteManager,toBallInt);
                    }
                    break;
                case "50/20":
                    setUpResultView();
                    
                    if (!first){
                        sauteuseValue= null;
                    }

                    if (attaqueTxt=="Sauteuse"){
                        if(sauteuseValue==null){
                            getSauteuseValue(fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
                            lauchGame = false;
                        }
                    }

                    if (lauchGame){
                        FiftyTwentyCmd fiftyTwenty = new FiftyTwentyCmd( nbPalierInt,  timePalierInt,  ifMaxPalierTxt,  gainResearchInt,  maxReachTxt,  chanceTxt,  attaqueTxt,  fromBallInt,  toBallInt,  fileNameTxt,  coinValueInt,  maxMiseInt, permanenceSelectedTxt, sauteuseValue, security,securityValue, typeOfMise);
                        montanteManager = fiftyTwenty.getMontanteManager();
                        fiftyTwenty.run();
                    }
                    break;
                default:
                    Debug.Log("non");
                    break;

            
                
            }
            bool stop = true;
            AllExecuteButton.gameObject.SetActive(true);
            string[,] result  = montanteManager.getLines();
                ExecuteButton.interactable = true;
                AllExecuteButton.interactable = true;

            setUpResult(result,toBallInt,-1, stop);

            if (SavePath!= ""){
                saveResult(SavePath,montanteManager,toBallInt);
            }
        }

        void setUpResult(string[,] result, int toBallInt, int start, bool stop){

            if (start < 0){
                start=0;
            }

            for (int i = start; i < toBallInt; i++)
            {   
                start=i;
                addResult(Int32.Parse(result[i, 0]),Int32.Parse(result[i, 1]),Int32.Parse(result[i, 2]),Int32.Parse(result[i, 3]),
                Int32.Parse(result[i, 4]),Int32.Parse(result[i, 5]),Int32.Parse(result[i, 6]),result[i, 7],result[i, 8],result[i, 9], result);

                setUpStat(Int32.Parse(result[i, 6]),Int32.Parse(result[i, 5]),Int32.Parse(result[i, 3]),Int32.Parse(result[i, 4]));        
                if (stop){
                    ExecuteButton.onClick.AddListener(() => setUpResult(result, toBallInt, i+1, stop));
                    AllExecuteButton.onClick.RemoveAllListeners();
                    AllExecuteButton.onClick.AddListener(() => setUpResult(result, toBallInt, i+1, false));
                    break;
                }

                
            }

            if (start+1==toBallInt){
                ExecuteButton.interactable = false;
                AllExecuteButton.interactable = false;

            }

        }

        // public (bool, string) setSauteuse(bool first){
        //     if (!first){
        //         sauteuseValue= null;
        //     }

        //     if (attaqueTxt=="Sauteuse"){
        //         if(sauteuseValue==null){
        //             getSauteuseValue(fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
        //             lauchGame = false;
        //         }
        //     }
        // }

        void getSauteuseValue(int fromBallInt, int toBallInt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt){
            resultView.SetActive(false);
            rightView.SetActive(false);
            leftView.SetActive(false);
            sauteuseView.SetActive(true);
            CancelButton.onClick.AddListener(() => setUpParamsView());
            ExecuteButton.onClick.AddListener(() => setUpSauteuseSequence(fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt));

        }

        public void saveResult(string filename, Montantes montante, int toBallInt){

            var csv = new StringBuilder();
            var filePath = filename+".csv";
            var newLine ="";

            var headerSize = lineGameObject[0].GetComponentsInChildren<Text>().Length;
            string[,] result = montante.getLines();
            if (headerSize == 6){
                newLine = "Boule num, Coup, Num, Mise, Bilan, Bilan Total";
            }else{
                newLine = "Boule num, Coup, Num, Mise1, Bilan1, Mise2, Bilan2, Mise, Bilan, Total";
            }

            csv.AppendLine(newLine);  

            for (int i = 0; i < toBallInt; i++)
            {
                if (headerSize == 6){
                    newLine = result[i,0]+","+result[i,1]+","+result[i,2]+","+result[i,3]+","+result[i,4]+","+result[i,5];
                }else{
                    newLine = result[i,0]+","+result[i,1]+","+result[i,2]+","+result[i,10]+" "+result[i,11]+","+result[i,12]+","+result[i,13]+" "+result[i,14]+","+result[i,15]+","+result[i,3]+","+result[i,4]+","+result[i,5];
                } 
                csv.AppendLine(newLine);
                
            }

            File.AppendAllText(filePath, csv.ToString());
        }

        public void setUpResultView(){
            resultView.SetActive(true);
            rightView.SetActive(false);
            leftView.SetActive(false);
            sauteuseView.SetActive(false);
            CancelButton.onClick.RemoveAllListeners();
            CancelButton.onClick.AddListener(() => setUpParamsView());
        }
        void ClosePopUp(){
            Debug.Log("BackgroundButtonClicked");
            popUpView.SetActive(false);
            montantePopUpView.SetActive(false);
            popUpBackgroundBtn.gameObject.SetActive(false);
            setUpParamsView();
        }

        public void setUpParamsView(){
            reset();
            resultView.SetActive(false);
            rightView.SetActive(true);
            leftView.SetActive(true);
            sauteuseView.SetActive(false);
            ExecuteButton.onClick.RemoveAllListeners();
            ExecuteButton.interactable = false;
            AllExecuteButton.onClick.RemoveAllListeners();
            AllExecuteButton.gameObject.SetActive(false);
            CancelButton.onClick.RemoveAllListeners();
            CancelButton.onClick.AddListener(() => ClosePopUp());
            ExecuteButton.onClick.AddListener(() => run(false));
        }

        void setUpSauteuseSequence(int fromBallInt, int toBallInt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt){
            Dropdown[] tempDropdown = sauteuseView.GetComponentsInChildren<Dropdown>();
            sauteuseValue = new List<string>();
            foreach (Dropdown item in tempDropdown)
            {
                sauteuseValue.Add(item.options[item.value].text);
            }
            run(true);
        }

        private void addResult(int index, int coup, int value, int mise, int coinValueInt,int bilanGame, int bilanTotal, string playerMise,string attaqueTxt,string win, string[,] fictive  ){
 
            var color = montanteManager.valueChance(value, "Rouge");
            GameObject template;

            int mise1 = 0;
            string player1Mise = "";
            int bilanGame1= 0;

            int mise2 = 0;
            string player2Mise= "";
            int bilanGame2 = 0;

            if (attaqueTxt.StartsWith("différentielle")){
                changeHeaderFictive();
                if (color == "Rouge"){
                     if (Convert.ToBoolean(win)){
                        template = templateLineRedFictive;
                    }
                    else{
                        template = templateLineRedFictiveLoose;   
                    }
                    // template = templateLineRedFictive;
                }else{
                    if (Convert.ToBoolean(win)){
                        template = templateLineBlackFictive;
                    }
                    else{
                        template = templateLineBlackFictiveLoose;   
                    }
                    // template = templateLineBlackFictive;
                }

                 mise1 = Int32.Parse(fictive[index,10]);
                 player1Mise = fictive[index,11];
                 bilanGame1 = Int32.Parse(fictive[index,12]);


                 mise2 = Int32.Parse(fictive[index,13]);
                 player2Mise = fictive[index,14];
                 bilanGame2 = Int32.Parse(fictive[index,15]);

            }else{
                changeHeader();
                if (color == "Rouge"){
                    if (Convert.ToBoolean(win)){
                        template = templateLineRed;
                    }
                    else{
                        template = templateLineRedLoose;   
                    }
                }else{
                    if (Convert.ToBoolean(win)){
                        template = templateLineBlack;
                    }
                    else{
                        template = templateLineBlackLoose;   
                    }
                }
            }

            GameObject newLine = Instantiate(template);

            newLine.transform.localScale = new Vector3(1, 1, 1);
            newLine.transform.position = new Vector2(0, calculateYposition(index));
            newLine.transform.SetParent(resultContent.transform, false);
            newLine.SetActive(true);

            Text[] tempText = newLine.GetComponentsInChildren<Text>();
            
            tempText[0].text = (index+1).ToString();
            tempText[1].text = coup.ToString();
            tempText[2].text = value.ToString();
            if (attaqueTxt.StartsWith("différentielle")){
                tempText[3].text = (mise1).ToString() + " " + player1Mise;
                tempText[4].text = bilanGame1.ToString();
                tempText[5].text = (mise2).ToString() + " " + player2Mise;
                tempText[6].text = bilanGame2.ToString();
                tempText[7].text = (mise).ToString() + " " + playerMise;
                tempText[8].text = bilanGame.ToString();
                tempText[9].text = bilanTotal.ToString(); 
            }else{
                tempText[3].text = (mise).ToString() + " " + playerMise;
                tempText[4].text = bilanGame.ToString();
                tempText[5].text = bilanTotal.ToString(); 
            }
            lineGameObject.Add(newLine);
        }
        

        private int calculateYposition(int index){
            // var y = 10;
            var y = 365;
            var ITEM_HEIGHT = 115;

            y -= ITEM_HEIGHT*index;

            return y;
        }
    }
}

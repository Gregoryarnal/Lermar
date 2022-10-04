
// using System.Diagnostics;
// using System.Diagnostics;
// using System.Diagnostics;
// using System.Diagnostics;
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

namespace Components
{
    public class MontanteViewDisplay : MonoBehaviour
    {
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
        public GameObject gainResearch;
        public GameObject nbPalier;
        public GameObject timePalier;
        

        public Button ExecuteButton;
        public Button CancelButton;


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
        public GameObject header;
        public GameObject headerFictive;
        public GameObject headerContent;
        public GameObject templateLineStat;
        

        //stat
        public GameObject bilanInputStat;
        public GameObject gameInputStat;
        public GameObject miseInputStat;
        public GameObject decouvertInputStat;

       //sauteuse
        public GameObject sauteuseView;

        public string maxReachTxt = "";
        // public int chocolat = "";

        Montantes montanteManager;

        public int[] redValue = new int[] {1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36};
        public int[] blackValue = new int[] {2,4,6,8,10,11,13,15,17,20,22,24,26,28,29,31,33,35};

        public List<GameObject> lineGameObject = new List<GameObject>();

        public List<string> sauteuseValue = null;

        bool lauchGame = true;

        public void changeHeaderFictive(){
            header.SetActive(false);
            headerContent.SetActive(true);

            headerFictive.SetActive(true);
        }

        public void changeHeader(){
            // Debug.Log("hearder");
            headerContent.SetActive(true);
            header.SetActive(true);
            headerFictive.SetActive(false);
        }

        public void Start()
        {
            reset();

            ExecuteButton.onClick.AddListener(() => run(true));
        }

        public void reset(){
            foreach (GameObject item in lineGameObject)
            {
                Debug.Log("DESTROY gameobject");
                 Destroy(item);
            }
            sauteuseValue = null;
            lineGameObject.Clear();
        }

        void setUpStat(int bilan, int parti, int mise, int coinValueInt){

            var Oldmise = Int32.Parse(miseInputStat.GetComponent<Text>().text);
            if (mise*coinValueInt>Oldmise){
                miseInputStat.GetComponent<Text>().text = (mise*coinValueInt).ToString();
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

        void run(bool first){
            var fromBallInt = Int32.Parse(fromBall.GetComponent<InputField>().text);
            var toBallInt = Int32.Parse(toBall.GetComponent<InputField>().text);
            var fileNameTxt = fileName.GetComponent<InputField>().text;
            var coinValueInt = Int32.Parse(coinValue.GetComponent<InputField>().text);
            var maxMiseInt = Int32.Parse(maxMise.GetComponent<InputField>().text);
            var permanenceSelectedTxt = permanenceSelected.GetComponent<Text>().text;
            var montanteSelectedTxt = montanteSelected.GetComponent<Text>().text;

            var gainResearchInt = Int32.Parse(gainResearch.GetComponent<InputField>().text);
            lauchGame = true;

            // lineGameObject = new List<GameObject>();


            switch (montanteSelectedTxt)
            {
                case "Apaliers":
                    setUpResultView();
                    
                    var nbPalierInt = Int32.Parse(nbPalier.GetComponent<InputField>().text);
                    var  timePalierInt = Int32.Parse(timePalier.GetComponent<InputField>().text);
                    var  ifMaxPalierTxt= ifMaxPalier.options[ifMaxPalier.value].text;
                    var  maxReachTxt= maxReach.options[maxReach.value].text;
                    var chanceTxt = chanceGame.options[chanceGame.value].text;
                    var attaqueTxt = Attaque.options[Attaque.value].text;

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
                        APalierCmd palier = new APalierCmd(  nbPalierInt,  timePalierInt,  ifMaxPalierTxt,  gainResearchInt,  maxReachTxt,  chanceTxt,  attaqueTxt,  fromBallInt,  toBallInt,  fileNameTxt,  coinValueInt,  maxMiseInt, permanenceSelectedTxt, sauteuseValue);
                        
                        montanteManager = palier.getMontanteManager();
                        palier.run();

                        string[,] result = palier.getLines();
                        // int last = montanteManager.readPermanenceFile(permanenceSelectedTxt, -1);
                        Debug.Log("toBallInt : " + toBallInt);
                        // Debug.Log();

                        for (int i = 0; i < toBallInt; i++)
                        {   
                            addResult(Int32.Parse(result[i, 0]),Int32.Parse(result[i, 1]),Int32.Parse(result[i, 2]),Int32.Parse(result[i, 3]),
                            Int32.Parse(result[i, 4]),Int32.Parse(result[i, 5]),Int32.Parse(result[i, 6]),result[i, 7],result[i, 8], result);
                            setUpStat(Int32.Parse(result[i, 6]),Int32.Parse(result[i, 5]),Int32.Parse(result[i, 3]),Int32.Parse(result[i, 4]));
                        }
                    }
                    
                    // lauchGame = true;
                   
                    break;

                default:
                    Debug.Log("non");
                    break;
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


        void saveResult(string filename){

            var csv = new StringBuilder();
            var filePath = "/Users/gregoryarnal/dev/FreeLance/Lermar/" + filename+".csv";

            foreach (GameObject line in lineGameObject)
            {
                Text[] contents = line.GetComponentsInChildren<Text>();
                var newLine ="";

                foreach (Text item in contents)
                {
                    newLine +=  item.text+",";
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

        public void setUpParamsView(){
            reset();
            resultView.SetActive(false);
            rightView.SetActive(true);
            leftView.SetActive(true);
            sauteuseView.SetActive(false);
            ExecuteButton.onClick.RemoveAllListeners();
            ExecuteButton.onClick.AddListener(() => run(false));

        }

        void setUpSauteuseSequence(int fromBallInt, int toBallInt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt){
            Dropdown[] tempDropdown = sauteuseView.GetComponentsInChildren<Dropdown>();
            sauteuseValue = new List<string>();
            foreach (Dropdown item in tempDropdown)
            {
                sauteuseValue.Add(item.options[item.value].text);
            }
            
            // setUpResultView();
            Debug.Log("Run with sauteuse");
            run(true);

            // APalier(fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
        }

        private void addResult(int index, int coup, int value, int mise, int coinValueInt,int bilanGame, int bilanTotal, string playerMise,string attaqueTxt, string[,] fictive  ){
            // Debug.Log("addResult");
            var color = montanteManager.valueChance(value, "Rouge");
            GameObject template;

            int mise1 = 0;
            string player1Mise = "";
            int bilanGame1= 0;

            int mise2 = 0;
            string player2Mise= "";
            int bilanGame2 = 0;

            if (attaqueTxt.StartsWith("différentielle")){
                Debug.Log("différentielle");
                changeHeaderFictive();
                if (color == "Rouge"){
                    template = templateLineRedFictive;
                }else{
                    template = templateLineBlackFictive;
                }

                 mise1 = Int32.Parse(fictive[index,9]);
                 player1Mise = fictive[index,10];
                 bilanGame1 = Int32.Parse(fictive[index,11]);


                 mise2 = Int32.Parse(fictive[index,12]);
                 player2Mise = fictive[index,13];
                 bilanGame2 = Int32.Parse(fictive[index,14]);

            }else{
                changeHeader();
                if (color == "Rouge"){
                    template = templateLineRed;
                }else{
                    template = templateLineBlack;
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
                tempText[3].text = (mise1*coinValueInt).ToString() + " " + player1Mise;
                tempText[4].text = bilanGame1.ToString();
                tempText[5].text = (mise2*coinValueInt).ToString() + " " + player2Mise;
                tempText[6].text = bilanGame2.ToString();
                tempText[7].text = (mise*coinValueInt).ToString() + " " + playerMise;
                tempText[8].text = bilanGame.ToString();
                tempText[9].text = bilanTotal.ToString(); 
            }else{
                
                tempText[3].text = (mise*coinValueInt).ToString() + " " + playerMise;
                tempText[4].text = bilanGame.ToString();
                tempText[5].text = bilanTotal.ToString(); 
            }
            

            lineGameObject.Add(newLine);
        }
        

        private int calculateYposition(int index){
            // var y = 10;
            var y = -220;
            var ITEM_HEIGHT = 115;

            y -= ITEM_HEIGHT*index;

            return y;

        }



    }
       


}

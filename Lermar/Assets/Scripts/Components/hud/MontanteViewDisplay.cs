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
    public class MontanteViewDisplay : MonoBehaviour
    {

        // public GameCmdFactory gameCmdFactory;

        //common
        public GameObject fromBall;
        public GameObject toBall;
        public Dropdown showEach;
        public Dropdown stopBetweenEach;
        public GameObject delaiBetweenBall;
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

        //stat
        public GameObject bilanInputStat;
        public GameObject gameInputStat;
        public GameObject miseInputStat;
        public GameObject decouvertInputStat;


        int[] redValue = new int[] {1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36};
        int[] blackValue = new int[] {2,4,6,8,10,11,13,15,17,20,22,24,26,28,29,31,33,35};

        // GameObject[] lineGameObject;
        List<GameObject> lineGameObject = new List<GameObject>();

        public void Start()
        {
            reset();
            ExecuteButton.onClick.AddListener(() => run());
            
            // Load();
        }

        
        void reset(){
            foreach (GameObject item in lineGameObject)
            {
                 Destroy(item);
            }
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

        void run(){
            var fromBallInt = Int32.Parse(fromBall.GetComponent<InputField>().text);
            var toBallInt = Int32.Parse(toBall.GetComponent<InputField>().text);
            var showEachTxt = showEach.options[showEach.value].text;
            var stopBetweenEachTxt = stopBetweenEach.options[stopBetweenEach.value].text;
            var delaiBetweenBallTxt = delaiBetweenBall.GetComponent<InputField>().text;
            var fileNameTxt = fileName.GetComponent<InputField>().text;
            var coinValueInt = Int32.Parse(coinValue.GetComponent<InputField>().text);
            var maxMiseInt = Int32.Parse(maxMise.GetComponent<InputField>().text);
            var permanenceSelectedTxt = permanenceSelected.GetComponent<Text>().text;
            var montanteSelectedTxt = montanteSelected.GetComponent<Text>().text;

            

            switch (montanteSelectedTxt)
            {
                case "Apaliers":
                    Debug.Log("palier");
                    setUpResultView();
                    ApalierCmd(fromBallInt, toBallInt, showEachTxt, stopBetweenEachTxt, delaiBetweenBallTxt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
                    break;
                default:
                    Debug.Log("non");
                    break;

            }
        }

        public void ApalierCmd(int fromBallInt, int toBallInt, string showEachTxt, string stopBetweenEachTxt, string delaiBetweenBallTxt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt){
            
            var maxReachTxt = maxReach.options[maxReach.value].text;
            var chanceTxt = chanceGame.options[chanceGame.value].text;
            var attaqueTxt = Attaque.options[Attaque.value].text;
            var gainResearchTxt = gainResearch.GetComponent<InputField>().text;
            var nbPalierInt = Int32.Parse(nbPalier.GetComponent<InputField>().text);

            var timePalierInt = Int32.Parse(timePalier.GetComponent<InputField>().text);
            var ifMaxPalierTxt = ifMaxPalier.options[ifMaxPalier.value].text;

            var miseInitial = 1;
            var mise = 1;
            var gain = 0;

            string[] simpleChance =  {"Rouge", "Noir", "Pair", "Impair", "Manque", "Passe"};
            string[] doubleChance =  {"Douzaines", "Colonnes"};

            var index = 0;
            var bilanTotal = 0;
            var bilanGame = 0;
            var coup = 0;
            var win = true;
            var value = -1;
            lineGameObject.Clear();
            int last = readPermanenceFile(permanenceSelectedTxt, -1);
            for (int i = fromBallInt-1; i < toBallInt; i++)
            {
                // var playerMise = mise(simpleChance, doubleChance);
                var playerMise = getPlayerMise(chanceTxt, attaqueTxt,value, win);
                value = readPermanenceFile(permanenceSelectedTxt,i);
                Debug.Log("New value : " + value);
                mise = calculateMise(coup, mise,  timePalierInt, nbPalierInt, playerMise);
                if (mise>=maxMiseInt){
                    if (ifMaxPalierTxt.StartsWith("Repartir")){
                        mise = coinValueInt;
                    }else{
                        mise=maxMiseInt;
                    }
                }
                win = calculateGain(value, playerMise, mise,coinValueInt, gain);
                if (win){

                    gain += mise*coinValueInt;
                    bilanGame += mise*coinValueInt;
                    bilanTotal += mise*coinValueInt;

                    coup += 1;

                }else{
                    gain -= mise*coinValueInt;
                    bilanGame -= mise*coinValueInt;
                    bilanTotal -= mise*coinValueInt;
                    coup += 1;
                }

                addResult(i,coup, value, mise,coinValueInt,bilanGame,bilanTotal, playerMise );
                setUpStat(bilanTotal,bilanGame,mise,coinValueInt);
                if (win && bilanGame>0){

                    Debug.Log("End");
                    gain = 0;
                    mise = miseInitial;
                    bilanGame = 0;
                    coup = 0;
                }

                index+=1;

            }

        }


        string getPlayerMise(string chanceTxt, string attaqueTxt, int lastValue, bool lastWin){
            var lastColor = valueChance(lastValue, "Rouge");
            if (attaqueTxt=="Série"){
                return chanceTxt;
            }else  if (attaqueTxt=="Sortante" && lastValue > -1){
                if (chanceTxt=="Noir" || chanceTxt=="Rouge"){
                    return lastColor;
                }else if (chanceTxt=="Pair" || chanceTxt=="Impair"){
                    if (lastValue%2==0){
                        return "Pair";
                    }else {
                        return "Impair";
                    }
                }else if (chanceTxt=="Passe" || chanceTxt=="Manque"){
                    if (lastValue<=18){
                        return "Manque";
                    }else {
                        return "Passe";
                    }
                }
            }else  if (attaqueTxt=="Perdante" && lastValue > -1){
                if (chanceTxt=="Noir" || chanceTxt=="Rouge"){
                    if (lastColor=="Rouge"){
                        return "Noir";
                    }else if (lastColor=="Noir"){
                        return "Rouge";
                    }
                }else if (chanceTxt=="Pair" || chanceTxt=="Impair"){
                    if (lastValue%2==0){
                        return "Impair";
                    }else {
                        return "Pair";
                    }
                }else if (chanceTxt=="Passe" || chanceTxt=="Manque"){
                    if (lastValue<=18){
                        return "Passe";
                    }else {
                        return "Manque";
                    }
                }
            return null;
                
                
            }else  if (attaqueTxt=="Av. derniere"){
            return null;
                
            }else  if (attaqueTxt=="C.A.derniere"){
            return null;
                
            }else  if (attaqueTxt=="Sauteuse"){
            return null;
                
            }else  if (attaqueTxt=="Differentielle directe"){
            return null;
                
            }else  if (attaqueTxt=="Differentielle composé"){
            return null;
                
            }
            return null;
        }

        void setUpResultView(){
            resultView.SetActive(true);
            rightView.SetActive(false);
            leftView.SetActive(false);
            CancelButton.onClick.AddListener(() => setUpParamsView());

        }

        void setUpParamsView(){
            reset();
            resultView.SetActive(false);
            rightView.SetActive(true);
            leftView.SetActive(true);
        }
        private void addResult(int index, int coup, int value, int mise, int coinValueInt,int bilanGame, int bilanTotal, string playerMise){
            
            var color = valueChance(value, "Rouge");
            GameObject template;
            if (color == "Rouge"){
                template = templateLineRed;
            }else{
                template = templateLineBlack;
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
            tempText[3].text = (mise*coinValueInt).ToString() + " " + playerMise;
            tempText[4].text = bilanGame.ToString();
            tempText[5].text = bilanTotal.ToString(); 

            lineGameObject.Add(newLine);
        }
        

        private int calculateYposition(int index){
            var y = 10;
            var ITEM_HEIGHT = 115;

            y -= ITEM_HEIGHT*index;

            return y;

        }

        private int calculateMise(int coup, int mise, int timePalier, int calculateMise, string playerMise){

            if (playerMise != null){
                Debug.Log(coup);
                // mise = 1;
                if (mise==0){
                    mise =1;
                }
                Debug.Log(timePalier);
                if (coup!= 0 && coup%timePalier==0){
                    mise += 1;
                }
                return mise;
            }
            return 0;
            
        }

        private bool calculateGain(int value, string playerMise,int mise,int coinValueInt, int gain){
            var result = valueChance(value, playerMise);
            Debug.Log("result : " + result);

            if (string.Compare(result,playerMise)==0){
                gain +=mise*coinValueInt;
                Debug.Log("Win : " + gain);
                return true;
            }else{
                gain -= mise*coinValueInt;
                Debug.Log("Loss : " + gain);
                return false;

            }

            // return gain;
        }

        private string valueChance(int value, string playerMise){
            string[] color =  {"Rouge", "Noir"};
            string[] pairImpair =  {"Pair", "Impair"};
            string[] manquePasse =  {"Manque", "Passe"};
            
            if (Array.IndexOf(color, playerMise)>=0){
                int indexRed = Array.IndexOf(redValue, value);
                int indexBlack = Array.IndexOf(blackValue, value);

                if (indexRed>=0){
                    return "Rouge";
                }else if (indexBlack>=0){
                    return "Noir";
                }
                return "error";
            }else if (Array.IndexOf(pairImpair, playerMise)>=0){
                if (value%2==0){
                    return "Pair";
                }else{
                    return "Impair";
                }
            }else if (Array.IndexOf(manquePasse, playerMise)>=0){
                if (value<=18){
                    return "Manque";
                }else{
                    return "Passe";
                }
            }else{
                return "error";
            }
            
            
        }

        // private string mise(string[] simple){
        //     Random random = new Random();
        //     int ind = random.Next(0, simple.Length);
        //     return simple[ind];
        // }
    
        private int readPermanenceFile(string nameFile, int index){
            var permanencePath = "/Users/gregoryarnal/dev/FreeLance/Lermar/Lermar/permanences/MC/" + nameFile;
            string[] lines = System.IO.File.ReadAllLines(permanencePath);
            if (index == -1){
                return lines.Length;
            }
            return Int32.Parse(lines[index]);
        }

    }
       


}

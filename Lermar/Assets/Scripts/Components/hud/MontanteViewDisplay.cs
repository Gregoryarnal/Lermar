
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
        public GameObject templateLineRedFictive;
        public GameObject templateLineBlackFictive;
        public GameObject header;
        public GameObject headerFictive;
        

        //stat
        public GameObject bilanInputStat;
        public GameObject gameInputStat;
        public GameObject miseInputStat;
        public GameObject decouvertInputStat;

       //sauteuse
        public GameObject sauteuseView;



        int[] redValue = new int[] {1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36};
        int[] blackValue = new int[] {2,4,6,8,10,11,13,15,17,20,22,24,26,28,29,31,33,35};

        // GameObject[] lineGameObject;
        List<GameObject> lineGameObject = new List<GameObject>();

            List<string> sauteuseValue = new List<string>();



        void changeHeaderFictive(){
            header.SetActive(false);
            headerFictive.SetActive(true);
        }

        void changeHeader(){
            header.SetActive(true);
            headerFictive.SetActive(false);
        }


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
                    setUpResultView();

                    ApalierCmd(fromBallInt, toBallInt, showEachTxt, stopBetweenEachTxt, delaiBetweenBallTxt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
                    break;
                default:
                    Debug.Log("non");
                    break;

            }
        }
        void getSauteuseValue(int fromBallInt, int toBallInt, string showEachTxt, string stopBetweenEachTxt, string delaiBetweenBallTxt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt){
            resultView.SetActive(false);
            rightView.SetActive(false);
            leftView.SetActive(false);
            sauteuseView.SetActive(true);
            CancelButton.onClick.AddListener(() => setUpParamsView());
            ExecuteButton.onClick.AddListener(() => setUpSauteuseSequence(fromBallInt, toBallInt, showEachTxt, stopBetweenEachTxt, delaiBetweenBallTxt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt));
        }

        public (string, int, int, bool) play(string chanceTxt, string attaqueTxt, int value, bool win, int index, string permanenceSelectedTxt, int coup,int mise,  int timePalierInt,int  nbPalierInt,int  coinValueInt, int maxMiseInt,string  ifMaxPalierTxt,string  maxReachTxt, int gain, bool diff){
            var newValue = readPermanenceFile(permanenceSelectedTxt,index);
            var lastValue = -1;
            if (index > 0){
                lastValue = readPermanenceFile(permanenceSelectedTxt,index-1);
            }
            var newPlayerMise = getPlayerMise(chanceTxt, attaqueTxt,newValue, win, index, permanenceSelectedTxt, lastValue);
            var newMise = calculateMise(coup, mise,  timePalierInt, nbPalierInt, newPlayerMise, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, diff);

            var newWin = calculateGain(newValue, newPlayerMise, newMise,coinValueInt, gain);
            
            return (newPlayerMise, newValue, newMise, newWin);
        }

        public void ApalierCmd(int fromBallInt, int toBallInt, string showEachTxt, string stopBetweenEachTxt, string delaiBetweenBallTxt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt){
            
            var maxReachTxt = maxReach.options[maxReach.value].text;
            var chanceTxt = chanceGame.options[chanceGame.value].text;
            var attaqueTxt = Attaque.options[Attaque.value].text;
            var gainResearchInt = Int32.Parse(gainResearch.GetComponent<InputField>().text);
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
            bool lauchGame = true;
            var playerMise = "";
            
            if (attaqueTxt=="Sauteuse"){
                if(sauteuseValue.Count!=6){
                    getSauteuseValue(fromBallInt, toBallInt, showEachTxt, stopBetweenEachTxt, delaiBetweenBallTxt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
                    lauchGame = false;
                    
                }
            }
            string[,] fictive =new string[0,0];

            if (lauchGame){
                for (int i = fromBallInt-1; i < toBallInt; i++)
                {
                    if (attaqueTxt.StartsWith("différentielle")){
                        (fictive, value) = calculateFictive(chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, fictive);
                    }

                    if (fictive.Length==0){
                        ( playerMise,value,mise,win ) = play(chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false);
                    }else{
                        if (Int32.Parse(fictive[0,0])==Int32.Parse(fictive[1,0])){
                            mise = 0;
                            playerMise = null;
                            win = false;
                        }else{
                            
                            if (Int32.Parse(fictive[0,0])>Int32.Parse(fictive[1,0])){ //mise1 suppe
                                mise = Int32.Parse(fictive[0,0])-Int32.Parse(fictive[1,0]);
                                chanceTxt = fictive[0,1];
                                ( playerMise,value,mise,win ) = play(chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, true);
                            }else{//mise2 suppe
                            
                                mise = Int32.Parse(fictive[1,0])-Int32.Parse(fictive[0,0]);
                                chanceTxt = fictive[1,1];
                                ( playerMise,value,mise,win ) = play(chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, true);
                            }
                        }
                    }
                    
                    if (win){
                        gain += mise*coinValueInt;
                        bilanGame += mise*coinValueInt;
                        bilanTotal += mise*coinValueInt;
                        // Debug.Log("Add coup");

                        coup += 1;
                    }else{
                        if (value == 0){
                            var ret = 0;
                            if ((mise*coinValueInt)%2==0){
                                ret =mise*coinValueInt/2;
                            }else{
                                ret =mise*coinValueInt/2+1;
                            }
                            gain += ret;
                            bilanGame += ret;
                            bilanTotal += ret;
                        }else{
                            gain -= mise*coinValueInt;
                            bilanGame -= mise*coinValueInt;
                            bilanTotal -= mise*coinValueInt;
                        }

                        coup += 1;
                    }

                    addResult(i,coup, value, mise,coinValueInt,bilanGame,bilanTotal, playerMise, attaqueTxt, fictive);
                    setUpStat(bilanTotal,bilanGame,mise,coinValueInt);

                    if (win && bilanGame>0){
                        gain = 0;
                        mise = miseInitial;

                        if (fictive.Length==6 && attaqueTxt == "différentielle directe"){
                            bilanGame = 0;
                        }else if(attaqueTxt == "différentielle compensée"){
                            coup = 0;
                            bilanGame = 0;
                            // fictive[0,3] = "0";
                            // fictive[1,3] = "0";
                            fictive = new string[0,0];
                        }else if(!attaqueTxt.StartsWith("différentielle")){
                            coup = 0;
                            bilanGame = 0;
                        }
                    }
            

                    if (bilanTotal>=gainResearchInt && gainResearchInt!=0){
                        Debug.Log("End gainResearchInt");
                        break;
                    }
                    
                    if (playerMise==null){
                        if(attaqueTxt!="différentielle directe"){        
                            coup = 0;
                         }
                    }
                    index+=1;
                }
            }
        }


        string getPlayerMise(string chanceTxt, string attaqueTxt, int value, bool lastWin, int index, string permanenceSelectedTxt, int lastValue){
            var color = valueChance(value, "Rouge");
            var lastColor = valueChance(lastValue, "Rouge");
            if (attaqueTxt=="Série"){
                return chanceTxt;
            }else  if (attaqueTxt=="Sortante" && lastValue != -1){
                if (chanceTxt=="Noir" || chanceTxt=="Rouge"){
                    return lastColor;
                }else if (chanceTxt=="Pair" || chanceTxt=="Impair"){
                    if (lastValue%2==0){
                        return "Pair";
                    }else {
                        return "Impair";
                    }
                }else if (chanceTxt=="Passe" || chanceTxt=="Manque"){
                    if (value<=18){
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
            }else  if (attaqueTxt=="Av. dernière"){
                if (index>=2){
                    int val = readPermanenceFile(permanenceSelectedTxt, index-2);
                    lastColor = valueChance(val, "Rouge");
                    if (chanceTxt=="Noir" || chanceTxt=="Rouge"){
                        if (lastColor=="Rouge"){
                            return "Rouge";
                        }else if (lastColor=="Noir"){
                            return "Noir";
                        }
                    }else if (chanceTxt=="Pair" || chanceTxt=="Impair"){
                        if (val%2==0){
                            return "Pair";
                        }else {
                            return "Impair";
                        }
                    }else if (chanceTxt=="Passe" || chanceTxt=="Manque"){
                        if (val<=18){
                            return "Manque";
                        }else {
                            return "Passe";
                        }
                    }
                }else{
                    return null;
                }
            }else  if (attaqueTxt=="C.A. dernière"){
                if (index>=2){
                    int val = readPermanenceFile(permanenceSelectedTxt, index-2);
                    lastColor = valueChance(val, "Rouge");
                    if (chanceTxt=="Noir" || chanceTxt=="Rouge"){
                        if (lastColor=="Rouge"){
                            return "Noir";
                        }else if (lastColor=="Noir"){
                            return "Rouge";
                        }
                    }else if (chanceTxt=="Pair" || chanceTxt=="Impair"){
                        if (val%2==0){
                            return "Impair";
                        }else {
                            return "Pair";
                        }
                    }else if (chanceTxt=="Passe" || chanceTxt=="Manque"){
                        if (val<=18){
                            return "Passe";
                        }else {
                            return "Manque";
                        }
                    }
                }else{
                    return null;
                }
                
            }else  if (attaqueTxt=="Sauteuse"){
                return sauteuseValue[index%6];
            }else  if (attaqueTxt=="différentielle directe"){
                return chanceTxt;
            }else  if (attaqueTxt=="différentielle compensée"){
                return chanceTxt;
            }
            return null;
        }

        void setUpResultView(){
            resultView.SetActive(true);
            rightView.SetActive(false);
            leftView.SetActive(false);
            sauteuseView.SetActive(false);

            CancelButton.onClick.AddListener(() => setUpParamsView());

        }

        void setUpParamsView(){
            reset();
            resultView.SetActive(false);
            rightView.SetActive(true);
            leftView.SetActive(true);
            sauteuseView.SetActive(false);

        }

        void setUpSauteuseSequence(int fromBallInt, int toBallInt, string showEachTxt, string stopBetweenEachTxt, string delaiBetweenBallTxt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt){
            Dropdown[] tempDropdown = sauteuseView.GetComponentsInChildren<Dropdown>();
            foreach (Dropdown item in tempDropdown)
            {
                sauteuseValue.Add(item.options[item.value].text);
            }
            
            setUpResultView();
            ApalierCmd(fromBallInt, toBallInt, showEachTxt, stopBetweenEachTxt, delaiBetweenBallTxt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
        }

        private void addResult(int index, int coup, int value, int mise, int coinValueInt,int bilanGame, int bilanTotal, string playerMise,string attaqueTxt, string[,] fictive ){
            
            var color = valueChance(value, "Rouge");
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
                    template = templateLineRedFictive;
                }else{
                    template = templateLineBlackFictive;
                }

                 mise1 = Int32.Parse(fictive[0,0]);
                 player1Mise = fictive[0,1];
                 bilanGame1 = Int32.Parse(fictive[0,2]);


                 mise2 = Int32.Parse(fictive[1,0]);

                 player2Mise = fictive[1,1];

                 bilanGame2 = Int32.Parse(fictive[1,2]);

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
            var y = 10;
            var ITEM_HEIGHT = 115;

            y -= ITEM_HEIGHT*index;

            return y;

        }

        private int calculateMise(int coup, int mise, int timePalier, int nbPalierInt, string playerMise, int coinValueInt, int maxMiseInt, string ifMaxPalierTxt,string maxReachTxt, bool diff){

            if (playerMise != null){
                if (mise==0){
                    mise = 1;
                }

                if (coup!= 0 && coup%timePalier==0 && !diff){
                    if (mise == nbPalierInt){
                        if (ifMaxPalierTxt.StartsWith("Recommencer")){
                            mise = 1;
                        }
                    }else{
                        mise += 1;
                    }
                    
                    if (mise*coinValueInt >= maxMiseInt){
                        if (maxReachTxt.StartsWith("Repartir")){
                            mise = 1;
                        }else{
                            mise = maxMiseInt/coinValueInt;
                        }
                    }
                    
                }
                return mise;
            }
            return 0;
            
        }

        private bool calculateGain(int value, string playerMise,int mise,int coinValueInt, int gain){
            var result = valueChance(value, playerMise);

            if (string.Compare(result,playerMise)==0){
                gain +=mise*coinValueInt;
                return true;
            }else{
                gain -= mise*coinValueInt;
                return false;

            }
        }

        (string[,], int) calculateFictive(string chanceTxt, string attaqueTxt, int value, bool win, int index, string permanenceSelectedTxt, int mise,  int timePalierInt,int  nbPalierInt,int  coinValueInt, int maxMiseInt,string  ifMaxPalierTxt,string  maxReachTxt, int gain, string[,] fictive){
            // string[,] fictive =new string[0,0];
            if (attaqueTxt.StartsWith("différentielle")){
                if (fictive.Length==0){
                    fictive = new string[2,4];
                    fictive[0,0] = mise.ToString(); //mise
                    fictive[1,0] = mise.ToString(); //mise

                    fictive[0,2] = "0"; //bilan
                    fictive[1,2] = "0";//bilan

                    fictive[0,3] = "0"; // coup
                    fictive[1,3] = "0";// coup
                }
                (var newPlayerMise1, var newValue1, var newMise1,var newWin1 ) = play("Noir", attaqueTxt,value, win, index, permanenceSelectedTxt,Int32.Parse(fictive[0,3]), Int32.Parse(fictive[0,0]),  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false);
                (var newPlayerMise2, var newValue2, var newMise2,var newWin2 ) = play("Rouge", attaqueTxt,value, win, index, permanenceSelectedTxt,Int32.Parse(fictive[1,3]), Int32.Parse(fictive[1,0]),  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false);

                fictive[0,0] = newMise1.ToString();
                fictive[1,0] = newMise2.ToString();

                fictive[0,1] = newPlayerMise1;
                fictive[1,1] = newPlayerMise2;

                var  bilanGame1 =0;
                var  bilanGame2 =0;


                
                if (Int32.Parse(fictive[0,2])>0){
                    fictive[0,2]="0";
                     fictive[0,0] = "1"; // mise
                }
                if (Int32.Parse(fictive[1,2])>0){
                    fictive[1,2]="0";
                     fictive[1,0] = "1"; // mise

                    // fictive[1,3] = "0";// coup

                }   
                if (newWin1){
                    bilanGame1 += newMise1*coinValueInt;
                }else{
                    if (newValue1 == 0){
                        var ret = 0;
                        if ((newMise1*coinValueInt)%2==0){
                            ret =newMise1*coinValueInt/2;
                        }else{
                            ret =newMise1*coinValueInt/2+1;
                        }
                        bilanGame1 += ret;
                    }else{
                        bilanGame1 -= newMise1*coinValueInt;
                    }
                }

                if (newWin2){
                    bilanGame2 += newMise2*coinValueInt;
                }else{
                    if (newValue2 == 0){
                        var ret = 0;
                        if ((newMise2*coinValueInt)%2==0){
                            ret =newMise2*coinValueInt/2;
                        }else{
                            ret =newMise2*coinValueInt/2+1;
                        }
                        bilanGame2 += ret;
                    }else{
                        bilanGame2 -= newMise2*coinValueInt;
                    }
                }

                fictive[0,2] = (Int32.Parse(fictive[0,2]) + bilanGame1).ToString();
                fictive[1,2] = (Int32.Parse(fictive[1,2]) + bilanGame2).ToString();

                 

                value = newValue2;

                fictive[0,3] = (Int32.Parse(fictive[0,3]) + 1).ToString();// coup

                fictive[1,3] = (Int32.Parse(fictive[1,3]) + 1).ToString();// coup
                if (Int32.Parse(fictive[0,2])>0){
                     fictive[0,3] = "0"; // coup
                }
                if (Int32.Parse(fictive[1,2])>0){
                    fictive[1,3] = "0";// coup

                }  

            }
            return (fictive, value);
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


// using System.Diagnostics;
// using System.Diagnostics;
// using System.Diagnostics;
// using System.Diagnostics;
// using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
// using UnityEngine.UI;
using UnityEngine.UI;
using UnityEngine;
using UniRx;
using ViewModel;
using System;
using Controllers;
using Montante;
using System.Threading;
// using System.Windows.Forms;
using Button = UnityEngine.UI.Button;
using Application = UnityEngine.Application;
// using System.Windows.Forms;
// 
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

        //alembert
        public Dropdown variante;
        // public GameObject startTo;

        //palier 
        public Dropdown chanceGame;
        public Dropdown Attaque;
        public Dropdown maxReach;
        public Dropdown FictiveMaxReach;
        public Dropdown ifMaxPalier;

        public Dropdown SecurityInput;
        public GameObject securityValueInput;
        public Dropdown gainOrLoos;
        public Dropdown gainType;

        public GameObject gainResearch;
        public GameObject nbPalier;
        public GameObject timePalier;
        

        public Button ExecuteButton;
        public Button AllExecuteButton;
        public Button CancelButton;
        public Button SolveurButton;
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
        public GameObject decouvertTotalInputStat;

        //Solveur
        public GameObject SolveurView;
        public GameObject MiseSolveur;
        public GameObject BilanSolveur;
        public GameObject DureeSolveur;
        

       //sauteuse
        public GameObject sauteuseView;

        // fibonaci
        public Dropdown fiboStartValue;
        public Dropdown fiboScheme;
        
        //TODO need to change
        public ScrollRect scrollRect;


        public string maxReachTxt = "";

        Montantes montanteManager;

        public int[] redValue = new int[] {1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36};
        public int[] blackValue = new int[] {2,4,6,8,10,11,13,15,17,20,22,24,26,28,29,31,33,35};

        public List<GameObject> lineGameObject = new List<GameObject>();

        public List<string> sauteuseValue = null;

        bool lauchGame = true;

        public string SavePath = "";
        public Button SaveButton;

        bool showResult = true;

        // int solveurResult = -32767;

        IDictionary<int,  Tuple<int, int>> solveurResult = new Dictionary<int,Tuple<int, int>>(){};

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
            // showResult = true;

            ExecuteButton.onClick.AddListener(() => run(true, null));
            SolveurButton.onClick.AddListener(() => runSolveur(true, null));
            popUpBackgroundBtn.onClick.AddListener(() => setUpParamsView());

            AllExecuteButton.gameObject.SetActive(false);

            characterTools.characterSavePath
                .Subscribe(AddSavePath)
                .AddTo(this);
        }

        public void AddSavePath(string file){
            SavePath = file;
            if (file == ""){
                SaveButton.GetComponentsInChildren<Text>()[0].text = "Sauvegarder le résultat";
            }else{
                SaveButton.GetComponentsInChildren<Text>()[0].text= Path.GetFileName(file)+".csv";
            }
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
            decouvertTotalInputStat.GetComponent<Text>().text =  "0";
            
        }

        (int, int,int,int, int) calculateStat(int bilan, int parti, int mise, int lastParti, int coinValueInt){
            int miseStat = 0,bilanStat= 0,gameStat= 0 ,perteMax= 0,decouvertTotalStat = 0;
            var Oldmise = Int32.Parse(miseInputStat.GetComponent<Text>().text);
            var OldPerteMax = Math.Abs(Int32.Parse(decouvertInputStat.GetComponent<Text>().text));
            var OldDecouvertTotal = Int32.Parse(decouvertTotalInputStat.GetComponent<Text>().text);

            if (mise>Oldmise){
                miseStat = mise;
            }else{
                miseStat = Oldmise;
            }
            if (parti<0 && mise != 0){
                
                if (parti<(-OldPerteMax)){
                    perteMax = Math.Abs(parti);
                }else{
                    decouvertTotalStat = OldDecouvertTotal;
                    perteMax = OldPerteMax;
                }
            }else{
                decouvertTotalStat = OldDecouvertTotal;
                perteMax = OldPerteMax;
            }

            if (lastParti<0 && mise != 0){
                if(Math.Abs(lastParti)+mise>decouvertTotalStat){
                    decouvertTotalStat = Math.Abs(lastParti)+mise;
                }else{
                    decouvertTotalStat = OldDecouvertTotal;
                }
            }else{
                decouvertTotalStat = OldDecouvertTotal;
            }

            bilanStat = bilan;
            gameStat = parti;

            return (miseStat,bilanStat,gameStat ,decouvertTotalStat, perteMax ); // inverser les deux derniers sur le reste
        }

        void setUpStat(int miseStat, int bilanStat, int gameStat,  int decouvertTotalStat, int perteMax){
            bilanInputStat.GetComponent<Text>().text = bilanStat.ToString();
            gameInputStat.GetComponent<Text>().text = gameStat.ToString();
            miseInputStat.GetComponent<Text>().text = miseStat.ToString();
            decouvertInputStat.GetComponent<Text>().text = (-perteMax).ToString();
            decouvertTotalInputStat.GetComponent<Text>().text =  (decouvertTotalStat).ToString();
        }

        void LoadStat(string[,] result){
            
        }

        void runSolveur(bool first, List<String> sauteuseValue){
            var coinValueInt = Int32.Parse(coinValue.GetComponent<InputField>().text);
            var  timePalierInt = Int32.Parse(timePalier.GetComponent<InputField>().text);
            // var gainResearchInt = Int32.Parse(gainResearch.GetComponent<InputField>().text);

            solveurResult = new Dictionary<int,  Tuple<int, int>>(){};
            showResult = false;
            for (int coin = 1; coin <= coinValueInt; coin++)
            {
                for (int i = 2; i <= timePalierInt; i++)
                {
                    // for (int max = coinValueInt; max <= gainResearchInt;max++)
                    // {
                        coinValue.GetComponent<InputField>().text = coin.ToString();
                        timePalier.GetComponent<InputField>().text =i.ToString();
                        // gainResearch.GetComponent<InputField>().text = max.ToString();
                        run(true, null);
                    // }
                }
            }

        SolveurView.SetActive(true);
            foreach(var kvp in solveurResult){
                MiseSolveur.GetComponent<Text>().text = kvp.Key.ToString();
                BilanSolveur.GetComponent<Text>().text = kvp.Value.Item1.ToString();
                DureeSolveur.GetComponent<Text>().text = kvp.Value.Item2.ToString();
            }
                

        }

        void run(bool first, List<String> sauteuseValue){
            setUpParamsView();
            // alembertView.SetActive(false);

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
            var fictiveMaxReachTxt = FictiveMaxReach.options[FictiveMaxReach.value].text;

            
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
            var typeOfGain = gainType.options[gainType.value].text;
           
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var  m_Path = Application.dataPath;
            // Thread myThread = null;
            
            // string text = "This is some Text that I wanted to show";
        //    System.Windows.Forms.MessageBox.Show("Hello World!");
            
            switch (montanteSelectedTxt)
            {
                case "Apaliers":
                    setUpResultView();
                    
                    if (!first){
                        sauteuseValue= null;
                    }

                    if (attaqueTxt=="Sauteuse"){
                        lauchGame = true;

                        if(sauteuseValue==null){
                            getSauteuseValue(fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
                            lauchGame = false;
                        }
                    }

                    if (lauchGame){
                        APalierCmd palier = new APalierCmd(m_Path, fictiveMaxReachTxt, typeOfGain, nbPalierInt,  timePalierInt,  ifMaxPalierTxt,  gainResearchInt,  maxReachTxt,  chanceTxt,  attaqueTxt,  fromBallInt,  toBallInt,  fileNameTxt,  coinValueInt,  maxMiseInt, permanenceSelectedTxt, sauteuseValue, security, securityValue, typeOfMise); 
                        montanteManager = palier.getMontanteManager();

                        // StartCoroutine(palier.run());
                        palier.run();
                        // myThread.Start();
                    }

                    break;
                case "D'Alembert":
                    setUpResultView();
                    
                    if (!first){
                        sauteuseValue= null;
                    }

                    if (attaqueTxt=="Sauteuse"){
                        lauchGame = true;

                        if(sauteuseValue==null){
                            getSauteuseValue(fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
                            lauchGame = false;
                        }
                    }

                    if (lauchGame){
                        var varianteTxt = variante.options[variante.value].text;
                        // var  startToInt = Int32.Parse(startTo.GetComponent<InputField>().text);

                        AlembertCmd alembert = new AlembertCmd(m_Path,fictiveMaxReachTxt, varianteTxt, typeOfGain, nbPalierInt,  timePalierInt,  ifMaxPalierTxt,  gainResearchInt,  maxReachTxt,  chanceTxt,  attaqueTxt,  fromBallInt,  toBallInt,  fileNameTxt,  coinValueInt,  maxMiseInt, permanenceSelectedTxt, sauteuseValue, security,securityValue, typeOfMise);
                        montanteManager = alembert.getMontanteManager();
                        alembert.run();
                        // myThread.Start();
                    }
                    break;
                
                case "Fibonaci":
                    setUpResultView();
                    
                    if (!first){
                        sauteuseValue= null;
                    }

                    if (attaqueTxt=="Sauteuse"){
                        lauchGame = true;

                        if(sauteuseValue==null){
                            getSauteuseValue(fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
                            lauchGame = false;
                        }
                    }
                    var fiboStartValueTxt = fiboStartValue.options[fiboStartValue.value].text;
                    var fiboSchemeTxt = fiboScheme.options[fiboScheme.value].text;


                    if (lauchGame){
                        FibonaciCmd fibo = new FibonaciCmd(m_Path, typeOfGain, fictiveMaxReachTxt, fiboStartValueTxt, fiboSchemeTxt, nbPalierInt,  timePalierInt,  ifMaxPalierTxt,  gainResearchInt,  maxReachTxt,  chanceTxt,  attaqueTxt,  fromBallInt,  toBallInt,  fileNameTxt,  coinValueInt,  maxMiseInt, permanenceSelectedTxt, sauteuseValue, security,securityValue, typeOfMise);
                        montanteManager = fibo.getMontanteManager();
                        fibo.run();
                        // myThread.Start();
                    }
                    break;
                case "50/20":
                    setUpResultView();
                    
                    if (!first){
                        sauteuseValue= null;
                    }

                    if (attaqueTxt=="Sauteuse"){
                        lauchGame = true;

                        if(sauteuseValue==null){
                            getSauteuseValue(fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
                            lauchGame = false;
                        }
                    }

                    if (lauchGame){
                        FiftyTwentyCmd fiftyTwenty = new FiftyTwentyCmd(m_Path, typeOfGain, fictiveMaxReachTxt,  nbPalierInt,  timePalierInt,  ifMaxPalierTxt,  gainResearchInt,  maxReachTxt,  chanceTxt,  attaqueTxt,  fromBallInt,  toBallInt,  fileNameTxt,  coinValueInt,  maxMiseInt, permanenceSelectedTxt, sauteuseValue, security,securityValue, typeOfMise);
                        montanteManager = fiftyTwenty.getMontanteManager();
                        fiftyTwenty.run();
                        // myThread.Start();
                    }
                    break;
                default:
                    break;   
            }
            
            watch.Stop();
            Debug.Log("time : " + watch.ElapsedMilliseconds/1000 + "s");
            // myThread.Join();
            if (lauchGame){
                bool stop = true;
                AllExecuteButton.gameObject.SetActive(true);
                string[,] result  = montanteManager.getLines();
                ExecuteButton.onClick.RemoveAllListeners();
                ExecuteButton.interactable = true;
                AllExecuteButton.interactable = true;
                
                // Thread thread = new Thread(() => setUpResult(result,toBallInt,fromBallInt-1, stop, montanteManager));
                // thread.Start();
                setUpResult(result,toBallInt,fromBallInt-1, stop, montanteManager);

                if (SavePath!= ""){
                    saveResult(SavePath,montanteManager,toBallInt,fromBallInt-1);
                }
            }
        }

        IEnumerator WaitOneFrame(float timeToWait) {
            //code
            yield return new WaitForSeconds(timeToWait);
            //code éventuel
        }

        void setUpResult(string[,] result, int toBallInt, int start, bool stop, Montantes montanteManager)
        {
            if (result[0, 8].StartsWith("différentielle")){
                changeHeaderFictive();
            }else{
                changeHeader();
            }
            for (int i = start; i < toBallInt; i++)
            {   
                Debug.Log("line : " + i);
                start=i;
                int lastBilan = 0;
                if (showResult){
                    try{
                        // StartCoroutine(
                             addResult(Int32.Parse(result[i, 0]),Int32.Parse(result[i, 1]),Int32.Parse(result[i, 2]),Int32.Parse(result[i, 3]),
                        Int32.Parse(result[i, 4]),Int32.Parse(result[i, 5]),Int32.Parse(result[i, 6]),result[i, 7],result[i, 8],result[i, 9], result);
                        // );
                        // thread.Start();

                        //     addResult(Int32.Parse(result[i, 0]),Int32.Parse(result[i, 1]),Int32.Parse(result[i, 2]),Int32.Parse(result[i, 3]),
                        // Int32.Parse(result[i, 4]),Int32.Parse(result[i, 5]),Int32.Parse(result[i, 6]),result[i, 7],result[i, 8],result[i, 9], result);
                        // ); //on attendra donc 2,5 secondes

                    }catch (ArgumentNullException){
                        result  = montanteManager.getLines();
                        setUpResult(result,toBallInt, start, stop, montanteManager);
                        break;
                    }
                }
                

                if (i>0 && i< toBallInt-1){
                    lastBilan = Int32.Parse(result[i-1, 5]);
                }

                int miseStat =0;
                int bilanStat =0;
                int gameStat =0;
                int decouvertStat =0;
                int decouvertTotalStat  =0;


                (miseStat, bilanStat, gameStat, decouvertStat, decouvertTotalStat) = calculateStat(Int32.Parse(result[i, 6]),Int32.Parse(result[i, 5]),Int32.Parse(result[i, 3]),lastBilan,Int32.Parse(result[i, 4]));
                
                if (showResult){
                    setUpStat(miseStat, bilanStat, gameStat, decouvertStat, decouvertTotalStat);  
                }else{
                    if (i==toBallInt-1){
                        if (solveurResult.Count > 0){
                            foreach(var res in solveurResult){
                                if (bilanStat>res.Value.Item1){
                                    solveurResult.Clear();
                                    var  timePalierInt = Int32.Parse(timePalier.GetComponent<InputField>().text);
                                    var coinValueInt = Int32.Parse(coinValue.GetComponent<InputField>().text);

                                    solveurResult.Add(coinValueInt,Tuple.Create(bilanStat,timePalierInt) );
                                    break;
                                }
                            }
                        }else{
                            var  timePalierInt = Int32.Parse(timePalier.GetComponent<InputField>().text);
                            var coinValueInt = Int32.Parse(coinValue.GetComponent<InputField>().text);

                            solveurResult.Add(coinValueInt,Tuple.Create(bilanStat,timePalierInt) );
                        }
                    }
                }

                if (stop && showResult){
                    ExecuteButton.onClick.RemoveAllListeners();
                    ExecuteButton.onClick.AddListener(() => setUpResult(result, toBallInt, i+1, stop, montanteManager));
                    AllExecuteButton.onClick.RemoveAllListeners();
                    AllExecuteButton.onClick.AddListener(() => setUpResult(result, toBallInt, i+1, false,montanteManager));
                    break;
                }
                // yield return new WaitForSeconds(2.5f);
            }

            if (start+1==toBallInt){
                ExecuteButton.interactable = false;
                AllExecuteButton.interactable = false;
            }
            // yield return new WaitForSeconds(2.5f);  
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
            ExecuteButton.interactable = true;
            CancelButton.onClick.AddListener(() => setUpParamsView());
            ExecuteButton.onClick.AddListener(() => setUpSauteuseSequence(fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt));
        }

        public void saveResult(string filename, Montantes montante, int toBallInt, int fromBallInt){

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
                    newLine = (Int32.Parse(result[i,0])+1)+","+result[i,1]+","+result[i,2]+","+result[i,3]+" "+result[i,7]+","+result[i,5]+","+result[i,6];
                }else{
                    newLine = (Int32.Parse(result[i,0])+1)+","+result[i,1]+","+result[i,2]+","+result[i,10]+" "+result[i,11]+","+result[i,12]+","+result[i,13]+" "+result[i,14]+","+result[i,15]+","+result[i,3]+","+result[i,5]+","+result[i,6];
                } 
                csv.AppendLine(newLine);
                
            }

            File.AppendAllText(filePath, csv.ToString());
        }

        public void setUpResultView(){
            resultView.SetActive(true);
            rightView.SetActive(false);
            //  Color oldColor = rightView.GetComponent<Renderer>().material.color;
            // Color newColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            // rightView.GetComponent<Renderer>().material.SetColor("a", newColor);

            leftView.SetActive(false);
            sauteuseView.SetActive(false);
            CancelButton.onClick.RemoveAllListeners();
            CancelButton.onClick.AddListener(() => {
                showResult = true;
                SolveurView.SetActive(false);
                setUpParamsView();
            });
        }
        void ClosePopUp(){
            // Debug.Log("BackgroundButtonClicked");
            popUpView.SetActive(false);
            montantePopUpView.SetActive(false);
            popUpBackgroundBtn.gameObject.SetActive(false);
            setUpParamsView();
        }

        public void setUpParamsView(){
            reset();

            // alembertView.SetActive(false);

            resultView.SetActive(false);
            rightView.SetActive(true);
            leftView.SetActive(true);
            sauteuseView.SetActive(false);

            ExecuteButton.onClick.RemoveAllListeners();
            ExecuteButton.interactable = true;
            AllExecuteButton.onClick.RemoveAllListeners();
            AllExecuteButton.gameObject.SetActive(false);
            CancelButton.onClick.RemoveAllListeners();
            CancelButton.onClick.AddListener(() => ClosePopUp());
            // showResult = true; 

            ExecuteButton.onClick.AddListener(() => run(false, null));
        }

        void setUpSauteuseSequence(int fromBallInt, int toBallInt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt){
            Dropdown[] tempDropdown = sauteuseView.GetComponentsInChildren<Dropdown>();
            sauteuseValue = new List<string>();
            foreach (Dropdown item in tempDropdown)
            {
                sauteuseValue.Add(item.options[item.value].text);
            }

            run(true, sauteuseValue);
        }

        public void addResult(int index, int coup, int value, int mise, int coinValueInt,int bilanGame, int bilanTotal, string playerMise,string attaqueTxt,string win, string[,] fictive  ){
 
            var color = montanteManager.valueChance(value, "Rouge");
            GameObject template;

            int mise1 = 0;
            string player1Mise = "";
            int bilanGame1= 0;

            int mise2 = 0;
            string player2Mise= "";
            int bilanGame2 = 0;

            if (attaqueTxt.StartsWith("différentielle")){
                // changeHeaderFictive();
                if (color == "Rouge"){
                     if (Convert.ToBoolean(win)){
                        template = templateLineRedFictive;
                    }
                    else{
                        template = templateLineRedFictiveLoose;   
                    }
                }else{
                    if (Convert.ToBoolean(win)){
                        template = templateLineBlackFictive;
                    }
                    else{
                        template = templateLineBlackFictiveLoose;   
                    }
                }

                 mise1 = Int32.Parse(fictive[index,10]);
                 player1Mise = fictive[index,11];
                 bilanGame1 = Int32.Parse(fictive[index,12]);


                 mise2 = Int32.Parse(fictive[index,13]);
                 player2Mise = fictive[index,14];
                 bilanGame2 = Int32.Parse(fictive[index,15]);

            }else{
                // changeHeader();
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
            var chanceTxt = chanceGame.options[chanceGame.value].text;

            newLine.transform.position = new Vector2(0, calculateYposition(index));
  
            newLine.transform.SetParent(resultContent.transform, false);
            newLine.SetActive(true);
            

            Text[] tempText = newLine.GetComponentsInChildren<Text>();
            
            tempText[0].text = (index+1).ToString();
            tempText[1].text = coup.ToString();
            tempText[2].text = value.ToString();
            if (attaqueTxt.StartsWith("différentielle")){
                tempText[3].text = (mise1).ToString() + " " + player1Mise.Substring(0, 1);
                tempText[4].text = bilanGame1.ToString();
                tempText[5].text = (mise2).ToString() + " " + player2Mise.Substring(0, 1);
                tempText[6].text = bilanGame2.ToString();
                tempText[7].text = (mise).ToString() + " " + (playerMise != null ? playerMise.Substring(0, 1) : playerMise) +""+ (playerMise != null && playerMise.EndsWith("(sécu)") ? " (sécu)" : "") ;
                tempText[8].text = bilanGame.ToString();
                tempText[9].text = bilanTotal.ToString(); 
            }else{
                tempText[3].text = (mise).ToString() + " " + playerMise;
                tempText[4].text = bilanGame.ToString();
                tempText[5].text = bilanTotal.ToString(); 
            }
            lineGameObject.Add(newLine);

            scrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical() ;
            scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical() ;
            scrollRect.verticalNormalizedPosition = 0 ;
            Canvas.ForceUpdateCanvases();
        }
        

        private int calculateYposition(int index){
            // var y = 10;
            var y = -100;
            // var y = 365;
            var ITEM_HEIGHT = 115;

            y -= ITEM_HEIGHT*index;




            return y;
        }
    }
}

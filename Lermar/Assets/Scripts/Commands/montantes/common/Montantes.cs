// using System.Diagnostics;

using System.ComponentModel;

using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRx;
using ViewModel;
using System;
using Components;

namespace Montante
{    
    public class Montantes
    {
        //common

        public int[] redValue = new int[] {1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36};
        public int[] blackValue = new int[] {2,4,6,8,10,11,13,15,17,20,22,24,26,28,29,31,33,35};
        string[] simpleChance =  {"Rouge", "Noir", "Pair", "Impair", "Manque", "Passe"};
        string[] doubleChance =  {"Douzaines", "Colonnes"};

        public int gainResearchInt;
        public string maxReachTxt;
        public string chanceTxt;
        public string attaqueTxt;
        public int fromBallInt;
        public int toBallInt;
        public string fileNameTxt;
        public int coinValueInt;
        public int maxMiseInt;
        public string permanenceSelectedTxt;
             
        public bool security;
        public int securityValue;
        public string typeOfMise;
        public List<string> sauteuseValue = new List<string>();
        public string[,] result = null;
        public string[,] fictivec = null;
        public List<string[,]> fictiveList = new List<string[,]>();

        int fiboCpt1 = 4;
        int fiboCpt2 = 4;
        // public List<string> sauteuseValue = new List<string>();


        public Montantes(){
            
        }

        public string[,] getLines(){
            return result;
        }

        public void resetResult(){
            result = null;
        }


        public string[,] getFictive(int index){
            return fictiveList[index];
        }

        public Montantes getMontanteManager(){
            return this;
        }

        public void setFictiveLine(string[,] line){
            fictiveList.Add(line);

        }

        public Montantes(int gainResearchIntn, string maxReachTxtn, string chanceTxtn, string attaqueTxtn, int fromBallIntn, int toBallIntn, string fileNameTxtn, int coinValueIntn, int maxMiseIntn,string permanenceSelectedTxtn, List<string> sauteuseValuen, bool securityn,int securityValuen, string typeOfMisen){
            gainResearchInt=gainResearchIntn;
            maxReachTxt=maxReachTxtn;
            chanceTxt=chanceTxtn;
            attaqueTxt=attaqueTxtn;
            fromBallInt=fromBallIntn;
            toBallInt=toBallIntn;
            fileNameTxt=fileNameTxtn;
            coinValueInt=coinValueIntn;
            maxMiseInt=maxMiseIntn;
            permanenceSelectedTxt=permanenceSelectedTxtn;
            sauteuseValue=sauteuseValuen;
            security=securityn;
            securityValue=securityValuen;
            typeOfMise=typeOfMisen;


            if (attaqueTxt.StartsWith("")){
                result = new string[readPermanenceFile(permanenceSelectedTxt, -1),16];

            }else{
                result = new string[readPermanenceFile(permanenceSelectedTxt, -1),10];

            }
        }


        public (string, int, int, bool) play(string chanceTxt, string attaqueTxt, int value, bool win, int index, string permanenceSelectedTxt, int coup,int mise,  int timePalierInt,int  nbPalierInt,int  coinValueInt, int maxMiseInt,string  ifMaxPalierTxt,string  maxReachTxt, int gain, bool diff, bool misecalc){
            var newValue = readPermanenceFile(permanenceSelectedTxt,index);
            var lastValue = -1;

            if (index > 0){
                lastValue = readPermanenceFile(permanenceSelectedTxt,index-1);
            }
            var newPlayerMise = getPlayerMise(chanceTxt, newValue, win, index,  lastValue);
            
            var newMise = mise;
                Debug.Log("misecalc  : " + misecalc);
                Debug.Log("newPlayerMise  : " + newPlayerMise);

            if (misecalc && newPlayerMise != null){
                // Debug.Log("calculateMise");
                newMise = calculateMise(coup, mise,  timePalierInt, nbPalierInt, newPlayerMise, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, diff);
                Debug.Log("calculateMise  : " + newMise);

                // newMise = checkMaxMise(newMise, coinValueInt,maxReachTxt,maxMiseInt);
                newMise = checkMaxMise(newMise);

            }

            var newWin = calculateGain(newValue, newPlayerMise, newMise,coinValueInt, gain);
            
            return (newPlayerMise, newValue, newMise, newWin);
        }

        public int calculateSecurity(int mise, int bilan){

            if (mise>Math.Abs(bilan)){
                if (Math.Abs(bilan-mise)>securityValue){
                    if (Math.Abs(bilan)==0){
                        return (1*coinValueInt);
                    } 
                    return Math.Abs(bilan)+1;
                }
            }
            
            return mise;
        }

        // public int checkMaxMise(int mise, int coinValueInt, string maxReachTxt, int maxMiseInt){
        public int checkMaxMise(int mise){
            if ((mise) >= maxMiseInt){
                if (maxReachTxt.StartsWith("Repartir")){
                    mise = 1*coinValueInt;
                }
                else{
                    mise = maxMiseInt;
                }
            }

            return mise;
        }

        public  int calculateMise(int coup, int mise, int timePalier, int nbPalierInt, string playerMise, int coinValueInt, int maxMiseInt, string ifMaxPalierTxt,string maxReachTxt, bool diff){
            // Debug.Log("need to be declare in child");
            if (playerMise != null){

                if (mise==0){
                    mise = 1;
                }

                // if (coup!= 0 && coup%timePalier==0 && (!diff) && (typeOfMise == "En perte")){
                if (coup!= 0 && coup%timePalier==0 && (!diff)){
                    if (mise == nbPalierInt){
                        if (ifMaxPalierTxt.StartsWith("Recommencer")){
                            mise = 1 ;
                        }
                    }else{
                        mise += 1;
                    }
                }
                return mise;
            }
            return 0;
        }
        
        public string valueChance(int value, string playerMise){
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

        public string inverse(string chance){
            if (chance=="Noir"){
                return "Rouge";
            }else if (chance=="Rouge"){
                return "Noir";
            }else if (chance=="Pair"){
                return "Impair";
            }else if (chance=="Impair"){
                return "Pair";
            }else if (chance=="Passe"){
                return "Manque";
            }else if (chance=="Manque"){
                return "Passe";
            }
            return null;
        }

        int calculateFibonacci(int len){
            int a = 0, b = 1, c = 0;
            for (int i = 2; i < len; i++)  
            {  
                c= a + b;  
                a= b;  
                b= c;  
            } 
            return a;
        }
        
        public (string[,], int) calculateFictive(object child, string chanceTxt, string attaqueTxt, int value, bool win, int index, string permanenceSelectedTxt, int mise,  int timePalierInt,int  nbPalierInt,int  coinValueInt, int maxMiseInt,string  ifMaxPalierTxt,string  maxReachTxt, int gain,bool calculMise, string montantetype, string[,] fictiv   ){

            if (attaqueTxt.StartsWith("différentielle")){
                if (fictivec==null || fictiv == null){
                    
                    fictivec = new string[2,5];
                    fictivec[0,0] = mise.ToString(); //mise
                    fictivec[1,0] = mise.ToString(); //mise

                    fictivec[0,2] = "0"; //bilan
                    fictivec[1,2] = "0";//bilan

                    fictivec[0,3] = "0"; // coup
                    fictivec[1,3] = "0";// coup

                     fictivec[0,4] = "false"; // win
                    fictivec[1,4] = "false";// win
                }

                if (fictiv!=null){
                    fictivec = fictiv;
                }

                (var newPlayerMise1, var newValue1, var newMise1,var newWin1 ) = play(chanceTxt, attaqueTxt,value, win, index, permanenceSelectedTxt,Int32.Parse(fictivec[0,3]), Int32.Parse(fictivec[0,0]),  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false, calculMise);
                // Debug.Log("NewMise1 fictive : " +newMise1 );
                
                var inverseChanceTxt = inverse(chanceTxt);
                fictivec[0,4] = newWin1.ToString();

                (var newPlayerMise2, var newValue2, var newMise2,var newWin2 ) = play(inverseChanceTxt, attaqueTxt,value, win, index, permanenceSelectedTxt,Int32.Parse(fictivec[1,3]), Int32.Parse(fictivec[1,0]),  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false, calculMise);
                fictivec[1,4] = newWin2.ToString();
                
                fictivec[0,0] = newMise1.ToString();
                fictivec[1,0] = newMise2.ToString();
    
                fictivec[0,1] = newPlayerMise1;
                fictivec[1,1] = newPlayerMise2;

                var  bilanGame1 =0;
                var  bilanGame2 =0;
                
                
                if (newWin1){
                    bilanGame1 += newMise1;
                }else{
                    if (newValue1 == 0){
                        var ret = 0;
                        if ((newMise1)%2==0){
                            ret =newMise1/2;
                        }else{
                            ret =newMise1/2+1;
                        }
                        bilanGame1 += ret;
                    }else{
                        bilanGame1 -= newMise1;
                    }
                }

                if (newWin2){
                    bilanGame2 += newMise2;
                }else{
                    if (newValue2 == 0){
                        var ret = 0;
                        if ((newMise2)%2==0){
                            ret =newMise2/2;
                        }else{
                            ret =newMise2/2+1;
                        }
                        bilanGame2 += ret;
                    }else{
                        bilanGame2 -= newMise2;
                    }
                }

                fictivec[0,2] = (Int32.Parse(fictivec[0,2]) + bilanGame1).ToString();
                fictivec[1,2] = (Int32.Parse(fictivec[1,2]) + bilanGame2).ToString();

                value = newValue2;

                fictivec[0,3] = (Int32.Parse(fictivec[0,3]) + 1).ToString();// coup

                fictivec[1,3] = (Int32.Parse(fictivec[1,3]) + 1).ToString();// coup
                if (Int32.Parse(fictivec[0,2])>0){
                    fictivec[0,3] = "0"; // coup
                }
                if (Int32.Parse(fictivec[1,2])>0){
                    fictivec[1,3] = "0";// coup
                }  
            }
            return (fictivec, value);
        }

        private bool calculateGain(int value, string playerMise,int mise,int coinValueInt, int gain){
            var result = valueChance(value, playerMise);
            if (string.Compare(result,playerMise)==0){
                gain +=mise;
                return true;
            }else{
                gain -= mise;
                return false;
            }
        }

        public void addResult(int index, int coup, int value, int mise, int coinValueInt,int bilanGame, int bilanTotal, string playerMise,string attaqueTxt, bool win, string[,] fictive ){

            result[index, 0] = index.ToString();
            result[index, 1] = coup.ToString();
            result[index, 2] = value.ToString();
            result[index, 3] = mise.ToString();
            result[index, 4] = coinValueInt.ToString();
            result[index, 5] = bilanGame.ToString();
            result[index, 6] = bilanTotal.ToString();
            result[index, 7] = playerMise;
            result[index, 8] = attaqueTxt;
            result[index, 9] = win.ToString();
            if (fictive != null){
                result[index, 10] = fictive[0,0];
                result[index, 11] =  fictive[0,1];
                result[index, 12] = fictive[0,2];

                result[index, 13] = fictive[1,0];
                result[index, 14] = fictive[1,1];
                result[index, 15] = fictive[1,2];

            }
            // result[index, 8] = a\ttaqueTxt;

            // result[result.Length, 9] = fictive;
        }

        public int readPermanenceFile(string nameFile, int index){
            // Debug.Log("permanenceSelectedTxt");
            // Debug.Log(permanenceSelectedTxt);
            // var permanencePath = "/Users/gregoryarnal/dev/FreeLance/Lermar/Lermar/permanences/MC/" + nameFile;
            var  m_Path = Application.dataPath;

            //Output the Game data path to the console
            var permanencePath = Path.GetDirectoryName(Application.dataPath) +"/permanences/MC/" + nameFile;
            // Debug.Log("index : " + index);

            string[] lines = System.IO.File.ReadAllLines(permanencePath);
            if (index == -1){
                return lines.Length;
            }
            // Debug.Log("lines[index] : " + lines[index]);

            return Int32.Parse(lines[index]);
        }

        string getPlayerMise(string chanceTxt, int value, bool lastWin, int index, int lastValue){
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


    }
}
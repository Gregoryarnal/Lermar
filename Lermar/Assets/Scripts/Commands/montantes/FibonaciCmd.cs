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
using Components;
using System.Numerics;
namespace Montante
{    
    public class FibonaciCmd : Montantes
    {
        // public Montantes parent {get; set;}
        public int nbPalierInt;
        public int timePalierInt;
        public string ifMaxPalierTxt;
        public int startvalue;
        public string scheme;
        int fiboCpt  ;
        List<int> cumulValue;
        List<int> cumulValue1;
        List<int> cumulValue2;
        // int[] cumulValue;
         


        public FibonaciCmd(string startvaluen, string schemen, int nbPalierIntn, int timePalierIntn, string ifMaxPalierTxtn, int gainResearchInt, string maxReachTxt, string chanceTxt, string attaqueTxt, int fromBallInt, int toBallInt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt, List<string> sauteuseValue, bool security, int securityValue, string typeOfMise) 
        : base(gainResearchInt, maxReachTxt, chanceTxt,  attaqueTxt, fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt,permanenceSelectedTxt, sauteuseValue,security,securityValue, typeOfMise)
        {
            nbPalierInt=nbPalierIntn;
            timePalierInt=timePalierIntn;
            ifMaxPalierTxt=ifMaxPalierTxtn;
            if (startvaluen=="{1,1,2..}"){
                fiboCpt= 3;
                startvalue=3;
            }else{
                fiboCpt=4;
                startvalue=4;
            }
            scheme=schemen;

            if (scheme.StartsWith("Cumul")){
                cumulValue = new List<int>();
                cumulValue.Add(1);
                if (attaqueTxt.StartsWith("différentielle")){
                    cumulValue1 = new List<int>();
                    cumulValue1.Add(1);
                    cumulValue2 = new List<int>();
                    cumulValue2.Add(1);
                }
            }
        }

        public void run(){

            var miseInitial = 1;
            var mise = 1;
            var gain = 0;



            var index = 0;
            var bilanTotal = 0;
            var bilanGame = 0;
            var coup = 0;
            var win = true;
            var value = -1;
            int fiboCpt = 4;
            int fiboCpt1 = 4;
            int fiboCpt2 = 4;
            // lineGameObject.Clear();
            // int last = readPermanenceFile(permanenceSelectedTxt, -1);
            bool lauchGame = true;
            var playerMise = "";
            



            string[,] fictive = null;

            // if (lauchGame){
                for (int i = fromBallInt-1; i < toBallInt; i++)
                {   
                    if (!scheme.StartsWith("Cumul")){
                        mise = calculateFibonacci(fiboCpt);
                        (mise,fiboCpt) = checkMaxMise(mise,fiboCpt);

                    }

                    if (attaqueTxt.StartsWith("différentielle")){
                        Debug.Log("fictive before calcul fictive");
                        Debug.Log(fictive);
                        if (fictive!=null){
                            int fmise;
                            (fmise,fiboCpt1,cumulValue1) = calculMise(Convert.ToBoolean(fictive[0,4]), Int32.Parse(fictive[0,0]),  fiboCpt1, cumulValue1);
                            fictive[0,0] = fmise.ToString();
                            (fmise,fiboCpt2,cumulValue2) = calculMise(Convert.ToBoolean(fictive[1,4]), Int32.Parse(fictive[1,0]),  fiboCpt2, cumulValue2);
                            fictive[1,0] = fmise.ToString();
                            value= -1;
                        }
                        
                        (fictive, value) = calculateFictive(this, chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false, "Fibonaci", fictive);
                        setFictiveLine(fictive);
                    }

                    if (fictive==null){
                        ( playerMise,value,mise,win ) = play(chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false, false);
                    }else{
                        if (Int32.Parse(fictive[0,0])==Int32.Parse(fictive[1,0])){
                            mise = 0;
                            playerMise = null;
                            win = false;
                        }else{
                            
                            if (Int32.Parse(fictive[0,0])>Int32.Parse(fictive[1,0])){ //mise1 suppe
                                mise = Int32.Parse(fictive[0,0])-Int32.Parse(fictive[1,0]);
                                var chanceTxtf = fictive[0,1];
                                ( playerMise,value,mise,win ) = play(chanceTxtf, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, true, false);
                            }else{//mise2 suppe
                                mise = Int32.Parse(fictive[1,0])-Int32.Parse(fictive[0,0]);
                                var chanceTxtf = fictive[1,1];
                                ( playerMise,value,mise,win ) = play(chanceTxtf, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, true, false);
                            }
                        }
                    }
                    
                    if (win){
                        gain += mise*coinValueInt;
                        bilanGame += mise*coinValueInt;
                        bilanTotal += mise*coinValueInt;
                        coup += 1;
                    }else{
                        if (value == 0){
                            var ret = 0;
                            if ((mise*coinValueInt)%2==0){
                                ret =mise*coinValueInt/2;
                            }else{
                                ret =mise*coinValueInt/2+1;
                            }
                            gain -= ret;
                            bilanGame -= ret;
                            bilanTotal -= ret;
                        }else{
                            gain -= mise*coinValueInt;
                            bilanGame -= mise*coinValueInt;
                            bilanTotal -= mise*coinValueInt;
                        }
                        coup += 1;
                    }

                    addResult(i,coup, value, mise,coinValueInt,bilanGame,bilanTotal, playerMise, attaqueTxt,win, fictive);


                    (mise,fiboCpt,cumulValue) = calculMise(win, mise,fiboCpt, cumulValue);
                    // mise = nmise;

                    if (win && bilanGame>0){
                        gain = 0;
                        mise = miseInitial;
                        fiboCpt = startvalue;
                         
                        cumulValue = new List<int>();
                        cumulValue.Add(1);

                        if (fictive!=null && attaqueTxt == "différentielle directe"){
                            bilanGame = 0;
                        }else if(attaqueTxt == "différentielle compensée"){
                            bilanGame = 0;
                            // Debug.Log(fictive);

                            fictive = null;
                            // Debug.Log(fictive);
                            Debug.Log("reset fictive au coup : " + coup);
                            // Debug.Log("reset fictive");

                            fiboCpt1 =startvalue;
                            cumulValue1 = new List<int>();
                            cumulValue1.Add(1);

                            fiboCpt2 = startvalue;
                            cumulValue2 = new List<int>();
                            cumulValue2.Add(1);

                            coup = 0;
                            bilanGame = 0;
                            // fictive = new string[0,0];
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
                        if(!attaqueTxt.StartsWith("différentielle")){        
                            coup = 0;
                        }
                    }

                    // if (security){
                    //     fiboCpt = calculateSecurity(mise,bilanGame,fiboCpt );
                    // }

                    index+=1;
                }
        }

        (int, int, List<int>) calculMise(bool win,int mise, int fiboCpt, List<int> cumulValue){
            if (win){
                // Debug.Log("win");
                // Debug.Log(typeOfMise);

                if (typeOfMise=="En gain"){

                    if (scheme.StartsWith("Cumul")){
                        if (cumulValue.Count>=2){
                            mise = cumulValue[cumulValue.Count-2]+cumulValue[cumulValue.Count-1];
                            cumulValue.Add(mise);
                        }else if(cumulValue.Count==1){
                            mise = cumulValue[cumulValue.Count-1];
                            if (mise==1 && startvalue==4){
                                mise +=1;
                            }
                            cumulValue.Add(mise);
                        }
                    }else if (scheme.StartsWith("Jean")){
                        fiboCpt = startvalue;
                    }else{
                        fiboCpt += 1;
                    }
                }else{
                    if (scheme.StartsWith("Cumul")){
                        if (cumulValue.Count!=0){
                            if (cumulValue.Count==1){
                                mise = cumulValue[cumulValue.Count-1];
                            }else{
                                if (cumulValue.Count>=3){
                                    cumulValue.RemoveAt(cumulValue.Count-1);  
                                }

                                cumulValue.RemoveAt(cumulValue.Count-1);
                                cumulValue.RemoveAt(cumulValue.Count-1);

                                if (cumulValue.Count==0){
                                    fiboCpt = startvalue;
                                    mise = calculateFibonacci(fiboCpt);
                                }else if (cumulValue.Count>=2){
                                    mise = cumulValue[cumulValue.Count-2]+cumulValue[cumulValue.Count-1];
                                }else{
                                    mise = cumulValue[cumulValue.Count-1];
                                }
                                cumulValue.Add(mise);

                            }   
                        }
                    }else if (scheme.StartsWith("Jean")){
                        fiboCpt = startvalue;
                    }
                }
            }else{
                if (typeOfMise!="En gain"){

                    if (scheme.StartsWith("Cumul")){
                        if (cumulValue.Count>=2){
                            mise = cumulValue[cumulValue.Count-2]+cumulValue[cumulValue.Count-1];
                            cumulValue.Add(mise);
                        }else if(cumulValue.Count==1){
                            mise = cumulValue[cumulValue.Count-1];
                            if (mise==1 && startvalue==4){
                                mise +=1;
                            }
                            cumulValue.Add(mise);
                        }
                    }else{
                        fiboCpt += 1;
                    }
                }else{
                    if (scheme.StartsWith("Cumul")){
                        if (cumulValue.Count!=0){
                            if (cumulValue.Count==1){
                                mise = cumulValue[cumulValue.Count-1];
                            }else{
                                if (cumulValue.Count>=3){
                                    cumulValue.RemoveAt(cumulValue.Count-1);  
                                }

                                cumulValue.RemoveAt(cumulValue.Count-1);
                                cumulValue.RemoveAt(cumulValue.Count-1);

                                if (cumulValue.Count==0){
                                    fiboCpt = startvalue;
                                    mise = calculateFibonacci(fiboCpt);
                                }else if (cumulValue.Count>=2){
                                    mise = cumulValue[cumulValue.Count-2]+cumulValue[cumulValue.Count-1];
                                }else{
                                    mise = cumulValue[cumulValue.Count-1];
                                }
                                cumulValue.Add(mise);
                            }
                        }
                    }
                }
            }
            return (mise,fiboCpt,cumulValue);
        }

         public (int, int) checkMaxMise(int mise, int fiboCpt){
           
            if ((mise*coinValueInt) >= maxMiseInt){
                if (maxReachTxt.StartsWith("Repartir")){
                    fiboCpt = 4;
                }
                else{
                    fiboCpt -= 1;
                }
            }
            // DebugLog("mise : " + mise);
            
            mise = calculateFibonacci(fiboCpt);

            return (mise,fiboCpt);
        }


        public int calculateSecurity(int mise, int bilan,int fibo){

             if (mise>Math.Abs(bilan)){
                if (Math.Abs(bilan-mise)>securityValue){
                    int cpt = fibo;
                    while (calculateFibonacci(cpt)>Math.Abs(bilan-mise)){
                        cpt -=1;
                    }
                    return cpt;
                }
            }
            return fibo;
        }

        public new void calculFiboMise(){
            Debug.Log("test");
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

        private int calculateYposition(int index){
            var y = 10;
            var ITEM_HEIGHT = 115;

            y -= ITEM_HEIGHT*index;

            return y;

        }

    }
}
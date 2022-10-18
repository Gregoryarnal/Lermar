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
using Components;
using System.Numerics;
namespace Montante
{    
    public class FiftyTwentyCmd : Montantes
    {
        // public Montantes parent {get; set;}
        public int nbPalierInt;
        public int timePalierInt;
        public string ifMaxPalierTxt;
        public int startvalue;
        public string scheme;
        int fiboCpt  ; 
         


        public FiftyTwentyCmd(int nbPalierIntn, int timePalierIntn, string ifMaxPalierTxtn, int gainResearchInt, string maxReachTxt, string chanceTxt, string attaqueTxt, int fromBallInt, int toBallInt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt, List<string> sauteuseValue, bool security, int securityValue, string typeOfMise) 
        : base(gainResearchInt, maxReachTxt, chanceTxt,  attaqueTxt, fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt,permanenceSelectedTxt, sauteuseValue,security,securityValue, typeOfMise)
        {
            nbPalierInt=nbPalierIntn;
            timePalierInt=timePalierIntn;
            ifMaxPalierTxt=ifMaxPalierTxtn;
            // if (startvaluen=="{1,1,2..}"){
            //     fiboCpt= 3;
            //     startvalue=3;
            // }else{
            //     fiboCpt=4;
            //     startvalue=4;
            // }
            // scheme=schemen;
        }

        public void run(){

            var miseInitial = 1*coinValueInt;
            var mise = 1 *coinValueInt ;
            var gain = 0;



            var index = 0;
            var bilanTotal = 0;
            var bilanGame = 0;
            var coup = 0;
            var win = true;
            var value = -1;
            int fiboCpt = 4;
            // lineGameObject.Clear();
            // int last = readPermanenceFile(permanenceSelectedTxt, -1);
            bool lauchGame = true;
            var playerMise = "";
            



            string[,] fictive = null;

            for (int i = fromBallInt-1; i < toBallInt; i++)
            {

                mise = checkMaxMise(mise);

                if (attaqueTxt.StartsWith("différentielle")){
                    (fictive, value) = calculateFictive(chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false, null, null);
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
                            chanceTxt = fictive[0,1];
                            ( playerMise,value,mise,win ) = play(chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, true, false);
                        }else{//mise2 suppe
                        
                            mise = Int32.Parse(fictive[1,0])-Int32.Parse(fictive[0,0]);
                            chanceTxt = fictive[1,1];
                            ( playerMise,value,mise,win ) = play(chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, true, false);
                        }
                    }
                }
                
                if (win){
                    gain += mise;
                    bilanGame += mise;
                    bilanTotal += mise;
                    coup += 1;
                }else{
                    if (value == 0){
                        var ret = 0;
                        if ((mise)%2==0){
                            ret =mise/2;
                        }else{
                            ret =mise/2+1;
                        }
                        gain -= ret;
                        bilanGame -= ret;
                        bilanTotal -= ret;
                    }else{
                        gain -= mise;
                        bilanGame -= mise;
                        bilanTotal -= mise;
                    }
                    coup += 1;
                }

                addResult(i,coup, value, mise,coinValueInt,bilanGame,bilanTotal, playerMise, attaqueTxt,win, fictive);

                if (win){
                    if (mise==3){
                        mise +=1;
                    }
                    if (typeOfMise=="En gain"){
                        mise = mise + Convert.ToInt32(Math.Ceiling(mise*0.2));
                    }else{
                        mise = mise - Convert.ToInt32(Math.Ceiling(mise*0.2));
                    }
                }else{
                    if (mise==3){
                        mise +=1;
                    }
                    if (typeOfMise=="En gain"){
                        mise = mise - Convert.ToInt32(Math.Ceiling(mise*0.5));
                    }else{
                        mise = mise + Convert.ToInt32(Math.Ceiling(mise*0.5));
                    }
                }
                if (mise == 1){
                    mise +=1;
                }
 
                if (win && bilanGame>0){
                    gain = 0;
                    mise = miseInitial;

                    if (fictive!=null && attaqueTxt == "différentielle directe"){
                        bilanGame = 0;
                    }else if(attaqueTxt == "différentielle compensée"){
                        bilanGame = 0;
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

        //  public int checkMaxMise(int mise){
           
        //     if ((mise*coinValueInt) >= maxMiseInt){
        //         if (maxReachTxt.StartsWith("Repartir")){
        //             mise = 1;
        //         }
        //         else{
        //             mise = maxMiseInt;
        //         }
        //     }
        //     return mise;
        // }


        public int calculateSecurity(int mise, int bilan,int fibo){
                        // Debug.Log("calculateFibonacci in calculateSecurity");
            
             if (mise>Math.Abs(bilan)){
                        // Debug.Log("fisrt if in calculateSecurity");

                if (Math.Abs(bilan-mise)>securityValue){
                        // Debug.Log("second if  in calculateSecurity");

                    int cpt = fibo;
                    while (calculateFibonacci(cpt)>Math.Abs(bilan-mise)){
                        // Debug.Log("calculateFibonacci in while");

                        cpt -=1;
                    }

            // Debug.Log("return cpt " + cpt);

                    return cpt;
                }
            }
            // Debug.Log("return fibo " + fibo);
            
            return fibo;
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
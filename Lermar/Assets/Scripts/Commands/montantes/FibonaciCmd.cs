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


        /// <summary>
        /// Create Fibonaci method
        /// </summary>
        /// <param name="startvaluen"></param>
        /// <param name="schemen"></param>
        /// <param name="nbPalierIntn"></param>
        /// <param name="timePalierIntn"></param>
        /// <param name="ifMaxPalierTxtn"></param>
        /// <param name="gainResearchInt"></param>
        /// <param name="maxReachTxt"></param>
        /// <param name="chanceTxt"></param>
        /// <param name="attaqueTxt"></param>
        /// <param name="fromBallInt"></param>
        /// <param name="toBallInt"></param>
        /// <param name="fileNameTxt"></param>
        /// <param name="coinValueInt"></param>
        /// <param name="maxMiseInt"></param>
        /// <param name="permanenceSelectedTxt"></param>
        /// <param name="sauteuseValue"></param>
        /// <param name="security"></param>
        /// <param name="securityValue"></param>
        /// <param name="typeOfMise"></param>

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

        /// <summary>
        /// Play fibonaci method 
        /// </summary>
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
                        if (fictive!=null){
                            int fmise;
                            int fbilan;

                            (fmise,fiboCpt1,cumulValue1,fbilan) = calculMise(Convert.ToBoolean(fictive[0,4]), Int32.Parse(fictive[0,0]),  fiboCpt1, cumulValue1, Int32.Parse(fictive[0,2]));
                            fictive[0,0] = fmise.ToString();
                            fictive[0,2] = fbilan.ToString();
                            
                            // Debug.Log("bef 2 " + i);
                            (fmise,fiboCpt2,cumulValue2,fbilan) = calculMise(Convert.ToBoolean(fictive[1,4]), Int32.Parse(fictive[1,0]),  fiboCpt2, cumulValue2, Int32.Parse(fictive[1,2]));
                            // Debug.Log("after calcukl mise : " + fmise);
                            fictive[1,0] = fmise.ToString();
                            fictive[1,2] = fbilan.ToString();
                            // Debug.Log("after 2 " + i);

                            value= -1;
                        }

                        (fictive, value) = calculateFictive(this, chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false, "Fibonaci", fictive);
                        // Debug.Log("after calculateFictive 2 " + fictive[1,0] + " at " + i);
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


                    (mise,fiboCpt,cumulValue, _) = calculMise(win, mise,fiboCpt, cumulValue, bilanGame);

                    if (fictive!=null){
                        if (Int32.Parse(fictive[0,2])>0){
                            fictive[0,2]="0";// bilan
                            fictive[0,0] = "1"; // mise
                        }
                        if (Int32.Parse(fictive[1,2])>0){
                            fictive[1,2]="0"; // bilan
                            fictive[1,0] = "1"; // mise
                        }   
                    }
                    


                    if (win && (bilanGame>0 || scheme.StartsWith("Jean")) && gainResearchInt==0){
                        gain = 0;
                        mise = miseInitial;
                        fiboCpt = startvalue;
                         
                        cumulValue = new List<int>();
                        cumulValue.Add(1);

                        if (fictive!=null && attaqueTxt == "différentielle directe"){
                            //  fictive = null;
                            // fiboCpt1 = startvalue;
                            // cumulValue1 = new List<int>();
                            // cumulValue1.Add(1);

                            // fiboCpt2 = startvalue;
                            // cumulValue2 = new List<int>();
                            // cumulValue2.Add(1);

                            coup = 0;
                            bilanGame = 0;

                        }else if(attaqueTxt == "différentielle compensée"){
                            fictive = null;
                            fiboCpt1 = startvalue;
                            cumulValue1 = new List<int>();
                            cumulValue1.Add(1);

                            fiboCpt2 = startvalue;
                            cumulValue2 = new List<int>();
                            cumulValue2.Add(1);

                            coup = 0;
                            bilanGame = 0;
                        }else if(!attaqueTxt.StartsWith("différentielle")){
                            coup = 0;
                            bilanGame = 0;
                        }
                    }else if (win && bilanGame>=gainResearchInt && gainResearchInt!=0){
                        
                        fictive = null;
                        fiboCpt1 = startvalue;
                        cumulValue1 = new List<int>();
                        cumulValue1.Add(1);

                        fiboCpt2 = startvalue;
                        cumulValue2 = new List<int>();
                        cumulValue2.Add(1);

                        fiboCpt=startvalue;

                        coup = 0;
                        bilanGame = 0;
                    }
                    
                    if (playerMise==null){
                        if(!attaqueTxt.StartsWith("différentielle")){        
                            coup = 0;
                        }
                    }

                    if (security){
                        fiboCpt = calculateSecurity(mise,bilanGame,fiboCpt );
                    }

                    index+=1;

                    
                }
        }
        
        /// <summary>
        /// Calcul specific mise for fibonaci, 3 methodes : Greg, classique, jean compta
        /// </summary>
        /// <param name="win"></param>
        /// <param name="mise"></param>
        /// <param name="fiboCpt"></param>
        /// <param name="cumulValue"></param>
        /// <returns>int</returns>
        (int, int, List<int> ,int) calculMise(bool win,int mise, int cpt, List<int> cumulValue, int bilan){
            if (win){
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
                        if (bilan > 0){
                            bilan = 0;
                        }
                        cpt = startvalue;
                        mise = calculateFibonacci(cpt);

                    }else{
                        cpt += 1;
                        mise = calculateFibonacci(cpt);
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
                    else if (scheme.StartsWith("Jean")){
                        cpt = startvalue;
                        if (bilan > 0){
                            bilan = 0;
                        }
                        mise = calculateFibonacci(cpt);

                    }

                }
            }else{
                if (typeOfMise=="En perte"){

                    if (scheme.StartsWith("Cumul")){
                        // Debug.Log("cumul Value " + cumulValue.Count);
                        if (cumulValue.Count>=2){
                            mise = cumulValue[cumulValue.Count-2]+cumulValue[cumulValue.Count-1];
                            cumulValue.Add(mise);
                            // Debug.Log("mise  " + mise);

                        }else if(cumulValue.Count==1){
                            mise = cumulValue[cumulValue.Count-1];
                            if (mise==1 && startvalue==4){
                                mise +=1;
                            }
                            cumulValue.Add(mise);
                        }
                    }else{
                        cpt += 1;
                        mise = calculateFibonacci(cpt);
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
                                    cpt = startvalue;
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
            return (mise,cpt,cumulValue, bilan);
        }

         public (int, int) checkMaxMise(int mise, int fiboCpt){
           
            if ((mise*coinValueInt) > maxMiseInt){
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
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

namespace Montante
{    
    public class AlembertCmd : Montantes
    {
        // public Montantes parent {get; set;}
        public int nbPalierInt;
        // public int timePalierInt;
        public string ifMaxPalierTxt;
        public string typeOfGainTxt;


        

        public AlembertCmd(string typeOfGain, int nbPalierIntn, int timePalierIntn, string ifMaxPalierTxtn, int gainResearchInt, string maxReachTxt, string chanceTxt, string attaqueTxt, int fromBallInt, int toBallInt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt, List<string> sauteuseValue, bool security,int securityValue,string typeOfMise) 
        : base(gainResearchInt, maxReachTxt, chanceTxt,  attaqueTxt, fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt,permanenceSelectedTxt, sauteuseValue,security,securityValue, typeOfMise,timePalierIntn)
        {
            typeOfGainTxt=typeOfGain;
            nbPalierInt=nbPalierIntn;
            // timePalierInt=timePalierIntn;
            ifMaxPalierTxt=ifMaxPalierTxtn;
        }

        public void run(){

            var miseInitial = 1*coinValueInt;
            var mise = 1*coinValueInt;
            var mise1 = 1*coinValueInt;
            var mise2 = 1*coinValueInt;
            var gain = 0;



            var index = 0;
            var bilanTotal = 0;
            var bilanGame = 0;
            var coup = 0;
            var win = true;
            var value = -1;

            bool lauchGame = true;
            var playerMise = "";

            var firstWin = false;
            var firstWin1 = false;
            var firstWin2 = false;

            var cptWin = 0;
            var cptWin1 = 0;
            var cptWin2 = 0;
            

            string[,] fictive = null;

                for (int i = fromBallInt-1; i < toBallInt; i++)
                {
                    timePalierInt = 1;
                    nbPalierInt=1;
                    if (attaqueTxt.StartsWith("différentielle")){
                        (fictive, value) = calculateFictive(this, chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false, "D'Alembert", fictive);
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

                    // if (fictive != null){
                    //     if ( Int32.Parse(fictive[0,2]) > 0){
                    //         fictive[0,0] = (Int32.Parse(fictive[0,0]) - 1).ToString();
                    //     }else{
                    //         fictive[0,0] = (Int32.Parse(fictive[0,0]) + 1).ToString();
                    //     }

                    //     if ( Int32.Parse(fictive[1,2]) > 0){
                    //         fictive[1,0] = (Int32.Parse(fictive[1,0]) - 1).ToString();
                    //     }else{
                    //         fictive[1,0] = (Int32.Parse(fictive[1,0]) + 1).ToString();
                    //     }
                    // }

                    if (fictive!=null){
                        if (typeOfMise=="En gain"){
                            (var nmise1,var ncptWin1,var nfirstWin1) =  calculeMise(Convert.ToBoolean(fictive[0,4]), typeOfMise, typeOfGainTxt, Int32.Parse(fictive[0,0]) , cptWin1, firstWin1, Int32.Parse(fictive[0,3]));
                            mise1 = nmise1;
                            cptWin1 = ncptWin1;
                            firstWin1 = nfirstWin1;
                        }else{
                            mise1 = calculMisePerte(Int32.Parse(fictive[0,0]), Convert.ToBoolean(fictive[0,4]));
                        }

                        fictive[0,0] = (mise1).ToString();

                        if (typeOfMise=="En gain"){

                            (var nmise2,var ncptWin2,var nfirstWin2) =  calculeMise(Convert.ToBoolean(fictive[1,4]), typeOfMise, typeOfGainTxt, Int32.Parse(fictive[1,0]) , cptWin2, firstWin2, Int32.Parse(fictive[1,3]));

                            mise2 = nmise2;
                            cptWin2 = ncptWin2;
                            firstWin2 = nfirstWin2;
                        }else{
                            mise2 = calculMisePerte(Int32.Parse(fictive[1,0]), Convert.ToBoolean(fictive[1,4]));
                        }

                        fictive[1,0] = (mise2).ToString();


                        if ((Convert.ToBoolean(fictive[0,4]) && Int32.Parse(fictive[0,2])>0 && gainResearchInt==0 && (typeOfGainTxt!="Continue" || (typeOfGainTxt=="Continue" && !firstWin1)))||(!Convert.ToBoolean(fictive[0,4]) && typeOfGainTxt=="Continue" && firstWin1)){
                            fictive[0,2] = "0"; //bilan
                            fictive[0,0] = "1"; // mise
                            fictivec[0,3] = "0";
                        }
                        if ((Convert.ToBoolean(fictive[1,4]) && Int32.Parse(fictive[1,2])>0 && gainResearchInt==0 && (typeOfGainTxt!="Continue" || (typeOfGainTxt=="Continue" && !firstWin2)))||(!Convert.ToBoolean(fictive[1,4]) && typeOfGainTxt=="Continue" && firstWin2)){
                            fictive[1,2]="0";//bilan
                            fictive[1,0] = "1"; // mise
                            fictivec[1,3] = "0";
                        }  
                        
                    }


                   if (typeOfMise=="En gain"){
                        (var nmise,var ncptWin,var nfirstWin) =  calculeMise(win, typeOfMise, typeOfGainTxt, mise, cptWin, firstWin, coup);
                        
                        mise = nmise;
                        cptWin = ncptWin;
                        firstWin = nfirstWin;
                   }else{

                    // (var nmise,var ncptWin,var nfirstWin)
                        mise = calculMisePerte(mise, win);
                   }

                    

                    if ((win && bilanGame>0 && gainResearchInt==0 && (typeOfGainTxt!="Continue" || (typeOfGainTxt=="Continue" && !firstWin)))||(!win && typeOfGainTxt=="Continue" && firstWin)){
                        gain = 0;
                        mise = miseInitial;
                        cptWin =0;

                        if(attaqueTxt == "différentielle compensée"){
                            bilanGame = 0;
                            fictive = null;
                            coup = 0;
                        }else if(!attaqueTxt.StartsWith("différentielle")){
                            coup = 0;
                            bilanGame = 0;
                        }
                    }else if (win && bilanGame>=gainResearchInt && gainResearchInt!=0){
                        gain = 0;
                        mise = miseInitial;

                        if(attaqueTxt == "différentielle compensée"){
                            bilanGame = 0;
                            fictive = null;
                            coup = 0;

                        }else if(!attaqueTxt.StartsWith("différentielle")){
                            coup = 0;
                            bilanGame = 0;
                        }
                    } 
                    
            



                    // if (bilanTotal>=gainResearchInt && gainResearchInt!=0){
                    //     Debug.Log("End gainResearchInt");
                    //     break;
                    // }
                    
                    if (playerMise==null){
                        if(attaqueTxt!="différentielle directe"){        
                            coup = 0;
                        }
                    }

                    if (security){
                        mise = calculateSecurity(mise,bilanGame, coup);
                    }
                    index+=1;
                }
            // }
            // if (fileNameTxt!=""){
            // saveResult(fileNameTxt);

            // }
        }

       int calculMisePerte(int misen, bool winn){
            if (winn) {
                if (misen > 1){
                    misen -= 1;
                }
            }else{
                misen += 1;

            }

            return misen;
       }
        

        private int calculateYposition(int index){
            var y = 10;
            var ITEM_HEIGHT = 115;

            y -= ITEM_HEIGHT*index;

            return y;

        }

    }
}
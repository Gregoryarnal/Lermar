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
    public class APalierCmd : Montantes
    {
        // public Montantes parent {get; set;}
        // public int nbPalierInt;
        // // public int timePalierInt;
        // public string ifMaxPalierTxt;
        public string typeOfGainTxt;

         


        public APalierCmd(string fictiveMaxReachTxtn, string typeOfGain, int nbPalierIntn, int timePalierIntn, string ifMaxPalierTxtn, int gainResearchInt, string maxReachTxt, string chanceTxt, string attaqueTxt, int fromBallInt, int toBallInt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt, List<string> sauteuseValue, bool security,int securityValue,string typeOfMise) 
        : base(fictiveMaxReachTxtn, gainResearchInt, maxReachTxt, chanceTxt,  attaqueTxt, fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt,permanenceSelectedTxt, sauteuseValue, security,securityValue, typeOfMise,timePalierIntn, ifMaxPalierTxtn, nbPalierIntn)
        {
            typeOfGainTxt=typeOfGain;
            // fictiveMaxReachTxt = fictiveMaxReachTxtn;
            if (typeOfMise=="En perte"){
                typeOfGainTxt="";
            }else if (typeOfMise=="En perte et en gain" && !attaqueTxt.StartsWith("différentielle")){
                typeOfMise = "En perte";
            }
        }



        public void run(){

            var miseInitial = 1*coinValueInt;
            var mise = 1*coinValueInt;
            var mise1 = 1*coinValueInt;
            var mise2 = 1*coinValueInt;
            var gain = 0;

            var firstWin = false;
            var firstWin1 = false;
            var firstWin2 = false;

            var index = 0;
            var bilanTotal = 0;
            var bilanGame = 0;
            var coup = 0;
            var win = true;
            var value = -1;
            // var lastValue = -1;

            // bool lauchGame = true;
            var playerMise = "";

            var cptWin = 0;
            var cptWin1 = 0;
            var cptWin2 = 0;
            
            int cptValuePlay = 0;
            int cptValuePlay1 = 0;
            int cptValuePlay2 = 0;

            string[,] fictive = null;

            // if (lauchGame){
                for (int i = fromBallInt-1; i < toBallInt; i++)
                {
                    if (attaqueTxt.StartsWith("différentielle")){
                        (fictive, value) = calculateFictive(this, cptValuePlay1,cptValuePlay2, chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false, "A paliers", fictive);
                        cptValuePlay1 = Int32.Parse(fictive[0,5]);
                        cptValuePlay2 = Int32.Parse(fictive[1,5]);
                        setFictiveLine(fictive);
                    }

                    if (fictive==null){
                        ( playerMise,value,mise,win, cptValuePlay ) =  play(cptValuePlay, chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false, false);
                    }else{
                        if (Int32.Parse(fictive[0,0])==Int32.Parse(fictive[1,0])){
                            mise = 0;
                            playerMise = null;
                            win = false;
                        }else{
                            
                            if (Int32.Parse(fictive[0,0])>Int32.Parse(fictive[1,0])){ //mise1 suppe
                                mise = Int32.Parse(fictive[0,0])-Int32.Parse(fictive[1,0]);
                                if (security){
                                    (mise, cptValuePlay1) = calculateSecurity(cptValuePlay1, mise,bilanGame, coup);
                                }
                                var chanceTxtf = fictive[0,1];
                                ( playerMise,value,mise,win, cptValuePlay ) =  play(cptValuePlay,chanceTxtf, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, true, true);
                            }else{//mise2 suppe
                            
                                mise = Int32.Parse(fictive[1,0])-Int32.Parse(fictive[0,0]);
                                if (security){
                                    (mise, cptValuePlay2) = calculateSecurity(cptValuePlay2, mise,bilanGame, coup);
                                }
                                var chanceTxtf = fictive[1,1];
                                ( playerMise,value,mise,win, cptValuePlay ) =  play(cptValuePlay,chanceTxtf, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, true, true);
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
                                ret =(mise/2)+1;
                            }
                            gain -= ret;
                            bilanGame -= ret;
                            bilanTotal -= ret;
                            // mise = ret;
                        }
                        else{
                            gain -= mise;
                            bilanGame -= mise;
                            bilanTotal -= mise;
                        }

                        coup += 1;
                    }

                    addResult(i,coup, value, mise,coinValueInt,bilanGame,bilanTotal, playerMise, attaqueTxt,win, fictive);                    

                    if (fictive!=null){
                        
                        var typeOfMise1 = typeOfMise;
                        var typeOfMise2 = typeOfMise;
                        if (typeOfMise=="En perte et en gain"){
                            typeOfMise1 = "En perte";
                            typeOfMise2 = "En gain";
                        }

                        (var nmise1,var ncptWin1,var nfirstWin1) =  calculeMise(Convert.ToBoolean(fictive[0,4]), typeOfMise1, typeOfGainTxt, Int32.Parse(fictive[0,0]) , cptWin1, firstWin1, Int32.Parse(fictive[0,3]));

                        mise1 = nmise1;
                        cptWin1 = ncptWin1;
                        firstWin1 = nfirstWin1;

                        fictive[0,0] = (mise1).ToString();

                        (var nmise2,var ncptWin2,var nfirstWin2) =  calculeMise(Convert.ToBoolean(fictive[1,4]), typeOfMise2, typeOfGainTxt, Int32.Parse(fictive[1,0]) , cptWin2, firstWin2, Int32.Parse(fictive[1,3]));

                        mise2 = nmise2;
                        cptWin2 = ncptWin2;
                        firstWin2 = nfirstWin2;

                        fictive[1,0] = (mise2).ToString();
                        Debug.Log("fictiveMaxReachTxt :" + fictiveMaxReachTxt);
                        if (
                            (Convert.ToBoolean(fictive[0,4]) && fictiveMaxReachTxt!="En continue" && Int32.Parse(fictive[0,2])> 0 && typeOfGainTxt!="Continue" ) ||
                            (Convert.ToBoolean(fictive[0,4]) && fictiveMaxReachTxt!="En continue" && Int32.Parse(fictive[0,2])> 0 && typeOfMise=="En gain" && typeOfGainTxt=="Continue" && !firstWin1) ||
                            (!Convert.ToBoolean(fictive[0,4]) && typeOfGainTxt=="Continue" && firstWin1) ||
                            (Convert.ToBoolean(fictive[0,4]) && fictiveMaxReachTxt!="En continue" && firstWin1 && ((typeOfMise=="En gain" && typeOfGainTxt!="Continue") || typeOfMise=="En perte"  ))
                            ){

                            if(attaqueTxt == "différentielle compensée"|| (attaqueTxt == "différentielle directe" && typeOfMise=="En gain")){
                                fictive[0,2] = "0"; //bilan
                                fictive[0,0] = miseInitial.ToString(); //mise
                                fictive[0,3] = "0"; //coup
                                cptWin1 = 0;
                            }else if(typeOfMise=="En perte"){
                                fictive[0,2] = "0"; //bilan
                                fictive[0,0] = miseInitial.ToString(); // mise
                                fictive[0,3] = "0";//coup
                                cptWin1 = 0;
                            }
                            firstWin1 = false;
                        }

                        
                        if (
                            (Convert.ToBoolean(fictive[1,4]) && fictiveMaxReachTxt!="En continue" && Int32.Parse(fictive[1,2])> 0  && typeOfGainTxt!="Continue" ) ||
                            (Convert.ToBoolean(fictive[1,4]) && fictiveMaxReachTxt!="En continue" && Int32.Parse(fictive[1,2])> 0 && typeOfMise=="En gain" && typeOfGainTxt=="Continue" && !firstWin2) ||
                            (!Convert.ToBoolean(fictive[1,4]) && typeOfGainTxt=="Continue" && firstWin2) ||
                            (Convert.ToBoolean(fictive[1,4]) && fictiveMaxReachTxt!="En continue" && firstWin2 && ((typeOfMise=="En gain" && typeOfGainTxt!="Continue") || typeOfMise=="En perte"))
                            ){
                            if(attaqueTxt == "différentielle compensée" || (attaqueTxt == "différentielle directe" && typeOfMise=="En gain")){
                                fictive[1,2]="0";//bilan
                                fictive[1,0] = miseInitial.ToString(); // mise
                                fictive[1,3] = "0"; //coup
                                cptWin2 = 0;
                            } else if(typeOfMise=="En perte"){
                                fictive[1,2]="0";//bilan
                                fictive[1,0] = miseInitial.ToString(); // mise
                                fictive[1,3] = "0"; //coup
                                cptWin2 = 0;
                                
                            }
                            firstWin2 = false;
                        }
                    }

                    (var nmise,var ncptWin,var nfirstWin) =  calculeMise( win, typeOfMise, typeOfGainTxt, mise, cptWin, firstWin, coup);

                    mise = nmise;
                    cptWin = ncptWin;
                    firstWin = nfirstWin;

                    if (
                        
                        (win && bilanGame>=gainResearchInt && gainResearchInt!=0 && typeOfGainTxt!="Continue" ) || 
                        (win && bilanGame>=gainResearchInt && gainResearchInt!=0 && typeOfGainTxt=="Continue" && typeOfMise=="En gain" && !firstWin)||
                        (!win && typeOfMise=="En gain" && typeOfGainTxt=="Continue" && firstWin)||
                        (win && firstWin && ((typeOfMise=="En gain" && typeOfGainTxt!="Continue") || typeOfMise=="En perte"  ))
                        
                        ){

                        gain = 0;
                        mise = miseInitial;
                        cptWin =0;
                        cptWin1 =0;
                        cptWin2 =0;
                        firstWin = false;
                        if(attaqueTxt == "différentielle compensée"){
                            bilanGame = 0;
                            fictive = null;
                            coup = 0;
                        }
                        else if(!attaqueTxt.StartsWith("différentielle")){
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

                    if (playerMise==null){
                        if(!attaqueTxt.StartsWith("différentielle")){        
                            coup = 0;
                        }
                    }

                   if (security){
                        (mise, cptValuePlay) = calculateSecurity(cptValuePlay,mise,bilanGame,coup);
                    }

                    index+=1;
                }
        }

        // (int,int,bool) calculeMise(bool gamewin, string typeOfMisen, string typeOfGainTxtn, int misen,int cptWinn, bool firstWinn,  int coupn){
        //     if(gamewin && typeOfMisen=="En gain" ){
        //         if (typeOfGainTxtn=="Normal"){
        //             misen += 1;
        //         }else if(typeOfGainTxtn=="Réel" ){
        //             cptWinn += 1;
        //             if (cptWinn%timePalierInt==0){
        //                 misen += 1;
        //                 cptWinn =0;
        //             }
        //         }else if(typeOfGainTxtn=="Continue"){
        //             misen += 1;
        //             if (coupn == 1){
        //                 firstWinn = true;
        //             } 
        //         }
        //     }else if (!gamewin && typeOfMisen=="En gain"){
        //         if (typeOfGainTxtn=="Continue" && coupn == 1 ){
        //             firstWinn = false;
        //         }else if(typeOfGainTxtn=="Réel" ){
        //             if (cptWinn%timePalierInt==0){
        //                 misen += 1; 
        //                 cptWinn =0;
        //             }
        //         }
        //     }
        //     return (misen,cptWinn, firstWinn);
        // }
        
        // void setUpSauteuseSequence(int fromBallInt, int toBallInt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt){
        //     Dropdown[] tempDropdown = sauteuseView.GetComponentsInChildren<Dropdown>();
        //     foreach (Dropdown item in tempDropdown)
        //     {
        //         sauteuseValue.Add(item.options[item.value].text);
        //     }
        //     setUpResultView();
        //     // ApalierCmd(fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt, permanenceSelectedTxt);
        // }

        private int calculateYposition(int index){
            var y = 10;
            var ITEM_HEIGHT = 115;

            y -= ITEM_HEIGHT*index;

            return y;

        }

    }
}
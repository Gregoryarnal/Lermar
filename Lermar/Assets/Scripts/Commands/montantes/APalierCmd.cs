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
        public int nbPalierInt;
        public int timePalierInt;
        public string ifMaxPalierTxt;

         


        public APalierCmd(int nbPalierIntn, int timePalierIntn, string ifMaxPalierTxtn, int gainResearchInt, string maxReachTxt, string chanceTxt, string attaqueTxt, int fromBallInt, int toBallInt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt, List<string> sauteuseValue, bool security,int securityValue,string typeOfMise) 
        : base(gainResearchInt, maxReachTxt, chanceTxt,  attaqueTxt, fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt,permanenceSelectedTxt, sauteuseValue, security,securityValue, typeOfMise)
        {
            nbPalierInt=nbPalierIntn;
            timePalierInt=timePalierIntn;
            ifMaxPalierTxt=ifMaxPalierTxtn;
        }



        public void run(){

            var miseInitial = 1*coinValueInt;
            var mise = 1*coinValueInt;
            var gain = 0;



            var index = 0;
            var bilanTotal = 0;
            var bilanGame = 0;
            var coup = 0;
            var win = true;
            var value = -1;
            // lineGameObject.Clear();
            // int last = readPermanenceFile(permanenceSelectedTxt, -1);
            bool lauchGame = true;
            var playerMise = "";
            

            string[,] fictive = null;

            // if (lauchGame){
                for (int i = fromBallInt-1; i < toBallInt; i++)
                {
                    if (attaqueTxt.StartsWith("différentielle")){
                        (fictive, value) = calculateFictive(this, chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, true, "A paliers", fictive);

                        setFictiveLine(fictive);
                    }

                    if (fictive==null){
                        ( playerMise,value,mise,win ) = play( chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false, true);
                    }else{
                        if (Int32.Parse(fictive[0,0])==Int32.Parse(fictive[1,0])){
                            mise = 0;
                            playerMise = null;
                            win = false;
                        }else{
                            
                            if (Int32.Parse(fictive[0,0])>Int32.Parse(fictive[1,0])){ //mise1 suppe
                                mise = Int32.Parse(fictive[0,0])-Int32.Parse(fictive[1,0]);
                                var chanceTxtf = fictive[0,1];
                                ( playerMise,value,mise,win ) = play(chanceTxtf, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, true, true);
                            }else{//mise2 suppe
                            
                                mise = Int32.Parse(fictive[1,0])-Int32.Parse(fictive[0,0]);
                                var chanceTxtf = fictive[1,1];
                                ( playerMise,value,mise,win ) = play(chanceTxtf, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, true, true);
                            }
                        }
                    }
                    

                     
                    
                    if (win){
                        gain += mise;
                        bilanGame += mise;
                        bilanTotal += mise;
                        // Debug.Log("Add coup");

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
                    if (fictive!=null){
                        if (Int32.Parse(fictive[0,2])>0){
                                fictive[0,2]="0";
                                fictive[0,0] = "1"; // mise
                            }
                        if (Int32.Parse(fictive[1,2])>0){
                            fictive[1,2]="0";
                            fictive[1,0] = "1"; // mise
                        }   
                    }
                    
                    if (win && bilanGame>0){
                        gain = 0;
                        mise = miseInitial;

                        if (fictive!=null && attaqueTxt == "différentielle directe"){
                            bilanGame = 0;
                        }else if(attaqueTxt == "différentielle compensée"){
                            bilanGame = 0;
                            fictive = null;
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

                    if (security){
                        mise = calculateSecurity(mise,bilanGame);
                    }

                    index+=1;
                }
        }


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
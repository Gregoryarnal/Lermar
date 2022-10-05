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
        public int timePalierInt;
        public string ifMaxPalierTxt;

         


        public AlembertCmd(int nbPalierIntn, int timePalierIntn, string ifMaxPalierTxtn, int gainResearchInt, string maxReachTxt, string chanceTxt, string attaqueTxt, int fromBallInt, int toBallInt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt, List<string> sauteuseValue) 
        : base(gainResearchInt, maxReachTxt, chanceTxt,  attaqueTxt, fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt,permanenceSelectedTxt, sauteuseValue)
        {
            nbPalierInt=nbPalierIntn;
            timePalierInt=timePalierIntn;
            ifMaxPalierTxt=ifMaxPalierTxtn;
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

            bool lauchGame = true;
            var playerMise = "";
            

            string[,] fictive = null;

                for (int i = fromBallInt-1; i < toBallInt; i++)
                {
                    if (attaqueTxt.StartsWith("différentielle")){
                        (fictive, value) = calculateFictive(chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain);
                        setFictiveLine(fictive);
                    }

                    if (fictive==null){
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
                        // mise -=1;
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

                    addResult(i,coup, value, mise,coinValueInt,bilanGame,bilanTotal, playerMise, attaqueTxt,fictive);
                    // setUpStat(bilanTotal,bilanGame,mise,coinValueInt);

                    if (win && bilanGame>0){
                        gain = 0;
                        mise = miseInitial;

                        if (fictive!=null && attaqueTxt == "différentielle directe"){
                            bilanGame = 0;
                        }else if(attaqueTxt == "différentielle compensée"){
                            coup = 0;
                            bilanGame = 0;
                            fictive = new string[0,0];
                        }else if(!attaqueTxt.StartsWith("différentielle")){
                            coup = 0;
                            bilanGame = 0;
                        }
                    }else if (win) {
                        mise -= 1;
                    }else{
                        mise += 1;
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
            // }
            // if (fileNameTxt!=""){
            // saveResult(fileNameTxt);

            // }
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
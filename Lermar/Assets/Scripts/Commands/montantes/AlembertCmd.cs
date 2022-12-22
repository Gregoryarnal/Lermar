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
        // public int nbPalierInt;
        public int startToInt;
        public string varianteTxt;
        // public string ifMaxPalierTxt;
        // public string typeOfGainTxt;


        

        public AlembertCmd(string m_Path, string fictiveMaxReachTxtn, string varianteTxtn, string typeOfGain, int nbPalierIntn, int timePalierIntn, string ifMaxPalierTxtn, int gainResearchInt, string maxReachTxt, string chanceTxt, string attaqueTxt, int fromBallInt, int toBallInt, string fileNameTxt, int coinValueInt, int maxMiseInt,string permanenceSelectedTxt, List<string> sauteuseValue, bool security,int securityValue,string typeOfMise) 
        : base(m_Path, typeOfGain, fictiveMaxReachTxtn,gainResearchInt, maxReachTxt, chanceTxt,  attaqueTxt, fromBallInt, toBallInt, fileNameTxt, coinValueInt, maxMiseInt,permanenceSelectedTxt, sauteuseValue,security,securityValue, typeOfMise,timePalierIntn, ifMaxPalierTxtn, nbPalierIntn)
        {
            // typeOfGainTxt=typeOfGain;
            // nbPalierInt=nbPalierIntns


            varianteTxt=varianteTxtn;
            // startToInt = startToIntn;
            // ifMaxPalierTxt=ifMaxPalierTxtn;
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

            // bool lauchGame = true;
            var playerMise = "";

            var firstWin = false;
            var firstWin1 = false;
            var firstWin2 = false;

            var cptWin = 0;
            var cptWin1 = 0;
            var cptWin2 = 0;
            
                        int cptValuePlay = 0;
            int cptValuePlay1 =0;
            int cptValuePlay2 = 0;

            string[,] fictive = null;

                for (int i = fromBallInt-1; i < toBallInt; i++)
                {
                    if (attaqueTxt.StartsWith("différentielle")){
                        (fictive, value) = calculateFictive(this, cptValuePlay1,cptValuePlay2,chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false, "D'Alembert", fictive);
                        cptValuePlay1 = Int32.Parse(fictive[0,5]);
                        cptValuePlay2 = Int32.Parse(fictive[1,5]);
                        setFictiveLine(fictive);
                    }

                    if (fictive==null){
                        ( playerMise,value,mise,win,cptValuePlay ) =  play(cptValuePlay,chanceTxt, attaqueTxt,value, win, i, permanenceSelectedTxt,coup, mise,  timePalierInt, nbPalierInt, coinValueInt, maxMiseInt, ifMaxPalierTxt, maxReachTxt, gain, false, false);
                    }else{
                        if (Int32.Parse(fictive[0,0])==Int32.Parse(fictive[1,0])){
                            mise = 0;
                            playerMise = null;
                            win = false;
                        }else{
                            (mise,playerMise, win,cptValuePlay,cptValuePlay1,cptValuePlay2, value ) = realGameWithFictive(fictive, mise, playerMise, win, cptValuePlay, cptValuePlay1, cptValuePlay2, bilanGame, coup, value, i, gain);
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

                    if (fictive!=null){

                                             var typeOfMise1 = typeOfMise;
                        var typeOfMise2 = typeOfMise;
                        if (typeOfMise=="En perte et en gain"){
                            typeOfMise1 = "En perte";
                            typeOfMise2 = "En gain";
                        }

                        (var nmise1,var ncptWin1,var nfirstWin1) =  calculeMise(Convert.ToBoolean(fictive[0,4]), typeOfMise, Int32.Parse(fictive[0,0]) , cptWin1, firstWin1, Int32.Parse(fictive[0,3]));
                        cptWin1 = ncptWin1;
                        firstWin1 = nfirstWin1;
                        mise1 = calculMisePerte(Int32.Parse(fictive[0,0]), Convert.ToBoolean(fictive[0,4]));

                        fictive[0,0] = (mise1).ToString();


                        (var nmise2,var ncptWin2,var nfirstWin2) =  calculeMise(Convert.ToBoolean(fictive[1,4]), typeOfMise, Int32.Parse(fictive[1,0]) , cptWin2, firstWin2, Int32.Parse(fictive[1,3]));

                        cptWin2 = ncptWin2;
                        firstWin2 = nfirstWin2;
                        mise2 = calculMisePerte(Int32.Parse(fictive[1,0]), Convert.ToBoolean(fictive[1,4]));

                        fictive[1,0] = (mise2).ToString();

                        (fictive, firstWin1,  firstWin2,  cptWin1,  cptWin2) = fictiveReset(fictive,  firstWin1,  firstWin2,  cptWin1,  cptWin2);
                        
                    }


                    (var nmise,var ncptWin,var nfirstWin) =  calculeMise(win, typeOfMise, mise, cptWin, firstWin, coup);
                    
                    cptWin = ncptWin;
                    firstWin = nfirstWin;
                    mise = calculMisePerte(mise, win);


                    (bilanGame, firstWin, gain, mise, cptWin, cptWin1, cptWin2, coup, fictive) = gameReset(win, bilanGame, firstWin, gain, mise, cptWin, cptWin1, cptWin2, coup, fictive );
                   
                    
                   if (playerMise==null){
                        if(!attaqueTxt.StartsWith("différentielle")){        
                            coup = 0;
                        }
                    }

                    if (security){
                         (mise, cptValuePlay)  = calculateSecurity( cptValuePlay,mise,bilanGame, coup);
                    }
                    index+=1;
                }
        }

       int calculMisePerte(int misen, bool winn){
            if (typeOfMise=="En perte"){
                if (winn) {
                    if (misen > 1){
                        if (varianteTxt=="Augmenter"){
                            misen -= 1;
                        }
                    }
                }else{
                    // if (varianteTxt=="Augmenter"){
                        misen += 1;
                    // }
                }

            }else if(typeOfMise=="En gain"){
                if (winn) {
                    // if (misen > 1){
                    //     if (varianteTxt=="Augmenter"){
                            misen += 1;
                        // }
                    // }
                }else{
                    if (misen > 1){
                        // Debug.Log("gain loose : " + misen);
                    // if (varianteTxt=="Augmenter"){
                        misen -= 1;
                        // Debug.Log("gain loose after : " + misen);

                    }
                }
            }
            
            return misen;
       }
    }
}
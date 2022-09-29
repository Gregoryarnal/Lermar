// using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ViewModel;
using Controllers;
using Infrastructure;
using System;

namespace Commands
{    
    public class PlayTurnCmd : ICommand
    {
        private readonly MonoBehaviour monoBehaviour;
        private CharacterTable characterTable;
        private CharacterTools characterTools;
        private GameRoullete gameRoullete;
        private IRound roundGateway;
        private IPayment paymentGateway;
        private string permanencePath;
        // private bool readPermanence;
        // private bool firstRun = true;

        public PlayTurnCmd(MonoBehaviour monoBehaviour, CharacterTable characterTable,CharacterTools characterTools, GameRoullete gameRoullete, IRound roundGateway, IPayment paymentGateway)
        {
            this.monoBehaviour = monoBehaviour;
            this.characterTable = characterTable;
            this.characterTools = characterTools;
            this.gameRoullete = gameRoullete;
            this.roundGateway = roundGateway;
            this.paymentGateway = paymentGateway;
            // this.readPermanence = true;
        }

        public void Execute()
        {
            int cpt = 0;
            if(characterTable.currentTableCount <= 0){
                return;
            }
            
            Debug.Log($"The game roullete is executing in {characterTable.tableName} with {characterTable.currentTableCount} chips in table!");
            PlayerSound.Instance.gameSound.OnSound.OnNext(PlayerSound.Instance.gameSound.audioReferences[7]);

            // permanencePath = "Lermar/permanences/MC/MC01.TXT";

            roundGateway.PlayTurn()
                .Do(_ => monoBehaviour.StartCoroutine(RoulleteGame(roundGateway.randomNumber)))
                .Do(_ => characterTable.lastNumber = roundGateway.randomNumber)
                .Subscribe();         
        }

        private int readNextPermanenceValue(int cpt){
            permanencePath = "/Users/gregoryarnal/dev/FreeLance/Lermar/Lermar/permanences/MC/" + characterTable.permFilePath;
            string[] lines = System.IO.File.ReadAllLines(permanencePath);
            
            return Int32.Parse(lines[cpt]); 
        }

        IEnumerator RoulleteGame(int num)
        {
            if (characterTable.readPermanence){
                num =  readNextPermanenceValue(characterTable.lastIndex );
                roundGateway.randomNumber = num;
                Debug.Log("characterTable.lastIndex : " + characterTable.lastIndex);
            }

            characterTable.OnRound.OnNext(true); // Initialize round
            characterTable.currentTableActive.Value = false; // Desactivete table buttons
            gameRoullete.OnRotate.OnNext(true);

            yield return new WaitForSeconds(.2f);
            gameRoullete.currentSpeed = 75f;
            yield return new WaitForSeconds(.1f);
            gameRoullete.currentSpeed = 145f;
            PlayerSound.Instance.gameSound.OnSound.OnNext(PlayerSound.Instance.gameSound.audioReferences[9]);
            yield return new WaitForSeconds(0.05f);
            gameRoullete.currentSpeed = 240f;
            yield return new WaitForSeconds(.12f);
            gameRoullete.currentSpeed = 245f;
            yield return new WaitForSeconds(.2f);
            gameRoullete.currentSpeed = 265;
            yield return new WaitForSeconds(.38f);
            gameRoullete.currentSpeed = 245;
            yield return new WaitForSeconds(.15f);
            gameRoullete.currentSpeed = 240f;
            yield return new WaitForSeconds(.15f);
            // Ball position
            gameRoullete.currentSpeed = 145;
            gameRoullete.OnNumber.OnNext(num);

            yield return new WaitForSeconds(.18f);
            gameRoullete.currentSpeed = 75f;
   
            yield return new WaitForSeconds(.5f);
            // Finish round
            gameRoullete.currentSpeed = gameRoullete.defaultSpeed;
            characterTable.OnRound.OnNext(false); 


            
            // Intialize the payment system and display the news values
            paymentGateway.PaymentSystem(characterTable)
                .Delay(TimeSpan.FromSeconds(.3))
                .Do(_ => characterTable.bilanGame += paymentGateway.PaymentValue)
                .Do(_ => characterTools.AddStatistics(characterTable.lastIndex+1, roundGateway.randomNumber, characterTable.characterMoney.characterBet.Value, paymentGateway.PaymentValue,characterTable.bilanGame))
                .Do(_ => OnPayment(paymentGateway.PaymentValue))
                .Do(_ => characterTable.OnWinButton.OnNext(num))
                .Do(_ => characterTable.lastIndex = characterTable.lastIndex+1)
                .Subscribe();

        }

        public void OnPayment(int value)
        {
            characterTable.characterMoney.currentPayment.Value = value;
            characterTable.characterMoney.PaymentSystem(value);
        }
    }
}

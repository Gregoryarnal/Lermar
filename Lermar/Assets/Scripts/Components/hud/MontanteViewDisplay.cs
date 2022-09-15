
// using System.Diagnostics;
using System.Transactions;
using System.Data.Common;
// using System.Diagnostics;
// using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRx;
using ViewModel;
using System;
using Controllers;

namespace Components
{
    public class MontanteViewDisplay : MonoBehaviour
    {

        public GameObject fromBall;
        public GameObject toBall;
        public Dropdown showEach;
        public Dropdown stopBetweenEach;
        public GameObject delaiBetweenBall;
        public GameObject fileName;
        public GameObject coinValue;
        public GameObject maxMise;
        public Dropdown changeGame;
        public Dropdown Attaque;
        public GameObject gainResearch;

        public Button ExecuteButton;

        public GameObject permanenceSelected;
        public GameObject montanteSelected;
        // public GameObject toolNameTopPopUp;
        // public GameObject permanenceNameTop;
        public void Start()
        {
            ExecuteButton.onClick.AddListener(() => run());
            // permanenceSelected.onClick.AddListener(() => getPermanence());
        }

        void getPermanence(){
            Debug.Log(permanenceSelected.GetComponent<Text>().text);
        }

        void run(){
            var fromBallTxt = fromBall.GetComponent<InputField>().text;
            var toBallTxt = toBall.GetComponent<InputField>().text;
            var showEachTxt = showEach.options[showEach.value].text;
            var stopBetweenEachTxt = stopBetweenEach.options[stopBetweenEach.value].text;
            var delaiBetweenBallTxt = delaiBetweenBall.GetComponent<InputField>().text;
            var fileNameTxt = fileName.GetComponent<InputField>().text;
            var coinValueTxt = coinValue.GetComponent<InputField>().text;
            var maxMiseTxt = maxMise.GetComponent<InputField>().text;
            var changeGameTxt = changeGame.options[changeGame.value].text;
            var AttaqueTxt = Attaque.options[Attaque.value].text;
            var gainResearchTxt = gainResearch.GetComponent<InputField>().text;
            var permanenceSelectedTxt = permanenceSelected.GetComponent<Text>().text;
            var montanteSelectedTxt = montanteSelected.GetComponent<Text>().text;

            
        }
    }
       
}

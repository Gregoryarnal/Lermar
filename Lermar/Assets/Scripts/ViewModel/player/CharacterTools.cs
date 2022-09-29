// using System.Diagnostics;
using System;
using System.Collections.ObjectModel;
// using System.Diagnostics;
// using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Controllers;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New Character Tools", menuName = "Scriptable/Character Tools")]
    public class CharacterTools : ScriptableObject
    {
        public StringReactiveProperty characterToolsView = new StringReactiveProperty();
        public StringReactiveProperty characterStatisticsView = new StringReactiveProperty();
        public bool firstRun = true;

        public void ResetView(){
            characterToolsView.Value = "";
            characterStatisticsView.Value = "";
        }


        public void AddPermanence(string permanence)
        {
            string aux = characterToolsView.Value;
            var type = "permanence";

            characterToolsView.Value = "\n" + permanence + "//" + type;
        }



        public void AddMethodes(string methode){
            var type = "methode";
            string aux = characterToolsView.Value;

            characterToolsView.Value = "\n" + methode + "//" + type;
        }

        public void AddMontantes(string montante){
            var type = "montante";
            string aux = characterToolsView.Value;

            characterToolsView.Value = "\n" + montante + "//" + type;
        }


        public void APalier(){
            // APalierCmd palier = new APalierCmd();
        }
        

         public void AddStatistics(int lastIndex, int value, int mise, int result, int bilan){

            // if (!firstRun){
                string aux = characterStatisticsView.Value;
                
                characterStatisticsView.Value = lastIndex.ToString() + "//" + value.ToString() 
                        + "//"+ mise.ToString() + "//"+ result.ToString()+ "//"+  bilan.ToString();
            // }
        }


    }
    
}

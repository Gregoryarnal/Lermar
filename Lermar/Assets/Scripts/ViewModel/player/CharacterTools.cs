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
        // public List<Button> button_list = new List<Button>();
        

        public void ResetView(){
            characterToolsView.Value = "";
        }


        public void AddPermanence(string permanence)
        {
            // addButton(permanence);
            Debug.Log("AddPermanence : " +permanence);

            // string aux = characterToolsView.Value;
            characterToolsView.Value = "\n" + permanence;
        }


        private void LoadPermanenceFile(){

        }
    }
    
}

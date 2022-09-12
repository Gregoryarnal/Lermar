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
            var type = "permanence";
            characterToolsView.Value = "\n" + permanence + "//" + type;
        }



        public void AddMethodes(string methode){
            var type = "methode";

            characterToolsView.Value = "\n" + methode + "//" + type;
            // characterToolsView.Value = "\n" + permanence;
        }
    }
    
}

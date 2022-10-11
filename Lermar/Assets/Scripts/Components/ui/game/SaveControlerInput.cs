using System.Collections;
using System.Collections.Generic;
using Commands;
using Components;
using UnityEngine;
using ViewModel;
using SFB;

namespace Components
{
    public class SaveControlerInput : MonoBehaviour
    {
        // public CharacterTable characterTable;
        public CharacterTools characterTools;
        // public GameCmdFactory gameCmdFactory;
        
        public void OnClick() 
        {
            var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "");
            characterTools.AddSavePath(path); 
        }
    }
}

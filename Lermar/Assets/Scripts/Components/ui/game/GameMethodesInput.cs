using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Commands;
using UnityEngine;
using ViewModel;

namespace Components
{

    public class GameMethodesInput : MonoBehaviour
    {
        public CharacterTable characterTable;
        public CharacterTools characterTools;
        public GameCmdFactory gameCmdFactory;

        string[] methodeName = new string[] { "Acoussur", "Osmose NAS", "Colonnes et douzaines", "Thaïlandaises", "Express 20/24", "Ad Libitum" };

        public void OnClick()
        {
            characterTools.ResetView();
            foreach (string item in methodeName)
            {
                characterTools.AddMethodes(item); 
            }
        }
    }
}

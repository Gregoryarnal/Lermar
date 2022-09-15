using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Commands;
using UnityEngine;
using ViewModel;

namespace Components
{

    public class GameMontanteInput : MonoBehaviour
    {
        public CharacterTable characterTable;
        public CharacterTools characterTools;
        // public GameRoullete gameRoullete;
        public GameCmdFactory gameCmdFactory;
        // string[] methodeName = ["aaaa", "sqdcsqdcqs", "pppp"];
        string[] montanteName = new string[] { "NAS", "D'Alembert", "Contre d'Alembert", "Hollandaise", "Americaine", "Piquemouche", "Wells", "Contre Wells", "MIDAS", "A paliers", "Pascal" };

        
        public void OnClick()
        {
            characterTools.ResetView();
            foreach (string item in montanteName)
            {
                characterTools.AddMontantes(item); 
            }
        }

        // public void OnClickTest()
        // {
        //     characterTools.ResetView();
        //     foreach (string item in methodeName)
        //     {
        //         characterTools.AddMethodes(item); 
        //     }
        // }


    }
}

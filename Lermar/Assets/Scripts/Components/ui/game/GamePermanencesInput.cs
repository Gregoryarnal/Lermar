using System.Collections;
using System.Collections.Generic;
using Commands;
using UnityEngine;
using ViewModel;

namespace Components
{
    public class GamePermanencesInput : MonoBehaviour
    {
        public CharacterTable characterTable;
        public CharacterTools characterTools;

        public GameCmdFactory gameCmdFactory;
        
        public void OnClick()
        {
            gameCmdFactory.PermanencesLoad(characterTable,characterTools).Execute(true);
        }
    }
}

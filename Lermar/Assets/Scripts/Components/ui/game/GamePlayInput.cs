using System.Collections;
using System.Collections.Generic;
using Commands;
using UnityEngine;
using ViewModel;

namespace Components
{
    public class GamePlayInput : MonoBehaviour
    {
        public CharacterTable characterTable;
        public GameRoullete gameRoullete;
        public GameCmdFactory gameCmdFactory;
        public CharacterTools characterTools;
        
        public void OnClick()
        {
            gameCmdFactory.PlayTurn(characterTable, gameRoullete, characterTools).Execute();
        }
    }
}

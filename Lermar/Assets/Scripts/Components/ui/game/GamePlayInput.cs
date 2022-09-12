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
        // private int cpt = 0;

        
        public void OnClick()
        {
            gameCmdFactory.PlayTurn(characterTable, gameRoullete).Execute();
            // cpt += 1;
        }
    }
}

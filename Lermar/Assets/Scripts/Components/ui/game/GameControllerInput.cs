using System.Collections;
using System.Collections.Generic;
using Commands;
using UnityEngine;
using ViewModel;

namespace Components
{
    public class GameControllerInput : MonoBehaviour
    {
        
        public void OnClick()
        {
             Application.Quit();
        }
    }
}

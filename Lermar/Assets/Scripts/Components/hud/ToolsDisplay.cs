using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRx;
using ViewModel;
using System;
using Controllers;

namespace Components
{
    public class ToolsDisplay : MonoBehaviour
    {

        // public CharacterTable characterTable;
        public CharacterTools characterTools;
        public Text textcontent;
        // public GameObject prefabButton;
        public GameObject content;
        public List<Button> button_list = new List<Button>();
        // public ScrollView scroll;
        // public Button button;


        public void Start()
        {
            try
            {
                characterTools.characterToolsView
                    .Subscribe(OnLoadTools)
                    .AddTo(this);

            }
            catch (System.Exception)
            {
                 Debug.Log("Donothing : ");
            } 
        }

        public Button addButton(string permName)
        {
            
            GameObject newButton = new GameObject();
            newButton.name = permName; //Optional
            newButton.transform.localScale = new Vector3(200, 20, 0);
            newButton.transform.position = new Vector2(65, 104);
            newButton.transform.SetParent(content.transform, false);
            // newButton.content = Resources.Load<Image>("PermanencesButton");

            //   myButtonImage = Resources.Load<Image>("switchCameraImg");
            Button tempButton = newButton.AddComponent<Button>();
            Text txtButton = newButton.AddComponent<Text>();
            // txtButton.Value = "iiii";
            tempButton.onClick.AddListener(() => ButtonClicked("permanence"));
            // tempButton
            return tempButton;
            // button_list.Add(newButton);

            // button_list.Add(new GameObject ("ButtonName",typeof(Button)));
            // $$

            // Debug.Log("add  button");

            // // GameObject permButton = Instantiate(prefabButton) as GameObject;

            // permButton.transform.SetParent(content.transform, false);
            // permButton.transform.localScale = new Vector3(100f, 100f, 100f);
        
            // Button tempButton = permButton.GetComponent<Button>();
            // Debug.Log("On click");
            // tempButton.onClick.AddListener(() => ButtonClicked("permanence"));
        }

    
        void ButtonClicked(string permanence)
        {
            Debug.Log ("Button clicked = " + permanence);
        }


        public void OnLoadTools(String value)
        {
            // public Button button;
            
            if (value == ""){
                textcontent.text = "";
            }else{
                textcontent.text += value.ToString();
                button_list.Add(addButton(value));
                // addButton(value);
            }

            
        }
    }
}

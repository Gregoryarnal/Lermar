// // using System.Diagnostics;
// // using System.Diagnostics;
// using System.IO;
// using System.Text;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.UI;
// using UnityEngine;
// using UniRx;
// using ViewModel;
// using System;
// using Components;
// using Commands;
// using UnityEditor;
// using UnityEngine;
// // using UnityEngine.UIElements;

// using SFB;

// namespace windows
// {    
//     // public class EditorWindows : EditorWindow
//     public class EditorWindows : MonoBehaviour
//     {
//         GameCmdFactory gameCmdFactory;
//         CharacterTable characterTable;
//         CharacterTools characterTools;


//         public EditorWindows(GameCmdFactory gameCmdFactoryn, CharacterTable characterTablen, CharacterTools characterToolsn) 
//         {
//             gameCmdFactory=gameCmdFactoryn;
//             characterTable=characterTablen;
//             characterTools=characterToolsn;

//             // var path =  EditorUtility.OpenFilePanel("Open File", "", "txt");

//             var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);



//             if (path.Length != 0)
//             {
//                 copiePermanence(path[0]);
//             }
//         }

//        void copiePermanence(string source){
//             var filename = Path.GetFileName(source);
//             var destination = Path.GetDirectoryName(Application.dataPath) +"/permanences/MC/" + filename;
//             File.Copy(source, destination, true);
//             gameCmdFactory.PermanencesLoad(characterTable,characterTools).Execute();
//        }
//     }
// }
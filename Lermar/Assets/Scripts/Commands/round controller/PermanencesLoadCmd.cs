using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ViewModel;
using Controllers;
using Infrastructure;
using System;
using System.IO;

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
using UnityEngine.UI;
namespace Commands
{    
    public class PermanencesLoadCmd : ICommand
    {
        private readonly MonoBehaviour monoBehaviour;
        private CharacterTable characterTable;
        private CharacterTools characterTools;
        private IRound roundGateway;
        private IPayment paymentGateway;
         public Text tools;

        // public string path = "permanences/MC";
        public string path = Path.GetDirectoryName(Application.dataPath) +"/permanences/MC";

        public PermanencesLoadCmd( CharacterTable characterTable, CharacterTools characterTools)
        {
            Debug.Log(path);

            // Debug.Log($"PermanencesLoadCmd : " + characterTable);
            this.characterTable = characterTable;
            this.characterTools = characterTools;
            // Debug.Log($"characterTable : " + characterTable);
            // Debug.Log($"characterTable.characterTools : " + characterTools);
        }


        public static string[] TraverseTree(string root)
        {
            // Debug.Log("root path : " + root); 
            Stack<string> dirs = new Stack<string>(20);
            string[] files = null;
            // string[] foundFiles = null;
            List<string> foundFiles = new List<string>();
            if (!System.IO.Directory.Exists(root))
            {
                throw new ArgumentException();
            }
            dirs.Push(root);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs;
                try
                {
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                
                try
                {
                    files = System.IO.Directory.GetFiles(currentDir);
                }

                catch (UnauthorizedAccessException e)
                {

                    Console.WriteLine(e.Message);
                    continue;
                }

                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                foreach (string file in files)
                {
                    try
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        // Debug.Log($"file : " + fi.Name);
                        foundFiles.Add(fi.Name);
                    }
                    catch (System.IO.FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }
                foreach (string str in subDirs)
                    dirs.Push(str);

                return foundFiles.ToArray();
            }
            return foundFiles.ToArray();
        }
        
        public void Execute()
        {}

        public void Execute(bool show, bool resetView)
        {
            
            if (resetView){

            characterTools.ResetView();
            }
		    
            if (show)
            {
                characterTools.AddPermanenceBtn("Ajouter");
            }  
            
            string[] filenames = TraverseTree(path);

            foreach (string el in filenames){
                if (!el.StartsWith("."))
                    if(show){
                        characterTools.AddPermanence(el);
                    }else{
                        characterTools.LoadPermanence(el);
                    }
            }

                      

        }

        
    }
}

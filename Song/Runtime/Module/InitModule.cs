using System;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Song.Runtime.Module
{
    public class InitModule : MonoBehaviour
    {
        public string sceneName;
        
        public void Start()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}

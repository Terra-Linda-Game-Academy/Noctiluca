using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ChessCompressor : MonoBehaviour
{

    public string filename = "C:\\Users\\tm02246\\Documents\\GitHub\\Noctiluca\\Assets\\Scripts\\UI\\PauseMenu\\Programs\\Chess\\resources\\chess\\pieces\\wb.png";
    
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath + "/chesscompressed.txt";
        string content = Convert.ToBase64String(File.ReadAllBytes(filename));
        File.WriteAllText(path, content);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

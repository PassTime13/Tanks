using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HighScores : MonoBehaviour
{
    public int[] scores = new int[10];

    string currentDirectory;

    public string scoreFileName = "highscores.txt";


    void Start()
    {
        currentDirectory = Application.dataPath;
        Debug.Log("Our current directory is: " + currentDirectory);

        LoadScoresFromFile();

    }

    void Update()
    {
        

    }

    public void LoadScoresFromFile() 
    {
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);
        if (fileExists == true)
        {
            Debug.Log("Found highscore file " + scoreFileName);

        }

        else 
        {
            Debug.Log("The file " + scoreFileName + " does not exist. No scores will be loaded.", this);
            return;
            
        }

        //new array with preset values, this ensures no previous scores are kept
        scores = new int[scores.Length];

        StreamReader fileReader = new StreamReader(currentDirectory + "\\" + scoreFileName);

        //counter to make sure we dont go past the end of our scores
        int scoreCount = 0;

        while (fileReader.Peek() != 0 && scoreCount < scores.Length)
        {
            string fileLine = fileReader.ReadLine();

            int readScore = -1;
            
            bool didParse = int.TryParse(fileLine, out readScore);

            if (didParse)
            {
                scores[scoreCount] = readScore;

            }

            else
            {
                Debug.Log("Invalid line in scores file at " + scoreCount + ", using default valuse.", this);
                scores[scoreCount] = 0;
            
            }
            //increase by one what line you are checking
            scoreCount++;

        }
        //close the stream
        fileReader.Close();
        Debug.Log("High scores read from " + scoreFileName);

    }

    public void SaveScoresToFile()
    {
        //create a steamwriter for our file path
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\" + scoreFileName);

        //writes the lines into the file
        for (int i = 0; i < scores.Length; i++)
        {
            fileWriter.WriteLine(scores[i]);
            
        }
        //close the stream
        fileWriter.Close();

        //Write a log message
        Debug.Log("Highscores written to " + scoreFileName);
        
    }

    public void AddScore(int newScore)
    {
        int desiredIndex = -1;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] > newScore || scores[1] == 0)
            {
                desiredIndex = i;
                break;
                
            }
            
        }
        //if no desired index was found then score wasnt high enough to get on the table
        if (desiredIndex < 0)
        {
            Debug.Log("Score of " + newScore + " not high enough for highscores list.", this);
            return;
            
        }

        //move all scores after that index back by one position through a loop
        for (int i = scores.Length - 1; i > desiredIndex; i--)
        {
            scores[i] = scores[i - 1];
            
        }
        //insert new score in its place
        scores[desiredIndex] = newScore;
        Debug.Log("Score of " + newScore + " entered into highscores at position " + desiredIndex, this);
        
    }

}

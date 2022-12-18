using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameSession : MonoBehaviour
{
    //Add this script to a game object in the hierarchy called GameSession
    [SerializeField] int playerLives = 3;
    void Awake()
    {
        //Creating an array that looks for the Length(how many) of GameSession objects
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        //Will destroy GameSession if more than 1 GameSession gameObject exist
        if(numberGameSessions >1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        } 
    }

   //This will handle player lives and resetting the game sessions.
   public void PlayerDeath()
   {
        if(playerLives > 1)
        {
            MinusLife();
        }
        else
        {
            ResetGameSession();
        }
   }

   void MinusLife()
   {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
   }
   void ResetGameSession()
   {
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(0);
   }
}

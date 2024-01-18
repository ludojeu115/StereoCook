using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void ScoreEvent(float score);
    public event ScoreEvent OnScore;
    
    public static float roomTemperature = 20f;
    //game scene path
    public static string gameScenePath = "Scenes/Game";
    //end game scene path
    public static string endGameScenePath = "Scenes/Retry Menu";
    
    public int burgerSent = 0;
    public List<int> currentRecipe;
    
    private bool started = false;
    private float score = 0f;
    
    public float GetRemainingTime()
    {
        return timeLeft;
    }
    public float Score
    {
        get => score;
        set
        {
            score = value;
            if (score < 0)
            {
                score = 0;
            }
        }
    }

    public float timeLimit;
    private float timeLeft = 0f;
    
    private static GameManager instance = null;
    public static GameManager Instance => instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
		
        timeLimit = 300f;
        
    }

    public void SendBurger(List<GameObject> burger)
    {
        if (burger.Count == 0)
        {
            return;
        }
        List<int> recipeInt = RecipeManager.GetRecipe(burger);
        
        
        /*
        List<string> recipeString = RecipeManager.recipeToString(recipeInt);
        Debug.Log("Recipe Given: ");
        foreach (string s in recipeString)
        {
            Debug.Log(s);
        }
        */
        
        Debug.Log("Score: ");
        float bscore = RecipeManager.compareRecipes(burger, currentRecipe);
        Debug.Log(bscore);
        Score += bscore;
        burgerSent++;
        OnScore?.Invoke(Score);
        currentRecipe = RecipeManager.generateRecipe(3+burgerSent);
        
    }
    public void StartGame()
    {
        timeLeft = timeLimit;
        score = 0f;
        burgerSent = 0;
        currentRecipe = RecipeManager.generateRecipe(3+burgerSent);
        Debug.Log("Recipe: ");
        foreach (int i in currentRecipe)
        {
            Debug.Log(i);
        }
        started = true;
    }
    
    private void EndGame()
    {
        started = false;
        //load end game scene
        
        SceneManager.LoadScene(endGameScenePath);
    }
    private void Update()
    {
        if (started)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                EndGame();
            }
        }
    }
}


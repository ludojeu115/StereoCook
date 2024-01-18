using System;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{

    public enum FoodType
    {
        Undefined,
        Bun,
        Tomato,
        Lettuce,
        Onion,
        Cheese,
        Meat
    }
    private static RecipeManager instance = null;
    public static RecipeManager Instance => instance;
    
    //new score event
    

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
            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        }
        DontDestroyOnLoad(this.gameObject);
		
    }
    
    public static FoodType GetFoodType(GameObject food)
    {
        Food component = food.GetComponent<Food>();
        if (component == null)
        {
            Debug.LogError("Food has no Food component");
            return FoodType.Undefined;
        }
        string foodName = component.GetName();

        switch (foodName)
        {
            case "Bun Bottom":
            case "Bun Top":
                return FoodType.Bun;
            case "Tomato":
            case "Tomato Slice":
                return FoodType.Tomato;
            case "Lettuce":
            case "Lettuce Slice":
                return FoodType.Lettuce;
            case "Onion": 
            case "Onion Rings":
                return FoodType.Onion;
            case "Cheese Full":
            case "Cheese Slice":
                return FoodType.Cheese;
            case "Meat":
                return FoodType.Meat;
            
        }
        
        
        return FoodType.Undefined;
    }
    public static List<int> generateRecipe(int length=3)
    {
        if (length < 3)
        {
            Debug.LogError("Recipe length must be at least 3");
            length = 3;
        }
        List<int> recipe = new List<int>();
        recipe.Add(1);
        for (int i = 0; i < length-2; i++)
        {
            recipe.Add(UnityEngine.Random.Range(1, 7));
        }
        recipe.Add(1);
        return recipe;
    }
    public static List<int> GetRecipe(List<GameObject> food)
    {
        List<int> recipe = new List<int>();
        foreach (GameObject f in food)
        {
            recipe.Add((int) GetFoodType(f));
        }

        return recipe;
    }
    //recipe 1 is the recipe the player has made, recipe 2 is the recipe the customer wants
    public static float compareRecipes(List<GameObject> recipe1, List<int> recipe2)
    {//count the matching ingredients
        float score = 0; 
        List<int> recipe1i = GetRecipe(recipe1);
        List<int> recipe2Copy = new List<int>(recipe2);
        int matchingIngredients = 0;
        foreach (int ingredient in recipe1i)
        {
            if (recipe2Copy.Contains(ingredient))
            {
                recipe2Copy.Remove(ingredient);
                matchingIngredients++;
            }
        }
        
        score += (float)matchingIngredients/recipe2.Count; 
        
        //check order of ingredients
        matchingIngredients = 0;
        for (int i = 0; i < recipe1i.Count; i++)
        {
            if (i >= recipe2.Count)
            {
                break;
            }
            if (recipe1i[i] == recipe2[i])
            {
                matchingIngredients++;
            }
        }
        score += (float)matchingIngredients/recipe2.Count;
        //Debug.Log("Score Before cooked: " + score);
        int canBeCooked = 0;
        float cookedScore = 0;
        foreach( GameObject ing in recipe1)
        {
            if (ing.GetComponent<Food>().canBeCooked)
            {
                canBeCooked++;
                if (ing.GetComponent<Food>().IsCooked())
                {
                    cookedScore++;
                }
            }
        } 
        if (canBeCooked > 0)
        {
            score += cookedScore/canBeCooked;
            score /= 3;
        }
        else
        {
            score /= 2;
        }
        
        //Debug.Log("Score normalised: " + score);
        
        //return the percentage of matching ingredients
        return score*recipe2.Count;
    }
    public static List<string> recipeToString(List<int> recipe)
    {
        List<string> recipeString = new List<string>();
        foreach (int ingredient in recipe)
        {
            switch (ingredient)
            {
                case 1:
                    recipeString.Add("Bun");
                    break;
                case 2:
                    recipeString.Add("Tomato");
                    break;
                case 3:
                    recipeString.Add("Lettuce");
                    break;
                case 4:
                    recipeString.Add("Onion");
                    break;
                case 5:
                    recipeString.Add("Cheese");
                    break;
                case 6:
                    recipeString.Add("Meat");
                    break;
            }
        }

        return recipeString;
    }
    
}

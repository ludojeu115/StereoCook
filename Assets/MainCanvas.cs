using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MainCanvas : MonoBehaviour
{
    private Canvas m_Canvas;
    private TextMeshProUGUI remainingTime;
    private TextMeshProUGUI toDo;

    private void Start()
    {
        m_Canvas = GetComponent<Canvas>();
        remainingTime = transform.GetChild(0).Find("Remaining").gameObject.GetComponent<TextMeshProUGUI>();
        toDo = transform.GetChild(0).Find("Current").gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        remainingTime.text = GameManager.Instance.GetRemainingTime().ToString("0.00")+ "s";
        var recipe = RecipeManager.recipeToString(GameManager.Instance.currentRecipe);
        string todo = recipe.Aggregate("", (current, s) => current + (s + "\n"));
        toDo.text = todo;
    }
}

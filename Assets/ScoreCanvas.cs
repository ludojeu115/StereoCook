using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCanvas : MonoBehaviour
{
    private Canvas m_Canvas;
    [SerializeField] private Texture starTexture;
    [SerializeField] private Texture starTextureEmpty;
    private GameObject star1;
    private GameObject star2;
    private GameObject star3;
    private GameObject scorevalue;
    private GameObject scoretotal;
    
    private float showFor = 5f;
    private float timeLeft = 0f;
    private float score = 0f;
    

    
    private void Update()
    {
        if (m_Canvas == null) return;
        if (m_Canvas.enabled)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                m_Canvas.enabled = false;
            }
        }
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y+180, 0f);
        
    }
    
    private void OnScore(float score)
    {
        if(m_Canvas ==null) return; 
        m_Canvas.enabled = true;
        timeLeft = showFor;
        this.score = score;
        scorevalue.GetComponent<TextMeshProUGUI>().text = ((int)score).ToString();
        float scorePercent = score / Math.Max(GameManager.Instance.burgerSent+2,1);
        Debug.Log(scorePercent);
        star1.GetComponent<RawImage>().texture = scorePercent >= 0.3333f ? starTexture : starTextureEmpty;
        star2.GetComponent<RawImage>().texture = scorePercent >= 0.6666f ? starTexture : starTextureEmpty;
        star3.GetComponent<RawImage>().texture = scorePercent >= 0.9999f ? starTexture : starTextureEmpty;
        
        scoretotal.GetComponent<TextMeshProUGUI>().text = ((int)GameManager.Instance.Score).ToString();
        
    }
    private void Start()
    {
        m_Canvas = GetComponent<Canvas>();
        GameManager.Instance.OnScore += OnScore;
        m_Canvas.enabled = false;
        star1 = transform.GetChild(0).Find("Star1").gameObject;
        star2 = transform.GetChild(0).Find("Star2").gameObject;
        star3 = transform.GetChild(0).Find("Star3").gameObject;
        scorevalue = transform.GetChild(0).Find("NewScore").gameObject;
        scoretotal = transform.GetChild(0).Find("TotalScore").gameObject;
    }

}

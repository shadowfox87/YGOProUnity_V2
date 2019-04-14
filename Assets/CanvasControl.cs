using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasControl : MonoBehaviour {
    public static Canvas canvas;
    private static CanvasGroup canvasGroup;
	// Use this for initialization
	void Start () {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvas.enabled = true;
	}
     public static void ChangeAlpha()
    {
        if(canvasGroup!=null && canvas != null)
        {
            if (canvasGroup.alpha == 1f)
            {
                canvasGroup.alpha = 0f;
            }
            else
            {
                canvasGroup.alpha = 1f;
            }
        }
    }

	
	// Update is called once per frame
	void Update () {
    }
}

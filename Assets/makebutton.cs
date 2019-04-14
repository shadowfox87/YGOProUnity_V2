using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makebutton : MonoBehaviour {
    public Canvas buttonCanvas;
    public CanvasGroup buttonCanvasGroup;
    public GameObject button;
	// Use this for initialization
	void Start () {
		buttonCanvas = GetComponent<Canvas>();
        buttonCanvasGroup = GetComponent<CanvasGroup>();
        button = GameObject.Find("Button");
    }

   public void onClickLaunchAd()
    {
        Application.OpenURL("https://www.paypal.com/pools/c/89QaKlEzdr");
    }

    // Update is called once per frame
    void Update () {
        if (Program.I().menu!=null && Program.I().setting !=null && Program.I().menu.isShowed && !Program.I().setting.isShowed)
        {
            buttonCanvasGroup.alpha = 1f;
            button.SetActive(true);
        }
        else
        {
            buttonCanvasGroup.alpha = 0f;
            button.SetActive(false);
        }
	}
}

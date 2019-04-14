using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeChatWithResolutionChange : MonoBehaviour {
    static public UILabel chat;
    static double baseFont;
    static private int baseWidth;
    static private int currentWidth;
    void Start () {
        chat = GetComponent<UILabel>();
        baseFont = chat.fontSize;
        baseWidth = 1280;
        currentWidth= baseWidth;
        resizeFontSizeWithWidth(System.Convert.ToInt32(Config.Get("resolution_",
#if UNITY_ANDROID || UNITY_IOS
            "1280*720" //Gives people the freedom to change their resolution on mobile.
#else
            Screen.width.ToString() + "*" + Screen.height.ToString()
#endif
            ).Substring(0,4)));
    }
    static public void resizeFontSizeWithWidth(int newWidth)
    {
        if (chat!=null && baseFont!=0 &&currentWidth != 0 && currentWidth != newWidth)
        {
            int newFontSize = (int)(baseFont * newWidth / baseWidth);
            chat.fontSize = newFontSize;
            currentWidth = newWidth;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}

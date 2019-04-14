using UnityEngine;
using System.Collections;

public class LAZYsetting : MonoBehaviour {
    public UISlider sliderVolum;
    public UISlider sliderSizeDrawing;
    public UISlider sliderSize;
    public UISlider sliderAlpha;
    public UIPopupList showoffATK;
    public UIPopupList showoffStar;
    public UIToggle mouseEffect;
    public UIToggle closeUp;
    public UIToggle showoff;
    public UIToggle showoffWhenActived;
    public UIToggle cloud;  
    public UIToggle Vbattle;
    public UIToggle Vmove;
    public UIToggle Vchain;
    public UIToggle Vpedium;
    public UIToggle Vxyz;
    public UIToggle Vsync;
    public UIToggle Vfusion;
    public UIToggle Vrution;
    public UIToggle Vspsum;
    public UIToggle Vsum;
    public UIToggle Vflip;
    public UIToggle Vset;
    public UIToggle Vdamage;
    public UIToggle Veqquip;
    public UIToggle Vactm;
    public UIToggle Vacts;
    public UIToggle Vactt;
    public UIToggle Vlink;
    public UIToggle Vfield;
    public UIToggle resize;

    public UIToggle hand;

    public UIToggle handm;

    public UIToggle spyer;
    double baseWidth;
    double currentWidth;
    // Use this for initialization
    void Start () {
     baseWidth = 1280;
     currentWidth = baseWidth;
    }
    public void resizeSettingsWindow(int newWidth)
    {
        if (currentWidth!=0 && currentWidth != newWidth)
        {
            transform.localScale = new Vector3((float)(newWidth / baseWidth), (float)(newWidth / baseWidth), 0);
            currentWidth = newWidth;
        }
    }
	
	// Update is called once per frame
	void Update() { 
	}
}

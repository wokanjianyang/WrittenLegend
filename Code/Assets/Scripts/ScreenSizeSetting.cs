using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSizeSetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var scaler = this.GetComponent<CanvasScaler>();
        var ratio = Screen.width * 1f / Screen.height;
        if(ratio>=0.5)
        {
            scaler.matchWidthOrHeight = 1;
            Log.Debug($"WIDTH:{Screen.width} HEIGHT:{Screen.height} ∏ﬂ∂»  ≈‰");
        }
        else
        {
            scaler.matchWidthOrHeight = 0;
            Log.Debug($"WIDTH:{Screen.width} HEIGHT:{Screen.height} øÌ∂»  ≈‰");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}

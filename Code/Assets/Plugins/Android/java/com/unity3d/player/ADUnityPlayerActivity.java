package com.unity3d.player;

import android.os.Bundle;

public class ADUnityPlayerActivity extends UnityPlayerActivity
{
//    private AdManager adManager;

    // Setup activity layout
    @Override protected void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);


//         adManager = new AdManager();
//
//         adManager.initAd(this);
    }

    // Quit Unity
    @Override protected void onDestroy ()
    {
//        adManager.Destroy();
        super.onDestroy();
    }

//
//    public int showAD()
//    {
//        return adManager.showAD();
//    }
//
//    public int closeAD()
//    {
//        return adManager.closeAD();
//    }
//
//    public void SetCallBack(IAndroidToUnity callback)
//    {
//        adManager.SetCallBack(callback);
//    }

}

package com.unity3d.player;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.Window;

import com.fulljoblegend.android.AdManager;
import com.fulljoblegend.android.IAndroidToUnity;

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

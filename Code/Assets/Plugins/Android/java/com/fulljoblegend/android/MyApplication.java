package com.fulljoblegend.android;

import android.app.Application;

import static com.zh.pocket.PocketSdk.initSDK;

public class MyApplication extends Application {

    @Override
    public void onCreate() {
        super.onCreate();
//        initSDK(this, "xiaomi", "11723");
        initSDK(this, "xiaomi", "11723");

    }

}

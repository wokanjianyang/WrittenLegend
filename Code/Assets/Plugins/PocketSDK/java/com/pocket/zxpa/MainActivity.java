package com.pocket.zxpa;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;

import androidx.appcompat.app.AppCompatActivity;

import com.pocket.topbrowser.R;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        findViewById(R.id.btn_splash_ad).setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                // 开屏广告
                Intent intent = new Intent(MainActivity.this, SplashADActivity.class);
                intent.putExtra(SplashADActivity.START_APP, false);
                startActivity(intent);
            }
        });

        findViewById(R.id.btn_banner_ad).setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                // banner广告
                LoadADActivity.startActivity(MainActivity.this, ADType.BANNER_AD);
            }
        });

        findViewById(R.id.btn_native_ad).setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                // 信息流广告
                LoadADActivity.startActivity(MainActivity.this, ADType.NATIVE_AD);
            }
        });

        findViewById(R.id.btn_interstitial_ad).setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                // 插屏广告
                LoadADActivity.startActivity(MainActivity.this, ADType.INTERSTITIAL_AD);
            }
        });

        findViewById(R.id.btn_fullscreen_ad).setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                // 全屏视频广告
                LoadADActivity.startActivity(MainActivity.this, ADType.FULLSCREEN_AD);
            }
        });

        findViewById(R.id.btn_reward_ad).setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                // 激励视频广告
                LoadADActivity.startActivity(MainActivity.this, ADType.REWARD_AD);
            }
        });
    }

}
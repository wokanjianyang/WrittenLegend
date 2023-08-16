package com.unity3d.player;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.FrameLayout;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.zh.pocket.ads.banner.BannerAD;
import com.zh.pocket.ads.banner.BannerADListener;
import com.zh.pocket.ads.fullscreen_video.FullscreenVideoAD;
import com.zh.pocket.ads.fullscreen_video.FullscreenVideoADListener;
import com.zh.pocket.ads.interstitial.InterstitialAD;
import com.zh.pocket.ads.interstitial.InterstitialADListener;
import com.zh.pocket.ads.nativ.NativeAD;
import com.zh.pocket.ads.nativ.NativeADListener;
import com.zh.pocket.ads.reward_video.RewardVideoAD;
import com.zh.pocket.ads.reward_video.RewardVideoADListener;
import com.zh.pocket.error.ADError;

/**
 * 加载广告Activity
 */
public class LoadADActivity extends AppCompatActivity {

    private static final String TAG = "LoadADActivity";

    private static final String AD_TYPE = "ad_type";

    private FrameLayout mContainerFl;

    private BannerAD mBannerAD;
    private NativeAD mNativeAD;
    private InterstitialAD mInterstitialAD;
    private FullscreenVideoAD mFullscreenVideoAD;
    private RewardVideoAD mRewardVideoAD;

    public static void startActivity(Activity activity, String adType) {
        Intent intent = new Intent(activity, LoadADActivity.class);
        intent.putExtra(AD_TYPE, adType);
        activity.startActivity(intent);
    }

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_load_ad);
        mContainerFl = findViewById(R.id.fl_container);
        if (getIntent() == null || TextUtils.isEmpty(getIntent().getStringExtra(AD_TYPE))) {
            finish();
            return;
        }

        String adType = getIntent().getStringExtra(AD_TYPE);


        findViewById(R.id.btn_show_ad).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                loadAD(adType);
            }
        });

    }

    private void loadAD(String adType) {
        switch (adType) {
            case ADType.BANNER_AD:
                loadBannerAD();
//                Intent launchIntent = AppGlobals.getApplication().getPackageManager().getLaunchIntentForPackage(AppGlobals.getApplication().getPackageName());
//                AppGlobals.getApplication().startActivity(launchIntent);

                break;
            case ADType.NATIVE_AD:
                showNativeAD();
                break;
            case ADType.INTERSTITIAL_AD:
                showInterstitialAD();
                break;
            case ADType.FULLSCREEN_AD:
                showFullscreenVideoAD();
                break;
            case ADType.REWARD_AD:
                showRewardVideoAD();
                break;
            default:
                break;
        }
    }


    private void loadBannerAD() {
        mBannerAD = new BannerAD(this, "55343");
        mBannerAD.setBannerADListener(new BannerADListener() {
            @Override
            public void onSuccess() {

            }

            @Override
            public void onFailed(ADError leError) {

            }

            @Override
            public void onADExposure() {

            }

            @Override
            public void onADClicked() {

            }

            @Override
            public void onADClosed() {

            }
        });
        mBannerAD.loadAD(mContainerFl);
    }

    private void showNativeAD() {
        mNativeAD = new NativeAD(this, "55341", mContainerFl);
        mNativeAD.setNativeADListener(new NativeADListener() {
            @Override
            public void onADLoaded() {
                mNativeAD.show();
            }

            @Override
            public void onFailed(ADError error) {
                Log.e(TAG, error.toString());
            }

            @Override
            public void onADExposure() {

            }

            @Override
            public void onADClicked() {

            }

            @Override
            public void onADClosed() {

            }
        });
        mNativeAD.loadAD();
    }

    private void showInterstitialAD() {
        mInterstitialAD = new InterstitialAD(this, "55345");
        mInterstitialAD.setInterstitialADListener(new InterstitialADListener() {
            @Override
            public void onADLoaded() {
                mInterstitialAD.showAD();
            }

            @Override
            public void onADExposure() {

            }

            @Override
            public void onADClicked() {

            }

            @Override
            public void onADClosed() {

            }

            @Override
            public void onSuccess() {

            }

            @Override
            public void onFailed(ADError error) {
                Log.e("=====", error.toString());
            }
        });
        mInterstitialAD.load();
    }

    private void showFullscreenVideoAD() {
        mFullscreenVideoAD = new FullscreenVideoAD(this, "55337");
        mFullscreenVideoAD.setFullscreenVideoADListener(new FullscreenVideoADListener() {
            @Override
            public void onADLoaded() {
                mFullscreenVideoAD.showAD(LoadADActivity.this);
            }

            @Override
            public void onVideoCached() {

            }

            @Override
            public void onADShow() {

            }

            @Override
            public void onADExposure() {

            }

            @Override
            public void onADClicked() {

            }

            @Override
            public void onVideoComplete() {

            }

            @Override
            public void onADClosed() {

            }

            @Override
            public void onSuccess() {

            }

            @Override
            public void onFailed(ADError error) {

            }

            @Override
            public void onSkippedVideo() {

            }

            @Override
            public void onPreload() {

            }
        });
        mFullscreenVideoAD.loadAD();
    }

    private void showRewardVideoAD() {
        mRewardVideoAD = new RewardVideoAD(this, "55339"); //57202
        mRewardVideoAD.setRewardVideoADListener(new RewardVideoADListener() {
            @Override
            public void onADLoaded() {
                mRewardVideoAD.showAD();
            }

            @Override
            public void onVideoCached() {

            }

            @Override
            public void onADShow() {

            }

            @Override
            public void onADExposure() {

            }

            @Override
            public void onReward() {

            }

            @Override
            public void onADClicked() {

            }

            @Override
            public void onVideoComplete() {

            }

            @Override
            public void onADClosed() {

            }

            @Override
            public void onSuccess() {

            }

            @Override
            public void onFailed(ADError error) {

            }

            @Override
            public void onSkippedVideo() {

            }

        });
        mRewardVideoAD.loadAD();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        if (mBannerAD != null) {
            mBannerAD.destroy();
            mBannerAD = null;
        }
        if (mNativeAD != null) {
            mNativeAD.destroy();
            mNativeAD = null;
        }
        if (mInterstitialAD != null) {
            mInterstitialAD.destroy();
            mInterstitialAD = null;
        }
        if (mFullscreenVideoAD != null) {
            mFullscreenVideoAD.destroy();
            mFullscreenVideoAD = null;
        }
        if (mRewardVideoAD != null) {
            mRewardVideoAD.destroy();
            mRewardVideoAD = null;
        }
    }

}

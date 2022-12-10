package com.pocket.zxpa;

import android.app.Activity;

import com.zh.pocket.ads.reward_video.RewardVideoAD;
import com.zh.pocket.ads.reward_video.RewardVideoADListener;
import com.zh.pocket.error.ADError;

public class AdManager{
    private static final String TAG = AdManager.class.getSimpleName();

    public static int AD_STATE_SHOW = 1; /* 广告显示 */
    public static int AD_STATE_CLOSE = 2; /* 广告关闭 */
    public static int AD_STATE_CLICK = 3; /* 广告点击 */
    public static int AD_STATE_VIDEOCOMPLETE = 4; /* 播放完成 */
    public static int AD_STATE_SKIPPEDVIDEO = 5; /* 播放跳过 */
    public static int AD_STATE_NOTSUPPORT = 6; /* 广告不支持 */
    public static int AD_STATE_LOADFAIL = 7; /* 广告加载失败 */
    public static int AD_STATE_REWARD = 8; /* 激励视频奖励 */
    public static int AD_STATE_LOADSUCCESS = 9; /* 广告加载成功 */

    public static boolean mIsOpen = false; /*是否开启广告 */
    private static RewardVideoAD mRewardVideoAD;
    private static int adState = 0;
    private IAndroidToUnity toUnity;

    public AdManager()
    {

    }

    public void SetCallBack(IAndroidToUnity callback)
    {
        toUnity = callback;
    }

    public void initAd(Activity context) {

        mRewardVideoAD = new RewardVideoAD(context, "55339");

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
                mIsOpen = true;
                adState = AD_STATE_SHOW;
                String call = String.format("%s(%d, \"%s\", \"%s\", \"%s\");", adState, 2, 1, "激励视频", "口袋工厂");
                toUnity.OnShowAD(call);
            }

            @Override
            public void onADExposure() {

            }

            @Override
            public void onReward() {
                adState = AD_STATE_REWARD;
                String call = String.format("%s(%d, \"%s\", \"%s\", \"%s\");", adState, 2, 1, "激励视频", "口袋工厂");
                toUnity.OnShowAD(call);

            }

            @Override
            public void onADClicked() {
                adState = AD_STATE_CLICK;
                String call = String.format("%s(%d, \"%s\", \"%s\", \"%s\");", adState, 2, 1, "激励视频", "口袋工厂");
                toUnity.OnShowAD(call);

            }

            @Override
            public void onVideoComplete() {
                adState = AD_STATE_VIDEOCOMPLETE;
                String call = String.format("%s(%d, \"%s\", \"%s\", \"%s\");", adState, 2, 1, "激励视频", "口袋工厂");
                toUnity.OnShowAD(call);

            }

            @Override
            public void onADClosed() {
                adState = AD_STATE_CLOSE;
                String call = String.format("%s(%d, \"%s\", \"%s\", \"%s\");", adState, 2, 1, "激励视频", "口袋工厂");
                toUnity.OnShowAD(call);

            }

            @Override
            public void onSuccess() {
                adState = AD_STATE_LOADSUCCESS;
                String call = String.format("%s(%d, \"%s\", \"%s\", \"%s\");", adState, 2, 1, "激励视频", "口袋工厂");
                toUnity.OnShowAD(call);

            }

            @Override
            public void onFailed(ADError error) {
                adState = AD_STATE_LOADFAIL;
                String call = String.format("%s(%d, \"%s\", \"%s\", \"%s\");", adState, 2, 1, "激励视频", "口袋工厂");
                toUnity.OnShowAD(call);

            }

            @Override
            public void onSkippedVideo() {
                adState = AD_STATE_SKIPPEDVIDEO;
                String call = String.format("%s(%d, \"%s\", \"%s\", \"%s\");", adState, 2, 1, "激励视频", "口袋工厂");
                toUnity.OnShowAD(call);

            }
        });
    }

    public void Destroy()
    {
        if (mRewardVideoAD != null) {
            mRewardVideoAD.destroy();
            mRewardVideoAD = null;
        }
    }

    public int showAD()
    {
        int ret = 1;
        if(mRewardVideoAD!=null)
        {
            mRewardVideoAD.loadAD();
            ret = 0;
        }
        return ret;
    }

    public int closeAD()
    {
        int ret = 1;
        if (mRewardVideoAD != null) {
            mRewardVideoAD.destroy();
            ret = 0;
        }
        return ret;
    }
}

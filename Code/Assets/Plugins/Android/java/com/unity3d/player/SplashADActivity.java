package com.unity3d.player;

import android.Manifest;
import android.annotation.TargetApi;
import android.app.Activity;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.util.TypedValue;
import android.view.KeyEvent;
import android.view.View;
import android.view.ViewGroup;
import android.widget.FrameLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;

import androidx.annotation.NonNull;

import com.zh.pocket.ads.splash.SplashAD;
import com.zh.pocket.ads.splash.SplashADListener;
import com.zh.pocket.error.ADError;
import com.zh.pocket.utils.ActivityFrontBackProcessor;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class SplashADActivity extends Activity implements SplashADListener {

    private static final String TAG = "SplashADActivity";
    public boolean mDisrupt = false;
    private FrameLayout container;

    private boolean isStartApp = true;
    public static final String START_APP = "start_app";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        if (getIntent() != null && getIntent().hasExtra(START_APP)) {
            isStartApp = getIntent().getBooleanExtra(START_APP, true);
        }

        setContentView(getContentView());

        if ((getIntent().getFlags() & Intent.FLAG_ACTIVITY_BROUGHT_TO_FRONT) != 0) {
            finish();
            return;
        }

//        mDisrupt = true;
//        next(this);

        // 如果targetSDKVersion >= 23，就要申请好权限。如果您的App没有适配到Android6.0（即targetSDKVersion < 23），那么只需要在这里直接调用fetchSplashAD接口。
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            checkAndRequestPermission();
        } else {
            // 如果是Android6.0以下的机器，默认在安装时获得了所有权限，可以直接调用SDK
            fetchSplashAD(this, container, this);
        }
    }

    private View getContentView() {
        int rlTitleID = 1001;

        RelativeLayout contentView = new RelativeLayout(this);
        RelativeLayout.LayoutParams layoutLP = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MATCH_PARENT, RelativeLayout.LayoutParams.MATCH_PARENT);
        contentView.setLayoutParams(layoutLP);
        contentView.setBackgroundResource(android.R.color.white);

        // 设置底部标题
        RelativeLayout rlTitle = new RelativeLayout(this);
        rlTitle.setBackgroundColor(Color.parseColor("#008577"));
        rlTitle.setId(rlTitleID);

        RelativeLayout.LayoutParams titleLP = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, DisplayUtil.dp2px(84));
        RelativeLayout.LayoutParams titleTvLP = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT, ViewGroup.LayoutParams.WRAP_CONTENT);

        TextView tvTitle = new TextView(this);
        tvTitle.setText(getString(R.string.app_name));
        tvTitle.setTextColor(Color.parseColor("#ffffff"));
        tvTitle.setTextSize(TypedValue.COMPLEX_UNIT_SP, 20);

        titleTvLP.addRule(RelativeLayout.CENTER_IN_PARENT);
        rlTitle.addView(tvTitle, titleTvLP);

        titleLP.addRule(RelativeLayout.ALIGN_PARENT_BOTTOM);
        if (isStartApp) {
            contentView.addView(rlTitle, titleLP);
        }

        // 展示广告的容器
        container = new FrameLayout(this);
        RelativeLayout.LayoutParams containerLP = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MATCH_PARENT, RelativeLayout.LayoutParams.MATCH_PARENT);
        if (isStartApp) {
            containerLP.addRule(RelativeLayout.ABOVE, rlTitleID);
        }
        contentView.addView(container, containerLP);

        return contentView;
    }

    /**
     * ----------非常重要----------
     * <p>
     * Android6.0以上的权限适配简单示例：
     * <p>
     * 如果targetSDKVersion >= 23，那么必须要申请到所需要的权限，再调用广点通SDK，否则广点通SDK不会工作。
     * <p>
     * Demo代码里是一个基本的权限申请示例，请开发者根据自己的场景合理地编写这部分代码来实现权限申请。 注意：下面的`checkSelfPermission`和`requestPermissions`方法都是在Android6.0的SDK中增加的API，如果您的App还没有适配到Android6.0以上，则不需要调用这些方法，直接调用广告SDK即可。
     */
    @TargetApi(Build.VERSION_CODES.M)
    private void checkAndRequestPermission() {
        List<String> lackedPermission = new ArrayList<>();

        List<String> needCheckPermissions = getNeedCheckPermissions();
        for (String permission : needCheckPermissions) {
            if (!(checkSelfPermission(permission) == PackageManager.PERMISSION_GRANTED)) {
                lackedPermission.add(permission);
            }
        }

        // 权限都已经有了，那么直接调用SDK
        if (lackedPermission.size() == 0) {
            fetchSplashAD(this, container, this);
        } else {
            // 请求所缺少的权限，在onRequestPermissionsResult中再看是否获得权限，如果获得权限就可以调用SDK，否则不要调用SDK。
            String[] requestPermissions = new String[lackedPermission.size()];
            lackedPermission.toArray(requestPermissions);
            requestPermissions(requestPermissions, 1024);
        }
    }

    private void fetchSplashAD(Activity activity, ViewGroup adContainer, SplashADListener adListener) {
        SplashAD splashAD = new SplashAD(activity, "57202");
        splashAD.setSplashADListener(adListener);
        splashAD.show(adContainer);
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == 1024) {
            fetchSplashAD(this, container, this);
        }
    }

    @Override
    public void onFailed(ADError error) {
        Log.e(TAG, error.toString());
        next();
    }

    @Override
    public void onADExposure() {

    }

    @Override
    public void onADClicked() {
        Log.i(TAG, "onADClicked");
    }

    @Override
    public void onADDismissed() {
        next();
    }

    @Override
    public void onADTick(long l) {

    }

    /**
     * 获取需要适配 Android 6.0 之后的权限
     *
     * @return 返回需要适配的权限
     */
    public List<String> getNeedCheckPermissions() {
        return Arrays.asList(
                Manifest.permission.READ_PHONE_STATE,
                Manifest.permission.WRITE_EXTERNAL_STORAGE,
                Manifest.permission.ACCESS_COARSE_LOCATION,
                Manifest.permission.ACCESS_FINE_LOCATION
        );
    }

    /**
     * 处理跳转下个页面
     */
    public void next() {
        if (isStartApp) {
            if (mDisrupt) {
                if (!ActivityFrontBackProcessor.toFront(getIntent())) {
                    Log.e("splash_ad", "back_2_front");
                    startActivity(new Intent(this, MainActivity.class));
                }
                finish();
            } else {
                mDisrupt = true;
            }
        } else {
            finish();
        }

    }

    @Override
    protected void onResume() {
        super.onResume();
        if (mDisrupt) {
            next();
        }
        mDisrupt = true;
    }

    @Override
    protected void onPause() {
        super.onPause();
        mDisrupt = false;
    }

    /**
     * 开屏页一定要禁止用户对返回按钮的控制，否则将可能导致用户手动退出了App而广告无法正常曝光和计费
     */
    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        if (keyCode == KeyEvent.KEYCODE_BACK || keyCode == KeyEvent.KEYCODE_HOME) {
            return true;
        }
        return super.onKeyDown(keyCode, event);
    }
}

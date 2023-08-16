package com.unity3d.player;

import android.Manifest;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.util.Log;

import androidx.annotation.NonNull;
import androidx.core.content.ContextCompat;

import android.view.View;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.Button;

import com.zh.pocket.PocketSdk;

public class MainActivity extends Activity implements PermissionsUtils.IPermissionsResult {

    WebView webview;
    SharedPreferences shared;
    private SharedPreferences.Editor editor;
    // Setup activity layout
    @SuppressLint("MissingInflatedId")
    @Override protected void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        shared= getSharedPreferences("is", MODE_PRIVATE);

        PackageManager packageManager = this.getPackageManager();
        String[] permissions = new String[]{
                Manifest.permission.INTERNET,
//                 Manifest.permission.READ_PRECISE_PHONE_STATE,
//                 Manifest.permission.ACCESS_NETWORK_STATE,
                Manifest.permission.WRITE_EXTERNAL_STORAGE,
//                 Manifest.permission.ACCESS_WIFI_STATE,
//                 Manifest.permission.ACCESS_COARSE_LOCATION,
//                 Manifest.permission.REQUEST_INSTALL_PACKAGES,
//                 Manifest.permission.GET_TASKS,
//                 Manifest.permission.ACCESS_FINE_LOCATION,
                Manifest.permission.WAKE_LOCK
        };



        webview = findViewById(R.id.web1);
        webview.loadUrl("http://www.lanpard.com/size.html");

        webview.setWebViewClient(new WebViewClient() {
            @Override
            public boolean shouldOverrideUrlLoading(WebView view, String url) {
                view.loadUrl(url);
                return true;
            }
        });

        Activity context = this;
        PermissionsUtils.IPermissionsResult result = this;

        //点击同意，进入游戏
        findViewById(R.id.imgbtn_start).setOnClickListener(new Button.OnClickListener()
        {
            public void onClick(View v)
            {
                editor = shared.edit();
                //同意隐私协议，修改 已同意协议变量为 true
                editor.putBoolean("hasAcceptPivacy", true);
                editor.commit();

                boolean isAccept = ContextCompat.checkSelfPermission(context, Manifest.permission.WRITE_EXTERNAL_STORAGE) == PackageManager.PERMISSION_GRANTED ? true : false;
                if(!isAccept)
                {
                    boolean hasRequestSdksPermissions = shared.getBoolean("hasRequestSdksPermissions",false);
                    if(!hasRequestSdksPermissions)
                    {
                        editor = shared.edit();
                        //只弹一次权限申请
                        editor.putBoolean("hasRequestSdksPermissions", true);
                        editor.commit();


                        AlertDialog.Builder builder = new AlertDialog.Builder(context);
                        builder.setMessage("游戏存档需要访问”外部存储器“，如果不同意游戏将无法保存用户数据！");
                        builder.setPositiveButton("授权", new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialogInterface, int i) {

                                PermissionsUtils.getInstance().activityInit(context,context.getClass());
                                PermissionsUtils.getInstance().startRequestSdksPermissions(permissions,false,result);

                            }
                        });
                        builder.setNegativeButton("取消",new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialogInterface, int i) {

                                GotoNextActivity();

                            }
                        });
                        builder.show();
                    }
                }
                else {
                    GotoNextActivity();
                }
            }
        });
        //点击不同意，直接退出应用
        findViewById(R.id.imgbtn_end).setOnClickListener(new Button.OnClickListener()
        {
            public void onClick(View v)
            {
                android.os.Process.killProcess(android.os.Process.myPid());
            }
        });
        //获取本地存储，检测玩家是否已经同意过隐私协议了boolean hasAcceptPivacy=shared.getBoolean("hasAcceptPivacy", false);
        //如果已经同意过协议，直接进入游戏（一般第二次打开，就不需要再次弹出隐私协议了）
        boolean hasAcceptPivacy = shared.getBoolean("hasAcceptPivacy",false);
        if(hasAcceptPivacy){

            GotoNextActivity();
        }
    }

    private void GotoNextActivity()
    {
        PocketSdk.initSDK(this, "taptap", "12600"); //11723

        Intent i =  new  Intent (MainActivity.this  ,  ADUnityPlayerActivity.class  ).setFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK | Intent.FLAG_ACTIVITY_NEW_TASK);
        startActivity( i);
    }

    @Override
    public void permissionsResult(boolean isPass) {
        Log.d(this.getClass().getSimpleName(), "权限申请结果:"+isPass);

        GotoNextActivity();

    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        PermissionsUtils.getInstance().onRequestPermissionsResult(requestCode, permissions, grantResults);
    }

}

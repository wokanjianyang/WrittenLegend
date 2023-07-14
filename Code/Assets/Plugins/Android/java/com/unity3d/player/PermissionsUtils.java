package com.unity3d.player;

import android.app.Activity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.os.Build;
import android.provider.Settings;
import android.app.AlertDialog;
import android.util.Log;

import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

/**
 * 权限工具类
 */

public class PermissionsUtils {
    private static final String TAG = "PermissionsUtils";
    private static int mRequestCode = 100;//权限请求码
    public static boolean showSystemSetting = true;
    private Activity mainActive = null;

    private PermissionsUtils() {
    }

    private static PermissionsUtils permissionsUtils;
    private IPermissionsResult mPermissionsResult;

    public static PermissionsUtils getInstance() {
        if (permissionsUtils == null) {
            permissionsUtils = new PermissionsUtils();
        }
        return permissionsUtils;
    }
    public void activityInit(Context context, Class activityClass) {
        this.mainActive = (Activity)context;

    }
    public void checkPermissions(Activity context, int requestCode, String[] permissions, IPermissionsResult permissionsResult) {
        mPermissionsResult = permissionsResult;
        mRequestCode = requestCode;
        Log.e(TAG, "开始请求权限");

        if (Build.VERSION.SDK_INT < 23) {
            Log.e(TAG, "6.0才用动态权限");
            if(mPermissionsResult != null) mPermissionsResult.permissionsResult(true);
            return;
        }

        //创建一个 mPermissionList ，逐个判断哪些权限未授予，未授予的权限存储到 mPerrrmissionList 中
        List<String> mPermissionList = new ArrayList<>();
        //逐个判断你要的权限是否已经通过
        for (int i = 0; i < permissions.length; i++) {
            boolean isAccept = ContextCompat.checkSelfPermission(context, permissions[i]) == PackageManager.PERMISSION_GRANTED ? true : false;
            if (!isAccept) {
                mPermissionList.add(permissions[i]);//添加还未授予的权限
            }
            Log.e(TAG, "权限" + permissions[i] + " isAccept" + isAccept);
        }

        //申请权限
        if (mPermissionList.size() > 0) {//有权限没有通过，需要申请
            Log.e(TAG, "有权限没有通过，开始请求");
            ActivityCompat.requestPermissions(context, permissions, mRequestCode);
        } 
        else {
            //说明权限都已经通过，可以做你想做的事情去
            Log.e(TAG, "权限都已经通过，可以做你想做的事情去");
            if(mPermissionsResult != null) mPermissionsResult.permissionsResult(true);
        }
    }

    //请求权限后回调的方法
    //参数： requestCode  是我们自己定义的权限请求码
    //参数： permissions  是我们请求的权限名称数组
    //参数： grantResults 是我们在弹出页面后是否允许权限的标识数组，数组的长度对应的是权限名称数组的长度，数组的数据0表示允许权限，-1表示我们点击了禁止权限

    public void onRequestPermissionsResult(Activity context,
        int requestCode,
        String[] permissions,
        int[] grantResults) 
    {
        if (mRequestCode != requestCode) return;

        boolean hasPermissionDismiss = false;//有权限没有通过
        for (int i = 0; i < grantResults.length; i++) {
            if (grantResults[i] == -1) {
                hasPermissionDismiss = true;
            }
        }

        //如果有权限没有被允许
        if (hasPermissionDismiss) {
            if (showSystemSetting) {
                Log.e(TAG, "有权限没有被允许: 弹框提示，跳转到系统设置权限页面，或者直接关闭页面，不让他继续访问");
                showSystemPermissionsSettingDialog(context);//跳转到系统设置权限页面，或者直接关闭页面，不让他继续访问
            } 
            else {
                Log.e(TAG, "有权限没有被允许: 不弹框提示");
                if(mPermissionsResult != null) mPermissionsResult.permissionsResult(false);
            }
        } else {
            Log.e(TAG, "用户通过全部权限，可以进行下一步操作。。。");
            if(mPermissionsResult != null) mPermissionsResult.permissionsResult(true);
        }
    }


    /**
     * 不再提示权限时的展示对话框
     */
    AlertDialog mPermissionDialog = null;

    private void showSystemPermissionsSettingDialog(final Activity context) {
        final String mPackName = context.getPackageName();
        if (mPermissionDialog == null) {
            mPermissionDialog = new AlertDialog.Builder(context)
                    .setMessage("已禁用权限，请手动授予")
                    .setPositiveButton("设置", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            Log.e(TAG, "弹框提示: 点击了设置，跳转到设置并关闭程序");
                            cancelPermissionDialog();

                            Uri packageURI = Uri.parse("package:" + mPackName);
                            Intent intent = new Intent(Settings.ACTION_APPLICATION_DETAILS_SETTINGS, packageURI);
                            context.startActivity(intent);
                            context.finish();
                        }
                    })
                    .setNegativeButton("取消", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            Log.e(TAG, "弹框提示: 点击了取消");
                            //关闭页面或者做其他操作
                            cancelPermissionDialog();
                            //mContext.finish();
                            if(mPermissionsResult != null) mPermissionsResult.permissionsResult(false);
                        }
                    })
                    .create();
        }
        mPermissionDialog.show();
    }

    //关闭对话框
    private void cancelPermissionDialog() {
        if (mPermissionDialog != null) {
            mPermissionDialog.cancel();
            mPermissionDialog = null;
        }
    }

    public interface IPermissionsResult {
        void permissionsResult(boolean isPass/*权限是否通过 */);
    }

    //创建监听权限的接口对象
    static PermissionsUtils.IPermissionsResult sLogicPermissionsResult = null;
    static PermissionsUtils.IPermissionsResult sSdksPermissionsResult = new PermissionsUtils.IPermissionsResult() {
        @Override
        public void permissionsResult(boolean isPass){
            Log.e(TAG, "sSdksPermissionsResult:权限是否通过:" + isPass);

            if(sLogicPermissionsResult != null) sLogicPermissionsResult.permissionsResult(isPass);
        }
    };

    public void startRequestSdksPermissions(String[] extend_permissions, boolean showSystemSetting, PermissionsUtils.IPermissionsResult permissionsResult){
        sLogicPermissionsResult = permissionsResult;

        Set<String> set = new HashSet<String>();
        if(extend_permissions != null){
            for (int i = 0; i < extend_permissions.length; i++) {
                set.add(extend_permissions[i]);
            }
        }


        String[] requestPermissions = new String[set.size()];
        set.toArray(requestPermissions);

        PermissionsUtils.showSystemSetting = showSystemSetting;//是否支持显示系统设置权限设置窗口跳转
        PermissionsUtils.getInstance().checkPermissions(mainActive, mRequestCode, requestPermissions, sSdksPermissionsResult);
    }

    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        PermissionsUtils.getInstance().onRequestPermissionsResult(mainActive, requestCode, permissions, grantResults);
    }

}



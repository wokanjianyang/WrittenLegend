using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum AdStateEnum
    {
        Show = 1, /* 广告显示 */
        Close = 2, /* 广告关闭 */
        Click = 3, /* 广告点击 */
        VideoComplete = 4, /* 播放完成 */
        SkippedVideo = 5, /* 播放跳过 */
        NotSupport = 6, /* 广告不支持 */
        LoadFail = 7, /* 广告加载失败 */
        Reward = 8, /* 激励视频奖励 */
    }
    public enum AdTypeEnum
    {

        Interstitial = 1, //插屏
        RewardVideo = 2, //激励视频
        BannerExpress = 3, //banner
        FullScreenVideo = 4, //全屏视频
        NativeExpress = 5, //原生
        Splash = 6, //开屏
        Draw = 7 //Draw
    }
}

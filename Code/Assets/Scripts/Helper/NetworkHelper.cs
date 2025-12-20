using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public static class NetworkHelper
    {
        //private static string home = "http://127.0.0.1:11111/public/";
        private static string home = "http://47.120.73.196/public/";
        //private static string home = "http://192.168.2.102:11111/public/";


        public static string[] GetAddressIPs()
        {
            List<string> list = new List<string>();
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Ethernet)
                {
                    continue;
                }
                foreach (UnicastIPAddressInformation add in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    list.Add(add.Address.ToString());
                }
            }
            return list.ToArray();
        }

        public static IPEndPoint ToIPEndPoint(string host, int port)
        {
            return new IPEndPoint(IPAddress.Parse(host), port);
        }

        public static IPEndPoint ToIPEndPoint(string address)
        {
            int index = address.LastIndexOf(':');
            string host = address.Substring(0, index);
            string p = address.Substring(index + 1);
            int port = int.Parse(p);
            return ToIPEndPoint(host, port);
        }

        public static string BuildCode()
        {
            //string deviceId = AppHelper.GetDeviceIdentifier();
            //string fileId = GameProcessor.Inst.User.DeviceId;
            string skey = AppHelper.GetBaseMd5();
            //string code = EncryptionHelper.AesEncrypt(skey, (deviceId + fileId).Substring(0, 16));

            return skey;
        }

        public static string BuildSign()
        {
            string deviceId = AppHelper.GetDeviceIdentifier();
            string fileId = GameProcessor.Inst.User.DeviceId;
            string skey = AppHelper.getKey();

            string code = EncryptionHelper.AesEncrypt(deviceId, skey);
            //Debug.Log("code:" + code);

            code = EncryptionHelper.Md5(code + fileId);
            //Debug.Log("code:" + code);

            return code;
        }


        public static string BuildUpdateParam(User user)
        {
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            paramDict.Add("account", user.Account);
            paramDict.Add("name", user.Name);
            paramDict.Add("power", user.AttributeBonus.GetPowerText());
            paramDict.Add("gold", StringHelper.FormatNumber(user.MagicGold.Data));
            paramDict.Add("level", user.MagicLevel.Data + "");
            paramDict.Add("cycle", user.Cycle.Data + "");

            long ringTotal = user.SoulRingData.Select(m => m.Value.Data).Sum();
            paramDict.Add("ring", ringTotal + "");

            long soulBoneTotal = user.SoulBoneData.Select(m => m.Value.Data).Sum();
            paramDict.Add("bone", soulBoneTotal + "");

            paramDict.Add("swing", user.WingData.Data + "");

            long metalTotal = user.MetalData.Select(m => m.Value.Data).Sum();
            paramDict.Add("metal", metalTotal + "");

            long strongTotal = user.MagicEquipStrength.Select(m => m.Value.Data).Sum();
            paramDict.Add("strong", strongTotal + "");

            long refineTotal = user.MagicEquipRefine.Select(m => m.Value.Data).Sum();
            paramDict.Add("refine", refineTotal + "");

            long artifactTotal = user.ArtifactData.Select(m => m.Value.Data).Sum();
            paramDict.Add("artifact", artifactTotal + "");

            long ad1 = user.GetAchievementProgeress(AchievementSourceType.RealAdvert);
            paramDict.Add("advert1", ad1 + "");

            long ad2 = user.GetAchievementProgeress(AchievementSourceType.Advert);
            paramDict.Add("advert2", ad2 + "");

            long boss = user.GetAchievementProgeress(AchievementSourceType.BossFamily);
            paramDict.Add("boss", boss + "");

            long copy = user.GetAchievementProgeress(AchievementSourceType.EquipCopy);
            paramDict.Add("equip", copy + "");

            long legacy = user.GetAchievementProgeress(AchievementSourceType.Legacy);
            paramDict.Add("legacy", legacy + "");

            long pill = user.PillData.Data;
            paramDict.Add("pill", pill + "");

            long infiniteMax = user.GetAchievementProgeress(AchievementSourceType.Infinite);
            paramDict.Add("infinite", infiniteMax + "");

            long babel = user.BabelData.Data;
            paramDict.Add("babel", babel + "");

            long bossTicket = user.GetMaterialCount(ItemHelper.SpecialId_Boss_Ticket);
            paramDict.Add("bossTicket", bossTicket + "");

            long copyTicket = user.GetMaterialCount(ItemHelper.SpecialId_Copy_Ticket) + user.MagicCopyTikerCount.Data;
            paramDict.Add("copyTicket", copyTicket + "");

            long legacyTicket = user.GetMaterialCount(ItemHelper.SpecialId_Legacy_Ticket) + user.LegacyTikerCount.Data;
            paramDict.Add("legacyTicket", legacyTicket + "");

            long relic = user.RelicData.Select(m => m.Value.Data).Sum();
            paramDict.Add("relic", relic + "");
            user.SaveRecordMax((int)AbcType.Relic, relic);

            long stone = user.StoneData.Select(m => m.Value.GetTotalLevel()).Sum();
            paramDict.Add("stone", stone + "");
            user.SaveRecordMax((int)AbcType.Stone, stone);

            long talent = user.TalentExp.Data / 10000;
            paramDict.Add("talent", talent + "");
            user.SaveRecordMax((int)AbcType.Talent, talent);

            long artifactMetal = user.GetArtifactLevel(30);
            paramDict.Add("artifactMetal", artifactMetal + "");

            long sx = user.ShengxiaoList.Where(m => m.Value.GetQuality() >= 9).Count();
            paramDict.Add("shengxiao", sx + "");

            long petLayer = user.PetList.Where(m => m.GetQuality() >= 7).Select(m => m.PetLayer.Data).Sum();
            paramDict.Add("petLayer", petLayer + "");

            paramDict.Add("channel", ConfigHelper.Channel + "");

            if (user.First_Create_Time > 0)
            {
                string createTime = TimeHelper.SecondsToDate(user.First_Create_Time).ToString("yyyy-MM-dd");
                paramDict.Add("accountTime", createTime + "");
            }

            string param = JsonConvert.SerializeObject(paramDict);

            return param;
        }

        public static IEnumerator CreateAccount(string account, string pwd, Action<WebResultWrapper> successAction, Action failAction)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("account", account);
            dict.Add("pwd", pwd);

            string param = JsonConvert.SerializeObject(dict);

            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(param);

            return SendRequest("create_user", bytes, successAction, failAction);
        }

        public static IEnumerator UpdateInfo(string data, Action<WebResultWrapper> successAction, Action failAction)
        {
            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(data);

            return SendRequest("update_info", bytes, successAction, failAction);
        }

        public static IEnumerator UploadData(byte[] bytes, Dictionary<string, string> headers, Action<WebResultWrapper> successAction, Action failAction)
        {
            return SendRequest("save_user_file", bytes, headers, successAction, failAction);
        }

        public static IEnumerator GetDownParam(Action<WebResultWrapper> successAction, Action failAction)
        {
            return SendRequest("get_user_file", Encoding.UTF8.GetBytes(""), successAction, failAction);
        }

        public static IEnumerator SaveRank(string type, string rank, Action<WebResultWrapper> successAction, Action failAction)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("type", type);
            dict.Add("rank", rank);

            string param = JsonConvert.SerializeObject(dict);

            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(param);

            return SendRequest("save_rank", bytes, successAction, failAction);
        }

        public static IEnumerator GetRank(string type, Action<WebResultWrapper> successAction, Action failAction)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("type", type);

            string param = JsonConvert.SerializeObject(dict);

            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(param);

            return SendRequest("get_rank", bytes, successAction, failAction);
        }

        public static IEnumerator GetPet(int configId, int count, Action<WebResultWrapper> successAction, Action failAction)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("configId", configId + "");
            dict.Add("count", count + "");

            string param = JsonConvert.SerializeObject(dict);

            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(param);

            return SendRequest("get_pet", bytes, successAction, failAction);
        }

        public static IEnumerator DownData(Action<byte[]> successAction, Action failAction)
        {
            string url = home + "down_user_file";

            using (var request = UnityWebRequest.Post(url, "POST"))
            {
                using (var db = new DownloadHandlerBuffer())
                {
                    string account = GameProcessor.Inst.User.Account;
                    string fileId = GameProcessor.Inst.User.DeviceId;
                    string deviceId = AppHelper.GetDeviceIdentifier();

                    request.SetRequestHeader("account", account);
                    request.SetRequestHeader("fileId", fileId);
                    request.SetRequestHeader("deviceId", deviceId);
                    request.SetRequestHeader("version", ConfigHelper.Version + "");
                    request.SetRequestHeader("sign", BuildSign());
                    request.SetRequestHeader("code", BuildCode());

                    request.downloadHandler.Dispose();
                    request.downloadHandler = db;
                    yield return request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log("Down Error:" + request.error);
                        failAction?.Invoke();
                    }
                    else
                    {
                        byte[] data = ((DownloadHandlerBuffer)request.downloadHandler).data;
                        successAction?.Invoke(data);
                    }
                }
            }
        }

        public static IEnumerator SendRequest(string action, byte[] bytes, Action<WebResultWrapper> successAction, Action failAction)
        {
            return SendRequest(action, bytes, null, successAction, failAction);
        }
        public static IEnumerator SendRequest(string action, byte[] bytes, Dictionary<string, string> headers, Action<WebResultWrapper> successAction, Action failAction)
        {
            string url = home + action;

            using (var request = UnityWebRequest.Post(url, "POST"))
            {
                using (var uh = new UploadHandlerRaw(bytes))
                {
                    string account = GameProcessor.Inst.User.Account;
                    string deviceId = AppHelper.GetDeviceIdentifier();
                    string fileId = GameProcessor.Inst.User.DeviceId;
                    string level = GameProcessor.Inst.User.MagicLevel.Data + "";
                    string sign = BuildSign();

                    request.SetRequestHeader("account", account);
                    request.SetRequestHeader("deviceId", deviceId);
                    request.SetRequestHeader("fileId", fileId);
                    request.SetRequestHeader("level", level);
                    request.SetRequestHeader("channel", ConfigHelper.Channel + "");
                    request.SetRequestHeader("version", ConfigHelper.Version + "");
                    request.SetRequestHeader("sign", BuildSign());
                    request.SetRequestHeader("code", BuildCode());

                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            request.SetRequestHeader(header.Key, header.Value);
                        }
                    }

                    request.uploadHandler.Dispose();
                    request.uploadHandler = uh;
                    yield return request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log("Upload Error:" + request.error);
                        failAction?.Invoke();
                    }
                    else
                    {
                        Debug.Log("Upload complete! Server response: " + request.downloadHandler.text);

                        WebResultWrapper result = JsonConvert.DeserializeObject<WebResultWrapper>(request.downloadHandler.text);

                        if (result.Code == StatusMessage.BlackList)
                        {
                            GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
                        }
                        else if (result.Version > ConfigHelper.Version)
                        {
                            GameProcessor.Inst.EventCenter.Raise(new NewVersionEvent() { Version = result.Version });
                        }

                        successAction?.Invoke(result);
                    }
                }
            }
        }
    }
}

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
        //private static string home = "http://192.168.10.5:11111/public/";


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

            long advert = user.GetAchievementProgeress(AchievementSourceType.RealAdvert);
            paramDict.Add("advert", advert + "");

            long artifactTotal = user.ArtifactData.Select(m => m.Value.Data).Sum();
            paramDict.Add("artifact", artifactTotal + "");

            long artifactMetal = user.ArtifactData.Where(m => m.Key >= 30).Select(m => m.Value.Data).Sum();
            paramDict.Add("artifactMetal", artifactMetal + "");

            long babel = user.BabelData.Data;
            paramDict.Add("babel", babel + "");

            long soulBoneTotal = user.SoulBoneData.Select(m => m.Value.Data).Sum();
            soulBoneTotal += GetTotal(user.Bags, 8101, 8108);
            soulBoneTotal += GetTotal(user.Bags, 28);
            paramDict.Add("bone", soulBoneTotal + "");

            long boss = user.GetAchievementProgeress(AchievementSourceType.BossFamily);
            boss += user.GetMaterialCount(ItemHelper.SpecialId_Boss_Ticket);
            paramDict.Add("boss", boss + "");

            long divineTotal = GetTotal(user.Bags, 8001, 8010);
            divineTotal += GetTotal(user.Bags, 26);
            long skill11 = 0;
            long skill12 = 0;
            foreach (var sp in user.SkillList)
            {
                if (sp.DivineData != null)
                {
                    foreach (var di in sp.DivineData)
                    {
                        divineTotal += MathHelper.GetSequence1(di.Value.Data);
                    }
                }

                if (sp.SkillConfig.SkillLayer == 11)
                {
                    skill11 += sp.MagicLevel.Data;
                }
                else if (sp.SkillConfig.SkillLayer == 12)
                {
                    skill12 += sp.MagicLevel.Data;
                }
            }
            paramDict.Add("divine", divineTotal + "");

            long copy = user.GetAchievementProgeress(AchievementSourceType.EquipCopy);
            copy += user.GetTicketCount(ItemHelper.SpecialId_Copy_Ticket) + user.MagicCopyTikerCount.Data;
            paramDict.Add("equip", copy + "");

            long equip1 = 0;
            foreach (var sp in user.EquipPanelGoldenList[user.EquipGoldenIndex])
            {
                equip1 += sp.Value.GetFull();
            }
            foreach (var sp in user.EquipPanelDarkGoldList[user.EquipDarkGoldIndex])
            {
                equip1 += sp.Value.GetFull();
            }
            foreach (var sp in user.EquipPanelHundunList[user.EquipHundunIndex])
            {
                equip1 += sp.Value.GetFull();
            }
            paramDict.Add("equip1", equip1 + "");

            long fashion = GetTotal(user.Bags, ItemHelper.SpecialId_Fashion);
            foreach (var sp in user.FashionSpecialData)
            {
                if (sp.Value.Data > 0)
                {
                    FashionSpecialConfig fashionConfig = FashionSpecialConfigCategory.Instance.Get(sp.Key);
                    fashion += sp.Value.Data * fashionConfig.Fee;
                }
            }
            paramDict.Add("fashion", fashion + "");

            long halidom = user.HalidomData.Select(m => m.Value.Data).Sum();
            paramDict.Add("halidom", halidom + "");

            long infiniteMax = user.GetAchievementProgeress(AchievementSourceType.Infinite);
            paramDict.Add("infinite", infiniteMax + "");

            long legacy = user.GetAchievementProgeress(AchievementSourceType.Legacy);
            legacy += user.GetTicketCount(ItemHelper.SpecialId_Legacy_Ticket) + user.LegacyTikerCount.Data;
            paramDict.Add("legacy", legacy + "");

            long metalTotal = user.MetalData.Select(m => m.Value.Data).Sum();
            paramDict.Add("metal", metalTotal + "");

            long pill = user.PillData.Data;
            paramDict.Add("pill", pill + "");

            long pill2 = user.PillData2.Data;
            paramDict.Add("pill2", pill2 + "");

            long pill3 = user.PillData3.Data;
            paramDict.Add("pill3", pill3 + "");

            long pet = 0;
            long petRed = 0;
            long petDard = 0;
            List<BoxItem> pets = user.Bags.Where(m => m.Item.Type == ItemType.Pet).ToList();
            foreach (var sp in pets)
            {
                Pet p = sp.Item as Pet;
                if (p.GetQuality() == 6)
                {
                    petRed += PetConfigCategory.Instance.GetPetTotalFee(p.PetLayer.Data) + 1;
                }
                else if (p.GetQuality() == 7)
                {
                    pet += PetConfigCategory.Instance.GetPetTotalFee(p.PetLayer.Data) + 1;
                }
            }
            foreach (var p in user.PetList)
            {
                if (p.GetQuality() == 6)
                {
                    petRed += PetConfigCategory.Instance.GetPetTotalFee(p.PetLayer.Data) + 1;
                }
                else if (p.GetQuality() == 7)
                {
                    pet += PetConfigCategory.Instance.GetPetTotalFee(p.PetLayer.Data) + 1;
                }
            }

            foreach (var sp in user.PetSpeicalLayerData)
            {
                long layer = sp.Value.Data;
                if (layer > 0)
                {
                    pet += 10;
                }
                petDard += PetConfigCategory.Instance.GetPetTotalFee(layer);
            }

            pet += GetTotal(user.Bags, ItemHelper.Specail_Pet_Layer[2]);
            petRed += GetTotal(user.Bags, ItemHelper.Specail_Pet_Layer[1]);
            petDard += GetTotal(user.Bags, ItemHelper.Specail_Pet_Speical);

            pet += GetTotal(user.Bags, 207, 210);
            petRed += GetTotal(user.Bags, 204);

            paramDict.Add("pet", pet + "");
            paramDict.Add("petRed", petRed + "");
            paramDict.Add("petDark", petDard + "");

            long refineTotal = user.MagicEquipRefine.Select(m => m.Value.Data).Sum();
            paramDict.Add("refine", refineTotal + "");

            long reformTotal = user.MagicEquipReform.Select(m => m.Value.Data).Sum();
            paramDict.Add("reform", reformTotal + "");

            long ringTotal = user.RingData.Where(m => m.Key <= 6).Select(m => m.Value.Data).Sum();
            ringTotal += GetTotal(user.Bags, 190001, 190006);
            ringTotal += GetTotal(user.Bags, 22);
            paramDict.Add("ring", ringTotal + "");

            long ring1Total = user.RingData.Where(m => m.Key >= 7).Select(m => m.Value.Data).Sum();
            ring1Total += GetTotal(user.Bags, 190007, 190012);
            ring1Total += GetTotal(user.Bags, 44);
            paramDict.Add("ring1", ring1Total + "");

            long relic = 0;
            long relic1 = 0;

            foreach (var sp in user.RelicData)
            {
                int fee = RelicConfigCategory.Instance.GetTotalFee(sp.Value.Data);
                relic += fee;
                if (sp.Key > 32)
                {
                    relic1 += fee;
                }
            }
            relic += GetTotal(user.Bags, 61000001, 61000040);
            relic1 += GetTotal(user.Bags, 61000033, 61000040);

            relic += GetTotal(user.Bags, 35);
            relic += GetTotal(user.Bags, 37, 41);
            relic1 += GetTotal(user.Bags, 40, 41);

            paramDict.Add("relic", relic + "");
            paramDict.Add("relic1", relic1 + "");
            //user.SaveRecordMax((int)AbcType.Relic, relic);


            long stone = user.StoneData.Select(m => m.Value.GetTotalLevel()).Sum();
            paramDict.Add("stone", stone + "");
            //user.SaveRecordMax((int)AbcType.Stone, stone);

            paramDict.Add("swing", user.WingData.Data + "");

            long strongTotal = user.MagicEquipStrength.Select(m => m.Value.Data).Sum();
            paramDict.Add("strong", strongTotal + "");

            long spirit = user.SpiritRecord.Select(m => m.Value.Level.Data).Sum();
            paramDict.Add("spirit", spirit + "");

            long talent = user.TalentExp.Data / 10000;
            paramDict.Add("talent", talent + "");
            //user.SaveRecordMax((int)AbcType.Talent, talent);

            long sx = user.ShengxiaoList.Where(m => m.Value.GetQuality() >= 9).Count();
            paramDict.Add("shengxiao", sx + "");

            skill11 += GetTotal(user.Bags, 1011);
            skill11 += GetTotal(user.Bags, 2011);
            skill11 += GetTotal(user.Bags, 3011);
            skill11 += GetTotal(user.Bags, 29);
            paramDict.Add("skill11", skill11 + "");

            skill12 += GetTotal(user.Bags, 1012);
            skill12 += GetTotal(user.Bags, 2012);
            skill12 += GetTotal(user.Bags, 3012);
            skill12 += GetTotal(user.Bags, 42);
            paramDict.Add("skill12", skill12 + "");

            long minVersion = user.VersionLog.Select(m => m.Key).Min();
            paramDict.Add("minVersion", minVersion + "");

            paramDict.Add("versionCount", user.VersionLog.Count + "");

            paramDict.Add("channel", ConfigHelper.Channel + "");

            if (user.First_Create_Time > 0)
            {
                string createTime = TimeHelper.SecondsToDate(user.First_Create_Time).ToString("yyyy-MM-dd");
                paramDict.Add("accountTime", createTime + "");
            }

            string param = JsonConvert.SerializeObject(paramDict);

            return param;
        }

        private static long GetTotal(List<BoxItem> Bags, int ConfigId)
        {
            long count = Bags.Where(m => m.Item.ConfigId == ConfigId).Select(m => m.MagicNubmer.Data).Sum();
            return count;
        }
        private static long GetTotal(List<BoxItem> Bags, int StartId, int EndId)
        {
            long count = Bags.Where(m => m.Item.ConfigId >= StartId && m.Item.ConfigId <= EndId).Select(m => m.MagicNubmer.Data).Sum();
            return count;
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

        public static IEnumerator CreateAccountNew(byte[] bytes, Dictionary<string, string> headers, Action<WebResultWrapper> successAction, Action failAction)
        {
            return SendRequest("create_user_new", bytes, headers, successAction, failAction);
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

        public static IEnumerator GetSerial(Action<WebResultWrapper> successAction, Action failAction)
        {
            return SendRequest("get_serial", null, successAction, failAction);
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
                    User user = GameProcessor.Inst.User;
                    string account = user.Account;
                    string deviceId = AppHelper.GetDeviceIdentifier();
                    string fileId = user.DeviceId;
                    string level = user.MagicLevel.Data + "";
                    string sign = BuildSign();

                    request.SetRequestHeader("account", account);
                    request.SetRequestHeader("deviceId", deviceId);
                    request.SetRequestHeader("fileId", fileId);
                    request.SetRequestHeader("level", level);
                    request.SetRequestHeader("channel", ConfigHelper.Channel + "");
                    request.SetRequestHeader("version", ConfigHelper.Version + "");
                    request.SetRequestHeader("sign", BuildSign());
                    request.SetRequestHeader("code", BuildCode());
                    if (action == "save_user_file")
                    {
                        Debug.Log("Start Serial:" + user.Serial);

                        int serial = user.Serial + 1;
                        request.SetRequestHeader("SaveCount", serial + "");
                    }

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
                        //Debug.Log("Upload complete! Server response: " + request.downloadHandler.text);

                        WebResultWrapper result = JsonConvert.DeserializeObject<WebResultWrapper>(request.downloadHandler.text);

                        if (result.Code == StatusMessage.BlackList)
                        {
                            GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
                        }
                        else if (result.Code == StatusMessage.OldFile)
                        {
                            GameProcessor.Inst.EventCenter.Raise(new NewVersionEvent() { Type = 1 });
                        }
                        else if (result.Version > ConfigHelper.Version)
                        {
                            GameProcessor.Inst.EventCenter.Raise(new NewVersionEvent() { Type = 2, Version = result.Version });
                        }

                        if (action == "save_user_file")
                        {
                            user.Serial++;
                            UserData.Save();

                            Debug.Log("End Serial:" + user.Serial);
                        }

                        successAction?.Invoke(result);
                    }
                }
            }
        }
    }
}

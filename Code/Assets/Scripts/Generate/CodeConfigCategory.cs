using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class CodeConfigCategory
    {
        public CodeConfig GetSpeicalConfig(string code)
        {
            string skey = AppHelper.getKey();
            string account = EncryptionHelper.Md5(GameProcessor.Inst.User.DeviceId);

            string realCode = EncryptionHelper.AesDecrypt(code, account);

            realCode = EncryptionHelper.AesDecrypt(realCode, skey);

            CodeConfig config = CodeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.code == realCode).FirstOrDefault();

            return config;
        }

        public string BuildSpecicalCode(string baseCode,string account)
        {
            string skey = AppHelper.getKey();
            account = EncryptionHelper.Md5(account);

            string code = EncryptionHelper.AesEncrypt(baseCode, skey);

            code = EncryptionHelper.AesEncrypt(code, account);

            return code;
        }
    }


}
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
            string account = GameProcessor.Inst.User.DeviceId;

            string realCode = EncryptionHelper.AesDecrypt(code, account);

            realCode = EncryptionHelper.AesDecrypt(realCode, skey);

            CodeConfig config = CodeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.code == realCode).FirstOrDefault();

            return config;
        }
    }


}
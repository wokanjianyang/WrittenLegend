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

            string realCode = EncryptionHelper.AesDecrypt(code, UserData.tapAccount);

            realCode = EncryptionHelper.AesDecrypt(realCode, skey);

            CodeConfig config = CodeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.code == realCode).FirstOrDefault();

            return config;
        }
    }


}
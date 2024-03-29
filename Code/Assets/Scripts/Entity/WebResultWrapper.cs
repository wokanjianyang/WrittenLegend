using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{

    public class WebResultWrapper
    {
        public static int WebCode_OK = 200;

        public int Code { get; set; }

        public string Msg { get; set; }

        public int Version { get; set; }

        public JObject Data { get; set; }
    }


}

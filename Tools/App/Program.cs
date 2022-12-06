using System;
using System.IO;
using System.Threading;
using CommandLine;
using NLog;
using System.Reflection;

namespace Game
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Log.Error(e.ExceptionObject.ToString());
            };

            try
            {
                TestConfig config = new TestConfig();
                TestConfigCategory.Instance = new TestConfigCategory();

                // 命令行参数
                //Parser.Default.ParseArguments<Options>(args)
                //        .WithNotParsed(error => throw new Exception($"命令行格式错误!"))
                //        .WithParsed(o => { options = o; });

                //Options.Instance = options;

                //否则动态编译没有DLL
                ProtobufHelper.Init();
                initProtoBuf();
                MongoRegister.Init();
                Log.ILog = new NLogger("Tool");
                Log.Info($"ExcelExporter start........................");

                Console.WriteLine("ExcelExporter start........................");

                ExcelExporter.Export();
            }
            catch (Exception e)
            {
                Log.Console(e.ToString());
            }
            return 1;
        }

        public static void initProtoBuf() {
            var person = new Person
            {
                Id = 12345,
                Name = "Fred",
                Address = new Address
                {
                    Line1 = "Flat 1",
                    Line2 = "The Meadows"
                }
            };

            string str = person.SerializeToString_PB();
            var strPerson = str.DeserializeFromString_PB<Person>();
            //Console.WriteLine("序列化结果（字符串）：" + str);

            //var arr = person.SerializeToByteAry_PB();
            //var arrPerson = arr.DeserializeFromByteAry_PB<Person>();
            //Console.WriteLine("序列化结果（字节数组）：" + BitConverter.ToString(arr));

            //string path = "person.bin";
            //person.SerializeToFile_PB(path);
            //var pathPerson = path.DeserializeFromFile_PB<Person>();
            //Console.WriteLine("序列化结果（二进制文件）：" + BitConverter.ToString(File.ReadAllBytes(path)));
        }
    }

}
using System;
using System.Net;
using System.Net.Sockets;

namespace Game
{
    public static class TimeHelper
    {
        private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
        /// <summary>
        /// 客户端时间
        /// </summary>
        /// <returns></returns>
        public static long ClientNow()
        {
            return (DateTime.UtcNow.Ticks - epoch) / 10000;
        }

        public static long ClientNowSeconds()
        {
            return (DateTime.UtcNow.Ticks - epoch) / 10000000;
        }

        public static int TodaySeed()
        {
            return (int)((DateTime.Today.Ticks - epoch) / 10000000);
        }

        public static int WeekSeed()
        {
            DateTime week = DateTime.Today.AddDays(1 - Convert.ToInt32(DateTime.Today.DayOfWeek.ToString("d")));
            return (int)((week.Ticks - epoch) / 10000000);
        }

        public static long Now()
        {
            return ClientNow();
        }

        public static DateTime SecondsToDate(long second)
        {
            return new DateTime(epoch).AddSeconds(second);
        }

        public static ulong GetNetworkTime()
        {
            try
            {
                // 从 NTP Pool Project 获取网络时间
                const string ntpServer = "www.baidu.com";
                var ntpData = new byte[48]; // 创建一个 48 字节大小的字节数组来存储 NTP 数据
                ntpData[0] = 0x1B; // 将 NTP 数据的第一个字节设置为 0x1B，这是 NTP 协议的请求数据格式

                var addresses = Dns.GetHostEntry(ntpServer).AddressList; // 获取 NTP 服务器的 IP 地址列表
                var ipEndPoint = new IPEndPoint(addresses[0], 80); // 创建用于连接的 IP 端点，使用第一个 IP 地址和 NTP 服务器的端口 123
                var socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); // 创建套接字，使用 IPv4 地址族、数据报套接字类型和 UDP 协议类型
                socket.Connect(ipEndPoint); // 连接到 NTP 服务器
                socket.Send(ntpData); // 发送 NTP 数据
                socket.Receive(ntpData); // 接收 NTP 响应数据
                socket.Close(); // 关闭套接字连接

                const byte serverReplyTime = 40; // 服务器响应时间在 NTP 数据中的偏移量
                ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime); // 从 NTP 数据中获取无符号 32 位整数部分
                ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4); // 从 NTP 数据中获取无符号 32 位小数部分
                                                                                       // 交换整数部分和小数部分的字节顺序，以适应本地字节顺序
                intPart = SwapEndianness(intPart);
                fractPart = SwapEndianness(fractPart);

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L); // 将整数部分和小数部分转换为毫秒数
                                                                                           // var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long) milliseconds); // 根据毫秒数计算网络时间（从 1900 年 1 月 1 日开始计算）
                return milliseconds;
            }
            catch (Exception e)
            {
                Log.Warning("获取网络时间失败: " + e.Message);
                return 0;
            }
        }

        // 交换字节顺序，将大端序转换为小端序或反之
        private static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                           ((x & 0x0000ff00) << 8) +
                           ((x & 0x00ff0000) >> 8) +
                           ((x & 0xff000000) >> 24));
        }

    }
}
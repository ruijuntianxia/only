using Newtonsoft.Json.Linq;
using only.Identity.Client;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;

namespace ConsoleApp1
{
    public class Program
    {

        private static readonly JObject MessageObject = new JObject {

            new JProperty("ORA-00076"," 未找到转储"),
            new JProperty("ORA-00077"," 转储 无效"),
            new JProperty("ORA-00078"," 无法按名称转储变量"),
            new JProperty("ORA-00079"," 未找到变量"),
            new JProperty("ORA-00080"," 层次 指定的全局区域无效"),
            new JProperty("ORA-00081"," 地址范围 [，) 不可读"),
            new JProperty("ORA-00082"," 的内存大小不在有效集合 [1], [2], [4] 之内"),
            new JProperty("ORA-00083"," 警告 可能损坏映射的 SGA"),
            new JProperty("ORA-00084"," 全局区域必须为 PGA, SGA 或 UGA"),
            new JProperty("ORA-00085"," 当前调用不存在"),
            new JProperty("ORA-00086"," 用户调用不存在"),
            new JProperty("ORA-00087"," 命令无法在远程例程上执行"),
            new JProperty("ORA-00088"," 共享服务器无法执行命令"),
            new JProperty("ORA-00089"," ORADEBUG 命令中无效的例程号"),
            new JProperty("ORA-00090"," 未能将内存分配给群集数据库 ORADEBUG 命令"),
            new JProperty("ORA-00091"," LARGE_POOL_SIZE 至少必须为"),
            new JProperty("ORA-00092"," LARGE_POOL_SIZE 必须大于 LARGE_POOL_MIN_ALLOC"),
            new JProperty("ORA-00093"," 必须介于 和 之间"),
            new JProperty("ORA-00094"," 要求整数值"),
            new JProperty("ORA-00096"," 值 对参数 无效，它必须来自 之间"),
            new JProperty("ORA-00097"," 使用 Oracle SQL 特性不在 SQL92 级中"),
            new JProperty("ORA-00099"," 等待资源时发生超时，可能是 PDML 死锁所致"),
            new JProperty("ORA-00100"," 未找到数据"),
            new JProperty("ORA-00101"," 系统参数 DISPATCHERS 的说明无效"),
            new JProperty("ORA-00102"," 调度程序无法使用网络协议"),
            new JProperty("ORA-00103"," 无效的网络协议；供调度程序备用"),
            new JProperty("ORA-00104"," 检测到死锁；全部公用服务器已锁定等待资源"),
            new JProperty("ORA-00105"," 未配置网络协议 的调度机制"),
            new JProperty("ORA-00106"," 无法在连接到调度程序时启动/关闭数据库"),
            new JProperty("ORA-00107"," 无法连接到 ORACLE 监听器进程"),
            new JProperty("ORA-00108"," 无法设置调度程序以同步进行连接"),
            new JProperty("ORA-00111"," 由于服务器数目限制在 , 所以没有启动所有服务器"),
            new JProperty("ORA-00112"," 仅能创建多达 (最多指定) 个调度程序"),
            new JProperty("ORA-00113"," 协议名 过长"),
            new JProperty("ORA-00114"," 缺少系统参数 SERVICE_NAMES 的值"),
            new JProperty("ORA-00115"," 连接被拒绝；调度程序连接表已满"),
            new JProperty("ORA-00116"," SERVICE_NAMES 名过长"),
            new JProperty("ORA-00117"," 系统参数 SERVICE_NAMES 的值超出范围"),
            new JProperty("ORA-00118"," 系统参数 DISPATCHERS 的值超出范围"),
            new JProperty("ORA-00119"," 系统参数 的说明无效"),
            new JProperty("ORA-00120"," 未启用或安装调度机制"),
            new JProperty("ORA-00121"," 在缺少 DISPATCHERS 的情况下指定了 SHARED_SERVERS"),
            new JProperty("ORA-00122"," 无法初始化网络配置"),
            new JProperty("ORA-00123"," 空闲公用服务器终止"),
            new JProperty("ORA-00124"," 在缺少 MAX_SHARED_SERVERS 的情况下指定了 DISPATCHERS"),
            new JProperty("ORA-00125"," 连接被拒绝；无效的演示文稿"),
            new JProperty("ORA-00126"," 连接被拒绝；无效的重复"),
            new JProperty("ORA-00127"," 调度进程 不存在"),
            new JProperty("ORA-00128"," 此命令需要调度进程名"),
            new JProperty("ORA-00129"," 监听程序地址验证失败 ''"),
            new JProperty("ORA-00130"," 监听程序地址 '' 无效"),
            new JProperty("ORA-00131"," 网络协议不支持注册 ''"),
            new JProperty("ORA-00132"," 语法错误或无法解析的网络名称 ''"),
            new JProperty("ORA-00150"," 重复的事务处理 ID"),
            new JProperty("ORA-00151"," 无效的事务处理 ID"),
            new JProperty("ORA-00152"," 当前会话与请求的会话不匹配"),
            new JProperty("ORA-00153"," XA 库中的内部错误"),
            new JProperty("ORA-00154"," 事务处理监视器中的协议错误"),
            new JProperty("ORA-00155"," 无法在全局事务处理之外执行工作"),
            new JProperty("ORA-00160"," 全局事务处理长度 超出了最大值 ()"),
            new JProperty("ORA-00161"," 事务处理的分支长度 非法 (允许的最大长度为 )"),
            new JProperty("ORA-00162"," 外部 dbid 的长度 超出了最大值 ()"),
            new JProperty("ORA-00163"," 内部数据库名长度 超出了最大值 ()"),
            new JProperty("ORA-00164"," 在分布式事务处理中不允许独立的事务处理"),
            new JProperty("ORA-00165"," 不允许对远程操作进行可移植分布式自治转换"),
            new JProperty("ORA-00200"," 无法创建控制文件"),
            new JProperty("ORA-00201"," 控制文件版本 与 ORACLE 版本 不兼容")
        };
        private static string message;
        public static void Main(string[] args)
        {




            Regex reg = new Regex("ORA-[0-9]{5}");

            Match match = reg.Match("ORA-00076 fdsfafdsf");

           string  header = match.Groups[0].Value;
            JToken dd= MessageObject[header];
            string  message = MessageObject[header].ToString();



            bool ddf=  IsIllegalCharacter("fjds$kfjsdlf");
            string[] sTemp = new string[] { "9008" };
            //通过ParameterizedThreadStart创建线程
            Thread thread = new System.Threading.Thread(GetCode)
            {
                IsBackground = true
            };
            thread.Start(sTemp);

            Console.WriteLine(message);

           

            //int num = 0;
            //ArrayList List = new ArrayList();
            //while (List.Count < 10000)
            //{
            //    num += 1;
            //    if (!List.Contains(num))
            //        Console.WriteLine("list:"+num);
            //        List.Add(num);

            //}
            //foreach (var item in List)
            //{
            //    Console.WriteLine(item);
            //}



            //Singleton ddd= Singleton.GetInstance();

            //var list1 = new LinkedList<int>();
            //list1.AddList(2);
            //list1.AddList(3);
            //list1.AddList(6);
            //foreach (int i in list1)
            //{
            //    Console.WriteLine(i);
            //}
            //Console.WriteLine("Hello World!");
        }

        public static void process(string str) {
            str = "A";
        }
        public static void process(StringBuffer sb) {
            sb = new StringBuffer();
            sb.append("A");
        }


        /// <summary>
        /// 验证非法字符
        /// </summary>        
        public static bool IsIllegalCharacter(string txt)
        {
            Regex objReg = new Regex("[?!@#$%\\^&*()]+");
            return objReg.IsMatch(txt);
        }
        static void GetCode(object oCode)
        {
            string[] oTemp = (string[])oCode;
            try
            {
                message = "fdsfs";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


        }
    }
}

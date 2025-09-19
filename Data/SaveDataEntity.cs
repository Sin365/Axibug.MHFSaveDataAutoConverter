using Axibug.MHFSaveAutoConverter.SQL;
using System.Data;
using static Axibug.MHFSaveAutoConverter.DataStruct.DataStruct;
using static Axibug.MHFSaveAutoConverter.DataStruct.MHFSaveDataCfg;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Axibug.MHFSaveAutoConverter.DataStruct
{
    public class SaveDataEntity
    {
        public MHFVer FromVer;
        public MHFVer TargetVer;
        public List<s_Base> saveHandles = new List<s_Base>();
        public SaveDataEntity(MHFVer from, MHFVer target, byte[] data)
        {
            FromVer = from;
            TargetVer = target;

            string[] nameArr = dictTypeWithCfg[from].Keys.ToArray();
            foreach (string className in nameArr)
            {
                string cName = typeof(DataStruct).FullName + "+" + className;
                try
                {
                    Type type = Type.GetType(cName);
                    if (type == null)
                    {
                        Console.WriteLine($"类型 {className} 未找到");
                        continue;
                    }
                    s_Base instance = (s_Base)Activator.CreateInstance(type);
                    saveHandles.Add(instance);
                    instance.Load(from, data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"处理 {className} 时出错: {ex.Message}");
                }
            }
            Console.WriteLine("====读取====");

            foreach (var singledata in saveHandles)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(singledata.GetType().Name);
                Console.ForegroundColor = ConsoleColor.White;
                string str = singledata.ToString();
                if (str.Length > 100)
                    str = str.Substring(0, 100) + "...";
                Console.WriteLine(str);
            }
        }
        public byte[] DoConvert()
        {
            Console.WriteLine("====读取模板数据====");
            byte[] data = File.ReadAllBytes("./savetemplete.bin");

            Console.WriteLine("====尝试开始写入====");
            foreach (var singledata in saveHandles)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(singledata.GetType().Name);
                Console.ForegroundColor = ConsoleColor.White;
                string str = singledata.ToString();
                if (str.Length > 100)
                    str = str.Substring(0, 100) + "...";

                bool ret = singledata.Write(TargetVer, data);
                Console.WriteLine($"写入:{singledata.GetType().Name} =>{(ret ? "成功" : "失败")}");
                Console.WriteLine(str);
            }
            Console.WriteLine("====写入完毕====");
            return data;
        }



        public byte[] FixedEquipBox(byte[] srcdata, out string log)
        {
            log = null;
            byte[] targetdata = srcdata.ToArray();

            Console.WriteLine("====尝试开始写入====");
            foreach (var singledata in saveHandles)
            {
                if (!(singledata is s_Itembox itembox))
                    continue;

                itembox.FixedData(out log);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(singledata.GetType().Name);
                Console.ForegroundColor = ConsoleColor.White;
                string str = singledata.ToString();
                if (str.Length > 100)
                    str = str.Substring(0, 100) + "...";

                bool ret = singledata.Write(TargetVer, targetdata);
                Console.WriteLine($"写入:{singledata.GetType().Name} =>{(ret ? "成功" : "失败")}");
                Console.WriteLine(str);
            }
            Console.WriteLine("====写入完毕====");
            return targetdata;
        }
    }

}

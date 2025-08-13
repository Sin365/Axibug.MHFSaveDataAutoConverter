using Axibug.MHFSaveAutoConverter.Data;
using Axibug.MHFSaveAutoConverter.SQL;
using HaoYue.MHFUserSrv.Server.Common;
using Npgsql;
using NpgsqlTypes;
using System.Text;
using static Axibug.MHFSaveAutoConverter.DataStruct.MHFSaveDataCfg;

namespace Axibug.MHFSaveAutoConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.Title = "皓月云MHF存档迁移工具 ver 0.1.0";
            Console.WriteLine("读取配置");
            try
            {
                Config.LoadCfg("cfg.ini");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"配置读取失败，{ex.ToString()}");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("配置读取成功");
            while (true)
            {
                Console.WriteLine($"==欢迎使用 {Console.Title} Axibug.MHFSaveAutoConverter==");
                string[] verlist = Enum.GetNames(typeof(MHFVer));
                
                MHFVer src;
                MHFVer target;
                while (true)
                {
                    Console.WriteLine("Step1.选择要继承的角色的原始MHF版本: Select the original MHF version of the character you want to inherit:");
                    for (int i = 0; i < verlist.Length; i++)
                        Console.WriteLine($"[{i}]{verlist[i]}");

                    int srcidx;
                    string ver = Console.ReadLine();
                    if (!int.TryParse(ver, out srcidx))
                        continue;
                    if (srcidx >= 0 && srcidx < verlist.Length)
                    {
                        src = (MHFVer)Enum.Parse(typeof(MHFVer), verlist[srcidx]);
                        break;
                    }
                }
                while (true)
                {
                    Console.WriteLine("Step2.请选择目标MHF版本: Please select the target MHF version:");
                    for (int i = 0; i < verlist.Length; i++)
                        Console.WriteLine($"[{i}]{verlist[i]}");

                    int idx;
                    string ver = Console.ReadLine();
                    if (!int.TryParse(ver, out idx))
                        continue;
                    if (idx >= 0 && idx < verlist.Length)
                    {
                        target = (MHFVer)Enum.Parse(typeof(MHFVer), verlist[idx]);
                        break;
                    }
                }

                Console.WriteLine($"step3.请输入[{src}]版本中的源Characters表中的角色ID: Please enter the character ID from the source Characters table in [{src}] version:");
                if (!long.TryParse(Console.ReadLine(), out long cid))
                {
                    Console.WriteLine("输入有误");
                    continue;
                }
                Console.WriteLine("===>操作的角色ID:" + cid);
                if (!SaveDataCoverter.loadCharacterOLD(cid, out string name,out bool is_female, out byte[] srcdata))
                {
                    Console.WriteLine("读取失败");
                    continue;
                }
                Console.WriteLine($"[{src}]角色{cid}:[{name}]数据加载完毕");

                Console.WriteLine($"step4.是否升级存档到{target}(y),或仅Dump来自的{src}存档(n)？");
                Console.WriteLine($"step4.Do you want to upgrade the save file to {target}(y), or just dump the {src} save file(n)?");
                bool bconvert = false;
                string tempinput = Console.ReadLine().ToLower();
                if (tempinput == "y")
                {
                    bconvert = true;
                }
                else if (tempinput != "n")
                {
                    Console.WriteLine("输入有误");
                    continue;
                }

                if (!SaveDataCoverter.ConvertSaveData(src, target, cid, name, !bconvert, srcdata, out byte[] targetdata, out string err))
                {
                    Console.WriteLine($"处理失败:{err}");
                    continue;
                }

                if (bconvert == false)
                    continue;

                Console.WriteLine($"step 5.[可选]将升级数据导入到目标数据库。若回车则取消。输入目标userid,则在目标版本数据库下Characters表插入数据据，并关联您的userid");
                Console.WriteLine($"Step 5. [Optional] Import the upgrade data into the target database. Press Enter to cancel. Enter the target userid, and the data will be inserted into the Characters table in the target version database, associated with your userid");
                if (!long.TryParse(Console.ReadLine(), out long targetuserid))
                {
                    Console.WriteLine("输入有误");
                    continue;
                }

                if (!InsertTargetDB(targetuserid, is_female, name, targetdata))
                {
                    Console.WriteLine($"写入目标{target}数据库失败");
                    continue;
                }
                Console.WriteLine($"写入目标{target}数据库成功!");

                Console.WriteLine("======继续？=====");
            }
        }


        public static bool InsertTargetDB(long uid,bool is_female,string name, byte[] targetdata)
        {
            string str = "INSERT INTO \"public\".\"characters\" (\"user_id\", \"is_female\", \"is_new_character\", \"name\", \"unk_desc_string\", \"gr\", \"hr\", \"weapon_type\", \"last_login\", \"savedata\", \"decomyset\", \"hunternavi\", \"otomoairou\", \"partner\", \"platebox\", \"platedata\", \"platemyset\", \"rengokudata\", \"savemercenary\", \"restrict_guild_scout\", \"minidata\", \"gacha_items\", \"daily_time\", \"house_info\", \"login_boost\", \"skin_hist\", \"kouryou_point\", \"gcp\", \"guild_post_checked\", \"time_played\", \"weapon_id\", \"scenariodata\", \"savefavoritequest\", \"friends\", \"blocked\", \"deleted\", \"cafe_time\", \"netcafe_points\", \"boost_time\", \"cafe_reset\", \"bonus_quests\", \"daily_quests\", \"promo_points\", \"rasta_id\", \"pact_id\", \"stampcard\", \"mezfes\") " +
                $"VALUES ({uid}, '{(is_female?"t":"f")}', 'f', '{name}', '', 0, 0, 0, 1750087006, @savedata, NULL, NULL, NULL, E'\\\\001cmp 20110113   \\\\000\\\\000\\\\002\\\\001\\\\250\\\\000\\\\377\\\\000\\\\255'::bytea, NULL, NULL, NULL, NULL, NULL, 'f', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2025-05-26 11:24:26.725902+08', 0, 0, E'\\\\000\\\\000\\\\000\\\\000\\\\000\\\\000\\\\000\\\\000\\\\000\\\\000'::bytea, E'\\\\001\\\\000\\\\001\\\\000\\\\001\\\\000\\\\001\\\\000\\\\001\\\\000\\\\000\\\\000\\\\000\\\\000\\\\000'::bytea, '', '', 'f', 0, 0, NULL, NULL, 0, 0, 0, NULL, NULL, 0, NULL);";
            var savedataparam = new NpgsqlParameter("@savedata", NpgsqlDbType.Bytea);
            savedataparam.Value = targetdata;
            return SQLRUN_TARGET_DB.ExcuteSQL(str,new List<NpgsqlParameter> { savedataparam });
        }

        //public static SaveDataEntity SetData(long cid, string name, byte[] srcdata)
        //{
        //    string path = $"savedata_cid_{cid}_{DateTime.Now.ToString("yyyyMMddHHmmss")}" + "_解密.bin";
        //    byte[] decdata = MHFCompression.Decompress(srcdata);

        //    SaveDataEntity se = new SaveDataEntity(MHFVer.FW5, MHFVer.GG, decdata);
        //    //SaveDataEntity se = new SaveDataEntity(MHFVer.GG, MHFVer.GG, decdata);
        //    System.IO.File.WriteAllBytes(path, decdata);
        //    Console.WriteLine($"角色数据{name},已保存数据到{path}");

        //    Console.WriteLine($"是否升级存档到MHFGG？");
        //    if (Console.ReadLine().ToLower() == "y")
        //    {
        //        string updateoutpath = $"升级GG_savedata_cid_{cid}_{DateTime.Now.ToString("yyyyMMddHHmmss")}" + "_已修改.bin";
        //        byte[] updatedata = se.DoConvert();
        //        System.IO.File.WriteAllBytes(updateoutpath, updatedata);

        //        //Console.WriteLine($"是否验证数据是否正确？");
        //        //if (Console.ReadLine().ToLower() == "y")
        //        //{
        //        //    new SaveDataEntity(MHFVer.GG, MHFVer.GG, updatedata);
        //        //}
        //    }
        //}
    }

}

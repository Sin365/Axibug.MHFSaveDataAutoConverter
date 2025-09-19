using Axibug.MHFSaveAutoConverter.DataStruct;
using Axibug.MHFSaveAutoConverter.SQL;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Text;
using static Axibug.MHFSaveAutoConverter.DataStruct.MHFSaveDataCfg;

namespace Axibug.MHFSaveAutoConverter.Data
{
    public class SaveDataCoverter
    {

        public static bool InsertTargetDB_new(long uid, bool is_female, string name, byte[] targetdata)
        {
            string str = "INSERT INTO \"public\".\"characters\" (\"user_id\", \"is_female\", \"is_new_character\", \"name\", \"unk_desc_string\", \"gr\", \"hr\", \"weapon_type\", \"last_login\", \"savedata\", \"decomyset\", \"hunternavi\", \"otomoairou\", \"partner\", \"platebox\", \"platedata\", \"platemyset\", \"rengokudata\", \"savemercenary\", \"restrict_guild_scout\", \"minidata\", \"gacha_items\", \"daily_time\", \"house_info\", \"login_boost\", \"skin_hist\", \"kouryou_point\", \"gcp\", \"guild_post_checked\", \"time_played\", \"weapon_id\", \"scenariodata\", \"savefavoritequest\", \"friends\", \"blocked\", \"deleted\", \"cafe_time\", \"netcafe_points\", \"boost_time\", \"cafe_reset\", \"bonus_quests\", \"daily_quests\", \"promo_points\", \"rasta_id\", \"pact_id\", \"stampcard\", \"mezfes\") " +
                $"VALUES ({uid}, '{(is_female ? "t" : "f")}', 'f', '{name}', '', 0, 0, 0, 1750087006, @savedata, NULL, NULL, NULL, E'\\\\001cmp 20110113   \\\\000\\\\000\\\\002\\\\001\\\\250\\\\000\\\\377\\\\000\\\\255'::bytea, NULL, NULL, NULL, NULL, NULL, 'f', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2025-05-26 11:24:26.725902+08', 0, 0, E'\\\\000\\\\000\\\\000\\\\000\\\\000\\\\000\\\\000\\\\000\\\\000\\\\000'::bytea, E'\\\\001\\\\000\\\\001\\\\000\\\\001\\\\000\\\\001\\\\000\\\\001\\\\000\\\\000\\\\000\\\\000\\\\000\\\\000'::bytea, '', '', 'f', 0, 0, NULL, NULL, 0, 0, 0, NULL, NULL, 0, NULL);";
            var savedataparam = new NpgsqlParameter("@savedata", NpgsqlDbType.Bytea);
            savedataparam.Value = targetdata;
            return SQLRUN_TARGET_DB.ExcuteSQL(str, new List<NpgsqlParameter> { savedataparam });
        }


        public static bool UpdateTargetDB_old(long cid, byte[] targetdata)
        {
            string str = $"UPDATE \"characters\" set savedata =  @savedata where \"id\" = {cid};";
            var savedataparam = new NpgsqlParameter("@savedata", NpgsqlDbType.Bytea);
            savedataparam.Value = targetdata;
            return SQLRUN_SRC_DB.ExcuteSQL(str, new List<NpgsqlParameter> { savedataparam });
        }
        public static bool loadCharacterOLD(long cid, out string name, out bool is_female, out byte[] data)
        {
            string sql = $"SELECT \"name\",savedata,is_female from \"characters\" WHERE id = {cid}";

            data = null;
            name = default;
            is_female = default;
            if (SQLRUN_SRC_DB.QuerySQL(sql, out DataTable dt))
            {
                name = string.Empty;
                DataRowCollection RowsData = dt.Rows;
                if (RowsData.Count > 0)
                {
                    name = (RowsData[0][0]).ToString();
                    data = (byte[])(RowsData[0][1]);
                    is_female = Convert.ToBoolean(RowsData[0][2]);
                    return true;
                }
                else
                    Console.WriteLine($"未查询到数据");
            }
            return false;
        }
        public static bool ConvertSaveData(MHFVer from, MHFVer target, long srccid, string name, bool onlydumpsrc, byte[] src, out byte[] targetdata, out string err)
        {
            try
            {
                string path = $"src_{from}_cid_{srccid}_{DateTime.Now.ToString("yyyyMMddHHmmss")}" + "_decrypt.bin";
                byte[] decdata = MHFCompression.Decompress(src);
                System.IO.File.WriteAllBytes(path, decdata);
                Console.WriteLine($"{from}_{srccid}_角色数据{name},已保存数据到:{path}");
                SaveDataEntity se = new SaveDataEntity(from, target, decdata);
                if (onlydumpsrc)
                {
                    err = default;
                    targetdata = default;
                    return true;
                }
                string updateoutpath = $"update_{target}_cid_{srccid}_{DateTime.Now.ToString("yyyyMMddHHmmss")}" + "_fixed.bin";
                targetdata = se.DoConvert();
                System.IO.File.WriteAllBytes(updateoutpath, targetdata);
                Console.WriteLine($"{target}_{srccid}_角色数据{name},已保存数据到:{updateoutpath}");
                err = default;
                return true;
            }
            catch (Exception ex)
            {
                err = ex.ToString();
                targetdata = default;
                return false;
            }
        }


        public static bool FixedSaveData(MHFVer from, MHFVer target, byte[] src, out byte[] targetdata, out string err)
        {
            try
            {
                byte[] decdata = MHFCompression.Decompress(src);
                SaveDataEntity se = new SaveDataEntity(from, target, decdata);
                targetdata = se.FixedEquipBox(decdata, out string log);
                err = default;
                Console.WriteLine(log);
                return true;
            }
            catch (Exception ex)
            {
                err = ex.ToString();
                targetdata = default;
                return false;
            }
        }
    }
}

using Axibug.MHFSaveAutoConverter.DataStruct;
using Axibug.MHFSaveAutoConverter.SQL;
using System.Data;
using System.Text;
using static Axibug.MHFSaveAutoConverter.DataStruct.MHFSaveDataCfg;

namespace Axibug.MHFSaveAutoConverter.Data
{
    public class SaveDataCoverter
    {
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
                if (onlydumpsrc)
                {
                    err = default;
                    targetdata = default;
                    return true;
                }
                SaveDataEntity se = new SaveDataEntity(from, target, decdata);
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
    }
}

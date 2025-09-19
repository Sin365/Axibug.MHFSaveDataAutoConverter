using Axibug.MHFSaveAutoConverter.Helper;
using System.Text;
using static Axibug.MHFSaveAutoConverter.DataStruct.DataStruct;
using static Axibug.MHFSaveAutoConverter.DataStruct.MHFSaveDataCfg;

namespace Axibug.MHFSaveAutoConverter.DataStruct
{

    public static class DataStructSet
    {
        public static bool GetCfg(this s_Base entity, MHFVer ver, out EntityCfg cfg)
        {
            if (dictTypeWithCfg.TryGetValue(ver, out var dict) && dict.TryGetValue(entity.GetType().Name, out cfg))
                return true;
            else
            {
                cfg = null;
                return false;
            }
        }
        public static T Create<T>(MHFVer form, byte[] src) where T : s_Base, new()
        {
            T obj = new T();
            obj.Load(form, src);
            return obj;
        }
    }
    public class EntityCfg
    {
        public int ptr = -1;
        public int len = -1;
        public int block_count = -1;
        public int block_single_len = -1;
    }
    public class DataStruct
    {
        #region 基类
        public abstract class s_Base
        {
            public MHFVer SrcVer;
            public EntityCfg SrcCfg;
            public bool SrcVerHad => SrcCfg.ptr > 0;
            public MHFVer TargetVer;
            public EntityCfg TargetCfg;
            public bool TargetHad => TargetCfg.ptr > 0;

            public virtual bool Load(MHFVer from, byte[] src)
            {
                SrcVer = from;
                this.GetCfg(from, out SrcCfg);
                return SrcVerHad;
            }

            public virtual bool Write(MHFVer target, byte[] targetdata)
            {
                if (!SrcVerHad)
                    return false;
                this.GetCfg(target, out TargetCfg);
                return TargetHad;
            }


            public virtual string ToString()
            {
                return this.ToString();
            }

            public virtual bool FixedData(out string log)
            {
                throw new NotImplementedException();
            }
        }

        public class s_base_Byte : s_Base
        {
            public byte data;
            public override bool Load(MHFVer from, byte[] src)
            {
                if (!base.Load(from, src))
                    return false;
                data = src[SrcCfg.ptr];
                return true;
            }
            public override bool Write(MHFVer target, byte[] targetdata)
            {
                if (!base.Write(target, targetdata)) return false;
                targetdata[TargetCfg.ptr] = data;
                return true;
            }
            public override string ToString()
            {
                return $"{this.GetType().Name}:{data}";
            }
            public static EntityCfg CreateCfg(int _ptr) { return new EntityCfg() { ptr = _ptr }; }
        }
        public class s_base_Bytesarray : s_Base
        {
            public byte[] data;
            public override bool Load(MHFVer from, byte[] src)
            {
                if (!base.Load(from, src)) return false;
                data = new byte[SrcCfg.len];
                for (int i = 0; i < SrcCfg.len; i++)
                    data[i] = src[SrcCfg.ptr + i];
                return true;
            }
            public override bool Write(MHFVer target, byte[] targetdata)
            {
                if (!base.Write(target, targetdata)) return false;
                int writeLenght = Math.Min(SrcCfg.len, TargetCfg.len);
                writeLenght = Math.Min(writeLenght, data.Length);
                for (int i = 0; i < writeLenght; i++)
                    targetdata[TargetCfg.ptr + i] = data[i];
                return true;
            }
            public override string ToString()
            {
                return $"{this.GetType().Name}:Length:{(data != null ? data.Length : "无")}";
            }
            public static EntityCfg CreateCfg(int _ptr, int _len) { return new EntityCfg() { ptr = _ptr, len = _len }; }
        }

        public class s_base_BytesarrayGroup : s_Base
        {
            public byte[] data;
            public override bool Load(MHFVer from, byte[] src)
            {
                if (!base.Load(from, src)) return false;
                List<byte> temp = new List<byte>();
                int alllen = SrcCfg.block_count * SrcCfg.block_single_len;
                data = new byte[alllen];
                for (int i = 0; i < alllen; i++)
                    temp.Add(src[SrcCfg.ptr + i]);
                data = temp.ToArray();
                return true;
            }
            public override bool Write(MHFVer target, byte[] targetdata)
            {
                if (!base.Write(target, targetdata)) return false;
                int srclen = SrcCfg.block_count * SrcCfg.block_single_len;
                int targetlen = TargetCfg.block_count * TargetCfg.block_single_len;
                int writeLenght = Math.Min(srclen, targetlen);
                writeLenght = Math.Min(writeLenght, data.Length);
                for (int i = 0; i < writeLenght; i++)
                    targetdata[TargetCfg.ptr + i] = data[i];
                return true;
            }
            public override string ToString()
            {
                return $"{this.GetType().Name}:Length:{(data != null ? data.Length : "无")}";
            }

            public static EntityCfg CreateCfg(int _ptr, int _block_single_len, int _block_count) { return new EntityCfg() { ptr = _ptr, block_count = _block_count, block_single_len = _block_single_len }; }
        }

        public class s_4byte_UInt32_Base : s_base_Bytesarray
        {
            public override string ToString()
            {
                return $"{this.GetType().Name}:{(data != null ? HexHelper.bytesToUInt(data, data.Length, 0).ToString() : "无")}";
            }
            public static EntityCfg CreateCfg(int _ptr) { return new EntityCfg() { ptr = _ptr, len = 4 }; }
        }
        public class s_2byte_Int16_Base : s_base_Bytesarray
        {
            public override string ToString()
            {
                return $"{this.GetType().Name}:{(data != null ? HexHelper.bytesToUInt(data, data.Length, 0).ToString() : "无")}";
            }
            public static EntityCfg CreateCfg(int _ptr) { return new EntityCfg() { ptr = _ptr, len = 2 }; }
        }
        public class s_String_Base : s_Base
        {
            public byte[] data;
            public string strdata;
            public override bool Load(MHFVer from, byte[] src)
            {
                if (!base.Load(from, src)) return false;
                List<byte> temp = new List<byte>();
                int ptr = SrcCfg.ptr;
                for (int i = 0; i < SrcCfg.len; i++)
                {
                    if (src[ptr + i] == 0x00 || src[ptr + i] == 0xFF)
                    {
                        temp.Add(src[ptr + i]);
                        break;
                    }
                    temp.Add(src[ptr + i]);
                }
                data = temp.ToArray();
                strdata = Encoding.GetEncoding("shift-jis").GetString(data);
                return true;
            }

            public override bool Write(MHFVer target, byte[] targetdata)
            {
                if (!base.Write(target, targetdata)) return false;
                int writeLenght = Math.Min(SrcCfg.len, TargetCfg.len);
                writeLenght = Math.Min(writeLenght, data.Length);
                for (int i = 0; i < writeLenght; i++)
                    targetdata[TargetCfg.ptr + i] = data[i];
                return true;
            }

            public override string ToString()
            {
                return strdata;
            }
            public static EntityCfg CreateCfg(int _ptr, int _len) { return new EntityCfg() { ptr = _ptr, len = _len }; }
        }
        #endregion

        public class s_RoleHandleGroup : s_base_Bytesarray { }
        public class s_Gender : s_base_Byte
        {
            public enum E_SEX : byte
            {
                M,
                W
            }
            public E_SEX sex => (E_SEX)data;
            public override string ToString()
            {
                return sex.ToString();
            }
        }
        public class s_Name : s_String_Base
        {
            public override string ToString()
            {
                return strdata;
            }
        }
        public class s_Zenny : s_4byte_UInt32_Base { }
        public class s_HRP : s_4byte_UInt32_Base { }
        public class s_EquipmentBox : s_base_BytesarrayGroup
        {
            public override string ToString()
            {
                if (data == null) return base.ToString();

                string str = string.Empty;
                List<uint> idlistdata = new List<uint>();

                for (int block = 0; block < SrcCfg.block_count; block++)
                {
                    int startptr = (block * SrcCfg.block_single_len);
                    byte[] itemiddata = new byte[2];
                    for (int i = 0; i < SrcCfg.block_single_len; i++)
                    {
                        if (i == 0) itemiddata[0] = data[startptr + i];
                        if (i == 1) itemiddata[1] = data[startptr + i];
                    }
                    idlistdata.Add(HexHelper.bytesToUInt(itemiddata, 2, 0));
                }

                foreach (var item in idlistdata)
                    str += $"装备ID{item}\r\n";
                return str;
            }
        }
        public class s_Itembox : s_base_BytesarrayGroup
        {
            public override string ToString()
            {
                string str = string.Empty;

                byte[] itemiddata = new byte[2];
                byte[] countdata = new byte[2];

                List<(int, int)> itemdata = new List<(int, int)>();
                for (int block = 0; block < SrcCfg.block_count; block++)
                {
                    int startptr = (block * SrcCfg.block_single_len);
                    for (int i = 0; i < SrcCfg.block_single_len; i++)
                    {
                        if (i == 0) itemiddata[0] = data[startptr + i];
                        if (i == 1) itemiddata[1] = data[startptr + i];
                        if (i == 2) countdata[0] = data[startptr + i];
                        if (i == 3) countdata[1] = data[startptr + i];
                    }
                    itemdata.Add((HexHelper.bytesToInt(itemiddata, 2, 0), HexHelper.bytesToInt(countdata, 2, 0)));
                }
                foreach (var item in itemdata)
                    str += $"{MHHelper.Get2MHFItemName(item.Item1)}:{item.Item2}\r\n";
                return str;
            }

            public override bool FixedData(out string log)
            {
                if (!SrcVerHad)
                {
                    log = "没有数据";
                    return false;
                }

                log = this.GetType().Name + "\r\n";
                byte[] itemiddata = new byte[2];
                byte[] countdata = new byte[2];
                List<(int, int)> itemdata = new List<(int, int)>();
                for (int block = 0; block < SrcCfg.block_count; block++)
                {
                    int startptr = (block * SrcCfg.block_single_len);
                    for (int i = 0; i < SrcCfg.block_single_len; i++)
                    {
                        if (i == 0) itemiddata[0] = data[startptr + i];
                        if (i == 1) itemiddata[1] = data[startptr + i];
                        if (i == 2) countdata[0] = data[startptr + i];
                        if (i == 3) countdata[1] = data[startptr + i];
                    }

                    uint itemid = HexHelper.bytesToUInt(itemiddata, 2, 0);
                    uint count = HexHelper.bytesToUInt(countdata, 2, 0);
                    if (itemid < 0)
                        continue;

                    //log += $"[{block}]{itemid}:{MHHelper.Get2MHFItemName((int)itemid)}:{count}\r\n";

                    if (SrcVer == MHFVer.GG)
                    {
                        bool needfix = (itemid >= 9749);

                        if (needfix)
                        {
                            //抹除数据
                            for (int i = 0; i < SrcCfg.block_single_len; i++)
                                data[startptr + i] = 0x00;
                            log += $"抹除数据:[{block}]{itemid}:{MHHelper.Get2MHFItemName((int)itemid)}:{count}\r\n";
                        }
                    }
                }

                return false;
            }
        }

        public class s_pPlaytime : s_4byte_UInt32_Base
        {
            public override string ToString()
            {
                return $"游戏时长:{this.GetType().Name}:{(data != null ? (HexHelper.bytesToInt(data, data.Length, 0) / 3600) + "小时" : "无")}";
            }
        }
        public class s_CurrentEquip : s_base_BytesarrayGroup
        {
        }
        public class s_pStandardInfoSub1 : s_base_Bytesarray { }
        public class s_pWeaponID : s_2byte_Int16_Base { }
        public class s_pWeaponType : s_base_Byte { };
        public class s_KillMonsterCountArr : s_base_BytesarrayGroup { };
        public class s_pWeaponTypeGroup : s_base_Bytesarray { };
        public class s_pHouseTier : s_4byte_UInt32_Base { }
        public class s_ItemPresets_Names : s_base_BytesarrayGroup
        {
            public override string ToString()
            {
                return "道具预设名称：" + Encoding.GetEncoding("shift-jis").GetString(data);
            }
        }
        public class s_ItemPresets_Ids : s_base_BytesarrayGroup
        {
            public override string ToString()
            {
                List<uint> idlistdata = new List<uint>();

                for (int block = 0; block < SrcCfg.block_count; block++)
                {
                    int startptr = (block * SrcCfg.block_single_len);
                    byte[] itemiddata = new byte[2];
                    for (int i = 0; i < SrcCfg.block_single_len; i++)
                    {
                        if (i == 0) itemiddata[0] = data[startptr + i];
                        if (i == 1) itemiddata[1] = data[startptr + i];
                    }
                    idlistdata.Add(HexHelper.bytesToUInt(itemiddata, 2, 0));
                }
                string str = "道具预设道具ID：";
                foreach (var item in idlistdata)
                    str += $"{item}{MHHelper.Get2MHFItemName((int)item)}\r\n";
                return str;
            }
        }
        public class s_SR_片手 : s_4byte_UInt32_Base { }
        public class s_SR_双刀 : s_4byte_UInt32_Base { }
        public class s_SR_大剑 : s_4byte_UInt32_Base { }
        public class s_SR_太刀 : s_4byte_UInt32_Base { }
        public class s_SR_锤子 : s_4byte_UInt32_Base { }
        public class s_SR_笛 : s_4byte_UInt32_Base { }
        public class s_SR_长枪 : s_4byte_UInt32_Base { }
        public class s_SR_铳枪 : s_4byte_UInt32_Base { }
        public class s_SR_穿龙棍 : s_4byte_UInt32_Base { }
        public class s_SR_轻弩 : s_4byte_UInt32_Base { }
        public class s_SR_重弩 : s_4byte_UInt32_Base { }
        public class s_SR_弓 : s_4byte_UInt32_Base { }

        public class s_SR_片手_Status1 : s_base_Byte { }
        public class s_SR_双刀_Status1 : s_base_Byte { }
        public class s_SR_大剑_Status1 : s_base_Byte { }
        public class s_SR_太刀_Status1 : s_base_Byte { }
        public class s_SR_锤子_Status1 : s_base_Byte { }
        public class s_SR_笛_Status1 : s_base_Byte { }
        public class s_SR_长枪_Status1 : s_base_Byte { }
        public class s_SR_铳枪_Status1 : s_base_Byte { }
        public class s_SR_穿龙棍_Status1 : s_base_Byte { }
        public class s_SR_轻弩_Status1 : s_base_Byte { }
        public class s_SR_重弩_Status1 : s_base_Byte { }
        public class s_SR_弓_Status1 : s_base_Byte { }

        public class s_SR_片手_Status2 : s_base_Byte { }
        public class s_SR_双刀_Status2 : s_base_Byte { }
        public class s_SR_大剑_Status2 : s_base_Byte { }
        public class s_SR_太刀_Status2 : s_base_Byte { }
        public class s_SR_锤子_Status2 : s_base_Byte { }
        public class s_SR_笛_Status2 : s_base_Byte { }
        public class s_SR_长枪_Status2 : s_base_Byte { }
        public class s_SR_铳枪_Status2 : s_base_Byte { }
        public class s_SR_穿龙棍_Status2 : s_base_Byte { }
        public class s_SR_轻弩_Status2 : s_base_Byte { }
        public class s_SR_重弩_Status2 : s_base_Byte { }
        public class s_SR_弓_Status2 : s_base_Byte { }

        public class s_ItemPresets_counts : s_base_BytesarrayGroup
        {
            public override string ToString()
            {
                List<uint> idlistdata = new List<uint>();

                for (int block = 0; block < SrcCfg.block_count; block++)
                {
                    int startptr = (block * SrcCfg.block_single_len);
                    byte[] itemiddata = new byte[2];
                    for (int i = 0; i < SrcCfg.block_single_len; i++)
                    {
                        if (i == 0) itemiddata[0] = data[startptr + i];
                        if (i == 1) itemiddata[1] = data[startptr + i];
                    }
                    idlistdata.Add(HexHelper.bytesToUInt(itemiddata, 2, 0));
                }
                string str = "道具预设道具数量：";
                foreach (var item in idlistdata)
                    str += $"{item}{MHHelper.Get2MHFItemName((int)item)}\r\n";
                return str;
            }
        }
        public class s_pToreData : s_base_Bytesarray { }
        public class s_pHR : s_4byte_UInt32_Base { }
        public class s_pGRP : s_4byte_UInt32_Base { }
        public class s_pHouseData : s_base_Bytesarray { }
        public class s_Gzenn : s_4byte_UInt32_Base { }
        public class s_Stylevouchers : s_4byte_UInt32_Base { }
        public class s_Dailyguild : s_4byte_UInt32_Base { }
        public class s_Socialize : s_base_Bytesarray
        {
            public override string ToString()
            {
                return "社交数据区块:" + base.ToString();
            }
        }
        public class s_CP : s_4byte_UInt32_Base { }
        public class s_知名度 : s_4byte_UInt32_Base { }
        public class s_知名度称号 : s_base_Byte { }
        public class s_pBookshelfData : s_base_Bytesarray { }
        public class s_pGalleryData : s_base_Bytesarray { }
        public class s_pGardenData花园 : s_base_Bytesarray { }
        public class s_pRP : s_2byte_Int16_Base { }
        public class s_pKQF : s_base_Bytesarray { }
        public class ExtraBox背包 : s_base_BytesarrayGroup { }
        public class s_ItemPouch背包 : s_base_BytesarrayGroup
        {

            public override string ToString()
            {
                string str = string.Empty;

                byte[] itemiddata = new byte[2];
                byte[] countdata = new byte[2];

                List<(int, int)> itemdata = new List<(int, int)>();
                for (int block = 0; block < SrcCfg.block_count; block++)
                {
                    int startptr = (block * SrcCfg.block_single_len);
                    for (int i = 0; i < SrcCfg.block_single_len; i++)
                    {
                        if (i == 0) itemiddata[0] = data[startptr + i];
                        if (i == 1) itemiddata[1] = data[startptr + i];
                        if (i == 2) countdata[0] = data[startptr + i];
                        if (i == 3) countdata[1] = data[startptr + i];
                    }
                    itemdata.Add((HexHelper.bytesToInt(itemiddata, 2, 0), HexHelper.bytesToInt(countdata, 2, 0)));
                }
                foreach (var item in itemdata)
                    str += $"{MHHelper.Get2MHFItemName(item.Item1)}:{item.Item2}\r\n";
                return str;
            }
        }
        public class s_Keyquestflag : s_base_Bytesarray { }
    }
}

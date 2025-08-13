using System.Text;
using static Axibug.MHFSaveAutoConverter.DataStruct.DataStruct;

namespace Axibug.MHFSaveAutoConverter.DataStruct
{
    public static class MHFSaveDataCfg
    {
        public static Dictionary<MHFVer, Dictionary<string, EntityCfg>> dictTypeWithCfg = new Dictionary<MHFVer, Dictionary<string, EntityCfg>>()
        {
            {
                MHFVer.FW5,new Dictionary<string, EntityCfg>()
                {
                   {nameof(s_RoleHandleGroup),s_RoleHandleGroup.CreateCfg(0x50,0x110-0x50)},//前面都是连续数据
                   {nameof(s_Gender),s_Gender.CreateCfg(0x50)},
                   {nameof(s_Name),s_Name.CreateCfg(0x58,50)},
                   {nameof(s_HRP),s_HRP.CreateCfg(0xAC)},
                   {nameof(s_Zenny),s_Zenny.CreateCfg(0xb0)},
                   {nameof(s_EquipmentBox),s_EquipmentBox.CreateCfg(0x110,16,100 * 20)},
                   //{nameof(s_Itembox),s_Itembox.CreateCfg(0x7E10,8,35 * 100)},
                   {nameof(s_Itembox),s_Itembox.CreateCfg(0x7E24,8,35 * 100)},
                   {nameof(s_pPlaytime),s_pPlaytime.CreateCfg(0xEBC4)},
                   {nameof(s_CurrentEquip),s_CurrentEquip.CreateCfg(0xEC54,6,16)},
                   //{nameof(s_pStandardInfoSub1),s_pStandardInfoSub1.CreateCfg(0xEC6A,1398)},
                   {nameof(s_pWeaponID),s_pWeaponID.CreateCfg(0xEC6A)},
                   {nameof(s_pWeaponTypeGroup),s_pWeaponTypeGroup.CreateCfg(0xED70,24)},
                   {nameof(s_pStandardInfoSub1),s_pStandardInfoSub1.CreateCfg(0xED75,0xF1E0 - 0xED75)},
                   {nameof(s_pWeaponType),s_pWeaponType.CreateCfg(0xED75)},
                   {nameof(s_KillMonsterCountArr),s_KillMonsterCountArr.CreateCfg(0xEFBE,2,212)},
                   {nameof(s_pHouseTier),s_pHouseTier.CreateCfg(0xF1CC)},
                   {nameof(s_ItemPresets_Names),s_ItemPresets_Names.CreateCfg(0xF1E0,32,24)},
                   {nameof(s_ItemPresets_Ids),s_ItemPresets_Ids.CreateCfg(0xF3C0,2,30)},
                   {nameof(s_SR_片手),s_SR_片手.CreateCfg(0xF624)},
                   {nameof(s_SR_双刀),s_SR_双刀.CreateCfg(0xF628)},
                   {nameof(s_SR_大剑),s_SR_大剑.CreateCfg(0xF62C)},
                   {nameof(s_SR_太刀),s_SR_太刀.CreateCfg(0xF630)},
                   {nameof(s_SR_锤子),s_SR_锤子.CreateCfg(0xF634)},
                   {nameof(s_SR_笛),s_SR_笛.CreateCfg(0xF638)},
                   {nameof(s_SR_长枪),s_SR_长枪.CreateCfg(0xF63C)},
                   {nameof(s_SR_铳枪),s_SR_铳枪.CreateCfg(0xF640)},
                   {nameof(s_SR_穿龙棍),s_SR_穿龙棍.CreateCfg(-1)},
                   {nameof(s_SR_轻弩),s_SR_轻弩.CreateCfg(0xF644)},
                   {nameof(s_SR_重弩),s_SR_重弩.CreateCfg(0xF648)},
                   {nameof(s_SR_弓),s_SR_弓.CreateCfg(0xF64C)},

                   {nameof(s_SR_片手_Status1),s_SR_片手_Status1.CreateCfg(0xF650)},
                   {nameof(s_SR_双刀_Status1),s_SR_双刀_Status1.CreateCfg(0xF650+(1*1))},
                   {nameof(s_SR_大剑_Status1),s_SR_大剑_Status1.CreateCfg(0xF650+(1*2))},
                   {nameof(s_SR_太刀_Status1),s_SR_太刀_Status1.CreateCfg(0xF650+(1*3))},
                   {nameof(s_SR_锤子_Status1),s_SR_锤子_Status1.CreateCfg(0xF650+(1*4))},
                   {nameof(s_SR_笛_Status1),s_SR_笛_Status1.CreateCfg(0xF650+(1*5))},
                   {nameof(s_SR_长枪_Status1),s_SR_长枪_Status1.CreateCfg(0xF650+(1*6))},
                   {nameof(s_SR_铳枪_Status1),s_SR_铳枪_Status1.CreateCfg(0xF650+(1*7))},
                   {nameof(s_SR_穿龙棍_Status1),s_SR_穿龙棍_Status1.CreateCfg(-1)},
                   {nameof(s_SR_轻弩_Status1),s_SR_轻弩_Status1.CreateCfg(0xF650+(1*8))},
                   {nameof(s_SR_重弩_Status1),s_SR_重弩_Status1.CreateCfg(0xF650+(1*9))},
                   {nameof(s_SR_弓_Status1),s_SR_弓_Status1.CreateCfg(0xF650+(1*10)) },

                   {nameof(s_SR_片手_Status2),s_SR_片手_Status2.CreateCfg(0xF65B)},
                   {nameof(s_SR_双刀_Status2),s_SR_双刀_Status2.CreateCfg(0xF65B+(1*1))},
                   {nameof(s_SR_大剑_Status2),s_SR_大剑_Status2.CreateCfg(0xF65B+(1*2))},
                   {nameof(s_SR_太刀_Status2),s_SR_太刀_Status2.CreateCfg(0xF65B+(1*3))},
                   {nameof(s_SR_锤子_Status2),s_SR_锤子_Status2.CreateCfg(0xF65B+(1*4))},
                   {nameof(s_SR_笛_Status2),s_SR_笛_Status2.CreateCfg(0xF65B+(1*5))},
                   {nameof(s_SR_长枪_Status2),s_SR_长枪_Status2.CreateCfg(0xF65B+(1*6))},
                   {nameof(s_SR_铳枪_Status2),s_SR_铳枪_Status2.CreateCfg(0xF65B+(1*7))},
                   {nameof(s_SR_穿龙棍_Status2),s_SR_穿龙棍_Status2.CreateCfg(-1)},
                   {nameof(s_SR_轻弩_Status2),s_SR_轻弩_Status2.CreateCfg(0xF65B+(1*8))},
                   {nameof(s_SR_重弩_Status2),s_SR_重弩_Status2.CreateCfg(0xF65B+(1*9))},
                   {nameof(s_SR_弓_Status2),s_SR_弓_Status2.CreateCfg(0xF65B+(1*10)) },

                   {nameof(s_ItemPresets_counts),s_ItemPresets_counts.CreateCfg(0xF960,1,30)},
                   {nameof(s_pToreData),s_pToreData.CreateCfg(0xF314,240)},
                   {nameof(s_pHR),s_pHR.CreateCfg(0xF456)},
                   {nameof(s_pGRP),s_pGRP.CreateCfg(-1)},
                   {nameof(s_pHouseData),s_pHouseData.CreateCfg(0xF461,195)},
                   {nameof(s_Gzenn) ,s_Gzenn.CreateCfg(-1)},
                   {nameof(s_Stylevouchers) ,s_Stylevouchers.CreateCfg(-1)},
                   {nameof(s_Socialize) ,s_Socialize.CreateCfg(0x1084C,0x10E30 - 0x1084C)},//社交数据
                   {nameof(s_CP) ,s_CP.CreateCfg(0x10944)},
                   {nameof(s_知名度) ,s_知名度.CreateCfg(0x10948)},
                   {nameof(s_知名度称号) ,s_知名度称号.CreateCfg(0x1094C)},
                   {nameof(s_pBookshelfData),s_pBookshelfData.CreateCfg(-1,-1)},
                   {nameof(s_pGalleryData),s_pGalleryData.CreateCfg(0x11980,1748)},
                   {nameof(s_pGardenData花园),s_pGardenData花园.CreateCfg(0x122B8,68)},
                   {nameof(s_pRP),s_pRP.CreateCfg(0x12376)},
                   {nameof(s_pKQF),s_pKQF.CreateCfg(-1,8)},
                   /*单个道具长度8byte，前20个是item/道具 后10个是ammo/弹药*/
                   {nameof(s_ItemPouch背包),s_ItemPouch背包.CreateCfg(-1,8,20+10)},
                   {nameof(s_Keyquestflag),s_Keyquestflag.CreateCfg(-1,8) },
                }
            },

            {
                MHFVer.GG,new Dictionary<string, EntityCfg>()
                {
                   {nameof(s_RoleHandleGroup),s_RoleHandleGroup.CreateCfg(0x50,0x110-0x50)},//前面都是连续数据
                   {nameof(s_Gender),s_Gender.CreateCfg(0x50)},
                   {nameof(s_Name),s_Name.CreateCfg(0x58,50)},
                   {nameof(s_HRP),s_HRP.CreateCfg(0xAC)},
                   {nameof(s_Zenny),s_Zenny.CreateCfg(0xb0)},
                   {nameof(s_EquipmentBox),s_EquipmentBox.CreateCfg(0x110,16,100 * 20)},
                   {nameof(s_Itembox),s_Itembox.CreateCfg(0xBCA4,8,35 * 100)},
                   {nameof(s_pPlaytime),s_pPlaytime.CreateCfg(0x168C4)},
                   {nameof(s_CurrentEquip),s_CurrentEquip.CreateCfg(0x16960,6,16)},
                   //{nameof(s_pStandardInfoSub1),s_pStandardInfoSub1.CreateCfg(0x1696A,1398)},
                   {nameof(s_pWeaponID),s_pWeaponID.CreateCfg(0x1696A)},
                   {nameof(s_pWeaponTypeGroup),s_pWeaponTypeGroup.CreateCfg(0x16A70,24)},
                   {nameof(s_pStandardInfoSub1),s_pStandardInfoSub1.CreateCfg(0x16A75,0x16EE0-0x16A75)},
                   {nameof(s_pWeaponType),s_pWeaponType.CreateCfg(0x16A75)},
                   {nameof(s_KillMonsterCountArr),s_KillMonsterCountArr.CreateCfg(0x16CBE,2,212)},//当然GG不可能只有212个怪物
                   {nameof(s_pHouseTier),s_pHouseTier.CreateCfg(0x16ECC)},
                   {nameof(s_ItemPresets_Names),s_ItemPresets_Names.CreateCfg(0x16EE0,32,24)},
                   {nameof(s_ItemPresets_Ids),s_ItemPresets_Ids.CreateCfg(0x170C0,2,30)},
                   {nameof(s_SR_片手),s_SR_片手.CreateCfg(0x1A374)},
                   {nameof(s_SR_双刀),s_SR_双刀.CreateCfg(0X1A378)},
                   {nameof(s_SR_大剑),s_SR_大剑.CreateCfg(0x1A37C)},
                   {nameof(s_SR_太刀),s_SR_太刀.CreateCfg(0x1A380)},
                   {nameof(s_SR_锤子),s_SR_锤子.CreateCfg(0x1A384)},
                   {nameof(s_SR_笛),s_SR_笛.CreateCfg(0x1A388)},
                   {nameof(s_SR_长枪),s_SR_长枪.CreateCfg(0x1A38C)},
                   {nameof(s_SR_铳枪),s_SR_铳枪.CreateCfg(0x1A390)},
                   {nameof(s_SR_穿龙棍),s_SR_穿龙棍.CreateCfg(0x1A394)},
                   {nameof(s_SR_轻弩),s_SR_轻弩.CreateCfg(0x1A398)},
                   {nameof(s_SR_重弩),s_SR_重弩.CreateCfg(0X1A39C)},
                   {nameof(s_SR_弓),s_SR_弓.CreateCfg(0x1A3A0)},

                   {nameof(s_SR_片手_Status1),s_SR_片手_Status1.CreateCfg(0x1A3A4)},
                   {nameof(s_SR_双刀_Status1),s_SR_双刀_Status1.CreateCfg(0x1A3A4+(1*1))},
                   {nameof(s_SR_大剑_Status1),s_SR_大剑_Status1.CreateCfg(0x1A3A4+(1*2))},
                   {nameof(s_SR_太刀_Status1),s_SR_太刀_Status1.CreateCfg(0x1A3A4+(1*3))},
                   {nameof(s_SR_锤子_Status1),s_SR_锤子_Status1.CreateCfg(0x1A3A4+(1*4))},
                   {nameof(s_SR_笛_Status1),s_SR_笛_Status1.CreateCfg(0x1A3A4+(1*5))},
                   {nameof(s_SR_长枪_Status1),s_SR_长枪_Status1.CreateCfg(0x1A3A4+(1*6))},
                   {nameof(s_SR_铳枪_Status1),s_SR_铳枪_Status1.CreateCfg(0x1A3A4+(1*7))},
                   {nameof(s_SR_穿龙棍_Status1),s_SR_穿龙棍_Status1.CreateCfg(0x1A3A4+(1*8))},
                   {nameof(s_SR_轻弩_Status1),s_SR_轻弩_Status1.CreateCfg(0x1A3A4+(1*9))},
                   {nameof(s_SR_重弩_Status1),s_SR_重弩_Status1.CreateCfg(0x1A3A4+(1*10))},
                   {nameof(s_SR_弓_Status1),s_SR_弓_Status1.CreateCfg(0x1A3A4+(1*11)) },

                   {nameof(s_SR_片手_Status2),s_SR_片手_Status2.CreateCfg(0x1A3B0)},
                   {nameof(s_SR_双刀_Status2),s_SR_双刀_Status2.CreateCfg(0x1A3B0+(1*1))},
                   {nameof(s_SR_大剑_Status2),s_SR_大剑_Status2.CreateCfg(0x1A3B0+(1*2))},
                   {nameof(s_SR_太刀_Status2),s_SR_太刀_Status2.CreateCfg(0x1A3B0+(1*3))},
                   {nameof(s_SR_锤子_Status2),s_SR_锤子_Status2.CreateCfg(0x1A3B0+(1*4))},
                   {nameof(s_SR_笛_Status2),s_SR_笛_Status2.CreateCfg(0x1A3B0+(1*5))},
                   {nameof(s_SR_长枪_Status2),s_SR_长枪_Status2.CreateCfg(0x1A3B0+(1*6))},
                   {nameof(s_SR_铳枪_Status2),s_SR_铳枪_Status2.CreateCfg(0x1A3B0+(1*7))},
                   {nameof(s_SR_穿龙棍_Status2),s_SR_穿龙棍_Status2.CreateCfg(0x1A3B0+(1*8))},
                   {nameof(s_SR_轻弩_Status2),s_SR_轻弩_Status2.CreateCfg(0x1A3B0+(1*9))},
                   {nameof(s_SR_重弩_Status2),s_SR_重弩_Status2.CreateCfg(0x1A3B0+(1*10))},
                   {nameof(s_SR_弓_Status2),s_SR_弓_Status2.CreateCfg(0x1A3B0+(1*11)) },

                   {nameof(s_ItemPresets_counts),s_ItemPresets_counts.CreateCfg(0x17660,1,30)},
                   {nameof(s_pToreData),s_pToreData.CreateCfg(0x17014,240)},
                   {nameof(s_pHR),s_pHR.CreateCfg(0x17156)},
                   {nameof(s_pGRP),s_pGRP.CreateCfg(0x1715C)},
                   {nameof(s_pHouseData),s_pHouseData.CreateCfg(0x17161,195)},
                   {nameof(s_Gzenn) ,s_Gzenn.CreateCfg(0x172C4)},
                   {nameof(s_Stylevouchers) ,s_Stylevouchers.CreateCfg(-1)},
                   {nameof(s_Socialize) ,s_Socialize.CreateCfg(0x1854C,0x18B30 - 0x1854C)},//社交数据
                   {nameof(s_CP) ,s_CP.CreateCfg(0x18644)},
                   {nameof(s_知名度) ,s_知名度.CreateCfg(0x18648)},
                   {nameof(s_知名度称号) ,s_知名度称号.CreateCfg(0x1864C)},
                   {nameof(s_pBookshelfData),s_pBookshelfData.CreateCfg(0x11A8,-1)},
                   {nameof(s_pGalleryData),s_pGalleryData.CreateCfg(0x19680,1748)},
                   {nameof(s_pGardenData花园),s_pGardenData花园.CreateCfg(0x19FB8,68)},
                   {nameof(s_pRP),s_pRP.CreateCfg(0x1A076)},
                   {nameof(s_pKQF),s_pKQF.CreateCfg(0x1B080,8)},
                   /*单个道具长度8byte，前20个是item/道具 后10个是ammo/弹药*/
                   {nameof(s_ItemPouch背包),s_ItemPouch背包.CreateCfg(-1,8,20+10)},
                   {nameof(s_Keyquestflag),s_Keyquestflag.CreateCfg(-1,8) },
                }
            }
        };
        public enum MHFVer
        {
            FW5,
            GG
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MonsterHunterSaveBruteForce
{
    class MH2DosSaveDataLoader
    {
        // 已知数值常量
        private const ushort KnownMoney = 5929;        // 0x1729
        private const ushort KnownExp = 6525;          // 0x197D
        private const ushort ItemLargePotion = 0x0008;  // 回復藥・大 的ID
        private const ushort ItemLargePotionQty = 5;    // 回復藥・大 的数量

        // 道具箱特征（前6个道具）
        private static readonly List<(ushort id, ushort quantity)> ItemPattern = new List<(ushort, ushort)>
        {
            (0x0001, 1),  // 調合書①入門編
            (0x0002, 1),  // 調合書②初級編
            (0x0003, 1),  // 調合書③中級編
            (0x0007, 3),  // 回復藥
            (ItemLargePotion, ItemLargePotionQty),  // 回復藥・大
            (0x0015, 2)   // 冷飲
        };

        public static void LoadSaveData()
        {
            Console.WriteLine("===== PS2怪物猎人2存档暴力破解工具 =====");
            Console.WriteLine("此工具使用金钱、经验和道具特征进行暴力破解");

            // 1. 加载存档
            byte[] saveData = File.ReadAllBytes("play00.bin");
            Console.WriteLine($"存档大小: {saveData.Length} 字节");

            // 2. 暴力破解
            BruteForceDecryption(saveData);
        }

        static void BruteForceDecryption(byte[] data)
        {
            Console.WriteLine("开始暴力破解，这可能需要一些时间...");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            int totalAttempts = 0;
            const int reportInterval = 100000; // 每100,000次尝试报告一次进度

            // 限制搜索范围提高效率
            //int searchRange = Math.Min(0x10000, data.Length - 20);
            int searchRange = data.Length - 20;

            // 尝试所有可能的密钥组合 (0x0000 - 0xFFFF)
            for (ushort key = 0; key < ushort.MaxValue; key++)
            {
                // 解密整个存档
                byte[] decrypted = DecryptWithKey(data, key);

                // 验证金钱和经验值
                var (moneyFound, expFound) = ValidateMoneyAndExp(decrypted);

                // 验证道具箱特征
                //bool itemsValid = ValidateItemBox(decrypted);

                bool itemsValid = true;

                // 如果三项验证都通过
                //if (moneyFound && expFound && itemsValid)
                if(ValidateName(decrypted))
                {
                    stopwatch.Stop();
                    Console.WriteLine($"\n暴力破解成功! 密钥: 0x{key:X4}");
                    Console.WriteLine($"耗时: {stopwatch.Elapsed.TotalSeconds:F2} 秒");
                    Console.WriteLine($"尝试次数: {totalAttempts}");

                    // 保存解密文件
                    //File.WriteAllBytes("decrypted_play00.bin", decrypted);
                    Console.WriteLine("解密文件已保存: decrypted_play00.bin");
                    return;
                }

                // 报告进度
                if (++totalAttempts % reportInterval == 0)
                {
                    Console.WriteLine($"已尝试 {totalAttempts} 种密钥组合... 当前密钥: 0x{key:X4}");
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"\n暴力破解完成，但未找到有效密钥。耗时: {stopwatch.Elapsed.TotalSeconds:F2} 秒");
            Console.WriteLine($"总尝试次数: {totalAttempts}");
        }

        static byte[] DecryptWithKey(byte[] data, ushort key)
        {
            byte[] keyBytes = BitConverter.GetBytes(key);
            byte[] decrypted = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                decrypted[i] = (byte)(data[i] ^ keyBytes[i % 2]);
            }
            return decrypted;
        }

        static (bool moneyFound, bool expFound) ValidateMoneyAndExp(byte[] decrypted)
        {
            bool moneyFound = false;
            bool expFound = false;

            // 扫描整个文件寻找金钱和经验值
            for (int i = 0; i < decrypted.Length - 1; i++)
            {
                ushort value = BitConverter.ToUInt16(decrypted, i);

                // 检查是否是金钱值
                if (value == KnownMoney)
                {
                    moneyFound = true;
                }

                // 检查是否是经验值
                if (value == KnownExp)
                {
                    expFound = true;
                }

                // 如果两个都已找到，提前退出
                if (moneyFound && expFound)
                {
                    break;
                }
            }

            return (moneyFound, expFound);
        }

        static bool ValidateName(byte[] data)
        {
            byte[] checkdata = new byte[4] { 0xE1, 0xA9, 0x8C, 0x8E };
            // 扫描整个文件寻找道具箱特征
            for (int offset = 0; offset < data.Length - checkdata.Length; offset++)
            {
                for (int i = 0; i < checkdata.Length; i++)
                {
                    if (data[0] == checkdata[0]
                        && data[1] == checkdata[1]
                        && data[2] == checkdata[2]
                        && data[3] == checkdata[3])
                        return true;
                }
            }

            return false;
        }

        static bool ValidateItemBox(byte[] decrypted)
        {
            // 尝试不同的道具结构 (4字节和8字节)
            return ValidateItemStructure(decrypted, 4) || ValidateItemStructure(decrypted, 8);
        }

        static bool ValidateItemStructure(byte[] data, int slotSize)
        {
            // 扫描整个文件寻找道具箱特征
            for (int offset = 0; offset < data.Length - (slotSize * 6); offset++)
            {
                bool patternMatch = true;

                // 检查前6个道具是否匹配已知模式
                for (int slot = 0; slot < 6; slot++)
                {
                    int slotOffset = offset + (slot * slotSize);

                    // 获取道具ID和数量
                    ushort itemId = BitConverter.ToUInt16(data, slotOffset);
                    ushort quantity = BitConverter.ToUInt16(data, slotOffset + 2);

                    // 检查是否匹配预期值
                    if (itemId != ItemPattern[slot].id || quantity != ItemPattern[slot].quantity)
                    {
                        patternMatch = false;
                        break;
                    }
                }

                // 如果找到匹配模式
                if (patternMatch)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
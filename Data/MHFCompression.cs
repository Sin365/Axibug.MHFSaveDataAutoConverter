namespace Axibug.MHFSaveAutoConverter.DataStruct
{
    public static class MHFCompression
    {
        // 文件头常量
        private static readonly byte[] HeaderBytes = {
        0x63, 0x6D, 0x70, 0x20,  // "cmp "
        0x32, 0x30, 0x31, 0x31, 0x30, 0x31, 0x31, 0x33,  // "20110113"
        0x20, 0x20, 0x20, 0x00   // "   \0"
    };

        public static byte[] Decompress(byte[] data)
        {
            // 检查文件头
            if (data.Length < 16 || !data.Take(16).SequenceEqual(HeaderBytes))
            {
                Console.WriteLine("存档存档无需解压");
                return data;
            }
            Console.WriteLine("存档解压");

            using var outputStream = new MemoryStream();
            using var reader = new BinaryReader(new MemoryStream(data));

            // 跳过16字节头
            reader.BaseStream.Seek(16, SeekOrigin.Begin);

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                byte currentByte = reader.ReadByte();

                if (currentByte == 0x00)
                {
                    // 读取零序列长度
                    int nullCount = reader.ReadByte();

                    // 写入相应数量的零
                    for (int i = 0; i < nullCount; i++)
                    {
                        outputStream.WriteByte(0x00);
                    }
                }
                else
                {
                    // 直接写入非零字节
                    outputStream.WriteByte(currentByte);
                }
            }

            return outputStream.ToArray();
        }

        public static byte[] Compress(byte[] data)
        {
            using var outputStream = new MemoryStream();
            using var writer = new BinaryWriter(outputStream);

            // 写入文件头
            writer.Write(HeaderBytes);

            using var reader = new BinaryReader(new MemoryStream(data));
            int nullCount = 0;

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                byte currentByte = reader.ReadByte();

                if (currentByte == 0x00)
                {
                    nullCount++;

                    // 处理连续零的序列
                    while (nullCount < 255 && reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        byte nextByte = reader.ReadByte();
                        if (nextByte == 0x00)
                        {
                            nullCount++;
                        }
                        else
                        {
                            // 写入压缩标记和零计数
                            writer.Write((byte)0x00);
                            writer.Write((byte)nullCount);

                            // 写入遇到的非零字节
                            writer.Write(nextByte);
                            nullCount = 0;
                            break;
                        }
                    }

                    // 处理达到最大计数或文件结束的情况
                    if (nullCount > 0)
                    {
                        writer.Write((byte)0x00);
                        writer.Write((byte)Math.Min(nullCount, 255));
                        nullCount = 0;
                    }
                }
                else
                {
                    // 直接写入非零字节
                    writer.Write(currentByte);
                }
            }

            // 处理结尾可能的剩余零序列
            if (nullCount > 0)
            {
                writer.Write((byte)0x00);
                writer.Write((byte)nullCount);
            }

            return outputStream.ToArray();
        }
    }
}
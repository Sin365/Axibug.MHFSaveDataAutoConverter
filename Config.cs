namespace HaoYue.MHFUserSrv.Server.Common
{
    public static class Config
    {
        public static Dictionary<string, string> DictCfg = new Dictionary<string, string>();
        public static void LoadCfg(string file)
        {
            DictCfg = ReadCfg(file);
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        static Dictionary<string, string> ReadCfg(string file)
        {
            Dictionary<string, string> a = new Dictionary<string, string>()
            {
                { "dbServer", string.Empty},
                { "dbPort", string.Empty},
                { "src_dbName", string.Empty},
                { "target_dbName", string.Empty},
                { "dbUser", string.Empty},
                { "dbPwd", string.Empty},
            };
            try
            {
                string cfg = string.Empty;
                if (File.Exists(file))
                {
                    cfg = File.ReadAllText(file);
                }
                if (!string.IsNullOrEmpty(cfg))
                {
                    string[] b;
                    foreach (var kv in cfg.Split("\r\n", StringSplitOptions.RemoveEmptyEntries))
                    {
                        b = kv.Split('=', StringSplitOptions.RemoveEmptyEntries);

                        if (b.Length == 2)
                        {
                            if (a.ContainsKey(b[0]))
                            {
                                if (string.IsNullOrEmpty(b[1]))
                                {
                                    continue;
                                }
                                else
                                {
                                    a[b[0]] = b[1];
                                }
                            }
                        }
                    }
                }
                return a;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"->ReadCfgCatch->{ex.Message}");
                return a;
            }
        }
    }
}

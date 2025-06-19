
using System;
using System.Configuration;
using System.Runtime.Remoting.Channels;

namespace CodingRep.service.stores
{
    public class EnvironmentStore
    {
        private readonly string _envFilePath;

        public EnvironmentStore()
        {
            _envFilePath = ConfigurationManager.AppSettings["EnvPath"];
            System.Diagnostics.Debug.WriteLine("EnvPath from config: " + _envFilePath);
            loadEnvironmentVariables();
        }

        private void loadEnvironmentVariables()
        {
            string path = _envFilePath;


            // 普通相对路径+程序基目录=绝对路径，咕
            path = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));

            System.Diagnostics.Debug.WriteLine("加载 .env 文件路径: " + path);

            if (System.IO.File.Exists(path))
            {
                foreach (var line in System.IO.File.ReadAllLines(path))
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    var parts = line.Split(new[] { '=' }, 2);
                    if (parts.Length != 2)
                        continue;

                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    if (value.StartsWith("\"") && value.EndsWith("\""))
                        value = value.Substring(1, value.Length - 2);

                    System.Environment.SetEnvironmentVariable(key, value);
                    System.Diagnostics.Debug.WriteLine($"加载环境变量: {key}={value}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("未找到 .env 文件: " + path);
            }
        }

    }
}

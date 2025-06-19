
using System.Runtime.Remoting.Channels;

namespace CodingRep.service.stores
{
    public class EnvironmentStore
    {
        private readonly string _envFilePath;

        public EnvironmentStore(string envFilePath = "../../.env")
        {
            _envFilePath = envFilePath;
            loadEnvironmentVariables();
        }

        private void loadEnvironmentVariables()
        {
            string expandedPath = System.Environment.ExpandEnvironmentVariables(_envFilePath);
            if (System.IO.File.Exists(expandedPath))
            {
                foreach (string line in System.IO.File.ReadAllLines(expandedPath))
                {
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                    {
                        string[] parts = line.Split(new[] { '=' }, 2);
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim();
                            string value = parts[1].Trim();

                            // Remove quotes if present
                            if (value.StartsWith("\"") && value.EndsWith("\""))
                                value = value.Substring(1, value.Length - 1);

                            System.Environment.SetEnvironmentVariable(key, value);
                        }
                    }
                }
            }
        }
    }
}

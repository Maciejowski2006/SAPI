using Newtonsoft.Json;
using  SAPI.LLAPI;

namespace SAPI
{
	internal static class Config
	{
		private static string dataFolder;
		private static string configFile;

		public static void Init()
		{
			dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.ChangeExtension(Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().ProcessName), null));
			configFile = Path.Combine(dataFolder, "config.json");

			if (File.Exists(configFile))
			{
				if (ReadConfig().ConfigVersion != new ConfigFile().ConfigVersion)
					CreateConfig(true);

				return;
			}

			CreateConfig();
		}

		private static void CreateConfig(bool update = false)
		{
			ConfigFile defaultConfig = new();
			if (update)
			{
				Debug.Warn("Old config file detected: updating, while keeping old settings");

				var oldConfig = ReadConfig();

				defaultConfig = oldConfig;
				defaultConfig.ConfigVersion = new ConfigFile().ConfigVersion;
				File.Delete(configFile);
			}

			var json = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented);
			File.WriteAllText(configFile, json);
		}

		public static ConfigFile ReadConfig()
		{
			using StreamReader sr = new(configFile);
			string json = sr.ReadToEnd();
			ConfigFile config = JsonConvert.DeserializeObject<ConfigFile>(json)!;
			return config;
		}
	}

	internal class ConfigFile
	{
		public float ConfigVersion = 3.0f;
		public bool Verbose = false;
		public string Url = "http://localhost:8000/";
		public bool EnableErrorReporting = true;
	}
}
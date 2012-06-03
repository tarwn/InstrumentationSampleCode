using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace SensitiveSettings {
	public class SettingsManager {

		private static Dictionary<string,string> _settings;

		public static Dictionary<string, string> Settings {
			get {
				if (_settings == null) {
					LoadSettings();
				}
				return _settings;
			}
		}

		public static void LoadFrom(string path) {
			LoadSettings(path);
		}

        private static void LoadSettings(string pathPrefix = ""){
			string content = File.ReadAllText(pathPrefix + "sensitive.config");
			_settings = new Dictionary<string, string>();
			var lines = content.Replace("\r\n", "\n")
							   .Split('\n')
							   .Select(s => s.Split(new char[] { '=' }, 2, StringSplitOptions.None))
							   .Where(s => s.Length == 2);

			foreach(string[] line in lines){
				_settings.Add(line[0].Trim(), line[1].Trim());
			}
		}

	}
}

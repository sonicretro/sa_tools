using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using SA_Tools;
using SonicRetro.SAModel;

namespace ModManagement
{
	public static class ModManagement
	{
		public static SA_Tools.DataMapping ExeDataINIFromList(List<string> includeFiles, string projectFolder, string destinationFolder, DataMapping sonicExeData)
		{
			DataMapping output = new DataMapping();
			output.Files = new Dictionary<string, SA_Tools.FileInfo>();
			foreach (KeyValuePair<string, SA_Tools.FileInfo> item in sonicExeData.Files.Where((a, i) => includeFiles.Contains(a.Key))) // listView1.CheckedIndices.Contains(i)
			{
				Environment.CurrentDirectory = projectFolder;
				string projectRelativeFileLocation = item.Value.Filename; // getting project-relative location (full path)

				if (Directory.Exists(projectRelativeFileLocation)) // this is actually trying to check and see if the item in question is a directory
					Directory.CreateDirectory(Path.Combine(destinationFolder, projectRelativeFileLocation)); // this won't work at all, since you can't combine two absolute paths
				else // this is how we know it is NOT a directory. But we'll need to turn it into one before we can drop our file into it.
					Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(destinationFolder, projectRelativeFileLocation)));

				switch (item.Value.Type)
				{
					case "deathzone":
						DeathZoneFlags[] list = DeathZoneFlagsList.Load(projectRelativeFileLocation);
						string path = Path.GetDirectoryName(projectRelativeFileLocation);
						for (int j = 0; j < list.Length; j++)
						{
							System.IO.FileInfo file = new System.IO.FileInfo(Path.Combine(path, j.ToString(NumberFormatInfo.InvariantInfo) + ".sa1mdl")); // todo: this extension isn't getting copied correctly
							file.CopyTo(Path.Combine(Path.Combine(destinationFolder, path), j.ToString(NumberFormatInfo.InvariantInfo)), true);
						}
						File.Copy(projectRelativeFileLocation, Path.Combine(destinationFolder, projectRelativeFileLocation), true);
						break;
					default:
						if (Directory.Exists(projectRelativeFileLocation))
							CopyDirectory(new DirectoryInfo(projectRelativeFileLocation), Path.Combine(destinationFolder, projectRelativeFileLocation));
						else
							File.Copy(projectRelativeFileLocation, Path.Combine(destinationFolder, projectRelativeFileLocation), true);
						break;
				}
				item.Value.MD5Hash = null;
				output.Files.Add(item.Key, item.Value);
			}
			return output;
		}

		private static void CopyDirectory(DirectoryInfo src, string dst)
		{
			if (!Directory.Exists(dst))
				Directory.CreateDirectory(dst);
			foreach (DirectoryInfo dir in src.GetDirectories())
				CopyDirectory(dir, Path.Combine(dst, dir.Name));
			foreach (System.IO.FileInfo fil in src.GetFiles())
				fil.CopyTo(Path.Combine(dst, fil.Name), true);
		}

		/// <summary>Specifies which ModLoader category a file belongs to.</summary>
		public enum DataSource 
		{
			/// <summary>File belongs to EXEData.</summary>
			EXEData,
			/// <summary>File belongs to DLLData.</summary>
			DllData,
			/// <summary>File doesn't belong to any data category, and doesn't need an ini entry.</summary>
			Loose
		}

		public static string[] SADXSystemDLLFiles = { "ADV00MODELS", "ADV01CMODELS", "ADV01MODELS", "ADV02MODELS", "ADV03MODELS", "BOSSCHAOS0MODELS", "CHAOSTGGARDEN02MR_DAYTIME",
													  "CHAOSTGGARDEN02MR_EVENING", "CHAOSTGGARDEN02MR_NIGHT", "CHRMODELS_orig" };
	}
}

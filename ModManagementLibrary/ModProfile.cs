using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SA_Tools;
using System.IO;
using System.Windows.Forms;

namespace ModManagement
{
	/// <summary>
	/// Also known as mod.ini
	/// </summary>
	public class ModProfile
	{
		public string Name;
		public string Description;
		public string Author;
		public Dictionary<string, string> IgnoreFiles;
		public Dictionary<string, string> ReplaceFiles;
		public Dictionary<string, string> SwapFiles;
		public string EXEFile;
		public string EXEData;
		public List<string> DLLFiles;
		public string GameType;

		public ModProfile(string modFilePath)
		{
			StreamReader modIniFile = File.OpenText(modFilePath);

			DLLFiles = new List<string>();

			while(!modIniFile.EndOfStream)
			{
				string readLine = modIniFile.ReadLine();
				string[] modSplit = readLine.Split('=');

				if (modSplit.Length == 0) continue; // don't bother

				if (modSplit[0] == "Name") Name = modSplit[1];
				else if (modSplit[0] == "Description") Description = modSplit[1];
				else if (modSplit[0] == "Author") Author = modSplit[1];
				else if (modSplit[0] == "EXEFile") EXEFile = modSplit[1];
				else if (modSplit[0] == "EXEData") EXEData = modSplit[1];
				else if (modSplit[0] == "Game") GameType = modSplit[1];
				else
				{
					bool found = false;
					foreach (string dllFile in ModManagement.SADXSystemDLLFiles)
					{
						if (modSplit[0] == string.Concat(dllFile, "Data"))
						{
							DLLFiles.Add(modSplit[1]);
							found = true;
							break;
						}
					}

					// todo: check SA2 dlls here

					if (!found)
					{
						MessageBox.Show(string.Concat("Found unknown line: ", readLine, "while loading mod ini"));
					}
				}
			}
		}
	}
}

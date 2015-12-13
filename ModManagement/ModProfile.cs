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
		public List<string> DLLDataFiles;
		public List<string> DLLFiles;
		public string Game;

		public ModProfile(string modFilePath)
		{
			StreamReader modIniFile = File.OpenText(modFilePath);

			DLLDataFiles = new List<string>();
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
				else if (modSplit[0] == "Game") Game = modSplit[1];
				else if (modSplit[0] == "DLLFile") DLLFiles.Add(modSplit[1]);
				else
				{
					bool found = false;
					foreach (string dllFile in ModManagement.SADXSystemDLLFiles)
					{
						if (modSplit[0] == string.Concat(dllFile, "Data"))
						{
							DLLDataFiles.Add(modSplit[1]);
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

			modIniFile.Close();
		}

		public void Save(string modFilePath)
		{
			FileStream fileStream = File.OpenWrite(modFilePath);
			StreamWriter modIniFile = new StreamWriter(fileStream);

			modIniFile.WriteLine("Name=" + Name);
			if(Description.Length > 0) modIniFile.WriteLine("Description=" + Description);
			if(Author.Length > 0) modIniFile.WriteLine("Author=" + Author);
			if(EXEFile.Length > 0) modIniFile.WriteLine("EXEFile=" + EXEFile);
			if(EXEData.Length > 0) modIniFile.WriteLine("EXEData=" + EXEData);
			foreach (string DLLFile in DLLFiles) modIniFile.WriteLine("DLLFile=" + DLLFile);
			foreach (string DLLData in DLLDataFiles) modIniFile.WriteLine("DLLData=" + DLLData);
			if(Game.Length > 0) modIniFile.WriteLine("Game=" + Game);

			modIniFile.Close();
		}
	}
}

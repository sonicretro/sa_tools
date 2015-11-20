using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SA_Tools;
using SonicRetro.SAModel;
using ModManagement;
using SonicRetro.SAModel.SAEditorCommon.UI;

namespace ModGenerator
{
	public partial class ModBuilder : Form
	{
		private Properties.Settings Settings;

		#region Type Maps
		static readonly Dictionary<string, string> DataTypeList = new Dictionary<string, string>()
		{
			{ "landtable", "LandTable" },
			{ "model", "Model" },
			{ "basicmodel", "Basic Model" },
			{ "basicdxmodel", "Basic Model (SADX)" },
			{ "chunkmodel", "Chunk Model" },
			{ "action", "Action (animation+model)" },
			{ "animation", "Animation" },
			{ "objlist", "Object List" },
			{ "startpos", "Start Positions" },
			{ "texlist", "Texture List" },
			{ "leveltexlist", "Level Texture List" },
			{ "triallevellist", "Trial Level List" },
			{ "bosslevellist", "Boss Level List" },
			{ "fieldstartpos", "Field Start Positions" },
			{ "soundtestlist", "Sound Test List" },
			{ "musiclist", "Music List" },
			{ "soundlist", "Sound List" },
			{ "stringarray", "String Array" },
			{ "nextlevellist", "Next Level List" },
			{ "cutscenetext", "Cutscene Text" },
			{ "recapscreen", "Recap Screen" },
			{ "npctext", "NPC Text" },
			{ "levelclearflags", "Level Clear Flags" },
			{ "deathzone", "Death Zones" },
			{ "skyboxscale", "Skybox Scale" },
			{ "stageselectlist", "Stage Select List" },
			{ "levelrankscores", "Level Rank Scores" },
			{ "levelranktimes", "Level Rank Times" },
			{ "endpos", "End Positions" },
			{ "animationlist", "Animation List" },
			{ "levelpathlist", "Path List" },
			{ "stagelightdatalist", "Stage Light Data List" }
		};

		static readonly Dictionary<string, string> DLLTypeMap = new Dictionary<string, string>()
		{
			{ "landtable", "LandTable *" },
			{ "landtablearray", "LandTable **" },
			{ "model", "NJS_OBJECT *" },
			{ "modelarray", "NJS_OBJECT **" },
			{ "basicmodel", "NJS_OBJECT *" },
			{ "basicmodelarray", "NJS_OBJECT **" },
			{ "basicdxmodel", "NJS_OBJECT *" },
			{ "basicdxmodelarray", "NJS_OBJECT **" },
			{ "chunkmodel", "NJS_OBJECT *" },
			{ "chunkmodelarray", "NJS_OBJECT **" },
			{ "actionarray", "NJS_ACTION **" }
		};
		#endregion

		public ModBuilder()
		{
			InitializeComponent();
		}

		DataMapping exeData;
		List<DllIniData> dllIniFiles;
		Game gameType;
		string gameFolder;
		string projectName;
		string projectFolder;

		private void MainForm_Load(object sender, EventArgs e)
		{
			Settings = Properties.Settings.Default;

			sA2ToolStripMenuItem.Checked = !Settings.ModBuilderSADX;
			sADXToolStripMenuItem.Checked = Settings.ModBuilderSADX;

			dllIniFiles = new List<DllIniData>();
		}

		private void ModBuilder_Shown(object sender, EventArgs e)
		{
			SetGameFolder();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog a = new OpenFileDialog()
			{
				DefaultExt = "ini",
				Filter = "INI Files|*.ini|All Files|*.*"
			})
				if (a.ShowDialog(this) == DialogResult.OK)
					LoadExeDataMapping(a.FileName);
		}

		private void PickProject()
		{
			using (ProjectSelector projectSelector = new ProjectSelector(gameFolder))
			{
				if (projectSelector.NoProjects)
				{
					MessageBox.Show("No projects found. You can create a new project in the Mod Generator main menu, by clicking the \"Start a new project\" button.");
					return;
				}
				else
				{
					projectSelector.ShowDialog();
					projectName = projectSelector.SelectedProjectName;
					projectFolder = projectSelector.SelectedProjectPath;
				}
			}

			// Load mod info
			ModProfile modProfile = new ModProfile(Path.Combine(projectFolder, "mod.ini"));
			gameType = ModManagement.ModManagement.GameFromString(modProfile.Game); // todo: rectify this with MainForm_Load's game type checkboxes

			// add all the files from the DataMappings folder
			string dataMappingsFolder = Path.Combine(projectFolder, "DataMappings");
			string[] dataMappingFiles = Directory.GetFiles(dataMappingsFolder, "*.ini");

			using (ProgressDialog progress = new ProgressDialog("Loading data mappings...", dataMappingFiles.Length))
			{
				progress.SetTask("Loading data mapping:");
				progress.Show(this);
				progress.Update();

				foreach (string iniFile in dataMappingFiles)
				{
					progress.SetStep(iniFile.Replace(projectFolder, string.Empty));

					if (Path.GetFileName(iniFile) == "sonic_data.ini")
					{
						LoadExeDataMapping(iniFile);
					}
					else // load our dll mapping
					{
						LoadDLLDataMapping(iniFile);
					}

					progress.StepProgress();
					progress.Update();
				}
			}
		}

		#region Loading Data Mappings
		private void LoadDLLDataMapping(string filename)
		{
			DllIniData dataMapping = IniSerializer.Deserialize<DllIniData>(filename);
			Environment.CurrentDirectory = projectFolder;

			string groupName = Path.GetFileName(filename);
			listView1.Groups.Add(groupName, groupName);

			listView1.BeginUpdate();
			foreach (KeyValuePair<string, FileTypeHash> item in dataMapping.Files)
			{
				bool modified = HelperFunctions.FileHash(string.Concat(projectFolder, "\\", item.Key)) != item.Value.Hash;
				// I'm sure the type column should actually be useful. For now, you get a GUJJOBU.
				listView1.Items.Add(new ListViewItem(new[] { item.Key, "GUJJOBU", modified ? "Yes" : "No" }) { Checked = modified, Group = listView1.Groups[groupName] });
			}
			listView1.EndUpdate();

			dllIniFiles.Add(dataMapping);
		}

		private void LoadExeDataMapping(string filename)
		{
			exeData = IniSerializer.Deserialize<DataMapping>(filename);

			Environment.CurrentDirectory = Path.GetDirectoryName(filename);

			string groupName = Path.GetFileName(filename);
			listView1.Groups.Add(groupName, groupName);

			listView1.BeginUpdate();
			foreach (KeyValuePair<string, SA_Tools.FileInfo> item in exeData.Files)
			{
				string projectRelativeFileLocation = Path.Combine(projectFolder, item.Value.Filename); // getting project-relative location

				bool? modified = null;
				switch (item.Value.Type)
				{
					case "cutscenetext":
						{
							modified = false;
							string[] hashes = item.Value.MD5Hash.Split(',');
							for (int i = 0; i < 5; i++)
							{
								string textname = Path.Combine(projectRelativeFileLocation, ((Languages)i).ToString() + ".txt");
								if (HelperFunctions.FileHash(textname) != hashes[i])
								{
									modified = true;
									break;
								}
							}
						}
						break;
					case "recapscreen":
						{
							modified = false;
							int count = int.Parse(item.Value.CustomProperties["length"], NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
							string[] hash2 = item.Value.MD5Hash.Split(':');
							string[][] hashes = new string[hash2.Length][];
							for (int i = 0; i < hash2.Length; i++)
								hashes[i] = hash2[i].Split(',');
							for (int i = 0; i < count; i++)
								for (int l = 0; l < 5; l++)
								{
									string textname = Path.Combine(Path.Combine(projectRelativeFileLocation, (i + 1).ToString(NumberFormatInfo.InvariantInfo)), ((Languages)l).ToString() + ".ini");
									if (HelperFunctions.FileHash(textname) != hashes[i][l])
									{
										modified = true;
										break;
									}
								}
						}
						break;
					case "npctext":
						{
							modified = false;
							int count = int.Parse(item.Value.CustomProperties["length"], NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
							string[] hash2 = item.Value.MD5Hash.Split(':');
							string[][] hashes = new string[hash2.Length][];
							for (int i = 0; i < hash2.Length; i++)
								hashes[i] = hash2[i].Split(',');
							for (int l = 0; l < 5; l++)
								for (int i = 0; i < count; i++)
								{
									string textname = Path.Combine(Path.Combine(projectRelativeFileLocation, (i + 1).ToString(NumberFormatInfo.InvariantInfo)), ((Languages)l).ToString() + ".ini");
									if (HelperFunctions.FileHash(textname) != hashes[l][i])
									{
										modified = true;
										break;
									}
								}
						}
						break;
					case "deathzone":
						{
							modified = false;
							string[] hashes = item.Value.MD5Hash.Split(',');
							if (HelperFunctions.FileHash(projectRelativeFileLocation) != hashes[0])
							{
								modified = true;
								break;
							}
							DeathZoneFlags[] flags = DeathZoneFlagsList.Load(projectRelativeFileLocation);
							if (flags.Length != hashes.Length - 1)
							{
								modified = true;
								break;
							}
							string path = Path.GetDirectoryName(projectRelativeFileLocation);
							for (int i = 0; i < flags.Length; i++)
								if (HelperFunctions.FileHash(Path.Combine(path, i.ToString(NumberFormatInfo.InvariantInfo) + 
									(exeData.Game == Game.SA2 || exeData.Game == Game.SA2B ? ".sa2mdl" : ".sa1mdl"))) != hashes[i + 1])
								{
									modified = true;
									break;
								}
						}
						break;
					case "levelpathlist":
						{
							modified = false;
							Dictionary<string, string[]> hashes = new Dictionary<string, string[]>();
							string[] hash1 = item.Value.MD5Hash.Split('|');
							foreach (string hash in hash1)
							{
								string[] hash2 = hash.Split(':');
								hashes.Add(hash2[0], hash2[1].Split(','));
							}
							foreach (string dir in Directory.GetDirectories(projectRelativeFileLocation))
							{
								string name = new DirectoryInfo(dir).Name;
								if (!hashes.ContainsKey(name))
								{
									modified = true;
									break;
								}
							}
							if (modified.Value)
								break;
							foreach (KeyValuePair<string, string[]> dirinfo in hashes)
							{
								string dir = Path.Combine(projectRelativeFileLocation, dirinfo.Key);
								if (!Directory.Exists(dir))
								{
									modified = true;
									break;
								}
								if (Directory.GetFiles(dir, "*.ini").Length != dirinfo.Value.Length)
								{
									modified = true;
									break;
								}
								for (int i = 0; i < dirinfo.Value.Length; i++)
								{
									string file = Path.Combine(dir, i.ToString(NumberFormatInfo.InvariantInfo) + ".ini");
									if (!File.Exists(file))
									{
										modified = true;
										break;
									}
									else if (HelperFunctions.FileHash(file) != dirinfo.Value[i])
									{
										modified = true;
										break;
									}
								}
								if (modified.Value)
									break;
							}
						}
						break;
					default:
						if (!string.IsNullOrEmpty(item.Value.MD5Hash))
							modified = HelperFunctions.FileHash(projectRelativeFileLocation) != item.Value.MD5Hash;
						break;
				}
				ListViewItem newItem = new ListViewItem(new[] { item.Key, DataTypeList[item.Value.Type], modified.HasValue ? (modified.Value ? "Yes" : "No") : "Unknown" })
				{
					Checked = modified ?? true,
					Group = listView1.Groups[groupName]
				};
				listView1.Items.Add(newItem);
			}
			listView1.EndUpdate();
		}
		#endregion

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Save();
		}

		private void CheckAll_Click(object sender, EventArgs e)
		{
			listView1.BeginUpdate();
			foreach (ListViewItem item in listView1.Items)
				item.Checked = true;
			listView1.EndUpdate();
		}

		private void CheckModified_Click(object sender, EventArgs e)
		{
			listView1.BeginUpdate();
			foreach (ListViewItem item in listView1.Items)
				item.Checked = item.SubItems[2].Text != "No";
			listView1.EndUpdate();
		}

		private void UnCheckAll_Click(object sender, EventArgs e)
		{
			listView1.BeginUpdate();
			foreach (ListViewItem item in listView1.Items)
				item.Checked = false;
			listView1.EndUpdate();
		}

		private void EXEExportCPP(TextWriter writer, bool SA2)
		{
			writer.WriteLine("// Generated by SA Tools Struct Converter");
			writer.WriteLine();
			writer.WriteLine("#include \"stdafx.h\"");
			writer.WriteLine(SA2 ? "#include \"SA2ModLoader.h\"" : "#include \"SADXModLoader.h\"");
			writer.WriteLine();

			Dictionary<uint, string> pointers = new Dictionary<uint, string>();
			if (exeData != null)
			{
				uint imagebase = exeData.ImageBase ?? 0x400000;
				ModelFormat modelfmt = 0;
				LandTableFormat landfmt = 0;
				switch (exeData.Game)
				{
					case Game.SA1:
						modelfmt = ModelFormat.Basic;
						landfmt = LandTableFormat.SA1;
						break;

					case Game.SADX:
						modelfmt = ModelFormat.BasicDX;
						landfmt = LandTableFormat.SADX;
						break;

					case Game.SA2:
					case Game.SA2B:
						modelfmt = ModelFormat.Chunk;
						landfmt = LandTableFormat.SA2;
						break;
				}

				Dictionary<string, string> models = new Dictionary<string, string>();
				List<string> labels = new List<string>();
				foreach (KeyValuePair<string, SA_Tools.FileInfo> item in exeData.Files.Where((a, i) => listView1.CheckedIndices.Contains(i)))
				{
					string name = item.Key.MakeIdentifier();
					SA_Tools.FileInfo data = item.Value;

					string projectRelativeFileLocation = Path.Combine(projectFolder, data.Filename); // getting project-relative location

					switch (data.Type)
					{
						case "landtable":
							LandTable tbl = LandTable.LoadFromFile(projectRelativeFileLocation);
							name = tbl.Name;
							writer.WriteLine(tbl.ToStructVariables(landfmt, labels));
							labels.AddRange(tbl.GetLabels());
							break;

						case "model":
							NJS_OBJECT mdl = new ModelFile(projectRelativeFileLocation).Model;
							name = mdl.Name;
							writer.WriteLine(mdl.ToStructVariables(modelfmt == ModelFormat.BasicDX, labels));
							models.Add(item.Key, mdl.Name);
							labels.AddRange(mdl.GetLabels());
							break;

						case "basicmodel":
							mdl = new ModelFile(projectRelativeFileLocation).Model;
							name = mdl.Name;
							writer.WriteLine(mdl.ToStructVariables(false, labels));
							models.Add(item.Key, mdl.Name);
							labels.AddRange(mdl.GetLabels());
							break;

						case "basicdxmodel":
							mdl = new ModelFile(projectRelativeFileLocation).Model;
							name = mdl.Name;
							writer.WriteLine(mdl.ToStructVariables(true, labels));
							models.Add(item.Key, mdl.Name);
							labels.AddRange(mdl.GetLabels());
							break;

						case "chunkmodel":
							mdl = new ModelFile(projectRelativeFileLocation).Model;
							name = mdl.Name;
							writer.WriteLine(mdl.ToStructVariables(false, labels));
							models.Add(item.Key, mdl.Name);
							labels.AddRange(mdl.GetLabels());
							break;

						case "action":
							Animation ani = Animation.Load(projectRelativeFileLocation);
							name = "action_" + ani.Name.MakeIdentifier();
							if (!labels.Contains(name))
							{
								writer.WriteLine(ani.ToStructVariables());
								writer.WriteLine("NJS_ACTION {0} = {{ &{1}, &{2} }};", name, models[data.CustomProperties["model"]],
									ani.Name.MakeIdentifier());
								labels.Add(name);
							}
							break;

						case "animation":
							ani = Animation.Load(projectRelativeFileLocation);
							name = ani.Name.MakeIdentifier();
							if (!labels.Contains(name))
							{
								writer.WriteLine(ani.ToStructVariables());
								labels.Add(name);
							}
							break;

						case "objlist":
							{
								ObjectListEntry[] list = ObjectList.Load(projectRelativeFileLocation, SA2);
								writer.WriteLine("ObjectListEntry {0}_list[] = {{", name);
								List<string> objs = new List<string>(list.Length);
								foreach (ObjectListEntry obj in list)
									objs.Add(obj.ToStruct() + " " + obj.Name.ToComment());
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								writer.WriteLine();
								writer.WriteLine("ObjectList {0} = {{ arraylengthandptr({0}_list) }};", name);
								break;
							}

						case "startpos":
							if (SA2)
							{
								Dictionary<SA2LevelIDs, SA2StartPosInfo> list = SA2StartPosList.Load(projectRelativeFileLocation);
								writer.WriteLine("StartPosition {0}[] = {{", name);
								List<string> objs = new List<string>(list.Count + 1);
								foreach (KeyValuePair<SA2LevelIDs, SA2StartPosInfo> obj in list)
									objs.Add(obj.ToStruct());
								objs.Add("{ LevelIDs_Invalid }");
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
							}
							else
							{
								Dictionary<SA1LevelAct, SA1StartPosInfo> list = SA1StartPosList.Load(projectRelativeFileLocation);
								writer.WriteLine("StartPosition {0}[] = {{", name);
								List<string> objs = new List<string>(list.Count + 1);
								foreach (KeyValuePair<SA1LevelAct, SA1StartPosInfo> obj in list)
									objs.Add(obj.ToStruct());
								objs.Add("{ LevelIDs_Invalid }");
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
							}
							break;

						case "texlist":
							{
								TextureListEntry[] list = TextureList.Load(projectRelativeFileLocation);
								writer.WriteLine("PVMEntry {0}[] = {{", name);
								List<string> objs = new List<string>(list.Length + 1);
								foreach (TextureListEntry obj in list)
									objs.Add(obj.ToStruct());
								objs.Add("{ 0 }");
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						case "leveltexlist":
							{
								LevelTextureList list = LevelTextureList.Load(projectRelativeFileLocation);
								writer.WriteLine("PVMEntry {0}_list[] = {{", name);
								List<string> objs = new List<string>(list.TextureList.Length);
								foreach (TextureListEntry obj in list.TextureList)
									objs.Add(obj.ToStruct());
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								writer.WriteLine();
								writer.WriteLine("LevelPVMList {0} = {{ {1}, arraylengthandptr({0}_list) }};", name, list.Level.ToC());
								break;
							}

						case "triallevellist":
							{
								SA1LevelAct[] list = TrialLevelList.Load(projectRelativeFileLocation);
								writer.WriteLine("TrialLevelListEntry {0}_list[] = {{", name);
								List<string> objs = new List<string>(list.Length);
								foreach (SA1LevelAct obj in list)
									objs.Add(string.Format("{{ {0}, {1} }}", obj.Level.ToC("LevelIDs"), obj.Act));
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								writer.WriteLine();
								writer.WriteLine("TrialLevelList {0} = {{ arrayptrandlength({0}_list) }};", name);
								break;
							}

						case "bosslevellist":
							{
								SA1LevelAct[] list = BossLevelList.Load(projectRelativeFileLocation);
								writer.WriteLine("__int16 {0}[] = {{", name);
								List<string> objs = new List<string>(list.Length + 1);
								foreach (SA1LevelAct obj in list)
									objs.Add(obj.ToC());
								objs.Add("levelact(LevelIDs_Invalid, 0)");
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						case "fieldstartpos":
							{
								Dictionary<SA1LevelIDs, FieldStartPosInfo> list = FieldStartPosList.Load(projectRelativeFileLocation);
								writer.WriteLine("FieldStartPosition {0}[] = {{", name);
								List<string> objs = new List<string>(list.Count + 1);
								foreach (KeyValuePair<SA1LevelIDs, FieldStartPosInfo> obj in list)
									objs.Add(obj.ToStruct());
								objs.Add("{ LevelIDs_Invalid }");
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						case "soundtestlist":
							{
								SoundTestListEntry[] list = SoundTestList.Load(projectRelativeFileLocation);
								writer.WriteLine("SoundTestEntry {0}_list[] = {{", name);
								List<string> objs = new List<string>(list.Length);
								foreach (SoundTestListEntry obj in list)
									objs.Add(obj.ToStruct() + " " + obj.Title.ToComment());
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								writer.WriteLine();
								writer.WriteLine("SoundTestCategory {0} = {{ arrayptrandlength({0}_list) }};", name);
								break;
							}

						case "musiclist":
							{
								MusicListEntry[] list = MusicList.Load(projectRelativeFileLocation);
								writer.WriteLine("MusicInfo {0}[] = {{", name);
								List<string> objs = new List<string>(list.Length);
								foreach (MusicListEntry obj in list)
									objs.Add(obj.ToStruct());
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						case "soundlist":
							{
								SoundListEntry[] list = SoundList.Load(projectRelativeFileLocation);
								writer.WriteLine("SoundFileInfo {0}_list[] = {{", name);
								List<string> objs = new List<string>(list.Length);
								foreach (SoundListEntry obj in list)
									objs.Add(obj.ToStruct());
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								writer.WriteLine();
								writer.WriteLine("SoundList {0} = {{ arraylengthandptr({0}_list) }};", name);
								break;
							}

						case "stringarray":
							{
								string[] strs = StringArray.Load(projectRelativeFileLocation);
								Languages lang = Languages.Japanese;
								if (data.CustomProperties.ContainsKey("language"))
									lang = (Languages)Enum.Parse(typeof(Languages), data.CustomProperties["language"], true);
								writer.WriteLine("char *{0}[] = {{", name);
								List<string> objs = new List<string>(strs.Length);
								foreach (string obj in strs)
									objs.Add(obj.ToC(lang) + " " + obj.ToComment());
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						case "nextlevellist":
							{
								NextLevelListEntry[] list = NextLevelList.Load(projectRelativeFileLocation);
								writer.WriteLine("NextLevelData {0}[] = {{", name);
								List<string> objs = new List<string>(list.Length + 1);
								foreach (NextLevelListEntry obj in list)
									objs.Add(obj.ToStruct());
								objs.Add("{ 0, -1 }");
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						case "cutscenetext":
							{
								CutsceneText texts = new CutsceneText(projectRelativeFileLocation);
								uint addr = (uint)(data.Address + imagebase);
								for (int j = 0; j < 5; j++)
								{
									string[] strs = texts.Text[j];
									Languages lang = (Languages)j;
									writer.WriteLine("char *{0}_{1}[] = {{", name, lang);
									writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", strs.Select((a) => a.ToC(lang) + " " + a.ToComment()).ToArray()));
									writer.WriteLine("};");
									writer.WriteLine();
									pointers.Add(addr, string.Format("{0}_{1}", name, lang));
									addr += 4;
								}
								break;
							}

						case "recapscreen":
							{
								uint addr = (uint)(data.Address + imagebase);
								RecapScreen[][] texts = RecapScreenList.Load(projectRelativeFileLocation, int.Parse(data.CustomProperties["length"], NumberStyles.Integer, NumberFormatInfo.InvariantInfo));
								for (int l = 0; l < 5; l++)
									for (int j = 0; j < texts.Length; j++)
									{
										writer.WriteLine("char *{0}_{1}_{2}_Text[] = {{", name, (Languages)l, j);
										writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", texts[j][l].Text.Split('\n').Select((a) => a.ToC((Languages)l) + " " + a.ToComment()).ToArray()));
										writer.WriteLine("};");
										writer.WriteLine();
									}
								for (int l = 0; l < 5; l++)
								{
									writer.WriteLine("RecapScreen {0}_{1}[] = {{", name, (Languages)l);
									List<string> objs = new List<string>(texts.Length);
									for (int j = 0; j < texts.Length; j++)
									{
										RecapScreen scr = texts[j][l];
										objs.Add(string.Format("{{ {0}, arraylengthandptr({1}_{2}_{3}_Text) }}",
											HelperFunctions.ToC(scr.Speed), name, (Languages)l, j));
									}
									writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
									writer.WriteLine("};");
									writer.WriteLine();
									pointers.Add(addr, string.Format("{0}_{1}", name, (Languages)l));
									addr += 4;
								}
								break;
							}

						case "npctext":
							{
								NPCText[][] texts = NPCTextList.Load(projectRelativeFileLocation, int.Parse(data.CustomProperties["length"], NumberStyles.Integer, NumberFormatInfo.InvariantInfo));
								uint headaddr = (uint)(data.Address + imagebase);
								for (int l = 0; l < 5; l++)
								{
									for (int j = 0; j < texts[l].Length; j++)
									{
										if (texts[l][j].Groups.Count == 0)
											continue;
										if (texts[l][j].HasControl)
										{
											writer.WriteLine("__int16 {0}_{1}_{2}_Control[] = {{", name, (Languages)l, j);
											bool first = true;
											List<string> objs = new List<string>();
											foreach (NPCTextGroup group in texts[l][j].Groups)
											{
												if (!first)
													objs.Add(NPCTextControl.NewGroup.ToC());
												else
													first = false;
												foreach (ushort flag in group.EventFlags)
												{
													objs.Add(NPCTextControl.EventFlag.ToC());
													objs.Add(flag.ToCHex());
												}
												foreach (ushort flag in group.NPCFlags)
												{
													objs.Add(NPCTextControl.NPCFlag.ToC());
													objs.Add(flag.ToCHex());
												}
												if (group.Character != (SA1CharacterFlags)0xFF)
												{
													objs.Add(NPCTextControl.Character.ToC());
													objs.Add(group.Character.ToC("CharacterFlags"));
												}
												if (group.Voice.HasValue)
												{
													objs.Add(NPCTextControl.Voice.ToC());
													objs.Add(group.Voice.Value.ToString());
												}
												if (group.SetEventFlag.HasValue)
												{
													objs.Add(NPCTextControl.SetEventFlag.ToC());
													objs.Add(group.SetEventFlag.Value.ToCHex());
												}
											}
											objs.Add(NPCTextControl.End.ToC());
											writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
											writer.WriteLine("};");
											writer.WriteLine();
										}
										if (texts[l][j].HasText)
										{
											writer.WriteLine("HintText_Text {0}_{1}_{2}_Text[] = {{", name, (Languages)l, j);
											List<string> objs = new List<string>();
											foreach (NPCTextGroup group in texts[l][j].Groups)
											{
												foreach (NPCTextLine line in group.Lines)
													objs.Add(line.ToStruct((Languages)l) + " " + line.Line.ToComment());
												objs.Add("{ 0 }");
											}
											writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
											writer.WriteLine("};");
											writer.WriteLine();
										}
									}
								}
								for (int l = 0; l < 5; l++)
								{
									if (l > 0)
										writer.WriteLine();
									writer.WriteLine("HintText_Entry {0}_{1}[] = {{", name, (Languages)l);
									List<string> objs = new List<string>();
									for (int j = 0; j < texts[l].Length; j++)
									{
										if (texts[l][j].Groups.Count == 0)
										{
											objs.Add("{ 0 }");
											continue;
										}
										StringBuilder line = new StringBuilder("{ ");
										if (texts[l][j].HasControl)
											line.AppendFormat("{0}_{1}_{2}_Control", name, (Languages)l, j);
										else
											line.Append("NULL");
										line.Append(", ");
										if (texts[l][j].HasText)
											line.AppendFormat("{0}_{1}_{2}_Text", name, (Languages)l, j);
										else
											line.Append("NULL");
										line.Append(" }");
										objs.Add(line.ToString());
									}
									writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
									writer.WriteLine("};");
									pointers.Add(headaddr, string.Format("{0}_{1}", name, (Languages)l));
									headaddr += 4;
								}
								break;
							}

						case "levelclearflags":
							{
								LevelClearFlag[] list = LevelClearFlagList.Load(projectRelativeFileLocation);
								writer.WriteLine("LevelClearFlagData {0}[] = {{", name);
								List<string> objs = new List<string>(list.Length);
								foreach (LevelClearFlag obj in list)
									objs.Add(obj.ToStruct());
								objs.Add("{ -1 }");
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						case "deathzone":
							{
								DeathZoneFlags[] list = DeathZoneFlagsList.Load(projectRelativeFileLocation);
								string path = Path.GetDirectoryName(projectRelativeFileLocation);
								List<string> mdls = new List<string>(list.Length);
								List<string> objs = new List<string>();
								for (int j = 0; j < list.Length; j++)
								{
									NJS_OBJECT obj = new ModelFile(Path.Combine(path,
										j.ToString(NumberFormatInfo.InvariantInfo) + ".sa1mdl")).Model;
									writer.WriteLine(obj.ToStructVariables(modelfmt == ModelFormat.BasicDX, objs));
									mdls.Add(obj.Name);
									objs.Clear();
								}
								writer.WriteLine("DeathZone {0}[] = {{", name);
								for (int j = 0; j < list.Length; j++)
									objs.Add(string.Format("{{ {0}, &{1} }}", list[j].Flags.ToC("CharacterFlags"), mdls[j]));
								objs.Add("{ 0 }");
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						case "skyboxscale":
							{
								uint headaddr = (uint)(data.Address + imagebase);
								int cnt = int.Parse(data.CustomProperties["count"], NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
								SkyboxScale[] sclini = SkyboxScaleList.Load(projectRelativeFileLocation);
								for (int j = 0; j < cnt; j++)
								{
									writer.WriteLine("SkyboxScale {0}_{1} = {2};", name, j, sclini[j].ToStruct());
									pointers.Add(headaddr, string.Format("{0}_{1}", name, j));
									headaddr += 4;
								}
								break;
							}

						case "stageselectlist":
							{
								StageSelectLevel[] list = StageSelectLevelList.Load(projectRelativeFileLocation);
								writer.WriteLine("StageSelectLevel {0}[] = {{", name);
								List<string> objs = new List<string>(list.Length);
								foreach (StageSelectLevel obj in list)
									objs.Add(obj.ToStruct());
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						case "levelrankscores":
							{
								Dictionary<SA2LevelIDs, LevelRankScores> list = LevelRankScoresList.Load(projectRelativeFileLocation);
								writer.WriteLine("LevelRankScores {0}[] = {{", name);
								List<string> objs = new List<string>(list.Count);
								foreach (KeyValuePair<SA2LevelIDs, LevelRankScores> obj in list)
									objs.Add(obj.ToStruct());
								objs.Add("{ LevelIDs_Invalid }");
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						case "levelranktimes":
							{
								Dictionary<SA2LevelIDs, LevelRankTimes> list = LevelRankTimesList.Load(projectRelativeFileLocation);
								writer.WriteLine("LevelRankTimes {0}[] = {{", name);
								List<string> objs = new List<string>(list.Count);
								foreach (KeyValuePair<SA2LevelIDs, LevelRankTimes> obj in list)
									objs.Add(obj.ToStruct());
								objs.Add("{ LevelIDs_Invalid }");
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						case "endpos":
							{
								Dictionary<SA2LevelIDs, SA2EndPosInfo> list = SA2EndPosList.Load(projectRelativeFileLocation);
								writer.WriteLine("LevelEndPosition {0}[] = {{", name);
								List<string> objs = new List<string>(list.Count);
								foreach (KeyValuePair<SA2LevelIDs, SA2EndPosInfo> obj in list)
									objs.Add(obj.ToStruct());
								objs.Add("{ LevelIDs_Invalid }");
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						case "animationlist":
							{
								SA2AnimationInfo[] list = SA2AnimationInfoList.Load(projectRelativeFileLocation);
								writer.WriteLine("AnimationInfo {0}[] = {{", name);
								List<string> objs = new List<string>(list.Length);
								foreach (SA2AnimationInfo obj in list)
									objs.Add(obj.ToStruct());
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						case "levelpathlist":
							{
								List<SA1LevelAct> levels = new List<SA1LevelAct>();
								foreach (string dir in Directory.GetDirectories(projectRelativeFileLocation))
								{
									SA1LevelAct level;
									try { level = new SA1LevelAct(new DirectoryInfo(dir).Name); }
									catch { continue; }
									levels.Add(level);
									List<PathData> paths = PathList.Load(dir);
									for (int i = 0; i < paths.Count; i++)
									{
										writer.WriteLine("Loop {0}_{1}_{2}_Entries[] = {{", name, level.ToString().MakeIdentifier(), i);
										List<string> objs = new List<string>(paths[i].Path.Count);
										foreach (PathDataEntry entry in paths[i].Path)
											objs.Add(entry.ToStruct());
										writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
										writer.WriteLine("};");
										writer.WriteLine();
										writer.WriteLine("LoopHead {0}_{1}_{2} = {{ {3}, LengthOfArray({0}_{1}_{2}_Entries), {4}, {0}_{1}_{2}_Entries, (ObjectFuncPtr){5} }};",
											name, level.ToString().MakeIdentifier(), i, paths[i].Unknown,
											HelperFunctions.ToC(paths[i].TotalDistance),
											HelperFunctions.ToCHex(paths[i].Code));
										writer.WriteLine();
									}
									writer.WriteLine("LoopHead *{0}_{1}[] = {{", name, level.ToString().MakeIdentifier());
									for (int i = 0; i < paths.Count; i++)
										writer.WriteLine("\t&{0}_{1}_{2},", name, level.ToString().MakeIdentifier(), i);
									writer.WriteLine("\tNULL");
									writer.WriteLine("};");
									writer.WriteLine();
								}
								writer.WriteLine("PathDataPtr {0}[] = {{", name);
								foreach (SA1LevelAct level in levels)
									writer.WriteLine("\t{{ {0}, {1}_{2} }},", level.ToC(), name,
										level.ToString().MakeIdentifier());
								writer.WriteLine("\t{ 0xFFFF }");
								writer.WriteLine("};");
								writer.WriteLine();
								break;
							}

						case "stagelightdatalist":
							{
								List<SA1StageLightData> list = SA1StageLightDataList.Load(projectRelativeFileLocation);
								writer.WriteLine("StageLightData {0}[] = {{", name);
								List<string> objs = new List<string>(list.Count + 1);
								foreach (SA1StageLightData obj in list)
									objs.Add(obj.ToStruct());
								objs.Add("{ 0xFFu }");
								writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", objs.ToArray()));
								writer.WriteLine("};");
								break;
							}

						default:
							throw new NotImplementedException("Unexpected value type");
					}
					writer.WriteLine();
					if (data.PointerList != null && data.PointerList.Length > 0)
						foreach (int ptr in data.PointerList)
							pointers.Add((uint)(ptr + imagebase), name);
				}
			}
			writer.WriteLine("PointerInfo pointers[] = {");
			List<string> ptrs = new List<string>(pointers.Count);
			foreach (KeyValuePair<uint, string> ptr in pointers)
				ptrs.Add(string.Format("ptrdecl({0}, &{1})", HelperFunctions.ToCHex(ptr.Key), ptr.Value));
			writer.WriteLine("\t" + string.Join("," + Environment.NewLine + "\t", ptrs.ToArray()));
			writer.WriteLine("};");
			writer.WriteLine();
		}

		private void ExportOldCPP_Click(object sender, EventArgs e)
		{
			bool SA2 = ((gameType == Game.SA2) || (gameType == Game.SA2B));
			using (SaveFileDialog fileDialog = new SaveFileDialog() { DefaultExt = "cpp", Filter = "C++ source files|*.cpp", InitialDirectory = Path.Combine(projectFolder, "src"), RestoreDirectory = true })
			{
				if (fileDialog.ShowDialog(this) == DialogResult.OK)
				{
					using (TextWriter writer = File.CreateText(fileDialog.FileName))
					{
						EXEExportCPP(writer, SA2);
						writer.WriteLine("extern \"C\" __declspec(dllexport) const ModInfo {0}ModInfo = {{ ModLoaderVer, NULL, NULL, 0, NULL, 0, NULL, 0, arrayptrandlength(pointers) }};", SA2 ? "SA2" : "SADX");
					}
				}
			}
		}

		private void ExportNewCPP_Click(object sender, EventArgs e)
		{
			bool SA2 = ((gameType == Game.SA2) || (gameType == Game.SA2B));
			using (SaveFileDialog fileDialog = new SaveFileDialog() { DefaultExt = "cpp", Filter = "C++ source files|*.cpp", InitialDirectory = Path.Combine(projectFolder, "src"), RestoreDirectory = true })
			{
				if (fileDialog.ShowDialog(this) == DialogResult.OK)
				{
					using (TextWriter writer = File.CreateText(fileDialog.FileName))
					{
						EXEExportCPP(writer, SA2);
						writer.WriteLine("extern \"C\" __declspec(dllexport) const PointerList Pointers = { arrayptrandlength(pointers) };");
						writer.WriteLine();
						writer.WriteLine("extern \"C\" __declspec(dllexport) const ModInfo {0}ModInfo = {{ ModLoaderVer }};", SA2 ? "SA2" : "SADX");
					}
				}
			}
		}

		private void INIExport_Click(object sender, EventArgs e)
		{
			string destinationFolder = Path.Combine(Path.Combine(gameFolder, "mods"), projectName);

			#region Generate our EXEData ini file
			// get our list of strings from the listview box
			List<string> files = new List<string>();
			foreach (ListViewItem item in listView1.Items)
			{
				if (item.Checked) files.Add(item.Text);
			}

			DataMapping output = ModManagement.ModManagement.ExeDataINIFromList(files, projectFolder, destinationFolder, exeData);

			IniSerializer.Serialize(output, Path.Combine(destinationFolder, "exeData.ini")); // todo: put a copy of this in the project folder so that sadxlvl2 knows what to copy
																							 // for it's built-in 'test/play' feature*/
			#endregion

			#region Add EXEData to mod.ini & copy to mod folder
			string modIniFile = Path.Combine(projectFolder, "mod.ini");
			StreamReader modReader = File.OpenText(modIniFile);
			List<string> modINILines = new List<string>();

			while (!modReader.EndOfStream)
			{
				modINILines.Add(modReader.ReadLine());
			}

			modReader.Close();

			int lineIndex = 0;
			bool replaceLine = false;
			bool addLine = true;
			foreach (string line in modINILines)
			{
				if (line.ToLowerInvariant() == "EXEData=exeData.ini".ToLowerInvariant())
				{
					addLine = false;
					replaceLine = false;
					break;
				}
				else
				{
					string[] linesSplit = line.Split('=');
					if (linesSplit[0].ToLowerInvariant() == "EXEData".ToLowerInvariant())
					{
						replaceLine = true;
						addLine = false;
						lineIndex = modINILines.FindIndex(item => item == line);
						break;
					}
				}
			}

			if (addLine) modINILines.Add("EXEData=exeData.ini");
			else if (replaceLine) modINILines[lineIndex] = "EXEData=exeData.ini";

			if (addLine || replaceLine) // we need to output our updated lines to the file
			{
				StreamWriter modWriter = new StreamWriter(modIniFile, false, Encoding.Default);
				foreach (string line in modINILines) modWriter.WriteLine(line);

				modWriter.Flush();
				modWriter.Close();
			}

			// copy mod.ini to mod folder
			File.Copy(modIniFile, Path.Combine(destinationFolder, "mod.ini"), true);
			#endregion

			MessageBox.Show("Done!");
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("This program allows you to convert split data into C code, that can be compiled into a DLL file for ModLoader.\nTo switch games, use the checkboxes in the File->Game menu.");
		}

		private void sADXToolStripMenuItem_Click(object sender, EventArgs e)
		{
			sA2ToolStripMenuItem.CheckState = CheckState.Unchecked;

			// todo: switch our game mode, then bring up the projects pop-up so the user can select.
			string errorMessage = "None supplied";

			if (Properties.Settings.Default.SADXPath == "" || (!ProjectSelector.VerifyGamePath(Game.SADX, Properties.Settings.Default.SADXPath, out errorMessage)))
			{
				// show an error message that the sadx path is invalid, ask for a new one.
				DialogResult lookForNewPath = MessageBox.Show(string.Format("The on-record SADX game directory doesn't appear to be valid because: {0}\nOK to supply one, Cancel to ignore.", errorMessage), "Directory Warning", MessageBoxButtons.OKCancel);
				if (lookForNewPath == DialogResult.OK)
				{
					if (folderBrowser.ShowDialog() == DialogResult.OK) Properties.Settings.Default.SADXPath = folderBrowser.SelectedPath;
				}
			}

			Properties.Settings.Default.ModBuilderSADX = true;

			Properties.Settings.Default.Save();
			SetGameFolder();
		}

		private void sA2ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			sADXToolStripMenuItem.CheckState = CheckState.Unchecked;

			// todo: switch our game mode, then bring up the projects pop-up so the user can select.
			string errorMessage = "None supplied";

			if (Properties.Settings.Default.SA2Path == "" || (!ProjectSelector.VerifyGamePath(Game.SA2B, Properties.Settings.Default.SA2Path, out errorMessage)))
			{
				// show an error message that the sadx path is invalid, ask for a new one.
				DialogResult lookForNewPath = MessageBox.Show(string.Format("The on-record SA2PC game directory doesn't appear to be valid because: {0}\nOK to supply one, Cancel to ignore.", errorMessage), "Directory Warning", MessageBoxButtons.OKCancel);
				if (lookForNewPath == DialogResult.OK)
				{
					if (folderBrowser.ShowDialog() == DialogResult.OK) Properties.Settings.Default.SA2Path = folderBrowser.SelectedPath;
				}
			}

			Properties.Settings.Default.ModBuilderSADX = false;

			Properties.Settings.Default.Save();
			SetGameFolder();
		}

		private void SetGameFolder()
		{
			if (sADXToolStripMenuItem.Checked) gameFolder = Properties.Settings.Default.SADXPath;
			else if (sA2ToolStripMenuItem.Checked) gameFolder = Properties.Settings.Default.SA2Path;

			PickProject();
		}

		private void projectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PickProject();
		}
	}

	static class Extensions
	{
		public static string MakeIdentifier(this string name)
		{
			StringBuilder result = new StringBuilder();
			foreach (char item in name)
				if ((item >= '0' & item <= '9') | (item >= 'A' & item <= 'Z') | (item >= 'a' & item <= 'z') | item == '_')
					result.Append(item);
			if (result[0] >= '0' & result[0] <= '9')
				result.Insert(0, '_');
			return result.ToString();
		}

		internal static List<string> GetLabels(this LandTable land)
		{
			List<string> labels = new List<string>() { land.Name };
			if (land.COLName != null)
			{
				labels.Add(land.COLName);
				foreach (COL col in land.COL)
					if (col.Model != null)
						labels.AddRange(col.Model.GetLabels());
			}
			if (land.AnimName != null)
			{
				labels.Add(land.AnimName);
				foreach (GeoAnimData gan in land.Anim)
				{
					if (gan.Model != null)
						labels.AddRange(gan.Model.GetLabels());
					if (gan.Animation != null)
						labels.Add(gan.Animation.Name);
				}
			}
			return labels;
		}

		internal static List<string> GetLabels(this NJS_OBJECT obj)
		{
			List<string> labels = new List<string>() { obj.Name };
			if (obj.Attach != null)
				labels.AddRange(obj.Attach.GetLabels());
			if (obj.Children != null)
				foreach (NJS_OBJECT o in obj.Children)
					labels.AddRange(o.GetLabels());
			return labels;
		}

		internal static List<string> GetLabels(this Attach att)
		{
			List<string> labels = new List<string>() { att.Name };
			if (att is BasicAttach)
			{
				BasicAttach bas = (BasicAttach)att;
				if (bas.VertexName != null)
					labels.Add(bas.VertexName);
				if (bas.NormalName != null)
					labels.Add(bas.NormalName);
				if (bas.MaterialName != null)
					labels.Add(bas.MaterialName);
				if (bas.MeshName != null)
					labels.Add(bas.MeshName);
			}
			else if (att is ChunkAttach)
			{
				ChunkAttach cnk = (ChunkAttach)att;
				if (cnk.VertexName != null)
					labels.Add(cnk.VertexName);
				if (cnk.PolyName != null)
					labels.Add(cnk.PolyName);
			}
			return labels;
		}
	}
}
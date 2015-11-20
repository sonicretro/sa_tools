using System.Collections.Generic;
using System.ComponentModel;
using SA_Tools;
using IniFile;

namespace SonicRetro.SAModel.SAEditorCommon
{
	/// <summary>
	/// Formerly known as 'IniData'. This class stores everything the editor needs to know about the game's 'universe', such as where all of the levels are stored,
	/// where the character models are, texture lists, etc.
	/// </summary>
	public class EditorUniverseData
	{
		public static EditorUniverseData Load(string filename)
		{
			return IniSerializer.Deserialize<EditorUniverseData>(filename);
		}

		public void Save(string filename)
		{
			IniSerializer.Serialize(this, filename);
		}

		public string SystemPath { get; set; }
		public string ObjectDefinitions { get; set; }
		public string ObjectTextureList { get; set; }
		public string LevelTextureLists { get; set; }
		public string MissionObjectList { get; set; }
		public string MissionTextureList { get; set; }
		public string Paths { get; set; }
		[IniName("Chars_")]
		[IniCollection(IniCollectionMode.NoSquareBrackets)]
		public Dictionary<string, EditorCharacterInfo> Characters { get; set; }
		[IniCollection(IniCollectionMode.IndexOnly)]
		public Dictionary<string, EditorLevelData> Levels { get; set; }
	}

	public class EditorCharacterInfo
	{
		public string Model { get; set; }
		public string Textures { get; set; }
		public string TextureList { get; set; }
		public float Height { get; set; }
		public string StartPositions { get; set; }
	}

	public class EditorLevelData
	{
		public string LevelGeometry { get; set; }
		[DefaultValue("0000")]
		public string LevelID { get; set; }
		public string SETName { get; set; }
		[IniCollection(IniCollectionMode.SingleLine, Format = ",")]
		public string[] Textures { get; set; }
		public string ObjectList { get; set; }
		public string ObjectTextureList { get; set; }
		public string DeathZones { get; set; }
		public string Effects { get; set; }
	}
}

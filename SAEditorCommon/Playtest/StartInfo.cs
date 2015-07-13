using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SA_Tools;

namespace SonicRetro.SAModel.SAEditorCommon.Playtest
{
	/// <summary> When playtesting, use this information to set up the session.</summary>
	[Serializable]
	public class StartInfo
	{
		[IniName("levelact")]
		public SA1LevelAct LevelAct { get; set; } // indicates the level and act to load
		[IniName("startingposition")]
		public SA1StartPosInfo StartingPosition { get; set; } // where in the world we will start
		[IniName("character")]
		public SA1Characters Character { get; set; }
		[IniName("allupgrades")]
		public bool AllUpgrades { get; set; } // if true, modloader will enable all the character's upgrades when starting

		public StartInfo(SA1LevelAct levelAct, SA1StartPosInfo startingPosition, bool allUpgrades, SA1Characters character)
		{
			LevelAct = levelAct;
			StartingPosition = startingPosition;
			AllUpgrades = allUpgrades;
			Character = character;
		}
	}
}

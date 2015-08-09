using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SA_Tools;

namespace ModManagement
{
	public class LoaderInfo
	{
		public bool DebugConsole { get; set; }
		public bool DebugScreen { get; set; }
		public bool DebugFile { get; set; }
		public bool? ShowConsole { get { return null; } set { if (value.HasValue) DebugConsole = value.Value; } }
		public bool DontFixWindow { get; set; }
		public bool DisableCDCheck { get; set; }
		public bool UseCustomResolution { get; set; }
		public int HorizontalResolution { get; set; }
		public int VerticalResolution { get; set; }
		public bool ForceAspectRatio { get; set; }
		public bool WindowedFullscreen { get; set; }
		public bool PauseWhenInactive { get; set; }
		public bool StretchFullscreen { get; set; }
		public int ScreenNum { get; set; }
		[IniName("Mod")]
		[IniCollection(IniCollectionMode.NoSquareBrackets, StartIndex = 1)]
		public List<string> Mods { get; set; }
		[IniName("Code")]
		[IniCollection(IniCollectionMode.NoSquareBrackets, StartIndex = 1)]
		public List<string> EnabledCodes { get; set; }

		public LoaderInfo()
		{
			Mods = new List<string>();
			EnabledCodes = new List<string>();
		}
	}
}

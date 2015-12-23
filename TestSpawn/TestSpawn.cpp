#include "stdafx.h"

#include <iostream>
#include <SADXModLoader.h>
#include <shellapi.h>	// for CommandLineToArgvW
#include <string>
#include <vector>

static const void* jumpAddress = (const void*)0x0040C318;

__declspec(naked) void ForceTrialMode()
{
	__asm
	{
		mov GameMode, GameModes_Trial
		jmp jumpAddress
	}
}

extern "C"
{
	__declspec(dllexport) ModInfo SADXModInfo = { ModLoaderVer };

	__declspec(dllexport) void __cdecl Init(const char* path, const HelperFunctions& helperFunctions)
	{
		int argc = 0;
		LPWSTR* argv = CommandLineToArgvW(GetCommandLineW(), &argc);
		bool spawnFlag = false;
		
		// Prevents CurrentCharacter from being overwritten. There could be other side effects,
		// but there are none that I've noticed thus far.
		std::vector<Uint8> nop(5, 0x90);
		WriteData((void*)0x00415007, nop.data(), nop.size());

		// TODO: Start position via command line and/or ini.
		for (int i = 1; i < argc; i++)
		{
			if (!wcscmp(argv[i], L"-testspawn"))
			{
				WriteJump((void*)0x0040C115, ForceTrialMode);
				spawnFlag = true;
				continue;
			}

			if (!spawnFlag)
				continue;

			if (!wcscmp(argv[i], L"-level"))
			{
				CurrentLevel = _wtoi(argv[++i]);
				PrintDebug("Loading level: %d\n", CurrentLevel);
			}
			else if (!wcscmp(argv[i], L"-act"))
			{
				CurrentAct = _wtoi(argv[++i]);
				PrintDebug("Loading act: %d\n", CurrentAct);
			}
			else if (!wcscmp(argv[i], L"-character"))
			{
				CurrentCharacter = _wtoi(argv[++i]);
				PrintDebug("Loading character: %d\n", CurrentCharacter);
			}
		}

		LocalFree(argv);
		return;

		/*
#ifdef _DEBUG
		PrintDebug("Loading SpawnData\n");
#endif

		// TODO: GetPrivateProfile*
		std::string startDataPath = (std::string)path + "\\StartData.ini";
		std::ifstream startDataStream(startDataPath);

		if (startDataStream.fail())
		{
			PrintDebug("Loading spawn data failed!\n");
		}
		else
		{
			while (!startDataStream.eof())
			{
				char currentLine[256];
				startDataStream.getline(currentLine, 256);

				if (strlen(currentLine) == 0) continue;

				char nameBuffer[256];
				char* name = nameBuffer;
				char valueBuffer[256];
				char* value = valueBuffer;
				name = strtok(currentLine, "=");
				value = strtok(NULL, "=");

				if (strcmp(name, "level") == 0)
				{
					CurrentLevel = atoi(value);
#ifdef _DEBUG
					PrintDebug("Loading level: %d\n", CurrentLevel);
#endif
				}
				else if (strcmp(name, "act") == 0)
				{
					CurrentAct = atoi(value);
#ifdef _DEBUG
					PrintDebug("Loading act: %d\n", CurrentAct);
#endif
				}
				else if (strcmp(name, "character") == 0)
				{
					CurrentCharacter = atoi(value);
#ifdef _DEBUG
					PrintDebug("Loading character: %d\n", CurrentCharacter);
#endif
				}
			}

			startDataStream.close();

#ifdef _DEBUG
			PrintDebug("Finished Loading Spawn!\n");
#endif
		}
		*/
	}
}
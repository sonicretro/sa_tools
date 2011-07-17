﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SonicRetro.SAModel
{
    public class LandTable
    {
        public List<COL> COL { get; set; }
        public List<GeoAnimData> Anim { get; set; }
        public int Flags { get; set; }
        public float Unknown1 { get; set; }
        public string TextureFileName { get; set; }
        public uint TextureList { get; set; }
        public int Unknown2 { get; set; }
        public int Unknown3 { get; set; }
        public string Name { get; set; }

        public static int Size { get { return 0x24; } }

        public LandTable(byte[] file, int address, uint imageBase, bool DX)
        {
            Name = "landtable_" + address.ToString("X8");
            short colcnt = BitConverter.ToInt16(file, address);
            short anicnt = BitConverter.ToInt16(file, address + 2);
            Flags = BitConverter.ToInt32(file, address + 4);
            Unknown1 = BitConverter.ToSingle(file, address + 8);
            COL = new List<COL>();
            uint tmpaddr = BitConverter.ToUInt32(file, address + 0xC);
            if (tmpaddr != 0)
            {
                tmpaddr -= imageBase;
                for (int i = 0; i < colcnt; i++)
                {
                    COL.Add(new COL(file, (int)tmpaddr, imageBase, DX));
                    tmpaddr += (uint)SAModel.COL.Size;
                }
            }
            Anim = new List<GeoAnimData>();
            tmpaddr = BitConverter.ToUInt32(file, address + 0x10);
            if (tmpaddr != 0)
            {
                tmpaddr -= imageBase;
                for (int i = 0; i < anicnt; i++)
                {
                    Anim.Add(new GeoAnimData(file, (int)tmpaddr, imageBase, DX));
                    tmpaddr += (uint)GeoAnimData.Size;
                }
            }
            tmpaddr = BitConverter.ToUInt32(file, address + 0x14);
            if (tmpaddr != 0)
            {
                tmpaddr -= imageBase;
                StringBuilder sb = new StringBuilder();
                while (file[tmpaddr] != 0)
                    sb.Append((char)file[tmpaddr]);
                TextureFileName = sb.ToString();
            }
            TextureList = BitConverter.ToUInt32(file, address + 0x18);
            Unknown2 = BitConverter.ToInt32(file, address + 0x1C);
            Unknown3 = BitConverter.ToInt32(file, address + 0x20);
        }

        public LandTable(Dictionary<string, Dictionary<string, string>> INI, string groupname)
        {
            Name = groupname;
            Dictionary<string, string> group = INI[groupname];
            COL = new List<COL>();
            if (group.ContainsKey("COL"))
            {
                string[] cols = group["COL"].Split(',');
                foreach (string item in cols)
                    COL.Add(new COL(INI, item));
            }
            Anim = new List<GeoAnimData>();
            if (group.ContainsKey("Anim"))
            {
                string[] cols = group["Anim"].Split(',');
                foreach (string item in cols)
                    Anim.Add(new GeoAnimData(INI, item));
            }
            Flags = int.Parse(group["Flags"], System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo);
            Unknown1 = float.Parse(group["Unknown1"], System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
            if (group.ContainsKey("TextureFileName"))
                TextureFileName = group["TextureFileName"];
            TextureList = uint.Parse(group["TextureList"], System.Globalization.NumberStyles.HexNumber);
            Unknown2 = int.Parse(group["Unknown2"], System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo);
            Unknown3 = int.Parse(group["Unknown3"], System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        public byte[] GetBytes(uint imageBase, bool DX, out uint address)
        {
            Dictionary<string, uint> attachaddrs = new Dictionary<string, uint>();
            List<byte> result = new List<byte>();
            byte[] tmpbyte;
            uint[] colmdladdrs = new uint[COL.Count];
            uint tmpaddr = 0;
            for (int i = 0; i < COL.Count; i++)
            {
                result.Align(4);
                tmpbyte = COL[i].Model.GetBytes(imageBase + (uint)result.Count, DX, attachaddrs, out tmpaddr);
                colmdladdrs[i] = tmpaddr + (uint)result.Count + imageBase;
                result.AddRange(tmpbyte);
            }
            uint[] animmdladdrs = new uint[Anim.Count];
            uint[] animaniaddrs = new uint[Anim.Count];
            for (int i = 0; i < Anim.Count; i++)
            {
                result.Align(4);
                tmpbyte = Anim[i].Model.GetBytes(imageBase + (uint)result.Count, DX, out tmpaddr);
                animmdladdrs[i] = tmpaddr + (uint)result.Count + imageBase;
                result.AddRange(tmpbyte);
                result.Align(4);
                tmpbyte = Anim[i].Animation.GetBytes(imageBase + (uint)result.Count, animmdladdrs[i], Anim[i].Model.GetObjects().Length, out tmpaddr);
                animaniaddrs[i] = tmpaddr + (uint)result.Count + imageBase;
                result.AddRange(tmpbyte);
            }
            uint coladdr = imageBase + (uint)result.Count;
            for (int i = 0; i < COL.Count; i++)
            {
                result.Align(4);
                result.AddRange(COL[i].GetBytes(imageBase + (uint)result.Count, colmdladdrs[i]));
            }
            uint animaddr = imageBase + (uint)result.Count;
            for (int i = 0; i < Anim.Count; i++)
            {
                result.Align(4);
                result.AddRange(Anim[i].GetBytes(imageBase + (uint)result.Count, animmdladdrs[i], animaniaddrs[i]));
            }
            result.Align(4);
            uint texnameaddr = 0;
            if (TextureFileName != null)
            {
                texnameaddr = imageBase + (uint)result.Count;
                result.AddRange(System.Text.Encoding.ASCII.GetBytes(TextureFileName));
                result.Add(0);
            }
            result.Align(4);
            address = (uint)result.Count;
            result.AddRange(BitConverter.GetBytes((ushort)COL.Count));
            result.AddRange(BitConverter.GetBytes((ushort)Anim.Count));
            result.AddRange(BitConverter.GetBytes(Flags));
            result.AddRange(BitConverter.GetBytes(Unknown1));
            result.AddRange(BitConverter.GetBytes(coladdr));
            result.AddRange(BitConverter.GetBytes(animaddr));
            result.AddRange(BitConverter.GetBytes(texnameaddr));
            result.AddRange(BitConverter.GetBytes(TextureList));
            result.AddRange(BitConverter.GetBytes(Unknown2));
            result.AddRange(BitConverter.GetBytes(Unknown3));
            return result.ToArray();
        }

        public void Save(Dictionary<string, Dictionary<string, string>> INI, string animpath)
        {
            Dictionary<string, string> group = new Dictionary<string, string>();
            if (COL.Count > 0)
            {
                List<string> cols = new List<string>();
                foreach (COL item in COL)
                {
                    item.Save(INI);
                    cols.Add(item.Name);
                }
                group.Add("COL", string.Join(",", cols.ToArray()));
            }
            if (Anim.Count > 0)
            {
                List<string> cols = new List<string>();
                foreach (GeoAnimData item in Anim)
                {
                    item.Save(INI, animpath);
                    cols.Add(item.Name);
                }
                group.Add("Anim", string.Join(",", cols.ToArray()));
            }
            group.Add("Flags", Flags.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            group.Add("Unknown1", Unknown1.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (!string.IsNullOrEmpty(TextureFileName))
                group.Add("TextureFileName", TextureFileName);
            group.Add("TextureList", TextureList.ToString("X8"));
            group.Add("Unknown2", Unknown2.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            group.Add("Unknown3", Unknown3.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            if (!INI.ContainsKey(Name))
                INI.Add(Name, group);
        }
    }
}
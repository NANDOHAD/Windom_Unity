﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
public class ani2
{
    public hod2v0 structure;
    public List<windomAnimation> animations;
    public string _filename;
    public bool load(string filename)
    {
        _filename = filename;
        BinaryReader br = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read));
        
        string signature = new string(br.ReadChars(3));
        if (signature == "AN2")
        {
            animations = new List<windomAnimation>();
            //Encoding ShiftJis = Encoding.GetEncoding(932);
            string robohod = USEncoder.ToEncoding.ToUnicode(br.ReadBytes(256)).TrimEnd('\0');
            structure = new hod2v0(robohod);
            structure.loadFromBinary(ref br);
            
            int aCount = br.ReadInt32();
            for (int i = 0; i < aCount; i++)
            {
                windomAnimation aData = new windomAnimation();
                aData.loadFromAni(ref br, ref structure);
                animations.Add(aData);
            }
        }
        else if (signature == "ANI")
        {
            StreamWriter debug = new StreamWriter("debug.txt");
            animations = new List<windomAnimation>();
            //Encoding ShiftJis = Encoding.GetEncoding(932);
            string robohod = USEncoder.ToEncoding.ToUnicode(br.ReadBytes(256)).TrimEnd('\0');
            hod1 oldStructure = new hod1(robohod);
            oldStructure.loadFromBinary(ref br);
            structure = oldStructure.convertToHod2v0();
            debug.WriteLine(br.BaseStream.Position.ToString());
            debug.Close();
            for (int i = 0; i < 200; i++)
            { 
                windomAnimation aData = new windomAnimation();
                aData.loadFromAniOld(ref br);
                animations.Add(aData);
            }
        }
        else if (signature == "HOD")
        {
            br.BaseStream.Seek(0, SeekOrigin.Begin);
            animations = new List<windomAnimation>();
            hod1 hodfile = new hod1("HOD1 FILE");
            hodfile.loadFromBinary(ref br);
            structure = hodfile.convertToHod2v0();
            windomAnimation aData = new windomAnimation();
            aData.frames = new List<hod2v1>();
            aData.frames.Add(hodfile.convertToHod2v1());
            aData.scripts = new List<script>();
            animations.Add(aData);
        }
        br.Close();

        return true;
    }

    public void save(string filename = "")
    {
        if (filename == "")
            filename = _filename;
        //Encoding ShiftJis = Encoding.GetEncoding(932);
        if (File.Exists(filename))
            File.Delete(filename);
        BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite));
        bw.Write(ASCIIEncoding.ASCII.GetBytes("AN2"));
        byte[] shiftjistext = USEncoder.ToEncoding.ToSJIS(structure.filename);
        bw.Write(shiftjistext);
        bw.BaseStream.Seek(256 - shiftjistext.Length, SeekOrigin.Current);
        structure.saveToBinary(ref bw);

        bw.Write(animations.Count);
        for (int i = 0; i < animations.Count; i++)
        {
            animations[i].saveToAni(ref bw);
        }

        bw.Close();
    }

    public void addPart(string partName, int parent)
    {
        //Debug.Log(structure.parts.Count);
        int level = structure.parts[parent].treeDepth + 1;
        hod2v0_Part pHod = structure.parts[parent];
        pHod.childCount++;
        structure.parts[parent] = pHod;
        hod2v0_Part nPart = new hod2v0_Part();
        nPart.name = partName;
        nPart.treeDepth = level;
        nPart.flag = 1;
        nPart.unk = new Vector3(1, 1, 1);
        nPart.position = new Vector3(0, 0, 0);
        nPart.rotation = new Quaternion(0, 0, 0, 1);
        nPart.scale = new Vector3(1, 1, 1);
        int i = parent + 1;
        for (; i < structure.parts.Count; i++)
        {
            if (structure.parts[i].treeDepth  <= structure.parts[parent].treeDepth)
            {
                break;
            }
        }
        structure.parts.Insert(i, nPart);
        //Debug.Log(structure.parts.Count);
        //Debug.Log(partName);
        hod2v1_Part nPart1 = new hod2v1_Part();
        nPart1.name = partName;
        nPart1.treeDepth = level;
        nPart1.position = new Vector3(0,0,0);
        nPart1.rotation = new Quaternion(0, 0, 0, 1);
        nPart1.scale = new Vector3(1, 1, 1);
        nPart1.unk1 = new Quaternion();
        nPart1.unk2 = new Quaternion();
        nPart1.unk3 = new Quaternion();
        for (int j = 0; j < animations.Count;j++)
        {
            for (int k = 0; k < animations[j].frames.Count; k++)
            {
                hod2v1_Part pHod1 = animations[j].frames[k].parts[parent];
                pHod1.childCount++;
                animations[j].frames[k].parts[parent] = pHod1;
                animations[j].frames[k].parts.Insert(i, nPart1);
            }
        }
    }

    public bool removePart(int index)
    {
        if (structure.parts[index].childCount == 0)
        {
            int i = index;
            for (; i >= 0; i--)
            {
                if (structure.parts[i].treeDepth < structure.parts[index].treeDepth)
                {
                    hod2v0_Part pHod = structure.parts[i];
                    pHod.childCount--;
                    structure.parts[i] = pHod;
                    structure.parts.RemoveAt(index);
                    break;
                }
            }

            for (int j = 0; j < animations.Count; j++)
            {
                for (int k = 0; k < animations[j].frames.Count; k++)
                {
                    hod2v1_Part pHod1 = animations[j].frames[k].parts[i];
                    pHod1.childCount--;
                    animations[j].frames[k].parts[i] = pHod1;
                    animations[j].frames[k].parts.RemoveAt(index);
                }
            }

        }
        else
            return false;
        return true;
    }
}


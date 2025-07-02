using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct SPT_BURNERSET
{
    public int id;
    public string part;
    public float size;
    public string vctr;
    
}

public struct Weapon_Data
{
    public int id;
    public string part;
}
public class SPT_Data
{
    scriptInterpret si;
    public List<SPT_BURNERSET> burnerList;
    public List<Weapon_Data> Guns;
    public List<Weapon_Data> Swords;
    public List<Weapon_Data> WeaponPoints;
    public MechaMovement mm;
    public CypherTranscoder transcoder;
    
    
    public void buildInpterpreter()
    {
        si = new scriptInterpret();
        si.registerVariable("Name", setName, getDummy);
        si.registerFunction("BURNERSET", readBurnerset);
        si.registerFunction("GUNFILENAME", gunFileName);
        si.registerFunction("SWORDFILENAME", swordFileName);
        si.registerFunction("WEAPONPOINT", weaponPoint);
        si.registerVariable("HP", setHP, getDummy);
        si.registerVariable("Generator", setGen, getDummy);
        si.registerVariable("Energy", setEn, getDummy);
    }
    public void readSPT(string file)
    {
        SPT_Data result;
        transcoder = new CypherTranscoder();
        result = new SPT_Data();
        burnerList = new List<SPT_BURNERSET> ();
        Guns = new List<Weapon_Data> ();
        Swords = new List<Weapon_Data> ();
        WeaponPoints = new List<Weapon_Data> ();
        byte[] data = transcoder.Transcode(file);
        string text = Encoding.GetEncoding("shift_jis").GetString(data);
        si.runScript(text);
        Debug.Log(text);
    }

    public void readBurnerset(scriptVar[] values)
    {
        if (values.Length >= 3)
        {
            SPT_BURNERSET b;
            b.id = Mathf.RoundToInt(values[0].num);
            b.part = values[1].str;
            b.size = values[2].num;
            b.vctr = values[3].str;
            burnerList.Add(b);
        }
    }

    public void gunFileName(scriptVar[] values)
    {
        if (values.Length >= 2)
        {
            Weapon_Data weapon;
            weapon.id = Mathf.RoundToInt(values[0].num);
            weapon.part = values[1].str;
            Guns.Add(weapon);
        }
    }

    public void swordFileName(scriptVar[] values)
    {
        if (values.Length >= 2)
        {
            Weapon_Data weapon;
            weapon.id = Mathf.RoundToInt(values[0].num);
            weapon.part = values[1].str;
            Swords.Add(weapon);
        }
    }

    public void weaponPoint(scriptVar[] values)
    {
        if (values.Length >= 2)
        {
            Weapon_Data weapon;
            weapon.id = Mathf.RoundToInt(values[0].num);
            weapon.part = values[1].str;
            WeaponPoints.Add(weapon);
        }
    }

    public void setName(scriptVar value)
    {
        mm.Name = value.str;
    }

    public void setHP(scriptVar value)
    {
        mm.HP = Mathf.RoundToInt(value.num);
        mm.MaxHP = mm.HP;
    }

    public void setGen(scriptVar value)
    {
        mm.Generator = Mathf.RoundToInt(value.num);
        mm.MaxGenerator = mm.Generator;
    }

    public void setEn(scriptVar value)
    {
        mm.Energy = Mathf.RoundToInt(value.num);
        mm.MaxEnergy = mm.Energy;
    }
    public scriptVar getDummy()
    {
        return new scriptVar();
    }
    
}

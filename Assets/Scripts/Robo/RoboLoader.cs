using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RoboLoader : MonoBehaviour
{
    public string MechaFolder;
    public GameObject Robo;
    public SPT_Data RoboData;
    public GameObject Thruster;
    public GameObject NoThruster;
    // Start is called before the first frame update
    void Start()
    {
        //loadMecha(MechaFolder);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadMecha(string folder)
    {
        DirectoryInfo mf = new DirectoryInfo(folder);
        RoboStructure rs = Robo.GetComponent<RoboStructure>();
        rs.folder = folder;
        rs.ani = new ani2();
        rs.transcoder = new CypherTranscoder();
        if (File.Exists(Path.Combine(folder, "Script.ani")))
        {
            rs.ani.load(Path.Combine(folder, "Script.ani"));
            rs.buildStructure(Path.Combine(folder, "Script.ani"));
            Debug.Log("ANIファイルを読み込みました");
        }
        else
            Debug.Log("ANIファイルが見つかりません");
        MechaAnimator anim = Robo.GetComponent<MechaAnimator>();
        MechaMovement mm = Robo.GetComponent<MechaMovement>();
        StateManager sm = Robo.GetComponent<StateManager>();
        sm.Setup();
        ScriptHook sh = Robo.GetComponent<ScriptHook>();
        sh.setup();
        RoboData = new SPT_Data();
        RoboData.mm = mm;
        RoboData.buildInpterpreter();
        if (File.Exists(Path.Combine(folder, "Script.spt")))
        {
            RoboData.readSPT(Path.Combine(folder, "Script.spt"));
            Debug.Log("SPTファイルを読み込みました");
            Debug.Log(RoboData.burnerList);
            Debug.Log(RoboData.burnerList.Count);
        }
        else
            Debug.Log("SPTファイルが見つかりません");
        
        //attach thrusters data
        int prtCount = rs.parts.Count;
        for (int i = 0; i < RoboData.burnerList.Count; i++)
        {
            for (int j = 0; j < prtCount; j++)
            {
                if (RoboData.burnerList[i].part == rs.parts[j].name)
                {        
                    if (Mathf.Approximately(RoboData.burnerList[i].size, 0.0f))
                    {
                        GameObject n = Instantiate<GameObject>(NoThruster);
                        Debug.Log("バーニアポイント登録をスキップします");
                        n.transform.parent = rs.parts[j].transform;
                        n.transform.localPosition = new Vector3(0, 0, 0);
                        // vctrがDOWNならZ軸180度回転、UPならそのまま
                        if (RoboData.burnerList[i].vctr == "DOWN")
                            n.transform.localRotation = Quaternion.Euler(0, 0, 180);
                        else // "UP"
                            n.transform.localRotation = Quaternion.identity;  
                        n.transform.localScale = new Vector3(0.1f * RoboData.burnerList[i].size, 0.1f * RoboData.burnerList[i].size, 0.1f * RoboData.burnerList[i].size);
                        mm.Thrusters.Add(RoboData.burnerList[i].id, n);
                        break;
                    }
                    else
                    {
                        GameObject n = Instantiate<GameObject>(Thruster);
                        n.transform.parent = rs.parts[j].transform;
                        n.transform.localPosition = new Vector3(0, 0, 0);
                        // vctrがDOWNならZ軸180度回転、UPならそのまま
                        if (RoboData.burnerList[i].vctr == "DOWN")
                            n.transform.localRotation = Quaternion.Euler(0, 0, 180);
                        else // "UP"
                            n.transform.localRotation = Quaternion.identity;  
                        n.transform.localScale = new Vector3(0.1f * RoboData.burnerList[i].size, 0.1f * RoboData.burnerList[i].size, 0.1f * RoboData.burnerList[i].size);
                        mm.Thrusters.Add(RoboData.burnerList[i].id, n);
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < RoboData.Guns.Count; i++)
        {
            for (int j = 0; j < prtCount; j++)
            {
                if (RoboData.Guns[i].part == rs.parts[j].name)
                {
                    mm.Guns[RoboData.Guns[i].id] = rs.parts[j];
                    rs.parts[j].SetActive(false);
                    break;
                }
            }
        }
        if (mm.Guns[0] != null)
            mm.Guns[0].SetActive(true);
        if (mm.Guns[1] != null)
            mm.Guns[1].SetActive(true);
        if (mm.Guns[8] != null)
            mm.Guns[8].SetActive(true);
        if (mm.Guns[9] != null)
            mm.Guns[9].SetActive(true);


        for (int i = 0; i < RoboData.Swords.Count; i++)
        {
            for (int j = 0; j < prtCount; j++)
            {
                if (RoboData.Swords[i].part == rs.parts[j].name)
                {
                    mm.Swords[RoboData.Swords[i].id] = rs.parts[j];
                    rs.parts[j].SetActive(false);
                    break;
                }
            }
        }
        

        for (int i = 0;  i < RoboData.WeaponPoints.Count; i++)
        {
            for (int j = 0; j < prtCount; j++)
            {
                if (RoboData.WeaponPoints[i].part == rs.parts[j].name)
                {
                    mm.weaponPoints[RoboData.WeaponPoints[i].id] = rs.parts[j];
                    break;
                }
            }
        }
    }



}

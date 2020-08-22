using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RobotRepair : MonoBehaviour
{
    public class partProperties
    {
        public partProperties(int o, int c, int i, int e, int a, int f)
        {
            //oil, clean, integrity, energy, ammo, fuel
            values = new int[] { o, c, i, e, a, f };
        }
        public int[] values;
    }
    
    public GameObject RoboReadout;
    public Dictionary <string, partProperties> bodyParts = new Dictionary <string, partProperties>();
    private TextMeshProUGUI Values;
    private TextMeshProUGUI Part;
    
    public string[] errors = new string[] { "STCK", "GRMY", "DMGD", "DRND", "SPNT", "SPNT" };
    public string[] names = new string[] {"OILL: ", "CLEN: ", "INTG: ", "ENRG: ", "AMMO: ", "FUEL: "};
    public string[] fullNames = new string[] { "Head", "Trso", "ArmL", "ArmR", "LegL", "LegR", "N//A" };

    // Start is called before the first frame update
    void Awake()
    {
        //generate values for body parts
        for (int i = 0; i < fullNames.Length; i++)
        {
            bodyParts[fullNames[i]] = new partProperties(Random.Range(0,100), Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100));
        }
        Values = GetChildWithName(RoboReadout, "Values").GetComponent<TextMeshProUGUI>();
        Part = GetChildWithName(RoboReadout, "Part").GetComponent<TextMeshProUGUI>();
        Debug.Log(Values.text);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            var nam = hit.collider.gameObject.name;
            Values.text = GetValuesString(bodyParts[nam]);
            Part.text = "READ: " + nam;
        }
        else
        {
            Values.text = "OILL: NONE\nCLEN: NONE\nINTG: NONE\nENRG: NONE\nAMMO: NONE\nFUEL: N//A";
            Part.text = "READ: NONE";
        }

    }

    string GetValuesString(partProperties props)
    {
        string output = "";
        for (int i = 0; i < props.values.Length; i++)
        {
            output += names[i];
            if (props.values[i] < 0) output += "N//N";
            else if (props.values[i] < 25) output += errors[i];
            else if (props.values[i] < 75) output += "NMNL";
            else output += "GOOD";
            output += "\n";
        }
        return output;
    }

    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }
}

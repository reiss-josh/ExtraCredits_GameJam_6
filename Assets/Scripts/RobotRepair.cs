using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RobotRepair : MonoBehaviour
{
    public class partProperties
    {
        public partProperties(int[] passedValues)
        {
            //oil, clean, integrity, energy, ammo, fuel
            values = passedValues;
        }
        public int[] values;
    }

    public bool robotStopped = false;
    private GameObject RoboReadout;
    private CustomCursor myCursor;
    public Dictionary <string, partProperties> bodyParts = new Dictionary <string, partProperties>();
    private TextMeshProUGUI Values;
    private TextMeshProUGUI Part;
    private Vector3 destinationVector = Vector3.zero;
    
    public string[] errors = new string[] { "STCK", "GRMY", "DMGD", "DRND", "SPNT", "SPNT" };
    public string[] names = new string[] {"OILL: ", "CLEN: ", "INTG: ", "ENRG: ", "AMMO: ", "FUEL: "};
    public string[] fullNames = new string[] { "Head", "Trso", "ArmL", "ArmR", "LegL", "LegR", "N//A" };

    // Start is called before the first frame update
    void Awake()
    {
        BodyRandomValues();
        RoboReadout = GameObject.Find("ReadoutHolder");
        myCursor = GameObject.Find("Cursor").GetComponent<CustomCursor>();
        Values = GetChildWithName(RoboReadout, "Values").GetComponent<TextMeshProUGUI>();
        Part = GetChildWithName(RoboReadout, "Part").GetComponent<TextMeshProUGUI>();
        RoboReadout.GetComponent<ReadoutScript>().doneEvent += UpdateAmDone;
    }

    void BodyRandomValues()
    {
        //generate values for body parts
        for (int i = 0; i < fullNames.Length; i++)
        {
            bodyParts[fullNames[i]] = new partProperties(new int[] { Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3) });
        }
    }

    void UpdateAmDone()
    {
        robotStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (robotStopped) NormalUpdate();
        else ConveyorUpdate();
    }

    void NormalUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            var nam = hit.collider.gameObject.name;
            Values.text = GetValuesString(bodyParts[nam]);
            Part.text = "READ: " + nam;
            if (Input.GetButtonDown("Fire1"))
            {
                if (myCursor.currTool > -1)
                {
                    Debug.Log(myCursor.currTool);
                    Debug.Log(nam);
                    bodyParts[nam].values[myCursor.currTool] += 1;
                }
            }
        }
        else
        {
            Values.text = "OILL: N//A\nCLEN: N//A\nINTG: N//A\nENRG: N//A\nAMMO: N//A\nFUEL: N//A";
            Part.text = "READ: NONE";
        }
    }//update stuff if we've reached our spot

    //update stuff if we are moving again
    void ConveyorUpdate()
    {
        if (transform.position == destinationVector)
        {
            if (destinationVector != Vector3.zero) Destroy(gameObject);
            robotStopped = true; destinationVector = new Vector3(27.5f, 0, 0);
        }
        transform.position = Vector3.MoveTowards(transform.position, destinationVector, 27.5f * Time.deltaTime);
    }

    string GetValuesString(partProperties props)
    {
        string output = "";
        for (int i = 0; i < props.values.Length; i++)
        {
            output += names[i];
            if (props.values[i] < 0) output += "N//N";
            else if (props.values[i] == 0) output += errors[i];
            else if (props.values[i] == 1) output += "NMNL";
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

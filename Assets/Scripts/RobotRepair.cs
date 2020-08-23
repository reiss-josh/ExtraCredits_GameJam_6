using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RobotRepair : MonoBehaviour
{
    //class used to store the values for a given bodypart
    public class partProperties
    {
        public partProperties(int[] passedValues)
        {
            //oil, clean, integrity, energy, ammo, fuel
            values = passedValues;
        }
        public int[] values;
    }

    //regular variables
    private Vector3 destinationVector = Vector3.zero;
    private Vector3 offScreenRight = new Vector3(27.5f, 0, 0);
    public bool robotStopped = false;    
    public Dictionary <string, partProperties> bodyParts = new Dictionary <string, partProperties>();
    public int status = 0;
    public string problemString;
    private int brokenPart, brokenAspect;

    //acquired components
    private CustomCursor myCursor;

    //text events
    public event System.Action<string, string> updateReadout;
    public event System.Action<string> updateDialogue;
    public event System.Action exitEvent;
    public event System.Action<int> updateStatus;
    private ReadoutScript readout;

    //strings for the status screen
    private static readonly string[] errors = { "STCK", "GRMY", "DMGD", "DRND", "SPNT", "SPNT" };
    private static readonly string[] aspectShorthands = {"OILL", "CLEN", "INTG", "ENRG", "AMMO", "FUEL"};
    private static readonly string[] partNames =  { "Head", "Trso", "LegL", "LegR", "ArmL", "ArmR"};
    private static readonly string[] cleanedNames = { "Head", "Torso", "Left Leg", "Right Leg", "Left Arm", "Right Arm" };
    private static readonly string[] cleanedErrors = {"STUCK", "GRIMY", "DAMAGED", "DRAINED", "SPENT", "SPENT" };

    // Start is called before the first frame update
    void Awake()
    {
        GenerateDefaultValues();
        GenerateProblem();
        problemString = GenerateProblemString();

        //get necessary components
        myCursor = GameObject.Find("Cursor").GetComponent<CustomCursor>();

        //add event listeners
        readout = GameObject.Find("ReadoutHolder").GetComponent<ReadoutScript>();
        readout.doneEvent += UpdateAmDone;
    }

    // Update is called once per frame
    void Update()
    {
        if (robotStopped) NormalUpdate();
        else ConveyorUpdate();
    }

    //regular update for interacting
    void NormalUpdate()
    {
        //get a raycast to the creeen
        RaycastHit2D mousePosition = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (mousePosition.collider != null)
        {
            var nam = mousePosition.collider.gameObject.name;
            //if the name of the object we're hovering over is in our names list...
            if (partNames.Contains(nam))
            {
                //update the readout to reflect the part we're looking at
                //Part.text = "READ: " + nam;
                //Values.text = GetValuesString(bodyParts[nam]);
                updateReadout(GetValuesString(bodyParts[nam]), "Read: " + nam);
                
                //if we click on the part, use our current tool on it.
                if (Input.GetButtonDown("Fire1"))
                {
                    UseTool(nam, myCursor.currTool);
                }
            }
        }
        //if we're not hovering over anything, empty out the readout
        else
        {
            updateReadout("OILL: N//A\nCLEN: N//A\nINTG: N//A\nENRG: N//A\nAMMO: N//A\nFUEL: N//A", "READ: NONE");
        }
    }

    //update stuff if we are moving in/out of position
    void ConveyorUpdate()
    {
        if (transform.position == destinationVector)
        {
            if (destinationVector == offScreenRight) exitEvent();
            else updateDialogue(problemString);
            robotStopped = true;
        }
        transform.position = Vector3.MoveTowards(transform.position, destinationVector, 27.5f * Time.deltaTime);
    }

    void GenerateDefaultValues()
    {
        for (int i = 0; i < partNames.Length; i++)
        {
            partProperties generatedProps = new partProperties(new int[] { 2,2,2,2, -1, -1});
            //generate additional props if necessary
            if (partNames[i] == "ArmL" || partNames[i] == "ArmR") generatedProps.values[4] = 2;
            if (partNames[i] == "LegL" || partNames[i] == "LegR") generatedProps.values[5] = 2;
            bodyParts[partNames[i]] = generatedProps;
        }
    }

    void GenerateProblem()
    {
        //choose which body part has a problem
        brokenPart = Random.Range(0, partNames.Length);
        //if it's an arm or leg, ammo or fuel can be the problem. otherwise, choose from the other 4 problem types.
        bool ammoValid = (partNames[brokenPart] == "ArmR" || partNames[brokenPart] == "ArmL");
        bool fuelValid = (partNames[brokenPart] == "LegL" || partNames[brokenPart] == "LegL");
        brokenAspect = ammoValid || fuelValid ? Random.Range(0, 5) : Random.Range(0, 4); //if fuel or ammo are valid, generate 0-4, else 0-3
        brokenAspect = brokenAspect == 4 && fuelValid ? brokenAspect = 5 : brokenAspect; //if fuel was valid, and we generated a 4, make it a 5

        //break the part
        bodyParts[partNames[brokenPart]].values[brokenAspect] = Random.Range(0,2);
    }

    string GenerateProblemString()
    {
        string result = "My " + cleanedNames[brokenPart] + " is " + cleanedErrors[brokenAspect];
        return result;
    }

    void UseTool(string bodyPart, int toolNum)
    {
        if (toolNum > -1 && bodyParts[bodyPart].values[toolNum] > 0)
        {
            bodyParts[bodyPart].values[toolNum] = (bodyParts[bodyPart].values[toolNum] + 1) % 3;
            if (bodyPart != partNames[brokenPart]) status += (bodyParts[bodyPart].values[toolNum] - 2); //if it's not the broken part, update our status
        }
    }

    void UpdateAmDone()
    {
        robotStopped = false;
        destinationVector = offScreenRight;
        status += DetermineStatus();
        updateStatus(status);
        updateDialogue("");
        readout.doneEvent -= UpdateAmDone;
        //Debug.Log(status);
    }

    int DetermineStatus()
    {
        return (bodyParts[partNames[brokenPart]].values[brokenAspect] - 2); //update our status based on the broken part
    }

    //generates the readout string from a body part's properties
    string GetValuesString(partProperties props)
    {
        string output = "";
        for (int i = 0; i < props.values.Length; i++)
        {
            output += aspectShorthands[i] + ": ";
            if (props.values[i] < 0) output += "N//N";
            else if (props.values[i] == 0) output += errors[i];
            else if (props.values[i] == 1) output += "NMNL";
            else output += "GOOD";
            output += "\n";
        }
        return output;
    }

    //gets a child of a given gameobject with a given name, if one exists
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

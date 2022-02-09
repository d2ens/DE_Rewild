//Base du L-System pour le projet Rewild : V1

using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;

public class TransformInfo
{
    public Vector3 position;
    public Quaternion rotation;
}

public class LSystemScript : MonoBehaviour
{
    [SerializeField] private int iterations = 4;
    [SerializeField] private GameObject Branch;
    [SerializeField] private float length = 10f;
    [SerializeField] private float angle = 30f;
    private const string axiom = "X";
    private Stack<TransformInfo> transformStack;
    private Dictionary<char, string> rules;
    private string currentString = string.Empty;
    // Start is called before the first frame update
    void Start()
    {
        transformStack = new Stack<TransformInfo>();
        rules = new Dictionary<char, string>
        {
            {'X',"[FX][-FX][+FX]"},
            {'F',"FF"}
        };
        Generate();
    }

    private void Generate(){
        currentString = axiom;
        StringBuilder sb= new StringBuilder();
        for (int i=0; i<iterations; i++)
        {
            foreach (char c in currentString)
            {
            sb.Append(rules.ContainsKey(c)? rules[c] : c.ToString());
            }

            currentString = sb.ToString();
            sb = new StringBuilder(); 
        }
        
        foreach (char c in currentString)
        {
            switch (c)
            {
                case 'F':
                    Vector3 initialPosition = transform.position;
                    transform.Translate(Vector3.up * length);
                    GameObject treeSegment = Instantiate(Branch);
                    treeSegment.GetComponent<LineRenderer>().SetPosition(0,initialPosition);
                    treeSegment.GetComponent<LineRenderer>().SetPosition(1,transform.position);
                    break;

                case 'X':
                    break;

                case '+':
                    transform.Rotate (Vector3.back * angle);
                    break;

                case '-':
                    transform.Rotate (Vector3.forward * angle);
                    break;

                case '[':
                    transformStack.Push(new TransformInfo(){
                        position = transform.position,
                        rotation = transform.rotation,
                    });
                    break;

                case ']':
                    TransformInfo ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation; 
                    break;

                default:
                throw new InvalidOperationException("Invalid L-Tree operation");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovementScript : MonoBehaviour
{
    private Transform m_transform;
    private int m_vdim;
    private Vector3 m_gridOrigin;
    private float m_vertexSize;
    private Vector3 m_numVertices;
    private Vector3 m_cubeLengths;
    private Vector2 m_innerBoundaryX, m_innerBoundaryY, m_innerBoundaryZ;
    private Vector3 m_velocity;

    public GameObject m_pipeGrid;
    public float m_vmag = 1;
    // public bool m_periodicBCs = false;

    // Called when scene first starts
    private void Awake()
    {
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialise(GameObject pipeGrid, Vector3 position, Vector3 origin, Vector2 innerBoundaryX, Vector2 innerBoundaryY, Vector2 innerBoundaryZ, float pipeLength, Vector3 numVertices, int vmag)
    {
        m_transform = GetComponent<Transform>();
        m_transform.position = position;
        m_pipeGrid = pipeGrid;
        m_gridOrigin = origin;
        m_innerBoundaryX = innerBoundaryX;
        m_innerBoundaryY = innerBoundaryY;
        m_innerBoundaryZ = innerBoundaryZ;
        m_vertexSize = pipeLength;
        m_numVertices = numVertices;
        m_cubeLengths = new Vector3(
            m_numVertices[0] - 1,
            m_numVertices[1] - 1,
            m_numVertices[2] - 1
        ) * m_vertexSize;
        m_vmag = vmag;
        Set_Velocity();

        // Debug.LogFormat("Initialised with m_transform = {0}, m_vdim = {1}, m_gridOrigin = {2}, m_vertexSize = {3}, m_numVertices = {4}, ", m_transform, m_vdim, m_gridOrigin, m_vertexSize, m_numVertices);
        // Debug.LogFormat("\tm_cubeLengths = {0}, m_velocity = {1}, m_pipeGrid = {2}, m_vmag = {3}", m_cubeLengths, m_velocity, m_pipeGrid, m_vmag);
}

    // Update is called once per frame -- but not every PHYSICS frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        //Do not update if the gridLengths have invalid values
        if (m_vertexSize<=0 || m_numVertices[0]<=0 || m_numVertices[1]<=0 || m_numVertices[2] <= 0){
            return;
        }

        bool atVertex = Update_Position();

        if (atVertex==true){
            Set_Velocity();
        }
    }

    private void _Set_Velocity_Switch(int r)
    {
        //Expects a random number from Set_Velocity.
        if (r == 0)
        {
            m_velocity[0] = m_vmag;
        }
        else if (r == 1)
        {
            m_velocity[0] = -m_vmag;
        }
        else if (r == 2)
        {
            m_velocity[1] = m_vmag;
        }
        else if (r == 3)
        {
            m_velocity[1] = -m_vmag;
        }
        else if (r == 4)
        {
            m_velocity[2] = m_vmag;
        }
        else if (r == 5)
        {
            m_velocity[2] = -m_vmag;
        }
    }
    private void Set_Velocity()
    {
        m_velocity = new Vector3(0f, 0f, 0f);
        int r = Random.Range(0, 6); //Hopefully, numbers between 0 and 5

        m_vdim = (int)(r / 2);
        _Set_Velocity_Switch(r);
    }

    private void Set_Velocity(int blockedDirection)
    {
        //Pick velocity in a direction away from a boundary we've reached.
        //blockedDirection is a number between 0 and 5.
        //Note, if TWO directions are blocked, we might have to repeatedly pick random numbers.
        m_velocity = new Vector3(0f, 0f, 0f);
        int r = Random.Range(0, 5); 
        if (r >= blockedDirection){
            r += 1;
        }

        m_vdim = (int)(r / 2);
        _Set_Velocity_Switch(r);
    }


    // private void _do_periodic_BCs(ref int d, ref float newpos)
    // {
    //     if (newpos < m_gridOrigin[d])
    //     { //modulo can't pick up on the negative values
    //         float diff = m_gridOrigin[d] - newpos;
    //         newpos = m_gridOrigin[d] + m_cubeLengths[d] - diff;
    //     }
    //     else
    //     {
    //         newpos = m_gridOrigin[d] + (newpos - m_gridOrigin[d]) % m_cubeLengths[d];
    //     }
    // }

    private bool _do_rebounding_BCs(ref int d, ref Vector3 newPosVec)
    {
        bool atEdge = false;
        float newPosFloat = newPosVec[d];

        //Outer boundaries
        if (newPosFloat <= m_gridOrigin[d])
        {
            newPosFloat = m_gridOrigin[d];
            Set_Velocity(d + m_velocity[d] > 0 ? 0 : 1);
            atEdge = true;
        }

        else if (newPosFloat >= m_gridOrigin[d] + m_cubeLengths[d])
        {
            newPosFloat = m_gridOrigin[d] + m_cubeLengths[d];

            //Choose a new velocity that is not in this direction. Note, this is not corner-proof.
            Set_Velocity(d + m_velocity[d] > 0 ? 0 : 1);
            atEdge = true;
        }

        if (atEdge) return atEdge;

        //Inner boundaries
        if (m_innerBoundaryX == null || m_innerBoundaryY == null || m_innerBoundaryZ == null){
            return atEdge;
        }

        bool inInnerRegionX = m_innerBoundaryX[0] <= newPosVec[0] && newPosVec[0] <= m_innerBoundaryX[1];
        bool inInnerRegionY = m_innerBoundaryY[0] <= newPosVec[1] && newPosVec[1] <= m_innerBoundaryY[1];
        bool inInnerRegionZ = m_innerBoundaryZ[0] <= newPosVec[2] && newPosVec[2] <= m_innerBoundaryZ[1];
        Vector2 innerBoundaryD = d == 0 ? m_innerBoundaryX : (d == 1 ? m_innerBoundaryY : m_innerBoundaryZ);

        if (inInnerRegionX && inInnerRegionY && inInnerRegionZ)
        {
            //If the old position was NOT in the inner region, then change our velocity
            if (m_transform.position[d] <= innerBoundaryD[0])
            {
                newPosVec[d] = innerBoundaryD[0];
                Set_Velocity(d + m_velocity[d] > 0 ? 0 : 1);
                atEdge = true;
            }

            else if (m_transform.position[d] >= innerBoundaryD[1])
            {
                newPosVec[d] = innerBoundaryD[1];
                Set_Velocity(d + m_velocity[d] > 0 ? 0 : 1);
                atEdge = true;
            }
        }

        return atEdge;
    }

    private bool _correct_position_to_grid(ref int d, ref float vdt, ref float newpos){
        //Calculate how far we've gone from our start position and truncate the integer part
        float unitsFromStart = (newpos - m_gridOrigin[d]) / m_vertexSize;
        float posDecimal = Mathf.Abs(unitsFromStart % 1f); //is %1 correct for negative numbers..? Need (-1.2)%1 == -0.2, not 0.8

        //If we're less than v*dt from an integer position, round to it
        bool atVertex = false;
        float absvdtDVS = Mathf.Abs(vdt / m_vertexSize);
        if (posDecimal < 0.1f * absvdtDVS || posDecimal > 1.0f - 0.9 * absvdtDVS)
        {
            newpos = m_gridOrigin[d] + Mathf.RoundToInt(unitsFromStart) * m_vertexSize;
            atVertex = true;
        }

        return atVertex;
    }
    private bool Update_Position(){
        //Calculate direction and size of movement change
        int d = m_vdim;
        float vdt = m_velocity[d] * Time.deltaTime;
        bool atVertex = false;

        //Calculate new position
        float newpos = m_transform.position[d] + vdt;
        // Debug.LogFormat("Time: {0}. Direction: {1}. Position: {2} + {3} -> {4}.", Time.time, d, m_transform.position[d], vdt, newpos);

        //Calculate a vector for the newposition
        Vector3 newPosVec = m_transform.position;
        newPosVec[d] = newpos;

        //Deal with boundary conditions
        atVertex = _do_rebounding_BCs(ref d, ref newPosVec);
        newpos = newPosVec[d];
        // Debug.LogFormat("\tBCs: -> {0}", newpos);

        //Deal with problems occurring due to float addition
        atVertex = _correct_position_to_grid(ref d, ref vdt, ref newpos);
        //Debug.LogFormat("\tVertex correction (pipe length {0}, cube length {1}): -> {2}", m_vertexSize, m_cubeLengths[d], newpos);

        //Finally, set the position
        Vector3 translation = new Vector3(0f, 0f, 0f);
        translation[d] = newpos - m_transform.position[d];
        m_transform.Translate(translation, Space.World);
        
        return atVertex;
    }
}

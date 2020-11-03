using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public GameObject m_pipePrefab;
    public GameObject m_lightPrefab;
    public int m_numCubesX;
    public int m_numCubesY;
    public int m_numCubesZ;
    public float m_pipeLength; //e.g. 100
    public float m_pipeRadius; //e.g  2
    public bool m_autoMaxLights;
    public int m_maxLights;  //giving 0 makes this be automatically calculated
    public float m_innerExclusionFraction;

    //Our own position, rotation and size
    private Transform m_transform;
    private Vector3 m_position;
    private Quaternion m_rotation;
    private Vector3 m_totalCubeLengths;

    //Derived position definitions
    private Vector3 m_origin;
    private Vector3 m_pipeOffset;

    //Inner region in which there are no pipes
    private Vector2 m_innerBoundaryX, m_innerBoundaryY, m_innerBoundaryZ;
    private int m_voidRegionMin_i, m_voidRegionMax_i, m_voidRegionMin_j, m_voidRegionMax_j, m_voidRegionMin_k, m_voidRegionMax_k;

    //Child arrays
    private int m_numPipes;
    private GameObject[] m_pipes;
    private int m_numLights;
    private GameObject[] m_lights;

    // Called when scene first starts
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        //Work out our own position
        m_transform = GetComponent<Transform>();
        m_position = m_transform.position;
        m_rotation = m_transform.rotation;

        //Set scale to be equal in each direction to prevent dodgy things happening
        m_transform.localScale = new Vector3(1, 1, 1);

        //Calculate number of cubes provided from scale given
        if (m_numCubesX % 2 == 1) m_numCubesX++; //Want an even number for symmetry and for making it easier to keep lights inside pipes
        if (m_numCubesY % 2 == 1) m_numCubesY++;
        if (m_numCubesZ % 2 == 1) m_numCubesZ++;

        //Calculate total size
        m_totalCubeLengths = m_pipeLength * new Vector3(m_numCubesX, m_numCubesY, m_numCubesZ);

        //Set size of pipe prefab
        m_pipePrefab.transform.localScale = new Vector3(m_pipeLength, m_pipeRadius, m_pipeRadius);

        //Calculate derived positions
        m_origin = m_position - m_totalCubeLengths/2;
        m_pipeOffset = new Vector3(0, 1, 1) * m_pipeLength / 2; //in the centre space
        if (0 < m_innerExclusionFraction && m_innerExclusionFraction < 1)
        {
            m_innerBoundaryX[0] = m_position[0] - m_totalCubeLengths[0] * m_innerExclusionFraction/2f; //e.g. if 20% is chosen, then the inner boundary is from 40% to 60% -> centre - length*frac/2 to centre + length*frac/2
            m_innerBoundaryX[1] = m_position[0] + m_totalCubeLengths[0] * m_innerExclusionFraction/2f;
            m_innerBoundaryY[0] = m_position[1] - m_totalCubeLengths[1] * m_innerExclusionFraction/2f;
            m_innerBoundaryY[1] = m_position[1] + m_totalCubeLengths[1] * m_innerExclusionFraction/2f;
            m_innerBoundaryZ[0] = m_position[2] - m_totalCubeLengths[2] * m_innerExclusionFraction/2f; 
            m_innerBoundaryZ[1] = m_position[2] + m_totalCubeLengths[2] * m_innerExclusionFraction/2f;

            m_voidRegionMin_i = (int)(0.5f * (1 - m_innerExclusionFraction) * m_numCubesX); m_voidRegionMax_i = (int)(0.5f * (1 + m_innerExclusionFraction) * m_numCubesX);
            m_voidRegionMin_j = (int)(0.5f * (1 - m_innerExclusionFraction) * m_numCubesY); m_voidRegionMax_j = (int)(0.5f * (1 + m_innerExclusionFraction) * m_numCubesY);
            m_voidRegionMin_k = (int)(0.5f * (1 - m_innerExclusionFraction) * m_numCubesZ); m_voidRegionMax_k = (int)(0.5f * (1 + m_innerExclusionFraction) * m_numCubesZ);

        }
        else
        {
            print("Warning: inner exclusion fraction was ignored because f<0 or f>1.");
        }

        //Instantiate array for pipes and spawn them
        if (m_numCubesX > 0 && m_numCubesY > 0 && m_numCubesZ > 0){
            m_numPipes = (m_numCubesX) * (m_numCubesY) * (m_numCubesZ) * 3; //Slightly more than we need because of missing pipes at edges
            m_pipes = new GameObject[m_numPipes];
            Spawn_All_Pipes();
        }
        else{
            print("Warning: No pipes will spawn because one of numCubesX, numCubesY, numCubesZ is less than 0.");
        }

        if (m_autoMaxLights){
            m_maxLights = (int)(Mathf.Pow(m_numCubesX * m_numCubesY * m_numCubesZ, 1f / 3) / 2);
            m_numLights = 0;
        }
        if (m_maxLights > 0){ //Checked without "else" because the pow command above might've made an invalid number of lights
            m_lights = new GameObject[m_maxLights];
            Spawn_All_Lights();
        }
        else{
            print("Warning: no lights will spawn because a negative number was provided, or the number of cubes is too small.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Spawn_All_Pipes()
    {
        Vector3 pos0, pos, offset;
        Quaternion rot;
        int index = 0;

        bool inVoidRegion = false;

        for (int i = 0; i < m_numCubesX; i++)
        {
            for (int j = 0; j < m_numCubesY; j++)
            {
                for (int k=0; k < m_numCubesZ; k++)
                {
                    inVoidRegion = ((m_voidRegionMin_i <= i && i < m_voidRegionMax_i) && (m_voidRegionMin_j <= j && j < m_voidRegionMax_j) && (m_voidRegionMin_k <= k && k < m_voidRegionMax_k));
                    if (inVoidRegion) continue;

                    //Find position of these three pipes. Were scale 1, this would be at (i, j, k)*pipeLength.
                    //However, x, y and z can have different scales, so the position is pipeLength/scaleX * (i*scaleX, j*scaleY, k*scaleZ).
                    offset = new Vector3(i, j, k);
                    pos0 = m_origin + m_pipeLength * offset;

                    //Pipe in x-direction
                    if (i < m_numCubesX - 1){
                        m_pipes[index++] = Instantiate(m_pipePrefab, pos0, m_rotation, m_transform); //last arg is parent
                        //m_pipes[index-1].transform.localScale = new Vector3(m_pipeLength, m_pipeRadius, m_pipeRadius);
                    }

                    //Pipe in y-direction
                    if (j < m_numCubesY - 1){
                        offset = new Vector3(-1, 1, 0);
                        pos = pos0 + m_pipeLength/2f * offset; //Scale is a dot product
                        rot = m_rotation * Quaternion.Euler(0, 0, 90); //rotation about z-axis
                        m_pipes[index++] = Instantiate(m_pipePrefab, pos, rot, m_transform);
                        //m_pipes[index-1].transform.localScale = new Vector3(m_pipeLength, m_pipeRadius, m_pipeRadius);
                    }

                    //Pipe in z-direction
                    if (k < m_numCubesZ - 1){
                        offset = new Vector3(-1, 0, 1);
                        pos = pos0 + m_pipeLength / 2f * offset;
                        rot = m_rotation * Quaternion.Euler(0, 90, 0); //rotation about y-axis
                        m_pipes[index++] = Instantiate(m_pipePrefab, pos, rot, m_transform);
                        //m_pipes[index-1].transform.localScale = new Vector3(m_pipeLength, m_pipeRadius, m_pipeRadius);
                    }
                }
            }
        }
    }

    private void Spawn_Light(Vector3 position)
    {
        if (m_numLights > m_maxLights){
            return;
        }

        //Make new light object
        Vector3 lightPos = position + new Vector3(m_pipeLength/2, m_pipeLength/2, m_pipeLength/2);
        m_lights[m_numLights] = Instantiate(m_lightPrefab, position, m_rotation);

        //Initialise the light's script
        LightMovementScript ms = m_lights[m_numLights].GetComponent<Light>().GetComponent<LightMovementScript>();
        ms.Initialise(gameObject, //a reference to the object this script is attached to
            position,
            m_origin + m_pipeOffset,
            m_innerBoundaryX, m_innerBoundaryY, m_innerBoundaryZ,
            m_pipeLength,
            new Vector3(m_numCubesX-1, m_numCubesY-1, m_numCubesZ-1), //Smaller distance because of initial offset & desire to keep lights inside boxes
            (int)(m_pipeLength/10)
        );

        //Set the range of the light
        m_lights[m_numLights].GetComponent<Light>().range = m_pipeLength*1.5f;
        
        // Debug.LogFormat("Spawned light @ position {0} with origin {1}, offset {2} and range {3}", position, m_origin, m_pipeOffset, m_pipeLength*1.5f);
        m_numLights++;
    }

    private void Spawn_All_Lights()
    {
        Vector3 pos;
        int pi, pj, pk;
        int adjustedNum_i = m_numCubesX - (m_voidRegionMax_i - m_voidRegionMin_i) - 1, //-1 because the lights spawn with an offset: we don't want them to appear OUTSIDE the box!
        adjustedNum_j     = m_numCubesX - (m_voidRegionMax_j - m_voidRegionMin_j) - 1,
        adjustedNum_k     = m_numCubesX - (m_voidRegionMax_k - m_voidRegionMin_k) - 1;

        for (int i=0; i<m_maxLights; i++)
        {
            pi = Random.Range(0, adjustedNum_i);
            pj = Random.Range(0, adjustedNum_j); 
            pk = Random.Range(0, adjustedNum_k);
            if (pi > m_voidRegionMin_i) pi += (m_voidRegionMax_i - m_voidRegionMin_i);
            if (pj > m_voidRegionMin_j) pj += (m_voidRegionMax_j - m_voidRegionMin_j);
            if (pk > m_voidRegionMin_k) pk += (m_voidRegionMax_k - m_voidRegionMin_k);

            pos = m_origin + m_pipeOffset + new Vector3(pi, pj, pk) * m_pipeLength;
            Spawn_Light(pos);
        }
    }
}

//#define CURVE_LINE_RENDERER_DEBUG

#undef CURVE_LINE_RENDERER_DEBUG

using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CurveLineRenderer : MonoBehaviour
{
    private const float epsilon = 0.0001f;

    /**
     * Type of the spline. Defines how the spline will be drawn near corners
     */
    public enum LineType
    {
        Default,  // Corners are not smoothed. Default value
        Rounded,  // Corners are smoothed along the arc of given radius
        Splitted, // Each segment is independent from other segments
    }

    /**
    * Type of the spline
    */
    public LineType type = LineType.Splitted;

    /**
     * Mode of the spline mesh building
     */
    public enum MeshBuildMode
    {
        Standart, //Standart mesh building mode, were used in old script versions
        Modern,   //Mesh building mode, based on Quantirion rotation between curve directions
    }

    public MeshBuildMode meshBuildMode = MeshBuildMode.Standart;

    /**
     * Width of the line
     */
    public float width = 1.0f;

    /**
     * A smoothing radius
     */
    public float radius = 1.0f;

    /**
     * Specifies a offset of smoothing angle. Defines count of the fragments of smoothing corners
     */
    public float roundedAngle = 15.0f;


    public enum UVDirect
    {
        Left_Right,
        Up_Down,
    }

    /// <summary>
    /// uv方向
    /// </summary>
    public UVDirect uvDirect = UVDirect.Up_Down;


    public enum LineDir
    {
        Road, // v3 ->0,0,-1
        Wall  //   v3 ->1,0,0
    }

    public LineDir lineType = LineDir.Wall;
    /**
     * Normal of the spline
     */
    // public Vector3 normal = new Vector3(1, 0, 0);

    /**
     * If enabled, the reverse side of the spline will be drawn
     */
    public bool reverseSideEnabled = false;

    /**
     * List of the spline vertices
     */
    public List<Vector3> vertices = new List<Vector3>()
    {
        new Vector3(0, 0, 0), new Vector3(0, 1, 0)
    };

    /// <summary>
    /// mesh normal
    /// </summary>
    public List<Vector3> meshNormal = new List<Vector3>();

    public  CurveLineRenderer nextCurve                 = null;
    public  bool              isCurveConnetionProcessed = false;
    public  CurveLineRenderer prevCurve                 = null;
    private float             previousLen               = 0f;
    private MeshRenderer meshRenderer = null;
    private Mesh mesh;
    
    public Vector2 maintexSize = Vector2.zero;


#if UNITY_EDITOR
    /// <summary>
    /// 数据改动时候调用
    /// </summary>
    public void OnValidate()
    {
        var MeshFilter = this.GetComponent<MeshFilter>();
        this.Rebuild(MeshFilter.sharedMesh);
    }

#endif

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        
        if (meshFilter && meshFilter.mesh)
        {
            mesh = meshFilter.mesh;
        }
        else
        {
            mesh            = new Mesh();
            mesh.name       = this.gameObject.name;
            meshFilter.mesh = mesh;
        }
        Invalidate();
    }

    /**
     * Set new list of vertices. Calls Invalidate()
     */
    public void SetVertices(List<Vector3> vertexList)
    {
        vertices.AddRange(vertexList);
        Invalidate();
    }

    /**
     * Add new vertex to exists list of vertices. Calls Invalidate()
     */
    public void AddVertex(Vector3 vertex)
    {
        vertices.Add(vertex);
        Invalidate();
    }

    /**
     * Insert an element into the list of vertives at the specified index. Calls Invalidate()
     */
    public void InsertVertex(int index, Vector3 vertex)
    {
        vertices.Insert(index, vertex);
        Invalidate();
    }

    /*
	 * Remove the vertex at the specified index of the list of vertices
	 */
    public void RemoveVertexAt(int index)
    {
        vertices.RemoveAt(index);
        Invalidate();
    }

    /**
     * Clear the list of vertices. Calls Invalidate()
     */
    public void ClearVertices()
    {
        vertices.Clear();
        Invalidate();
    }

    /**
     * Invalidate the whole spline. Apply changes of fields and call redraw of spline
     */
    public void Invalidate()
    {
        if (mesh == null)
        {
            Start();
        }

        mesh = Rebuild(mesh);
    }


    /// <summary>
    /// 获取line的方向
    /// </summary>
    /// <returns></returns>
    public Vector3 GetLineDirect()
    {
        if (lineType == LineDir.Wall)
        {
            return new Vector3(1, 0, 0);
        }
        else if (lineType == LineDir.Road)
        {
            return new Vector3(0, 0, -1);
        }

        return Vector3.zero;
    }

    

    /**
     * Build curve line mesh with defined parameters 
     */
    public Mesh Rebuild(Mesh mesh)
    {
        if (mesh == null)
            return null;

        //没有任何变化则不更新
        //if (!CheckVertsChange()) return mesh;
        
        mesh.Clear();
        if (vertices == null || vertices.Count < 2 || width <= 0f ||
            (type == LineType.Rounded && (roundedAngle <= 0f || radius <= 0f)))
            return mesh;

        float   length;
        Vector3 ver1, ver2;

        Vector3 nextVer1, nextVer2;

        Vector3 normal = GetLineDirect();

        // Normalize the normal vector
        Vector3 n = normal.normalized;
        // Define the list of vertices depending on the type
        List<Vector3> vertexList = new List<Vector3>();
        switch (type)
        {
            case LineType.Rounded:
                vertexList = getRoundedVertices();
                break;
            case LineType.Default:
                vertexList = getDefaultVertices();
                break;
            case LineType.Splitted:
                vertexList = getSplittedVertices();
                break;
            default:
                break;
        }

        List<Vector3> meshVertices = new List<Vector3>();
        List<Vector2> meshUvs      = new List<Vector2>();

        // First vertex
        Vector3 direction = vertexList[1] - vertexList[0];
        direction.Normalize();

        Vector3 qdir = Vector3.Cross(direction, n).normalized;

        ver1 = vertexList[0] - qdir * width * 0.5f;
        meshVertices.Add(ver1);

        ver2 = vertexList[0] + qdir * width * 0.5f;
        meshVertices.Add(ver2);
        float len = 0f;
        if (prevCurve != null && !isCurveConnetionProcessed)
        {
            len = prevCurve.previousLen;
        }

        //控制uv
        AddToUV(meshUvs, len);
        //
        // Inner vertices


        for (int i = 1; i < vertexList.Count - 1; ++i)
        {
            Vector3 nextDirection = vertexList[i + 1] - vertexList[i];
            nextDirection.Normalize();
            Vector3 nextQdir = calculateQdir(direction, nextDirection, n, qdir);
            if (type == LineType.Splitted)
            {
                // Splitted type

                len += (vertexList[i] - vertexList[i - 1]).magnitude;
                //qlen /= 2f;
                // Add both vertices                
                meshVertices.Add(vertexList[i] - qdir * width * 0.5f);
                meshVertices.Add(vertexList[i] + qdir * width * 0.5f);

                //控制uv
                AddToUV(meshUvs, len);

                meshVertices.Add(vertexList[i] - nextQdir * width * 0.5f);
                meshVertices.Add(vertexList[i] + nextQdir * width * 0.5f);

                //控制uv
                AddToUV(meshUvs, len);
            }
            else if (type == LineType.Rounded)
            {
                // Rounded type
                Vector3 pdir = (qdir + nextQdir).normalized;
                float   w    = width / Mathf.Sin(Vector3.Angle(direction, pdir) * Mathf.PI / 180.0f);

                nextVer1 = vertexList[i] - pdir * w * 0.5f;
                nextVer2 = vertexList[i] + pdir * w * 0.5f;

                len += ((nextVer1 - ver1).magnitude + (nextVer2 - ver2).magnitude) * 0.5f;


                //控制uv
                AddToUV(meshUvs, len);

                meshVertices.Add(nextVer1);
                meshVertices.Add(nextVer2);

                //控制uv
                AddToUV(meshUvs, len);

                ver1 = nextVer1;
                ver2 = nextVer2;
            }
            else
            {
                // Default type
                Vector3 pdir = (qdir + nextQdir).normalized;
                float   w    = width / Mathf.Sin(Vector3.Angle(direction, pdir) * Mathf.PI / 180.0f);

                nextVer1 = vertexList[i] - pdir * w * 0.5f;
                nextVer2 = vertexList[i] + pdir * w * 0.5f;

                float length1 = (nextVer1 - ver1).magnitude;
                float length2 = (nextVer2 - ver2).magnitude;

                if (Mathf.Abs(length1 - length2) < epsilon)
                {
                    len += (length1 + length2) * 0.5f;

                    //控制uv
                    AddToUV(meshUvs, len);

                    meshVertices.Add(nextVer1);
                    meshVertices.Add(nextVer2);

                    //控制uv
                    AddToUV(meshUvs, len);
                }
                else
                {
                    int segmentCount =
                        Mathf.CeilToInt(Mathf.Max(Mathf.Atan(Mathf.Abs(length1 - length2) / width) * 180f / Mathf.PI / Mathf.Max(1f, roundedAngle),
                                                  1f));

                    length = (length1 + length2) * 0.5f;
                    float segmentDelta = length / segmentCount;

                    for (int j = 0; j < segmentCount; ++j)
                    {
                        len += segmentDelta;

                        //控制uv
                        AddToUV(meshUvs, len);

                        meshVertices.Add(ver1 + direction * length1 * (j + 1) / segmentCount);
                        meshVertices.Add(ver2 + direction * length2 * (j + 1) / segmentCount);

                        //控制uv
                        AddToUV(meshUvs, len);
                    }
                }

                ver1 = nextVer1;
                ver2 = nextVer2;
            }

            direction = nextDirection;
            qdir      = nextQdir;
        }

        // Last vertex       
        if (type == LineType.Splitted)
        {
            meshVertices.Add(vertexList[vertexList.Count - 1] - qdir * width * 0.5f);
            meshVertices.Add(vertexList[vertexList.Count - 1] + qdir * width * 0.5f);

            var len1 = len + (vertexList[vertexList.Count - 1] - vertexList[vertexList.Count - 2]).magnitude;

            //控制uv
            AddToUV(meshUvs, len1);
        }
        else
        {
            len += (vertexList[vertexList.Count - 1] - vertexList[vertexList.Count - 2]).magnitude;

            //控制uv
            AddToUV(meshUvs, len);

            meshVertices.Add(vertexList[vertexList.Count - 1] - qdir * width * 0.5f);
            meshVertices.Add(vertexList[vertexList.Count - 1] + qdir * width * 0.5f);

            //控制uv
            AddToUV(meshUvs, len);
        }

        if (nextCurve != null)
        {
            nextCurve.prevCurve = GetComponent<CurveLineRenderer>();
            Vector3   lastVertex         = vertices[vertices.Count - 1];
            Vector3   nextCurveNormal    = nextCurve.GetLineDirect().normalized;
            float     nextCurveWidth     = nextCurve.width;
            Transform nextCurveTransform = nextCurve.transform;
            Vector3   nextCurveBegin     = nextCurve.vertices[0];
            Vector3   nextCurveDirection = (nextCurve.vertices[1] - nextCurve.vertices[0]).normalized;
            Vector3   nextCurveQDir      = Vector3.Cross(nextCurveDirection, nextCurveNormal).normalized;
            //Vector3 position = gameObject.transform.position;
            Vector3   position  = Vector3.zero;
            Transform transform = gameObject.transform;
            nextVer1 = nextCurveBegin -
                        nextCurveQDir * nextCurveWidth * 0.5f;
            nextVer2 = nextCurveBegin +
                        nextCurveQDir * nextCurveWidth * 0.5f;
            nextVer1 =  transform.InverseTransformPoint(nextCurveTransform.TransformPoint(nextVer1)) + position;
            nextVer2 =  transform.InverseTransformPoint(nextCurveTransform.TransformPoint(nextVer2)) + position;
            len      += (nextCurveBegin - lastVertex + nextCurve.transform.position - transform.position).magnitude;

            meshUvs.Add(new Vector2(1, len));
            meshUvs.Add(new Vector2(0, len));

            meshVertices.Add(nextVer1);
            meshVertices.Add(nextVer2);

            meshUvs.Add(new Vector2(1, len));
            meshUvs.Add(new Vector2(0, len));

            ver1        = nextVer1;
            ver2        = nextVer2;
            previousLen = len;
        }

        // Build mesh
        BuildMesh(mesh, meshVertices, meshUvs, meshUvs.Count / 4);
        if (prevCurve != null && !isCurveConnetionProcessed)
        {
            isCurveConnetionProcessed = true;
            prevCurve.Invalidate();
        }

        return mesh;
    }




    /// <summary>
    /// 添加到uv
    /// </summary>
    /// <param name="meshUvs"></param>
    /// <param name="len"></param>
    private void AddToUV(List<Vector2> meshUvs, float len)
    {
        if (meshRenderer == null)
        {
            meshRenderer = this.GetComponent<MeshRenderer>();
        }


        if (uvDirect == UVDirect.Left_Right)
        {
            meshUvs.Add(new Vector2(1, len));
            meshUvs.Add(new Vector2(0, len));
        }
        else if (uvDirect == UVDirect.Up_Down)
        {
//            meshUvs.Add(new Vector2( len,0));
//            meshUvs.Add(new Vector2( len,1));

//            if (len == 0)
//            {
//                meshUvs.Add(new Vector2(0, 0));
//                meshUvs.Add(new Vector2(0, 1));
//
//                return;
//            }

            var scale = maintexSize.x / maintexSize.y;
            meshUvs.Add(new Vector2(len / scale / width, 0));
            meshUvs.Add(new Vector2(len / scale / width, 1));
        }
    }

    /**
     * Build mesh by list of vertices
     */
    private void BuildMesh(Mesh mesh, List<Vector3> meshVertices, List<Vector2> meshUvs, int segmentCount)
    {
        int trianglesCount = segmentCount * 2 * 3 * (reverseSideEnabled ? 2 : 1);

        List<int> triangles = new List<int>();
        for (int i = 0; i < trianglesCount; ++i)
            triangles.Add(i);

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs      = new List<Vector2>();
        List<Vector3> normals  = new List<Vector3>();
        for (int i = 0; i < segmentCount; ++i)
        {
            // Top side of the spline
            if (type == LineType.Splitted)
            {
                vertices.Add(meshVertices[i * 4]);
                vertices.Add(meshVertices[i * 4 + 1]);
                vertices.Add(meshVertices[i * 4 + 2]);
                vertices.Add(meshVertices[i * 4 + 2]);
                vertices.Add(meshVertices[i * 4 + 1]);
                vertices.Add(meshVertices[i * 4 + 3]);
            }
            else
            {
                vertices.Add(meshVertices[i * 2]);
                vertices.Add(meshVertices[i * 2 + 1]);
                vertices.Add(meshVertices[i * 2 + 2]);
                vertices.Add(meshVertices[i * 2 + 2]);
                vertices.Add(meshVertices[i * 2 + 1]);
                vertices.Add(meshVertices[i * 2 + 3]);
            }

            uvs.Add(meshUvs[i * 4]);
            uvs.Add(meshUvs[i * 4 + 1]);
            uvs.Add(meshUvs[i * 4 + 2]);
            uvs.Add(meshUvs[i * 4 + 2]);
            uvs.Add(meshUvs[i * 4 + 1]);
            uvs.Add(meshUvs[i * 4 + 3]);

            for (int j = 0; j < 6; j++)
            {
                normals.Add(meshNormal[i]);
            }
            // Back side of the spline
            if (reverseSideEnabled)
            {
                if (type == LineType.Splitted)
                {
                    vertices.Add(meshVertices[i * 4 + 1]);
                    vertices.Add(meshVertices[i * 4]);
                    vertices.Add(meshVertices[i * 4 + 2]);
                    vertices.Add(meshVertices[i * 4 + 1]);
                    vertices.Add(meshVertices[i * 4 + 2]);
                    vertices.Add(meshVertices[i * 4 + 3]);
                }
                else
                {
                    vertices.Add(meshVertices[i * 2 + 1]);
                    vertices.Add(meshVertices[i * 2]);
                    vertices.Add(meshVertices[i * 2 + 2]);
                    vertices.Add(meshVertices[i * 2 + 1]);
                    vertices.Add(meshVertices[i * 2 + 2]);
                    vertices.Add(meshVertices[i * 2 + 3]);
                }

                uvs.Add(meshUvs[i * 4]);
                uvs.Add(meshUvs[i * 4 + 1]);
                uvs.Add(meshUvs[i * 4 + 3]);
                uvs.Add(meshUvs[i * 4]);
                uvs.Add(meshUvs[i * 4 + 3]);
                uvs.Add(meshUvs[i * 4 + 2]);
            }
        }

        // Set parameters to the mesh
        mesh.vertices  = vertices.ToArray();
        mesh.uv        = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        if (lineType == LineDir.Wall)
        {
            mesh.normals = normals.ToArray();
        }
        else
        {
            mesh.RecalculateNormals();
        }

        mesh.RecalculateBounds();

#if UNITY_EDITOR
        
#endif
    }

    private Vector3 calculateQdir(Vector3 dir, Vector3 nextDir, Vector3 n, Vector3 qdir)
    {
        Vector3 nextQdir;
        if (meshBuildMode == MeshBuildMode.Standart)
        {
            nextQdir = (Mathf.Abs(Vector3.Angle(Vector3.Cross(dir, nextDir), n) - 90f) < 1f)
                           ? qdir
                           : Vector3.Cross(nextDir, n).normalized;
        }
        else
        {
            nextQdir = rotate(dir, nextDir, qdir);
        }

        return nextQdir;
    }

    /**
     * Get the list of vertices for Default spline type
     */
    private List<Vector3> getDefaultVertices()
    {
        Vector3 n = GetLineDirect().normalized;
        float   r = (radius < width / 2) ? width / 2 : radius;

        List<Vector3> defaultVertices = new List<Vector3>();
        Vector3       dir             = (vertices[1] - vertices[0]);
        dir.Normalize();

        Vector3 qdir = Vector3.Cross(dir, n).normalized;
        defaultVertices.Add(vertices[0]);
        n = Vector3.Cross(qdir, dir);
        n.Normalize();

        for (int i = 1; i < vertices.Count - 1; ++i)
        {
            Vector3 nextDir  = vertices[i + 1] - vertices[i];
            Vector3 nextQdir = calculateQdir(dir, nextDir, n, qdir);

            nextDir.Normalize();
            nextQdir.Normalize();

            if (Vector3.Angle(nextDir, dir) < 1.0f)
            {
                // Next direction is equal to current direction
                defaultVertices.Add(vertices[i]);
            }
            else
            {
                float maxR = Mathf.Min((vertices[i] - vertices[i - 1]).magnitude,
                                       (vertices[i               + 1] - vertices[i]).magnitude) * 0.5f;

                //float r = Radius;
                Vector3 pdir = ((-dir).normalized + nextDir.normalized).normalized;
                if (pdir.magnitude < epsilon || (qdir - nextQdir).magnitude < epsilon)
                {
                    // Normal is in surface of rotation
                    Vector3 normal1 = -Vector3.Cross(dir, qdir);
                    Vector3 normal2 = -Vector3.Cross(nextDir, nextQdir);
                    pdir = (normal1 + normal2).normalized;
                }

                float angle   = Vector3.Angle(dir, pdir);
                float rwidth  = r / Mathf.Sin(angle * Mathf.PI / 180.0f);
                float rlength = Mathf.Sqrt(rwidth              * rwidth - r * r);

                if (rlength > maxR)
                {
                    rlength = maxR;
                    rwidth  = rlength / Mathf.Cos(angle * Mathf.PI / 180.0f);
                }

                Vector3 rightPoint = vertices[i] + nextDir * rlength;
                Vector3 leftPoint  = vertices[i] - dir     * rlength;

                defaultVertices.Add(leftPoint);
                defaultVertices.Add(vertices[i]);
                defaultVertices.Add(rightPoint);
            }

            dir  = nextDir;
            qdir = nextQdir;
        }

        defaultVertices.Add(vertices[vertices.Count - 1]);

        return defaultVertices;
    }

    /**
    * Vector rotations on transformation between from and two vectors
    */

    private Vector3 rotate(Vector3 from, Vector3 to, Vector3 rotatedVector)
    {
        Quaternion transform = Quaternion.FromToRotation(from.normalized, to.normalized);
        Vector3    result    = transform * rotatedVector.normalized;
        return result.normalized;
    }

    /**
     * Get the list of vertices for Rounded spline type
     */
    private List<Vector3> getRoundedVertices()
    {
        Vector3 n = GetLineDirect().normalized;
        Vector3 center;
        float   Radius = (radius < width / 2) ? width / 2 : radius;

        List<Vector3> roundedVertices = new List<Vector3>();
        Vector3       dir             = (vertices[1] - vertices[0]);
        dir.Normalize();

        Vector3 qdir = Vector3.Cross(dir, n).normalized;
        n = Vector3.Cross(qdir, dir);
        n.Normalize();
        roundedVertices.Add(vertices[0]);
        for (int i = 1; i < vertices.Count - 1; ++i)
        {
            Vector3 nextDir  = vertices[i + 1] - vertices[i];
            Vector3 nextQdir = calculateQdir(dir, nextDir, n, qdir);
            nextDir.Normalize();
            nextQdir.Normalize();

            if (Vector3.Angle(nextDir, dir) < 1.0f)
            {
                // Next direction is equal to current direction
                roundedVertices.Add(vertices[i]);
            }
            else
            {
                float maxR = Mathf.Min((vertices[i] - vertices[i - 1]).magnitude,
                                       (vertices[i               + 1] - vertices[i]).magnitude) * 0.5f;

                float   r    = Radius;
                Vector3 pdir = ((-dir).normalized + nextDir.normalized).normalized;
                if (pdir.magnitude < epsilon || (qdir - nextQdir).magnitude < epsilon)
                {
                    // Normal is in surface of rotation
                    Vector3 normal1 = -Vector3.Cross(dir, qdir);
                    Vector3 normal2 = -Vector3.Cross(nextDir, nextQdir);
                    pdir = (normal1 + normal2).normalized;
                }

                float angle   = Vector3.Angle(dir, pdir);
                float rwidth  = r / Mathf.Sin(angle * Mathf.PI / 180.0f);
                float rlength = Mathf.Sqrt(rwidth              * rwidth - r * r);

                if (rlength > maxR)
                {
                    rlength = maxR;
                    rwidth  = rlength / Mathf.Cos(angle * Mathf.PI / 180.0f);
                }

                Vector3 vertex1 = vertices[i] + pdir * rwidth;
                Vector3 vertex2 = vertices[i] - pdir * rwidth;

                Vector3 rightPoint = vertices[i] + nextDir * rlength;
                Vector3 leftPoint  = vertices[i] + dir     * (-rlength);

                center = (Mathf.Abs(Vector3.Dot(leftPoint - vertex1, dir)) < epsilon) ? vertex1 : vertex2;

                Vector3 rotateAxis   = Vector3.Cross(dir, nextDir);
                Vector3 rotateVector = leftPoint - center;

                angle = Vector3.Angle(leftPoint - center, rightPoint - center);
                int   segmentCount = (int) (angle / roundedAngle + 0.5f);
                float angleDelta   = angle / segmentCount;

                Quaternion q = Quaternion.AngleAxis(angleDelta, rotateAxis);

                roundedVertices.Add(leftPoint);
                for (int j = 0; j < segmentCount - 1; ++j)
                {
                    rotateVector = q * rotateVector;
                    roundedVertices.Add(center + rotateVector);
                }

                roundedVertices.Add(rightPoint);
            }

            dir  = nextDir;
            qdir = nextQdir;
        }

        roundedVertices.Add(vertices[vertices.Count - 1]);

        return roundedVertices;
    }


    private List<Vector3> getSplittedVertices()
    {
        return new List<Vector3>(vertices);
    }


    /// <summary>
    /// 计算法线
    /// </summary>
    /// <param name="i"></param>
    public Vector3 CalcNormal(int index)
    {
        while (meshNormal.Count < vertices.Count)
        {
            meshNormal.Add(Vector3.zero);
        }

        if (index >= vertices.Count - 1)
        {
            return Vector3.zero;
        }

        var p1 = vertices[index];
        var p2 = vertices[index + 1];

        var linedir  = p2      - p1;
        var linetype = linedir + new Vector3(0, 0, 1);

        var normal = Vector3.Cross(linedir, linetype).normalized;


        return normal;
    }
}
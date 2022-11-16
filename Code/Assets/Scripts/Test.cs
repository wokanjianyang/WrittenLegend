using System.Collections;
using System.Collections.Generic;
using DigitalRuby.FastLineRenderer;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    /// <summary>
    /// Fast line renderer script
    /// </summary>
    [Tooltip("Fast line renderer script")]
    public FastLineRenderer LineRenderer;
    
    [LabelText("列数")]
    public int ColCount = 9;

    [LabelText("行数")]
    public int RowCount = 16;

    [LabelText("线宽")]
    public float LineWidth = 1f;

    private float CellWidth = 120;

    // Start is called before the first frame update
    void Start()
    {
        LineRenderer.Reset();
        List<Vector3> points = new List<Vector3>();

        for (var i = 0; i <= ColCount; i++)
        {
            points.Add(new Vector3(i*CellWidth,0));
            points.Add(new Vector3(i*CellWidth,RowCount*CellWidth));

        }
        for (var j = 0; j <= RowCount; j++)
        {
            points.Add(new Vector3(0,j*CellWidth));
            points.Add(new Vector3(ColCount*CellWidth,j*CellWidth));
        }
        // create properties - do this once, before your loop
        FastLineRendererProperties props = new FastLineRendererProperties();
        props.Radius = LineWidth;
        props.LineJoin = FastLineRendererLineJoin.Round;
        
        LineRenderer.AddLines(props, points,(_props) =>
        {
            
        },true,true);

        LineRenderer.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

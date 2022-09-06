using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineRendererHUD : Graphic
{

    public Vector2Int gridSize = new Vector2Int(1, 1);
    public float thickness = 10f;


    float width;
    float height;
    float cellWidth;
    float cellHeight;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        cellWidth = width / (float)gridSize.x;
        cellHeight = height / (float)gridSize.y;


        int count = 0;

        for(int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                DrawCell(x, y, count, vh);
                count++;
            }
        }

    }


    private void DrawCell(int x, int y, int index, VertexHelper vh)
    {
        float xPos = cellWidth * x;
        float yPos = cellHeight * y;

        UIVertex vertext = UIVertex.simpleVert;
        vertext.color = color;

        vertext.position = new Vector3(xPos, yPos);
        vh.AddVert(vertext);

        vertext.position = new Vector3(xPos, yPos+ cellHeight);
        vh.AddVert(vertext);

        vertext.position = new Vector3(xPos+ cellWidth, yPos+ cellHeight);
        vh.AddVert(vertext);


        vertext.position = new Vector3(xPos+ cellWidth, yPos);
        vh.AddVert(vertext);




        float widthSqr = thickness * thickness;
        float distanceSqr = widthSqr / 2f;
        float distance = Mathf.Sqrt(distanceSqr);

        vertext.position = new Vector3(xPos + distance, yPos + distance);
        vh.AddVert(vertext);

        vertext.position = new Vector3(xPos + distance, yPos + (cellHeight - distance));
        vh.AddVert(vertext);

        vertext.position = new Vector3(xPos + (cellWidth - distance), yPos+( cellHeight - distance));
        vh.AddVert(vertext);

        vertext.position = new Vector3(xPos + (cellWidth- distance), yPos + distance);
        vh.AddVert(vertext);

        int offset = index * 8;

        vh.AddTriangle(offset + 0, offset + 1, offset + 5);
        vh.AddTriangle(offset + 5, offset + 4, offset + 0);
                       
        vh.AddTriangle(offset + 1, offset + 2, offset + 6);
        vh.AddTriangle(offset + 6, offset + 5, offset + 1);
                       
        vh.AddTriangle(offset + 2, offset + 3, offset + 7);
        vh.AddTriangle(offset + 7, offset + 6, offset + 2);
                       
        vh.AddTriangle(offset + 3, offset + 0, offset + 4);
        vh.AddTriangle(offset + 4, offset + 7, offset + 3);
    }
}
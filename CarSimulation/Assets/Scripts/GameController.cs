using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public Camera mainCamera;

    Vector2 cameraSize;
    Vector2 cameraPos;

    List<Vector2> ptList;
    List<Vector2> vtList;
    List<Vector2> vnList;

    // Use this for initialization
    void Start()
    {

        ptList = new List<Vector2>();
        vtList = new List<Vector2>();
        vnList = new List<Vector2>();

        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;

        cameraSize = new Vector2(width, height);
        cameraPos.x = cameraPos.x - mainCamera.orthographicSize;
        cameraPos.y = cameraPos.y - (mainCamera.orthographicSize * mainCamera.aspect);

        ComputeFunction();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        for (int i = 0; i < vtList.Count; i++)
        {
            Debug.DrawRay(ptList[i], vtList[i], Color.black);
            //Debug.DrawRay(ptList[i], vnList[i], Color.white);
            //Debug.DrawRay(ptList[i], vnList[i] * -1, Color.white);
        }
    }

    void ComputeFunction()
    {
        Vector2 PosF = mainCamera.transform.position;
        PosF.y -= cameraSize.y / 2f;

        Vector2 Pos0 = mainCamera.transform.position;
        Pos0.y += cameraSize.y / 2f;

        float init = mainCamera.transform.position.x - cameraSize.x / 2f;
        float limit = mainCamera.transform.position.x + cameraSize.x / 2f;

        float step = 1f / 10f;
        float dstep = 0.7f;

        for (float x = init; x < limit; x += step)
        {
            Pos0.x = PosF.x = x;

            RaycastHit2D hit_t0 = Physics2D.Raycast(PosF, Pos0 - PosF, (Pos0 - PosF).magnitude, LayerMask.GetMask("Walls"));

            if (hit_t0.collider != null)
            {
                float a = x;
                float f2a = hit_t0.point.y;

                if (f2a == 4.83f)
                    Debug.DrawLine(new Vector2(a, f2a), new Vector2(a + 1, f2a + 1), Color.yellow);


                
                Pos0.x += dstep;
                PosF.x += dstep;

                RaycastHit2D hit_t1 = Physics2D.Raycast(PosF, Pos0 - PosF, (Pos0 - PosF).magnitude, LayerMask.GetMask("Walls"));
                float f2a_1 = hit_t1.point.y;

                Pos0.x -= dstep;
                PosF.x -= dstep;

                float df2_dx = f2a_1 - f2a;
                //float _df2_dx = 1f / df2_dx;

                float alpha = 0f;
                Vector2 vt_ = new Vector2(a, f2a) + (alpha - 0.5f) * (new Vector2(a + 1f, df2_dx + f2a_1) - new Vector2(a, f2a));
                Vector2 vt0 = new Vector2(a, f2a) + alpha * (new Vector2(a + 1f, df2_dx + f2a_1) - new Vector2(a, f2a));
                Vector2 vt1 = new Vector2(a, f2a) + (alpha + 0.5f) * (new Vector2(a + 1f, df2_dx + f2a_1) - new Vector2(a, f2a));

                Vector2 pt = vt0;
                Vector2 vt = vt1 - vt0;

                Vector2 vn = vt;
                vn.x = vt.y;
                vn.y = vt.x;

                ptList.Add(pt);

                vtList.Add(vt);
                vnList.Add(vn);

            }
        }

        for (float x = init * 2f; x < limit; x += step)
        {
            Pos0.x = PosF.x = x;

            RaycastHit2D hit_t0 = Physics2D.Raycast(Pos0, PosF - Pos0, (PosF - Pos0).magnitude, LayerMask.GetMask("Walls"));

            if (hit_t0.collider != null)
            {
                float a = x;
                float f2a = hit_t0.point.y;

                if (f2a == 4.83f)
                    Debug.DrawLine(new Vector2(a, f2a), new Vector2(a + 1, f2a + 1), Color.yellow);

                Pos0.x += dstep;
                PosF.x += dstep;

                RaycastHit2D hit_t1 = Physics2D.Raycast(Pos0, PosF - Pos0, (PosF - Pos0).magnitude, LayerMask.GetMask("Walls"));
                float f2a_1 = hit_t1.point.y;

                Pos0.x -= dstep;
                PosF.x -= dstep;

                float df2_dx = f2a_1 - f2a;
                //float _df2_dx = 1f / df2_dx;

                float alpha = 0f;
                Vector2 vt_ = new Vector2(a, f2a) + (alpha - 0.5f) * (new Vector2(a + 1f, df2_dx + f2a_1) - new Vector2(a, f2a));
                Vector2 vt0 = new Vector2(a, f2a) + alpha * (new Vector2(a + 1f, df2_dx + f2a_1) - new Vector2(a, f2a));
                Vector2 vt1 = new Vector2(a, f2a) + (alpha + 0.5f) * (new Vector2(a + 1f, df2_dx + f2a_1) - new Vector2(a, f2a));

                Vector2 pt = vt0;
                Vector2 vt = vt1 - vt0;

                Vector2 vn = vt;
                vn.x = vt.y;
                vn.y = vt.x;

                ptList.Add(pt);

                vtList.Add(vt);
                vnList.Add(vn);

            }

        }

    }
}

using System;
using System.Collections.Generic;
using UnityCore;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 曲线编辑器
/// </summary>
//[CanEditMultipleObjects]
[CustomEditor(typeof(CqCurveMono))]
public class CqCurveMonoEditor : Editor
{
    [PreferenceItem("曲线编辑器配置")]
    static void PreferencesGUI()
    {
        //EditorGUILayout.LabelField("SVN Settings", EditorStyles.boldLabel);
        var config = CqCurveEditorConfig.Inst;
        var sphereSize = EditorGUILayout.Slider("顶点球大小", config.sphereSizeScale, 0.1f, 10f);
        if (sphereSize != config.sphereSizeScale)
        {
            config.sphereSizeScale = sphereSize;
            CqCurveEditorConfig.Inst = config;
        }

        var closeGrid = EditorGUILayout.FloatField("顶点坐标栅格化单位", config.closeGrid);
        if (closeGrid != config.closeGrid)
        {
            config.closeGrid = closeGrid;
            CqCurveEditorConfig.Inst = config;
        }

        var inTangentColor = EditorGUILayout.ColorField("进入切线颜色", config.inTangentColor);
        if (inTangentColor != config.inTangentColor)
        {
            config.inTangentColor = inTangentColor;
            CqCurveEditorConfig.Inst = config;
        }
        var outTangentColor = EditorGUILayout.ColorField("离开切线颜色", config.outTangentColor);
        if (outTangentColor != config.outTangentColor)
        {
            config.outTangentColor = outTangentColor;
            CqCurveEditorConfig.Inst = config;
        }

        var symmetricTangentColor = EditorGUILayout.ColorField("对称切线颜色", config.symmetricTangentColor);
        if (symmetricTangentColor != config.symmetricTangentColor)
        {
            config.symmetricTangentColor = symmetricTangentColor;
            CqCurveEditorConfig.Inst = config;
        }

        var lineColor = EditorGUILayout.ColorField("曲线颜色", config.lineColor);
        if (lineColor != config.lineColor)
        {
            config.lineColor = lineColor;
            CqCurveEditorConfig.Inst = config;
        }
        var lineWidth = EditorGUILayout.FloatField("曲线宽度", config.lineWidth);
        if (lineWidth != config.lineWidth)
        {
            config.lineWidth = lineWidth;
            CqCurveEditorConfig.Inst = config;
        }
        var smoothK = EditorGUILayout.FloatField("贝塞尔平滑系数", config.smoothK);
        if (smoothK != config.smoothK)
        {
            config.smoothK = lineWidth;
            CqCurveEditorConfig.Inst = config;
        }
        var noneColor = EditorGUILayout.ColorField("顶点正常状态", config.noneColor);
        if (noneColor != config.noneColor)
        {
            config.noneColor = noneColor;
            CqCurveEditorConfig.Inst = config;
        }

        var selectColor = EditorGUILayout.ColorField("顶点选中状态", config.selectColor);
        if (selectColor != config.selectColor)
        {
            config.selectColor = selectColor;
            CqCurveEditorConfig.Inst = config;
        }

        var nearColor = EditorGUILayout.ColorField("顶点悬浮状态", config.nearColor);
        if (nearColor != config.nearColor)
        {
            config.nearColor = nearColor;
            CqCurveEditorConfig.Inst = config;
        }
    }
    public new CqCurveMono target
    {
        get
        {
            return (CqCurveMono)base.target;
        }
    }

    string[] mGroupNames;

    private void OnEnable()
    {
        Tools.current = Tool.View;
        var list = NPCActionConfig.Inst.groups.ConvertAll(x => x.name);
        list.Insert(0, "无");
        mGroupNames = list.ToArray();
    }
    private void OnDisable()
    {
    }

    //鼠标操作的点索引
    int nearIndex = -1;

    CqCurvePoint selectNode;

    int editorOpr = 0;
    string[] editorOprs = new string[] { "位置编辑", "切线编辑" };
    const string winName = "曲线编辑器";
    const string lockDraw = "锁定在场景中绘制";

    const string selectDesc = "选中顶点({0})";

    const string smoothDesc = "平滑";
    const string symmetricDesc = "切线对称";
    const string closeDesc = "闭合曲线";
    const string delDesc = "删除(D)";
    const string moveCenterDesc = "平移中心点";
    float sphereSize = 1f;

    GUILayoutOption[] buttonStyle = new[] { GUILayout.Height(30), GUILayout.Width(100) };

    GUILayoutOption[] windowStyle = new[] { GUILayout.Height(500), GUILayout.Width(120) };
    GUILayoutOption[] rightWindowStyle = new[] { GUILayout.Height(500), GUILayout.Width(100) };
    Rect rightRect = new Rect() { width = Screen.width, height = Screen.height, x = Screen.width - 100, y = 120 };

    GUILayoutOption[] buttonDelStyle = new[] { GUILayout.Height(20), GUILayout.Width(20) };
    GUILayoutOption[] actionStyle = new[] { GUILayout.Height(20) };
    /// <summary>
    /// 通过c复制位置
    /// </summary>
    static Vector3? pickupPoint;

    bool reCalc;
    void DrawUIOnScene()
    {
        var evt = Event.current;
        var config = CqCurveEditorConfig.Inst;
        Handles.BeginGUI();
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(winName, "window", windowStyle);

        target.lockDrawInScene = GUILayout.Toggle(target.lockDrawInScene, lockDraw);
        target.curve.close = GUILayout.Toggle(target.curve.close, closeDesc);

        EditorGUI.indentLevel++;
        if (GUILayout.Button(moveCenterDesc, buttonStyle))
        {
            target.curve.MoveCenterToZero();
        }
        for (int i = 0; i < editorOprs.Length; i++)
        {
            GUILayout.Space(5);
            var bl = (editorOpr == i);
            var temp = GUILayout.Toggle(bl, editorOprs[i], "button", buttonStyle);
            if (temp != bl)
            {
                editorOpr = i;
                Tools.current = Tool.View;
            }
        }
        if (selectNode != null)
        {
            GUILayout.Space(10);
            GUILayout.Label(string.Format(selectDesc, target.curve.points.IndexOf(selectNode)));
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            var bl = GUILayout.Toggle(!selectNode.singular, symmetricDesc, buttonStyle);
            if (bl == selectNode.singular)
            {
                selectNode.singular = !bl;
                selectNode.outVec = -selectNode.inVec;
            }
            if (GUILayout.Button(delDesc, buttonStyle) || (evt.keyCode == KeyCode.D && evt.modifiers == EventModifiers.None))
            {
                target.curve.points.Remove(selectNode);
                selectNode = null;
                return;
            }
            if (GUILayout.Button(smoothDesc, buttonStyle))
            {
                var index = target.curve.points.IndexOf(selectNode);
                target.curve.Smooth(index, config.smoothK);
            }
            if (GUILayout.Button("折点", buttonStyle))
            {
                selectNode.inVec = selectNode.outVec = Vector3.zero;
            }


            var group = NPCActionConfig.Inst.GetById(selectNode.data);

            var lastIndex = NPCActionConfig.Inst.groups.IndexOf(group) + 1;

            var tempIndex = EditorGUILayout.Popup(lastIndex, mGroupNames, actionStyle);
            if (tempIndex != lastIndex)
            {
                if (tempIndex == 0)
                {
                    selectNode.data = 0;
                }
                else
                {
                    selectNode.data = NPCActionConfig.Inst.groups[tempIndex - 1].Id;
                }
            }
            if (pickupPoint != null)
            {
                GUILayout.Label("已拾取位置,设置(V):\n" + (Vector3)pickupPoint);
            }
            else
            {
                GUILayout.Label("拾取一个位置(C)");
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

        }

        GUILayout.EndVertical();
        EditorGUI.indentLevel--;
        //if (selectNode != null && selectNode.listData != null)
        //{
        //    GUILayout.BeginArea(rightRect);
        //    GUILayout.BeginVertical("附加数据", "window", windowStyle);

        //    var names = EnumUtil.GetEnumNames<CqCurvePointDataStyle>();
        //    for (int i = 0; i < selectNode.listData.Count; i++)
        //    {
        //        var it = selectNode.listData[i];
        //        if (it == null)
        //        {
        //            selectNode.listData = null;
        //            break;
        //        }
        //        GUILayout.BeginHorizontal();
        //        var style = (int)it.a;
        //        var x = EditorGUILayout.Popup(style, names, actionStyle);

        //        if (it.a == CqCurvePointDataStyle.PlayAni)
        //        {

        //            it.strValue = EditorGUILayout.TextField(it.strValue);
        //        }
        //        else
        //        {
        //            it.floatValue = EditorGUILayout.FloatField(it.floatValue);
        //        }
        //        if (GUILayout.Button("-", buttonDelStyle))
        //        {
        //            selectNode.listData.RemoveAt(i);
        //            return;
        //        }
        //        if (x != style)
        //        {
        //            it.a = (CqCurvePointDataStyle)x;
        //        }
        //        GUILayout.EndHorizontal();
        //    }
        //    if (GUILayout.Button("+", buttonDelStyle))
        //    {
        //        selectNode.listData.Add(new CqCurvePointData { });
        //    }
        //    GUILayout.EndVertical();
        //    GUILayout.EndArea();
        //}

        GUILayout.EndHorizontal();
        Handles.EndGUI();
    }
    void OnSceneGUI()
    {
        var cqCurve = target.curve;
        if (cqCurve == null) return;
        DrawCurve();
        var points = cqCurve.points;
        if (points == null || points.Count < 2) return;

        var config = CqCurveEditorConfig.Inst;
        var evt = Event.current;
        var transform = target.transform;

        var worldMat = transform.localToWorldMatrix;
        var localMat = transform.worldToLocalMatrix;
        DrawUIOnScene();
        Func<Vector3, Vector3> ToWorld = (a) => worldMat.MultiplyPoint(a);
        Func<Vector3, Vector3> ToLocal = (a) => localMat.MultiplyPoint(a);

        Camera cam = SceneView.currentDrawingSceneView.camera;
        sphereSize = CqCurveEditorConfig.Inst.sphereSizeScale * (Vector3.Distance(cam.transform.position, transform.position) / 100f);
        Func<Vector3, float, bool> Button = (position, scale) =>
        {
            return Handles.Button(position, Quaternion.identity, sphereSize / 2, sphereSize / 2, Handles.DotHandleCap);
        };


        #region 左键移动时(右键弹起会暂停屏幕线程所以也要算一次)计算和哪个顶点比较近,获得索引

        if (
            (evt.type == EventType.MouseMove || evt.type == EventType.Used) ||
             (evt.button == 1 && evt.type == EventType.MouseUp))
        {
            var tempNearIndex = -1;
            var ray = SceneViewUtil.GetClickRay().Multiply(localMat);

            for (int i = 0; i < points.Count; i++)
            {
                var vec = ray.GetVerticalVec(points[i].point);

                if (worldMat.MultiplyVector(vec).magnitude < sphereSize * 2)
                {
                    tempNearIndex = i;
                    break;
                }
            }
            if (nearIndex != tempNearIndex)
            {
                nearIndex = tempNearIndex;

                Repaint();
            }
        }

        //Handles.SphereHandleCap(0, SceneView.currentDrawingSceneView.pivot, Quaternion.identity, sphereSize, EventType.Repaint);

        //单击选中
        if (nearIndex != -1 && evt.button == 0 && evt.type == EventType.MouseDown)
        {
            selectNode = points[nearIndex];
            SceneView.currentDrawingSceneView.LookAt(ToWorld(selectNode.point));
        }
        else if (evt.button == 0 && evt.type == EventType.MouseUp)
        {
            //selectNode = null;
        }
        var selectIndex = points.IndexOf(selectNode);
        #endregion


        //双击
        if (evt.button == 0 && evt.type == EventType.MouseDown)
        {
            if (Time.realtimeSinceStartup - lastClickTime < 0.3f)
            {
                OnDoubleClick(worldMat);
            }
            lastClickTime = Time.realtimeSinceStartup;
        }
        #region 绘制球和顶点索引
        for (int i = 0; i < points.Count; i++)
        {
            var it = points[i];
            var p = ToWorld(it.point);
            var group = NPCActionConfig.Inst.GetById(it.data);

            Handles.Label(p, (group == null) ? i.ToString() : string.Format("{0}({1})", i, group.name));

            var size = sphereSize;
            if (i == selectIndex)
            {
                Handles.color = config.selectColor;
                size *= 2;
            }
            else if (i == nearIndex)
            {
                Handles.color = config.nearColor;
                size *= 2;
            }
            else
            {
                Handles.color = config.noneColor;
            }

            Handles.SphereHandleCap(0, p, Quaternion.identity, size, EventType.Repaint);
            //Gizmos.DrawSphere(p, sphereSize(p) * (nearIndex == i ? 2 : 1));
        }
        #endregion


        if (selectNode != null)
        {
            switch (evt.keyCode)
            {
                case KeyCode.C:
                    {
                        pickupPoint = ToWorld(selectNode.point);
                        break;
                    }
                case KeyCode.V:
                    {
                        if (pickupPoint != null) selectNode.point = ToLocal((Vector3)pickupPoint);
                        break;
                    }
            }
            var pointWorld = ToWorld(selectNode.point);
            var inTangentWorld = ToWorld(selectNode.inTangent);
            var outTangentWorld = ToWorld(selectNode.outTangent);
            switch (editorOpr)
            {
                case 0://位置编辑
                    {
                        var p = Handles.DoPositionHandle(pointWorld, Quaternion.identity/*worldMat.rotation*/);

                        if (pointWorld != p)
                        {
                            selectNode.point = ToLocal(p).Rasterize(config.closeGrid);
                        }
                        break;
                    }
                case 1://切线编辑
                    {
                        if (cqCurve.close || selectIndex != 0)
                        {
                            Handles.color = selectNode.singular ? config.inTangentColor : config.symmetricTangentColor;
                            Handles.DrawLine(pointWorld, inTangentWorld);

                            if (Button((pointWorld + inTangentWorld) / 2, 1f))
                            {
                                selectNode.inVec = Vector2.zero;
                                if (!selectNode.singular)
                                {
                                    selectNode.outVec = Vector2.zero;
                                }
                            }

                            var tangentPos = Handles.DoPositionHandle(inTangentWorld, Quaternion.identity);
                            if (tangentPos != inTangentWorld)
                            {
                                selectNode.inVec = ToLocal(tangentPos) - selectNode.point;

                                if (!selectNode.singular)
                                {
                                    selectNode.outVec = -selectNode.inVec;
                                }
                            }
                        }
                        if (cqCurve.close || selectIndex != points.Count - 1)
                        {
                            Handles.color = selectNode.singular ? config.outTangentColor : config.symmetricTangentColor;
                            Handles.DrawLine(pointWorld, outTangentWorld);

                            {
                                if (Button((pointWorld + outTangentWorld) / 2, 1f))
                                {
                                    selectNode.outVec = Vector3.zero;
                                    if (!selectNode.singular)
                                    {
                                        selectNode.inVec = Vector2.zero;
                                    }
                                }
                            }
                            var tangentPos = Handles.DoPositionHandle(outTangentWorld, Quaternion.identity);
                            if (tangentPos != outTangentWorld)
                            {
                                selectNode.outVec = ToLocal(tangentPos) - selectNode.point;
                                if (!selectNode.singular)
                                {
                                    selectNode.inVec = -selectNode.outVec;
                                }
                            }
                        }
                        break;
                    }
            }
        }

        //AddRightMouseMenu();
    }
    float lastClickTime;
    void OnDoubleClick(Matrix4x4 worldMat)
    {
        var ray = SceneViewUtil.GetClickRay().Multiply(target.transform.worldToLocalMatrix);

        int index;
        var t = target.curve.GetCrossoverPoint(ray, out index, worldMat, sphereSize * 2);
        if (index != -1)//点击到曲线上一点
        {
            var point = target.curve.points[index].Split(target.curve.points.GetItemByRound(index + 1), t);
            target.curve.points.Insert(index + 1, point);
            selectNode = point;
        }
    }

    /// <summary>
    /// 绘制曲线
    /// </summary>
    private void DrawCurve()
    {
        var curve = target.curve;
        var transform = target.transform;
        {
            if (curve == null || curve.points == null || curve.points.Count < 2) return;
            var config = CqCurveEditorConfig.Inst;
            var worldMat = transform.localToWorldMatrix;

            var worldList = new List<Vector3>();
            for (int i = 0; i < curve.points.Count; i++)
            {
                var p = worldMat.MultiplyPoint(curve.points[i].point);
                worldList.Add(p);
            }

            int j = curve.points.Count - 1;
            if (curve.close)
            {
                j++;
            }
            for (int i = 0; i < j; i++)
            {
                var a = i;
                var b=(i + 1)% curve.points.Count;
                var p1 = curve.points[a];
                var p2 = curve.points[b];
                Handles.DrawBezier(
                    worldList[a],
                    worldList[b],
                    worldMat.MultiplyPoint(p1.outTangent),
                    worldMat.MultiplyPoint(p2.inTangent),
                    config.lineColor, null, config.lineWidth);
            }
        }
    }

    /// <summary>
    /// 添加右键菜单
    /// </summary>
    [Obsolete]
    void AddRightMouseMenu()
    {
        var evt = Event.current;
        if (evt != null && evt.button == 1 && evt.type == EventType.MouseUp)
        {
            if (nearIndex == -1)
            {
                GenericMenu menu = new GenericMenu();
                int i = 0;
                foreach (var it in editorOprs)
                {
                    menu.AddItem(new GUIContent(it), i == editorOpr, OnMenuClick, i);
                    i++;
                }
                menu.ShowAsContext();
            }
            else
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent(symmetricDesc), target.curve.points[nearIndex].singular, OnSmoothChange, null);
                menu.ShowAsContext();
            }
        }
    }
    [Obsolete]
    void OnMenuClick(object userData)
    {
        editorOpr = (int)userData;
        //EditorUtility.DisplayDialog("Tip", "OnMenuClick" + userData.ToString(), "Ok");
    }
    [Obsolete]
    void OnSmoothChange(object userData)
    {
        var curve = target.curve;
        var list = curve.points;
        var point = list[nearIndex];
        point.singular = !point.singular;

        if (point.singular)
        {
            var outDis = point.outVec.magnitude;
            var inDis = point.inVec.magnitude;

            if (outDis < inDis)
            {
                point.outVec = -point.inVec;
            }
            else
            {
                point.inVec = -point.outVec;
            }
        }
    }
}
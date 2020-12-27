using CqCore;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityCore
{
    /// <summary>
    /// 滑动列表(每单元格滑动方向上的长度可以不定,不能循环)<para/>
    /// 挂在ScrollRect同物体下,循环重复利用<para/>
    /// 取content下第一个元素作为克隆体<para/>
    /// 通过UpdateData更新单元格<para/>
    /// 通过Insert/Add 添加单元格<para/>
    /// 通过RemoveAt 移除单元格
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollList : MonoBehaviourExtended, IListMono
    {


        ScrollRect sr;

        CqTweenLerp_Vector2 sizeDelta_tweenHandle;

        CqTweenLerp_Vector2 anchoredPosition_tweenHandle;

        RectTransform cloneItem;
        RectTransform viewport;

        /// <summary>
        /// 创建的行数或者列数
        /// </summary>
        int createLineCount;

        List<ScrollListItem> clones;

        /// <summary>
        /// 滑动时回调数据索引,更新控件
        /// </summary>
        public Action<GameObject, int> UpdateData { get; set; }

        [TextBox("滑动方向间距")]
        public float spacing = 10;

        /// <summary>
        /// 多生成的行/列数
        /// </summary>
        public const int createMoreLineCount = 2;


        /// <summary>
        /// 包含间隔的单元格宽
        /// </summary>
        float cell;


        /// <summary>
        /// 播放缓动
        /// </summary>
        bool playTween;

        /// <summary>
        /// 缓动时间
        /// </summary>
        [TextBox("滑动时间"), ToolTip("添加/删除/定位时的滑动时间")]
        public float moveTime = 0.5f;

        /// <summary>
        /// 最后一个数据对应的克隆控件索引
        /// </summary>
        int lastDataIndex;


        int mDataCount;

        public int DataCount
        {
            get
            {
                return mDataCount;
            }
            set
            {
                Init();
                sizeDelta_tweenHandle.Cancel();
                for (int i = 0; i < clones.Count; i++)
                {
                    var it = clones[i];
                    if (it != null) it.Clear();
                }
                //if (mDataCount != value)
                {
                    mDataCount = value;
                    var sizeDelta = sr.content.sizeDelta;
                    sizeDelta_tweenHandle.start = sizeDelta;
                    if (sr.horizontal)
                    {
                        sizeDelta.x = mDataCount * cell - spacing;
                    }
                    else if (sr.vertical)
                    {
                        sizeDelta.y = mDataCount * cell - spacing;
                    }
                    if (playTween)
                    {
                        sizeDelta_tweenHandle.end = sizeDelta;
                        sizeDelta_tweenHandle.Play(moveTime, DestroyHandle);
                    }
                    else
                    {
                        sr.content.sizeDelta = sizeDelta;
                    }

                    if (sr.horizontal)
                    {
                        OnScroll(-sr.content.anchoredPosition.x / cell, (-sr.content.anchoredPosition.x + viewport.rect.width) / cell);
                    }
                    else
                    {
                        OnScroll(sr.content.anchoredPosition.y / cell, (sr.content.anchoredPosition.y + viewport.rect.height) / cell);
                    }
                }
            }
        }

        /// <summary>
        /// 滑动列表位置
        /// </summary>
        public Vector2 Pos
        {
            set
            {
                sr.content.anchoredPosition = value;
            }
            get
            {
                return sr.content.anchoredPosition;
            }
        }

        public enum TargetScrollPos
        {
            [EnumLabel("左/上")]
            Start = 0,
            [EnumLabel("中间")]
            Center = 1,
            [EnumLabel("右/下")]
            End = 2,
        }

        /// <summary>
        /// 计算在totalWidth中生成多少个itemWidth可以最大限度填满<para/>
        /// </summary>
        int CalcCount(float totalWidth, float itemWidth)
        {
            if (totalWidth < itemWidth) totalWidth = itemWidth;
            return Mathf.FloorToInt((totalWidth - itemWidth) / (itemWidth + spacing)) + 1;
        }

        void Awake()
        {
            Init();
        }
        bool hasInit;
        /// <summary>
        /// 处理:
        /// 1.拿到克隆体
        /// 2.获得单元格宽高
        /// 3.克隆填充面板
        /// 4.监听面板滑动
        /// </summary>
        private void Init()
        {
            if (hasInit) return;
            hasInit = true;
            Func<float, float> tweenCurve = UnityEngine.EaseFun.GetEase(EaseFunEnum.Quadratic, EaseStyleEnum.EaseOut);
            sr = GetComponent<ScrollRect>();
            sizeDelta_tweenHandle = new CqTweenLerp_Vector2()
            {
                memberProxy = MemberProxy.GetMemberProxy(sr.content, "sizeDelta"),
                Evaluate = tweenCurve,
            };
            anchoredPosition_tweenHandle = new CqTweenLerp_Vector2()
            {
                memberProxy = MemberProxy.GetMemberProxy(sr.content, "anchoredPosition"),
                Evaluate = tweenCurve,
            };
            viewport = sr.viewport;
            cloneItem = sr.content.GetChild(0).GetComponent<RectTransform>();
            cloneItem.gameObject.SetActive(false);

            //在误差范围内不产生滑动回调
            float Epsilon = 0.5f;
            if (sr.horizontal)
            {
                
                var lastX = float.NaN;
                sr.onValueChanged.SetCallBack(v =>//v是滑动条系数
                {
                    if (lastX.EqualsByEpsilon(sr.content.anchoredPosition.x, Epsilon)) return;
                    lastX = sr.content.anchoredPosition.x;
                    OnScroll(-sr.content.anchoredPosition.x / cell, (-sr.content.anchoredPosition.x + viewport.rect.width) / cell);
                }, DestroyHandle);
                cell = viewport.rect.height;

                createLineCount = CalcCount(viewport.rect.width, cloneItem.rect.width);
            }
            else if (sr.vertical)
            {
                var lastY = float.NaN;

                sr.onValueChanged.SetCallBack(v =>//v是滑动条系数
                {
                    if (lastY.EqualsByEpsilon(sr.content.anchoredPosition.y, Epsilon)) return;
                    lastY = sr.content.anchoredPosition.y;
                    OnScroll(sr.content.anchoredPosition.y / cell, (sr.content.anchoredPosition.y + viewport.rect.height) / cell);
                }, DestroyHandle);
                cell = viewport.rect.width;

                createLineCount = CalcCount(viewport.rect.height, cloneItem.rect.height);
            }

            clones = new List<ScrollListItem>();

            createLineCount += 1 + createMoreLineCount;
            for (int i = 0; i < createLineCount; i++)
            {
                clones.Add(null);
            }
        }

        /// <summary>
        /// 在删除数据之后调用该方法作界面表现
        /// 1.错位更新单元格
        /// 2.滑动定位新位置
        /// </summary>
        public bool RemoveAt(int dataIndex)
        {
            if (dataIndex >= DataCount || dataIndex < 0) return false;
            var removeCloneIndex = MathUtil.MoveToRange(dataIndex, 0, clones.Count);
            var cloneIndexBylastData = MathUtil.MoveToRange(lastDataIndex, 0, clones.Count);

            var removeItem = this[removeCloneIndex];
            var lastItem = this[cloneIndexBylastData];
            removeItem.anchoredPosition = lastItem.anchoredPosition;
            removeItem.SiblingIndex = lastItem.SiblingIndex;
            clones.RemoveAt(removeCloneIndex);
            clones.Insert(cloneIndexBylastData, removeItem);
            if (removeCloneIndex > cloneIndexBylastData)
            {
                var item = clones[0];
                clones.RemoveAt(0);
                clones.Add(item);
            }

            playTween = true;
            DataCount = DataCount - 1;
            playTween = false;
            return true;
        }
        public bool Add()
        {
            return Insert(DataCount);
        }
        /// <summary>
        /// 在添加数据之后调用该方法作界面表现
        /// 1.错位更新单元格
        /// 2.滑动定位新位置
        /// </summary>
        public bool Insert(int dataIndex)
        {
            if (dataIndex > DataCount || dataIndex < 0) return false;
            var addCloneIndex = MathUtil.MoveToRange(dataIndex, 0, clones.Count);
            var cloneIndexBylastData = MathUtil.MoveToRange(lastDataIndex, 0, clones.Count);

            var addItem = this[addCloneIndex];
            var lastItem = this[cloneIndexBylastData];

            lastItem.anchoredPosition = addItem.anchoredPosition;
            lastItem.SiblingIndex = addItem.SiblingIndex;
            clones.RemoveAt(cloneIndexBylastData);
            clones.Insert(addCloneIndex, lastItem);
            if (addCloneIndex > cloneIndexBylastData)
            {
                var item = clones[clones.Count - 1];
                clones.RemoveAt(clones.Count - 1);
                clones.Insert(0, item);
            }

            playTween = true;
            DataCount = DataCount + 1;
            playTween = false;
            return true;
        }
        /// <summary>
        /// 滑动到数据索引关联的对象可见.
        /// </summary>
        /// <param name="dataIndex">数据索引</param>
        /// <param name="offsetCount">偏移多少个单位数据项</param>
        public void MoveToVisible(int dataIndex, float offsetCount = 0.5f)
        {
            if (sr.horizontal)
            {
                var minPos = GetScrollTarget(dataIndex, TargetScrollPos.Start, -offsetCount);
                var maxPos = GetScrollTarget(dataIndex, TargetScrollPos.End, offsetCount);
                var x = Pos.x;
                if (x < minPos.x)
                {
                    MoveContentTo(minPos);
                }
                else if (x > maxPos.x)
                {
                    MoveContentTo(maxPos);
                }
            }
            else if (sr.vertical)
            {
                var maxPos = GetScrollTarget(dataIndex, TargetScrollPos.Start, -offsetCount);
                var minPos = GetScrollTarget(dataIndex, TargetScrollPos.End, offsetCount);
                var y = Pos.y;
                if (y < minPos.y)
                {
                    MoveContentTo(minPos);
                }
                else if (y > maxPos.y)
                {
                    MoveContentTo(maxPos);
                }
            }
        }

        /// <summary>
        /// 滑动定位到数据索引
        /// </summary>
        public void MoveToIndex(int dataIndex, TargetScrollPos tsp = TargetScrollPos.Center)
        {
            MoveContentTo(GetScrollTarget(dataIndex, tsp, 0));
        }
        /// <summary>
        /// 滑动定位到数据索引
        /// </summary>
        /// <param name="dataIndex">定位的数据索引</param>
        /// <param name="tsp">定位样式</param>
        /// <param name="deltaCount">定位偏移多少个单位数据宽度</param>
        /// <returns></returns>
        Vector2 GetScrollTarget(int dataIndex, TargetScrollPos tsp, float deltaCount)
        {
            var lineIndex = dataIndex;
            Vector2 pos = Pos;

            if (sr.horizontal)
            {
                pos.x = (lineIndex + deltaCount) * cell- (viewport.rect.width - cloneItem.rect.width) * ((int)tsp) * 0.5f;
                MathUtil.BetweenRange(ref pos.x, 0, sr.content.rect.width - viewport.rect.width);
                pos.x = -pos.x;
            }
            else if (sr.vertical)
            {
                pos.y = (lineIndex + deltaCount) * cell - (viewport.rect.height - cloneItem.rect.height) * ((int)tsp) * 0.5f;
                MathUtil.BetweenRange(ref pos.y, 0, sr.content.rect.height - viewport.rect.height);
            }
            return pos;
        }

        void MoveContentTo(Vector2 pos)
        {
            anchoredPosition_tweenHandle.Cancel();
            anchoredPosition_tweenHandle.start = anchoredPosition_tweenHandle.current;
            anchoredPosition_tweenHandle.end = pos;
            anchoredPosition_tweenHandle.Play(moveTime, DestroyHandle);
        }


        void OnScroll(float startIndex, float endIndex)
        {
            int startDataIndex = Mathf.FloorToInt(startIndex - createMoreLineCount / 2f) ;
            int endDataIndex = startDataIndex + createLineCount ;
            //Debug.Log("起始:" + startDataIndex + " 终止:" + endDataIndex);
            for (int dataIndex = startDataIndex; dataIndex < endDataIndex; dataIndex++)
            {
                var cloneIndex = MathUtil.MoveToRange(dataIndex, 0, createLineCount );

                var item = this[cloneIndex];
                if (item.dataIndex != dataIndex)
                {
                    item.dataIndex = dataIndex;

                    Vector2 targetPos = cloneItem.anchoredPosition;

                    if (sr.horizontal)
                    {
                        targetPos += new Vector2(dataIndex * cell, 0);
                    }
                    else if (sr.vertical)
                    {
                        targetPos += new Vector2(0, dataIndex * -cell);
                    }
                    item.UpdateItem(targetPos, playTween);
                }
            }

            lastDataIndex = (startDataIndex + createLineCount)  - 1;
        }

        /// <summary>
        /// 当数据量小时,不会重复克隆填满.需要时才做克隆
        /// </summary>
        ScrollListItem this[int index]
        {
            get
            {
                if (clones[index] == null)
                {
                    var obj = cloneItem.gameObject.Clone();
                    var item = obj.AddComponent<ScrollListItem>();
                    item.Init(this);
                    clones[index] = item;
                }
                return clones[index];
            }
        }
    }
}


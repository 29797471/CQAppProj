using CqCore;
using System;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 滑动表格中的单元格
    /// </summary>
    public class ScrollListItem : MonoBehaviourExtended
    {
        CqTweenLerp_Vector2 anchoredPosition_tweenHandle;
        RectTransform mRt;
        RectTransform Rt
        {
            get
            {
                if (mRt == null)
                {
                    mRt = GetComponent<RectTransform>();
                }
                return mRt;
            }
        }
        ScrollList mRs;
        public Vector2 anchoredPosition
        {
            set
            {
                Rt.anchoredPosition = value;
            }
            get
            {
                return Rt.anchoredPosition;
            }
        }
        public int SiblingIndex
        {
            get
            {
                return Rt.GetSiblingIndex();
            }
            set
            {
                Rt.SetSiblingIndex(value);
            }
        }
        [TextBox("关联数据索引:"), IsEnabled(false)]
        public int dataIndex = int.MaxValue;

        /// <summary>
        /// 断开和数据关联
        /// </summary>
        public void Clear()
        {
            dataIndex = int.MaxValue;
        }

        string cloneName;
        public void Init(ScrollList sg)
        {
            mRs = sg;
            cloneName = name;
            Func<float, float> tweenCurve = UnityEngine.EaseFun.GetEase(EaseFunEnum.Quadratic, EaseStyleEnum.EaseOut);
            anchoredPosition_tweenHandle = new CqTweenLerp_Vector2()
            {
                memberProxy = MemberProxy.GetMemberProxy(Rt, "anchoredPosition"),
                Evaluate = tweenCurve,
            };
        }
        public void UpdateItem(Vector2 targetPos, bool playTween)
        {
            anchoredPosition_tweenHandle.Cancel();

            if (playTween)
            {
                anchoredPosition_tweenHandle.start = anchoredPosition_tweenHandle.current;
                anchoredPosition_tweenHandle.end = targetPos;
                anchoredPosition_tweenHandle.Play(mRs.moveTime, DestroyHandle);
            }
            else
            {
                anchoredPosition = targetPos;
            }
            name = string.Format("{0}_{1}", cloneName, dataIndex);
            //item.name = string.Format("{0}_{1}_{2}", cloneItem.name, cloneIndex, dataIndex);

            if (dataIndex >= 0 && dataIndex < mRs.DataCount)
            {
                gameObject.SetActive(true);
                if (mRs.UpdateData != null) mRs.UpdateData(gameObject, dataIndex);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}



using CqCore;
using SLua;
using UnityEngine;

namespace CustomModel
{
    /// <summary>
    /// 语言相关文本的获取,设置
    /// 
    /// </summary>
    public class LanguageMgr : Singleton<LanguageMgr>
    {
        public void UpdateTimeAndNumberByLanguage()
        {
            switch (SettingModel.instance.SystemLanguage)
            {
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                    numberTxt = new string[] { "", "万", "亿", "兆", "京" };
                    numberZero = 4;
                    style = null;
                    timeTxt = new string[] { "秒", "分", "时", "日", "月", "年" };
                    break;
                case SystemLanguage.ChineseTraditional:
                    numberTxt = new string[] { "", "萬", "億", "兆", "京" };
                    numberZero = 4;
                    style = null;
                    timeTxt = new string[] { "秒", "分", "時", "日", "月", "年" };
                    break;
                default:
                    numberTxt = new string[] { "", "K", "M", "B", "T" };
                    numberZero = 3;
                    style = "D2";
                    timeTxt = new string[] { "s", "m", "H", "d", "M", "y" };
                    break;
            }
        }

        //string[] numberTxt;
        //int numberZero;
        string[] numberTxt = { "", "万", "亿", "兆", "京" };
        int numberZero = 4;
        string style = null;
        string[] timeTxt = { "秒", "分", "时", "日", "月", "年" };


        public enum NumberStyle
        {
            /// <summary>
            /// 数字
            /// </summary>
            Number = 1,
            /// <summary>
            /// 单位
            /// </summary>
            Unit = 2,
        }
        /// <summary>
        /// 数字文本显示规则
        /// 1.将大于10000的数字,按对应语言的单位进位显示, 进位后带上单位后缀
        /// 2.保留4位有效数字(四舍五入)
        /// 3.小数点后最多2位
        /// </summary>
        public string ToNumber(double value, NumberStyle style = NumberStyle.Number | NumberStyle.Unit)
        {
            if (value < 0) return "-" + ToNumber(-value);
            int index = 0;
            //4进位
            while (value >= 10000 && index + 1 < numberTxt.Length)
            {
                value /= System.Math.Pow(10, numberZero);
                index++;
            }

            //小数点后保留2位
            value = (value * 100).FloorByEpsilon() / 100;

            //保留4位有效数字
            value = MathUtil.KeepNumber(value, 4);
            var str = value.ToString();

            //在超过1000时,插入一个逗号
            //if (value >= 1000)
            //{
            //    var dot = str.IndexOf('.');
            //    if (dot == -1)
            //    {
            //        dot = str.Length;
            //    }
            //    str = str.Insert(dot - 3, ",");
            //}
            switch (style)
            {
                case NumberStyle.Number:
                    return str;
                case NumberStyle.Unit:
                    return numberTxt[index];
                default:
                    return str + numberTxt[index];
            }
        }

        /// <summary>
        /// 数字以,分隔
        /// </summary>
        public string ToNumberByDot(long number)
        {
            return number.ToString("N0");
        }

        /// <summary>
        /// 按对应语言转时间文本显示
        /// </summary>
        public string TimeSpan(int seconds)
        {
            if (seconds <= 0) return "";
            if (seconds < 60)
            {
                return TimeUtil.TimeSpanFormat(seconds, string.Format("ss{0}", timeTxt[0]), style);
            }
            if (seconds < 60 * 60)
            {
                if (seconds % (60) == 0)
                {
                    return TimeUtil.TimeSpanFormat(seconds, string.Format("mm{0}", timeTxt[1]), style);
                }
                return TimeUtil.TimeSpanFormat(seconds, string.Format("mm{0}ss{1}", timeTxt[1], timeTxt[0]), style);
            }
            if (seconds < 60 * 60 * 24)
            {
                if (seconds % (60 * 60) == 0)
                {
                    return TimeUtil.TimeSpanFormat(seconds, string.Format("HH{0}", timeTxt[2]), style);
                }
                return TimeUtil.TimeSpanFormat(seconds, string.Format("HH{0}mm{1}", timeTxt[2], timeTxt[1]), style);
            }
            if (seconds % (60 * 60 * 24) == 0)
            {
                return TimeUtil.TimeSpanFormat(seconds, string.Format("dd{0}", timeTxt[3]), style);
            }
            return TimeUtil.TimeSpanFormat(seconds, string.Format("dd{0}HH{1}", timeTxt[3], timeTxt[2]), style);
        }
    }
}


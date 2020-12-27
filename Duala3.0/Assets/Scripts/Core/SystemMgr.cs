using System;

/// <summary>
/// 单例系统管理类<para/>
/// 重启时清除
/// </summary>
public class SystemMgr<T> : IDisposable where T : class, new()
{
    protected static T mInst;

    /// <summary>
    /// 不应由反射来调用构造
    /// </summary>
    protected SystemMgr()
    {
        GlobalMgr.instance.ExitHandle.CancelAct+=Dispose;
    }

    public static T Inst
    {
        get
        {
            if (mInst == null)
            {
                mInst = new T();
            }
            return mInst;
        }
    }

    public virtual void Dispose()
    {
        mInst = null;
    }
}



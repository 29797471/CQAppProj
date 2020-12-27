public class FastStack<T> where T : new()
{
    T[] mObjects;

    int iCap;
    int iCount = 0;
    public FastStack()
    {
        iCap = 10;
        mObjects = new T[iCap];
    }
    public FastStack(int _iCap)
    {
        iCap = _iCap;
        if (iCap < 10) iCap = 10;
        mObjects = new T[iCap];
    }
    public int Add(T obj)
    {
        return Push(obj);
    }
    public T Last
    {
        get
        {
            return this[iCount - 1];
        }
    }

    void RenewObjects()
    {
        T[] temp = new T[iCap * 2];
        System.Array.Copy(mObjects, 0, temp, 0, iCap);
        mObjects = temp;

        iCap *= 2;
    }

    public int Push()
    {
        if (iCount >= iCap)
        {
            RenewObjects();
        }
        if (mObjects[iCount] == null) mObjects[iCount] = new T();
        iCount++;

        return iCount - 1;
    }
    public int Push(T obj)
    {
        if (iCount >= iCap)
        {
            RenewObjects();
        }

        mObjects[iCount] = obj;
        iCount++;

        return iCount - 1;
    }
    public int Count
    {
        get
        {
            return iCount;
        }
    }

    public T this[int index]
    {
        get
        {
            return Get(index);
        }
    }
    public T Get(int index)
    {
        if (index >= 0 && index < iCount)
            return mObjects[index];

        return default(T);
    }
    public bool Contains(T t)
    {
        for (int i = 0; i < iCount; i++)
        {
            if (mObjects[i].Equals(t)) return true;
        }
        return false;
    }

    public T Pop()
    {
        if (iCount <= 0)
            return default(T);

        iCount--;
        return mObjects[iCount];
    }

    public T Top(int iIndex = -1)
    {
        return mObjects[iCount - iIndex];
    }

    public bool IsEmpty()
    {
        return iCount <= 0;
    }
    public void Clear()
    {
        iCount = 0;
    }
}

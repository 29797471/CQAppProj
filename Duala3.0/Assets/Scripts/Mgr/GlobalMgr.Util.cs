public partial class GlobalMgr : Singleton<GlobalMgr>
{

    //8位数字大小写字母组合
    public string Calc(string Command)
    {
        int times = 2;
        string _temmp = Command;
        for (int i = 0; i < times; i++)
        {
            _temmp = StringUtil.Md5Sum(_temmp);
        }
        var temp = _temmp.ToCharArray();
        var x = new char[8];

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                x[i] += temp[i * 4 + j];
            }
            switch (i)
            {
                case 0:
                    x[i] = ToUpperWord(x[i]);
                    break;
                case 1:
                    x[i] = ToLowerWord(x[i]);
                    break;
                case 7:
                    x[i] = ToNumber(x[i]);
                    break;
                default:
                    x[i] = ToNumberWord(x[i]);
                    break;
            }

        }
        return new string(x);
    }
    char ToNumberWord(char c)
    {
        var n = 26 + 26 + 10;
        var d = c % n;
        if (d < 10)
        {
            return (char)(48 + d);
        }
        else
        {
            d -= 10;
            if (d < 26)
            {
                return (char)(65 + d);
            }
            else
            {
                d -= 26;
                return (char)(97 + d);
            }
        }
    }
    char ToNumber(char c)
    {
        var d = c % 10;
        return (char)(48 + d);
    }
    char ToUpperWord(char c)
    {
        var d = c % 26;
        return (char)(65 + d);
    }
    char ToLowerWord(char c)
    {
        var d = c % 26;
        return (char)(97 + d);
    }

    public void Test(int sec,float deltaTime)
    {
        ServerTime.instance.SyncTime(TimeUtil.Unix_timestamp_long+sec*1000, deltaTime);
    }
}

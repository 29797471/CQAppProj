public class BuffAttrChange
{
    public float percent;
    public float delta;
    public float Calculate(float data)
    {
        //ZLogger.Debug("基础:" + data+" 结果:"+(data * (1 + percent / 10000) + delta));
        return data * (1 + percent / 10000) + delta;
    }
}
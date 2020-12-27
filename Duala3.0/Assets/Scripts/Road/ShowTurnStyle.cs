using CqCore;
using UnityEngine;

public class ShowTurnStyle : MonoBehaviour
{
    public TurnStyle icon;

    public bool equal;

    public bool reverse;

    public void SetTurnStyle(TurnStyle turn)
    {
        if(equal)
        {
            var bl = (turn == icon);
            if (reverse) bl = !bl;
            gameObject.SetActive(bl);
        }
        else
        {
            var bl = MathUtil.StateCheck(turn, icon);
            if (reverse) bl = !bl;
            gameObject.SetActive(bl);
        }
        
    }
    
}

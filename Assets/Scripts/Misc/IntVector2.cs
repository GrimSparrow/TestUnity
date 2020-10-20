using System;

[Serializable]
public struct IntVector2
{
    public int x, y;

    public IntVector2(int num1, int num2)
    {
        x = num1;
        y = num2;
    }

    public static IntVector2 one
    {
        get { return new IntVector2(1, 1); }
        
    }

    public static IntVector2 oneNeg
    {
        get { return new IntVector2(-1, -1); }

    }

    public static IntVector2 zero
    {
        get{ return new IntVector2(0, 0); }
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        IntVector2 ret;

        ret.x = a.x + b.x;
        ret.y = a.y + b.y;
        return ret;
    }

    public static bool operator ==(IntVector2 a, IntVector2 b)
    {
        if ((a.x == b.x) && (a.y == b.y))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator !=(IntVector2 a, IntVector2 b)
    {
        if ((a.x != b.x) && (a.x != b.x))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static IntVector2 ParseStringArray(string[] text)
    {
        
        int.TryParse(text[0], out var X);
        int.TryParse(text[0], out var Y);
        
        return new IntVector2(X, Y);
    }
    
    public override bool Equals(object o)
    {
        return true;
    }
    
    public override int GetHashCode()
    {
        return 0;
    }
}
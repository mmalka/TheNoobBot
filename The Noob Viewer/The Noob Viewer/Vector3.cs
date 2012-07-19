using System;
using System.Globalization;

public struct Vector3
{
	private	float	x;
	private	float	y;
	private	float	z;

	public Vector3(float x, float y, float z)
	{
		this.x = 0;
		this.y = 0;
		this.z = 0;

		X = x;
		Y = y;
		Z = z;
	}

	public Vector3(float[] xyz)
	{
		this.x = 0;
		this.y = 0;
		this.z = 0;

		Array = xyz;
	}

	public Vector3(Vector3 v1)
	{
		this.x = 0;
		this.y = 0;
		this.z = 0;

		X = v1.X;
		Y = v1.Y;
		Z = v1.Z;
	}

	public float X
    {
        get{return x;}
		set{x = value;}
    }

    public float Y
    {
        get{return y;}
		set{y = value;}
    }

    public float Z
    {
        get{return z;}
		set{z = value;}
    }

	public float[] Array
	{
		get{return new float[] {x,y,z};}
		set
		{
			if(value.Length == 3)
			{
				x = value[0];
				y = value[1];
				z = value[2];
			}
			else
			{
                throw new ArgumentException("Array must contain exactly three components , (x,y,z)");
			}
		}
	}
}

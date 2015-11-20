using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace SonicRetro.SAModel
{
	[Serializable]
	[TypeConverter(typeof (VertexConverter))]
	public class Vertex : IEquatable<Vertex>
	{
		public delegate void VertexChangedHandler(Vertex sender);
		public event VertexChangedHandler Changed;

		private float x;
		private float y;
		private float z;

		public float X { get { return x; } set { x = value; if (Changed != null) { Changed(this); } } }
		public float Y { get { return y; } set { y = value; if (Changed != null) { Changed(this); } } }
		public float Z { get { return z; } set { z = value; if (Changed != null) { Changed(this); } } }

		public static int Size
		{
			get { return 12; }
		}

		public Vertex()
		{
		}

		public Vertex(byte[] file, int address)
		{
			x = ByteConverter.ToSingle(file, address);
			y = ByteConverter.ToSingle(file, address + 4);
			z = ByteConverter.ToSingle(file, address + 8);
		}

		public Vertex(string data)
		{
			string[] a = data.Split(',');
			x = float.Parse(a[0], NumberStyles.Float, NumberFormatInfo.InvariantInfo);
			y = float.Parse(a[1], NumberStyles.Float, NumberFormatInfo.InvariantInfo);
			z = float.Parse(a[2], NumberStyles.Float, NumberFormatInfo.InvariantInfo);
		}

		public Vertex(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vertex(float[] data)
		{
			x = data[0];
			y = data[1];
			z = data[2];
		}

		public byte[] GetBytes()
		{
			List<byte> result = new List<byte>();
			result.AddRange(ByteConverter.GetBytes(X));
			result.AddRange(ByteConverter.GetBytes(Y));
			result.AddRange(ByteConverter.GetBytes(Z));
			return result.ToArray();
		}

		public override string ToString()
		{
			return x.ToString(NumberFormatInfo.InvariantInfo) + ", " + y.ToString(NumberFormatInfo.InvariantInfo) + ", " +
			       z.ToString(NumberFormatInfo.InvariantInfo);
		}

		public string ToStruct()
		{
			if (x == 0 && y == 0 && z == 0)
				return "{ 0 }";
			return "{ " + x.ToC() + ", " + x.ToC() + ", " + z.ToC() + " }";
		}

		public float[] ToArray()
		{
			float[] result = new float[3];
			result[0] = x;
			result[1] = y;
			result[2] = z;
			return result;
		}

		public float this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return x;
					case 1:
						return y;
					case 2:
						return z;
					default:
						throw new IndexOutOfRangeException();
				}
			}
			set
			{
				switch (index)
				{
					case 0:
						x = value;
						return;
					case 1:
						y = value;
						return;
					case 2:
						z = value;
						return;
					default:
						throw new IndexOutOfRangeException();
				}
			}
		}

		public static readonly Vertex UpNormal = new Vertex(0, 1, 0);

		/// <summary>
		/// Returns the center of a list of Vertex points.
		/// </summary>
		/// <param name="points">List of points to use.</param>
		/// <returns></returns>
		public static Vertex CenterOfPoints(Vertex[] points)
		{
			Vertex center = new Vertex(0, 0, 0);

			if (points == null || points.Length == 0)
				return center;

			float xTotal = 0;
			float yTotal = 0;
			float zTotal = 0;

			foreach (Vertex vertex in points)
			{
				xTotal += vertex.X;
				yTotal += vertex.Y;
				zTotal += vertex.Z;
			}

			center.X = xTotal / points.Length;
			center.Y = yTotal / points.Length;
			center.Z = zTotal / points.Length;

			return center;
		}

		[Browsable(false)]
		public bool IsEmpty
		{
			get { return x == 0 && y == 0 && z == 0; }
		}

		public override bool Equals(object obj)
		{
			if (obj is Vertex)
				return Equals((Vertex)obj);
			return false;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
		}

		public bool Equals(Vertex other)
		{
			return x == other.x && y == other.y && z == other.z;
		}

		public static Vertex operator +(Vertex v1, Vertex v2)
		{
			return new Vertex(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
		}

		public static Vertex operator *(Vertex v1, float scale)
		{
			return new Vertex(v1.x * scale, v1.y * scale, v1.z * scale);
		}
	}

	public class VertexConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(Vertex))
				return true;
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is Vertex)
				return ((Vertex)value).ToString();
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
				return new Vertex((string)value);
			return base.ConvertFrom(context, culture, value);
		}
	}
}
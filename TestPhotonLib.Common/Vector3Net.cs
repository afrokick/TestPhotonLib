using System;

namespace TestPhotonLib.Common
{
    public class Vector3Net
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3Net(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static byte[] Serialize(object arg)
        {
            var m = arg as Vector3Net;

            if (m == null)
            {
                return new byte[] { (byte)0 };
            }
            byte[] result = new byte[12];
            BitConverter.GetBytes(m.X).CopyTo(result, 0);
            BitConverter.GetBytes(m.Y).CopyTo(result, 4);
            BitConverter.GetBytes(m.Z).CopyTo(result, 8);
            return result;
        }

        public static object Deserialize(byte[] arg)
        {
            if (arg.Length != 12)
            {
                return null;
            }
            byte[] xMas = new byte[4];
            byte[] yMas = new byte[4];
            byte[] zMas = new byte[4];
            Array.Copy(arg,0,xMas,0,4);
            Array.Copy(arg, 4, yMas, 0, 4);
            Array.Copy(arg, 8, zMas, 0, 4);

            float x = BitConverter.ToSingle(xMas, 0);
            float y = BitConverter.ToSingle(yMas, 0);
            float z = BitConverter.ToSingle(zMas, 0);
            return new Vector3Net(x, y, z);
        }
    }
}

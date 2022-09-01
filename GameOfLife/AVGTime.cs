using System.Globalization;

namespace Foo
{
    public /*unsafe*/ class AVGTime<T> where T :
        struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
    {
        private T[] values;
        private int type;
        private int valuesPtr;
        private bool isReady;

        public AVGTime(int size)
        {
            values = new T[size];
            type = (int)(new T().GetTypeCode());
            valuesPtr = 0;

            //// test if type is valid
            //isReady = true;
            //GetAVG();
            //isReady = false;
        }

        public bool IsReady()
        {
            return isReady;
        }

        public void Push(T value)
        {
            valuesPtr++;
            if (valuesPtr == values.Length)
            {
                valuesPtr = 0;
                isReady = true;
            }

            values[valuesPtr] = value;
        }

        public string GetAVG(CultureInfo? culture)
        {
            if (culture is null)
                return GetAVG().ToString(new CultureInfo(4));
            else
                return GetAVG().ToString(culture);
        }

        public double GetAVG()
        {
            if (!isReady)
                return double.NaN;

            double valueAVG = 0;

            /* sick code :p */

            if (type == (int)TypeCode.Double)
                for (int i = 0; i < values.Count(); i++)
                    valueAVG += (Double)(object)values[i];

            else if (type == (int)TypeCode.Single)
                for (int i = 0; i < values.Count(); i++)
                    valueAVG += (Single)(object)values[i];

            else if (type == (int)TypeCode.UInt64)
                for (int i = 0; i < values.Count(); i++)
                    valueAVG += (UInt64)(object)values[i];

            else if (type == (int)TypeCode.UInt32)
                for (int i = 0; i < values.Count(); i++)
                    valueAVG += (UInt32)(object)values[i];

            else if (type == (int)TypeCode.Int64)
                for (int i = 0; i < values.Count(); i++)
                    valueAVG += (Int64)(object)values[i];

            else if (type == (int)TypeCode.Int32)
                for (int i = 0; i < values.Count(); i++)
                    valueAVG += (Int32)(object)values[i];

            else if (type == (int)TypeCode.Int16)
                for (int i = 0; i < values.Count(); i++)
                    valueAVG += (Int16)(object)values[i];

            else if (type == (int)TypeCode.UInt16)
                for (int i = 0; i < values.Count(); i++)
                    valueAVG += (UInt16)(object)values[i];

            else if (type == (int)TypeCode.SByte)
                for (int i = 0; i < values.Count(); i++)
                    valueAVG += (SByte)(object)values[i];

            else
                throw new NotImplementedException("Type not implementet");

            return valueAVG / values.Length;
        }
    }
}
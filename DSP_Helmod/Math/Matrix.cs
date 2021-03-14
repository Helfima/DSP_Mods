using DSP_Helmod.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Math
{
    public class Matrix
    {
        private MatrixHeader[] columns = new MatrixHeader[0];
        private MatrixHeader[] headers = new MatrixHeader[0];
        private double[,] rows = new double[0,0];
        private Dictionary<string, int> columnIndex = new Dictionary<string, int>();

        public Matrix()
        {

        }
        public Matrix(int row, int col)
        {
            this.columns = new MatrixHeader[col];
            this.headers = new MatrixHeader[row];
            this.rows = new double[row, col];
        }
        public Matrix(MatrixHeader[] headers, MatrixRow[] rows)
        {
            for (int irow = 0; irow < headers.Length; irow++)
            {
                AddRow(headers[irow], rows[irow]);
            }
        }

        public int X
        {
            get { return headers.Length; }
        }

        public int Y
        {
            get { return columns.Length; }
        }

        
        internal string GetColumnKey(MatrixElement column)
        {
            return $"{column.Type}.{column.Name}";
        }

        public int GetColumnIndex(MatrixElement column)
        {
            string key = GetColumnKey(column);
            if (columnIndex.ContainsKey(key))
            {
                return columnIndex[key];
            }
            return -1;
        }

        public MatrixHeader[] Columns
        {
            get { return columns; }
        }
        public MatrixHeader[] Headers
        {
            get { return headers; }
        }
        public double[,] Values
        {
            get { return rows; }
        }

        public void AddColumn(MatrixHeader column)
        {
            string key = GetColumnKey(column);
            if (!columnIndex.ContainsKey(key))
            {
                // add column
                int icol = columns.Length;
                Array.Resize(ref columns, icol + 1);
                columns[icol] = column;
                columnIndex.Add(key, icol);
                rows = ResizeArray<double>(rows, headers.Length, columns.Length);
            }
        }
        
        public void AddRow(MatrixHeader header, MatrixRow row)
        {
            // Add header
            int i = headers.Length;
            Array.Resize(ref headers, i + 1);
            headers[i] = header;
            // Add column
            for(int icol = 0; icol < row.Columns.Length; icol++)
            {
                AddColumn(row.Columns[icol]);
            }
            // Add row
            int irow = headers.Length-1;
            rows = ResizeArray<double>(rows, headers.Length, columns.Length);
            for (int icol = 0; icol < columns.Length; icol++)
            {
                double value = row.GetValue(columns[icol]);
                rows[irow, icol] = value;
            }
            
        }

        protected T[,] ResizeArray<T>(T[,] original, int x, int y)
        {
            T[,] newArray = new T[x, y];
            int minX = System.Math.Min(original.GetLength(0), newArray.GetLength(0));
            int minY = System.Math.Min(original.GetLength(1), newArray.GetLength(1));

            for (int i = 0; i < minX; ++i)
                Array.Copy(original, i * original.GetLength(1), newArray, i * newArray.GetLength(1), minY);

            return newArray;
        }

        public Matrix Clone()
        {
            Matrix newMatrix = new Matrix(this.headers.Length, this.columns.Length);
            Array.Copy(this.headers, newMatrix.headers, this.headers.Length);
            Array.Copy(this.columns, newMatrix.columns, this.columns.Length);
            Array.Copy(this.rows, newMatrix.rows, this.rows.Length);
            newMatrix.columnIndex = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> entry in this.columnIndex)
            {
                newMatrix.columnIndex.Add(entry.Key, entry.Value);
            }
            return newMatrix;
        }

        public override string ToString()
        {
            StringBuilder value = new StringBuilder();
            value.AppendLine();
            value.AppendLine("Matrix");
            value.AppendLine($"Row:{rows.GetLength(0)}");
            value.AppendLine($"Col:{rows.GetLength(1)}");
            value.Append($"|{"Base",-30}");
            for (int icol = 0; icol < columns.Length; icol++)
            {
                value.Append($"|{columns[icol].Type + "." + columns[icol].Name,-30}");
            }
            value.AppendLine();
            for (int irow = 0; irow < rows.GetLength(0); irow++)
            {
                value.Append($"|{headers[irow].Type + "." + headers[irow].Name,-30}");
                for (int icol = 0; icol < rows.GetLength(1); icol++)
                {
                    value.Append($"|{rows[irow, icol],-30}");
                }
                value.AppendLine();
            }
            return value.ToString();
        }

    }
    public class MatrixHeader : MatrixElement, IEquatable<MatrixHeader>
    {
        public double Production = 1;
        public MatrixHeader(string type, string name, double production = 1) : base(type, name) {
            Production = production;
        }

        public bool Equals(MatrixHeader other)
        {
            return other.Type.Equals(other.Type) && other.Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Name.GetHashCode();
        }
    }

    public class MatrixRow : MatrixElement
    {
        private MatrixHeader[] columns = new MatrixHeader[0];
        private double[] values = new double[0];
        private Dictionary<string, int> columnIndex = new Dictionary<string, int>();
        public MatrixRow(string type, string name) : base(type, name) { }
        
        public double[] Values
        {
            get { return values; }
        }
        public MatrixHeader[] Columns
        {
            get { return columns; }
        }
        internal string GetColumnKey(MatrixValue value)
        {
            return $"{value.Type}.{value.Name}";
        }
        internal string GetColumnKey(MatrixHeader value)
        {
            return $"{value.Type}.{value.Name}";
        }
        public void AddColumn(MatrixValue value)
        {
            string key = GetColumnKey(value);
            if (!columnIndex.ContainsKey(key))
            {
                MatrixHeader column = new MatrixHeader(value.Type, value.Name);
                int icol = columns.Length;
                Array.Resize(ref columns, icol + 1);
                columns[icol] = column;
                columnIndex.Add(key, icol);
            }
        }
        public void AddValue(MatrixValue value)
        {
            string key = GetColumnKey(value);
            if (columnIndex.ContainsKey(key))
            {
                int icol = columnIndex[key];
                values[icol] += value.Value;
            }
            else
            {
                int i = values.Length;
                Array.Resize(ref values, i + 1);
                values[i] = value.Value;
                AddColumn(value);
            }
        }

        public double GetValue(int icol)
        {
            return values[icol];
        }

        public double GetValue(MatrixHeader column)
        {
            string key = GetColumnKey(column);
            if (columnIndex.ContainsKey(key))
            {
                return values[columnIndex[key]];
            }
            return 0;
        }
    }

    public class MatrixValue : MatrixElement, IEquatable<MatrixValue>
    {
        private double value;
        public MatrixValue(string type, string name) : base(type, name) { }
        public MatrixValue(string type, string name, double value) : base(type, name)
        {
            this.value = value;
        }

        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public bool Equals(MatrixValue other)
        {
            return Type.Equals(other.Type) && Name.Equals(other.Name) && Name.Equals(other.Value);
        }

        
        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Name.GetHashCode() ^ Value.GetHashCode();
        }

        public MatrixValue Clone()
        {
            return new MatrixValue(Type, Name, Value);
        }

        public override string ToString()
        {
            return $"MatrixValue => {Type}.{Name}={Value}";
        }
    }

    abstract public class MatrixElement
    {
        private string name;
        private string type;
        public MatrixElement(string type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }
        public string Type
        {
            get { return type; }
        }

    }

}

using DSP_Helmod.Math;
using DSP_Helmod.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMatrix();
        }

        static private void TestMatrix()
        {
            Nodes sheet = Test1();


            List<MatrixHeader> rowHeaders = new List<MatrixHeader>();
            List<MatrixRow> rowDatas = new List<MatrixRow>();
            foreach (Node node in sheet.Children)
            {
                rowHeaders.Add(new MatrixHeader(node.GetType().Name, node.Name));
                MatrixRow rowData = new MatrixRow(node.GetType().Name, node.Name);
                foreach (Item item in node.Products)
                {
                    rowData.AddValue(new MatrixValue(item.GetType().Name, item.Name, item.Count));
                }
                foreach (Item item in node.Ingredients)
                {
                    rowData.AddValue(new MatrixValue(item.GetType().Name, item.Name, -item.Count));
                }
                rowDatas.Add(rowData);
            }
            Matrix matrix = new Matrix(rowHeaders.ToArray(), rowDatas.ToArray());

            Compute compute = new Compute();
            compute.Update(sheet);
            System.Console.WriteLine("end");
        }

        private class TestNode : Node
        {
            public TestNode(string name)
            {
                this.Name = name;
            }
        }

        private static Nodes Test1()
        {
            Nodes sheet = new Nodes();

            Node node1 = new TestNode("Recipe Magnetic coil");
            Item node1_p1 = new Item("Magnetic coil", 2);
            Item node1_i1 = new Item("Magnet", 2);
            Item node1_i2 = new Item("Copper Iron", 1);
            node1.Products.Add(node1_p1);
            node1.Ingredients.Add(node1_i1);
            node1.Ingredients.Add(node1_i2);
            sheet.Add(node1);

            Node node2 = new TestNode("Recipe Magnet");
            Item node2_p1 = new Item("Magnet", 1);
            Item node2_i1 = new Item("Iron Ore", 2);
            node2.Products.Add(node2_p1);
            node2.Ingredients.Add(node2_i1);
            sheet.Add(node2);

            Node node3 = new TestNode("Recipe Copper Iron");
            Item node3_p1 = new Item("Copper Iron", 1);
            Item node3_i1 = new Item("Copper Ore", 1);
            node3.Products.Add(node3_p1);
            node3.Ingredients.Add(node3_i1);
            sheet.Add(node3);

            return sheet;
        }
    }
}

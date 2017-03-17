using System;
using System.Collections.Generic;
using System.Text;

namespace ArrayBinaryTree
{
    class Node
    {
        public int value;
        public Node right;
        public Node left;
    }

    class Program
    {
        static void Main(string[] args)
        {
            int[, ,] original = { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 }}, { { 10, 11 }, { 12, 13 } } };
            Node head = ConvertMultiDimArrayToTree(original);
            int[, ,] result = ConvertTreeToMultiDimArray(head);
        }

        //Convert To 3-D Array
        private static int[, ,] ConvertTreeToMultiDimArray(Node treeHead)
        {
            List<int> liearRep = new List<int>();
            TraverseTree(treeHead, liearRep);

            return ConvertLinearArrayToMultiDimArray(liearRep);
        }

        private static void TraverseTree(Node currentNode, List<int> result)
        {
            result.Add(currentNode.value);

            if (currentNode.left != null)
                TraverseTree(currentNode.left, result);

            if (currentNode.right != null)
                TraverseTree(currentNode.right, result);
        }

        private static int[, ,] ConvertLinearArrayToMultiDimArray(List<int> linear)
        {
            int dim3 = linear[0];
            int dim2 = linear[1];
            int dim1 = linear[2];
            int dim1Index = 0, dim2Index = 0, dim3Index = 0;
            int[, ,] result = new int[dim3, dim2, dim1];

            for (int i = 3; i < linear.Count; i++)
            {
                result[dim3Index, dim2Index, dim1Index] = linear[i];
                dim1Index++;

                if (dim1Index >= dim1)
                {
                    dim1Index = 0;
                    dim2Index++;

                    if (dim2Index >= dim2)
                    {
                        dim2Index = 0;
                        dim3Index++;
                    }
                }
            }

            return result;
        }


        //Convert To Tree
        private static Node ConvertMultiDimArrayToTree(int[, ,] array)
        {
            List<int> linear = ConvertMultiDimArrayToLinearArray(array);
            linear.Insert(0, array.GetLength(0));
            linear.Insert(0, array.GetLength(1));
            linear.Insert(0, array.GetLength(2));
            Node head = new Node();
            CreateEmptyTree(head, linear.Count);
            BuildTree(head, linear);

            return head;
        }

        private static List<int> ConvertMultiDimArrayToLinearArray(int[, ,] multiDimArray)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < multiDimArray.GetLength(0); i++)
                for (int j = 0; j < multiDimArray.GetLength(1); j++)
                    for (int k = 0; k < multiDimArray.GetLength(2); k++)
                        result.Add(multiDimArray[i, j, k]);

            return result;
        }

        private static void CreateEmptyTree(Node headNode, int remainingNodes)
        {
            Queue<Node> queue = new Queue<Node>();
            Node temp;
            queue.Enqueue(headNode);
            remainingNodes--;

            while (queue.Count > 0)
            {
                temp = queue.Dequeue();

                if (remainingNodes == 0)
                    break;

                temp.left = new Node();
                remainingNodes--;
                queue.Enqueue(temp.left);

                if (remainingNodes == 0)
                    break;

                temp.right = new Node();
                remainingNodes--;
                queue.Enqueue(temp.right);
            }

        }

        private static void BuildTree(Node currentNode, List<int> result)
        {
            if(result.Count == 0)
                return;

            currentNode.value = result[0];
            result.RemoveAt(0);

            if (currentNode.left != null)
                BuildTree(currentNode.left, result);

            if (currentNode.right != null)
                BuildTree(currentNode.right, result);
        } 
    }
}

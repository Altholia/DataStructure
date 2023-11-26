using System.Reflection;
using BinaryTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BinaryTreeTests
{
    [TestClass]
    public class BiTreeTests
    {
        [TestMethod]
        public void Constructor_PreOrderIsNull_ThrowsArgumentNullException()
        {
            IEnumerable<string> preOrder = null!;
            IEnumerable<string> inOrder = new[] { "1", "2", "3" };

            Assert.ThrowsException<ArgumentNullException>(()
                => new BiTree<string>(preOrder, inOrder));
        }

        [TestMethod]
        public void Constructor_InOrderIsNull_ThrowsArgumentNullException()
        {
            IEnumerable<string> preOrder = new[] { "1", "2", "3" };
            IEnumerable<string> inOrder = null!;

            Assert.ThrowsException<ArgumentNullException>(()
                => new BiTree<string>(preOrder, inOrder));
        }

        [TestMethod]
        public void Constructor_PreOrderIsEmpty_ThrowsInvalidOperationException()
        {
            IEnumerable<string> preOrder = Array.Empty<string>();
            IEnumerable<string> inOrder = new[] { "1", "2", "3" };

            Assert.ThrowsException<InvalidOperationException>(()
                => new BiTree<string>(preOrder, inOrder));
        }

        [TestMethod]
        public void Constructor_InOrderIsEmpty_ThrowsInvalidOperationException()
        {
            IEnumerable<string> preOrder = new[] { "1", "2", "3" };
            IEnumerable<string> inOrder = Array.Empty<string>();

            Assert.ThrowsException<InvalidOperationException>(()
                => new BiTree<string>(preOrder, inOrder));
        }

        [TestMethod]
        public void Constructor_PreOrderLengthNotEqualInOrderLength_ThrowsInvalidOperationException()
        {
            IEnumerable<string> preOrder = new[] { "1", "2", "3" };
            IEnumerable<string> inOrder = new[] { "1", "2", "3", "4" };

            Assert.ThrowsException<InvalidOperationException>(()
                => new BiTree<string>(preOrder, inOrder));
        }

        [TestMethod]
        public void Constructor_BuilderLeftObliqueTree_ReturnExceptionLefOblTree()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "C", "D", "E" };
            IEnumerable<string> inOrder = new[] { "E", "D", "C", "B", "A" };

            BiTree<string> tree = new(preOrder, inOrder);

            var headNode = typeof(BiTree<string>)
                .GetField("_headNode", BindingFlags.NonPublic | BindingFlags.Instance)
                !.GetValue(tree);

            string expectedValue = "A";
            for (int i = 0; i < preOrder.Count()-1; i++)
            {
                var headNodeVal = headNode
                    !.GetType()
                    .GetProperty("Data")
                    !.GetValue(headNode);

                headNode = headNode
                    !.GetType()
                    .GetProperty("LeftNode")
                    !.GetValue(headNode);

                Assert.AreEqual(expectedValue, headNodeVal);
                expectedValue=preOrder.ElementAt(i+1);
            }
        }

        [TestMethod]
        public void Constructor_BuilderRightObliqueTree_ReturnExceptionRigOblTree()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "C", "D", "E" };
            IEnumerable<string> inOrder = new[] { "A", "B", "C", "D", "E" };

            BiTree<string> tree = new(preOrder, inOrder);

            var headNode = typeof(BiTree<string>)
                .GetField("_headNode", BindingFlags.NonPublic | BindingFlags.Instance)
                !.GetValue(tree);

            string expectedValue = "A";
            for (int i = 0; i < preOrder.Count() - 1; i++)
            {
                var headNodeVal = headNode
                    !.GetType()
                    .GetProperty("Data")
                    !.GetValue(headNode);

                headNode = headNode
                    !.GetType()
                    .GetProperty("RightNode")
                    !.GetValue(headNode);

                Assert.AreEqual(expectedValue, headNodeVal);
                expectedValue = preOrder.ElementAt(i + 1);
            }
        }

        [TestMethod]
        public void Constructor_BuilderTree_ReturnExceptionTree()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "C", "D", "E" };
            IEnumerable<string> inOrder = new[] { "B", "A", "D", "C", "E" };

            BiTree<string> tree = new(preOrder, inOrder);

            IEnumerable<string> actualLevelOrder = tree.LevelOrderTraversal();
#pragma warning disable IDE0300
            string[] expectedLevelOrder = { "A", "B", "C", "D", "E" };
#pragma warning restore IDE0300

            CollectionAssert.AreEqual(expectedLevelOrder, actualLevelOrder.ToArray());
        }

        [TestMethod]
        public void Constructor_BuilderFullTree_ReturnExceptionFullTree()
        {
            IEnumerable<string> preOrder = new[] { "A","B","D","E","C","F","G" };
            IEnumerable<string> inOrder = new[] { "D","B","E","A","F","C","G" };

            BiTree<string> tree = new(preOrder, inOrder);

            IEnumerable<string> actualLevelOrder = tree.LevelOrderTraversal();
#pragma warning disable IDE0300
            string[] expectedLevelOrder = { "A","B","C","D","E","F","G"};
#pragma warning restore IDE0300

            CollectionAssert.AreEqual(expectedLevelOrder, actualLevelOrder.ToArray());
        }

        [TestMethod]
        public void GetNodeVal_ArgLessThanZero_ThrowsIndexOutOfRangeException()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);

            int index = -1;
            Assert.ThrowsException<IndexOutOfRangeException>(() => tree.GetNodeVal(index));
        }

        [TestMethod]
        public void GetNodeVal_ArgEqualThanCount_ThrowsIndexOutOfRangeException()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);

            int index = 12;
            Assert.ThrowsException<IndexOutOfRangeException>(() => tree.GetNodeVal(index));
        }

        [TestMethod]
        public void GetNodeVal_ArgGreaterThanCount_ThrowsIndexOutOfRangeException()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);

            int index = 13;
            Assert.ThrowsException<IndexOutOfRangeException>(() => tree.GetNodeVal(index));
        }

        [TestMethod]
        public void GetNodeVal_ArgValid_ReturnExceptionNodeVal()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);

            int index = 0;
            string expectedNodeVal = "A";
            string actualNodeVal = tree.GetNodeVal(index);
            Assert.AreEqual(expectedNodeVal, actualNodeVal);

            index = 11;
            expectedNodeVal = "L";
            actualNodeVal = tree.GetNodeVal(index);
            Assert.AreEqual(expectedNodeVal, actualNodeVal);

            index = 5;
            expectedNodeVal = "F";
            actualNodeVal = tree.GetNodeVal(index);
            Assert.AreEqual(expectedNodeVal, actualNodeVal);
        }

        [TestMethod]
        public void GetLeafCount_TreeIsEmpty_ThrowsArgumentNullException()
        {
            BiTree<string> tree = new();
            Assert.ThrowsException<ArgumentNullException>(()=>tree.GetLeafCount());
        }

        [TestMethod]
        public void GetLeafCount_ReturnExceptedResult()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);

            int expectedLeafCount = 3;
            int actualLeafCount = tree.GetLeafCount();
            Assert.AreEqual(expectedLeafCount, actualLeafCount);
        }

        [TestMethod]
        public void GetDepth_TreeIsEmpty_ReturnZero()
        {
            BiTree<string> tree = new();

            int excepted = 0;
            int actual = tree.GetDepth();

            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void GetDepth_TreeValid_ReturnExceptedDepth()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);

            int expectedLeafCount = 6;
            int actualLeafCount = tree.GetDepth();
            Assert.AreEqual(expectedLeafCount,actualLeafCount);
        }

        [TestMethod]
        public void AddNode_ValIsNull_ThrowsArgumentNullException()
        {

            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);

            Assert.ThrowsException<ArgumentNullException>(()
                => tree.AddNode(null!, 0, LefOrRigEnum.Left));
        }

        [TestMethod]
        public void AddNode_IndexLessThanZero_ThrowsIndexOutOfRangeException()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);

            Assert.ThrowsException<IndexOutOfRangeException>(()
                => tree.AddNode("L", -1, LefOrRigEnum.Right));
        }

        [TestMethod]
        public void AddNode_IndexEqualThanCount_ThrowsIndexOutOfRangeException()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);

            Assert.ThrowsException<IndexOutOfRangeException>(()
                => tree.AddNode("L", 12, LefOrRigEnum.Right));
        }

        [TestMethod]
        public void AddNode_AddLeftNodeError_ThrowsInvalidOperationException()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);
            Assert.ThrowsException<InvalidOperationException>(()
                => tree.AddNode("M", 1, LefOrRigEnum.Left));
        }

        [TestMethod]
        public void AddNode_AddRightNodeError_ThrowsInvalidOperationException()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);
            Assert.ThrowsException<InvalidOperationException>(() 
                => tree.AddNode("M", 2, LefOrRigEnum.Right));
        }

        [TestMethod]
        public void AddNode_AddLeftNodeValid_ReturnExceptionVal()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);
            tree.AddNode("M",2,LefOrRigEnum.Left);

            IEnumerable<string> valEnu = tree.LevelOrderTraversal();

            string exceptedVal = "M";
            string actualVal = valEnu.Skip(4).First();
            Assert.AreEqual(exceptedVal, actualVal);
        }

        [TestMethod]
        public void AddNode_AddRightNodeValid_ReturnExceptionVal()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);
            tree.AddNode("M",4,LefOrRigEnum.Right);

            IEnumerable<string> valEnu = tree.LevelOrderTraversal();

            string exceptedVal = "M";
            string actualVal = valEnu.Skip(7).First();
            Assert.AreEqual(exceptedVal, actualVal);
        }

        [TestMethod]
        public void RemoveNode_IndexLessThanZero_ThrowsIndexOutOfRangeException()
        {
            BiTree<string> tree = new();
            Assert.ThrowsException<IndexOutOfRangeException>(()
                => tree.RemoveNode(-1));
        }

        [TestMethod]
        public void RemoveNode_IndexEqualThanCount_ThrowsIndexOutOfRangeException()
        {
            BiTree<string> tree = new();
            Assert.ThrowsException<IndexOutOfRangeException>(()
                => tree.RemoveNode(0));
        }

        [TestMethod]
        public void RemoveNode_IndexGreaterThanCount_ThrowsIndexOutOfRangeException()
        {
            BiTree<string> tree = new();
            Assert.ThrowsException<IndexOutOfRangeException>(()
                => tree.RemoveNode(1));
        }

        [TestMethod]
        public void RemoveNode_DeleteLeafNode_ReturnExceptionVal()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "K", "C", "E", "G", "I", "J", "L" };
            IEnumerable<string> inOrder = new[] { "D", "H", "K", "F", "B", "A", "C", "I", "G", "L", "J", "E" };

            BiTree<string> tree = new(preOrder, inOrder);
            tree.RemoveNode(8);

            int exceptedCount = 11;
            int actualCount = tree.Count;
            Assert.AreEqual(exceptedCount, actualCount);

            Node<string> node = tree.GetNode(6);
            Assert.IsNull(node.LeftNode);
        }

        [TestMethod]
        public void RemoveNode_DeleteNodeIsLeftAndHaveLeftChildTree_ReturnExceptionVal()
        {
            IEnumerable<string> preOrder = new[] {"A","B","D","F","H","I","C","E","G"};
            IEnumerable<string> inOrder = new[] {"H","F","I","D","B","A","E","G","C"};
            BiTree<string> tree = new(preOrder, inOrder);

            tree.RemoveNode(1);
            int exceptedCount = 8;
            int actualCount = tree.Count;
            Assert.AreEqual(exceptedCount,actualCount);

            Node<string> node = tree.GetNode(1);
            string exceptedVal = "D";
            string actualVal = node.Data;
            Assert.AreEqual(exceptedVal,actualVal);
        }

        [TestMethod]
        public void RemoveNode_DeleteNodeIsRightAndHaveLeftChildTree_ReturnExceptionVal()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "F", "H", "I", "C", "E", "G" };
            IEnumerable<string> inOrder = new[] { "H", "F", "I", "D", "B", "A", "E", "G", "C" };
            BiTree<string> tree = new(preOrder, inOrder);

            tree.RemoveNode(2);

            int exceptedCount = 8;
            int actualCount = tree.Count;
            Assert.AreEqual(exceptedCount, actualCount);

            string exceptedVal = "E";
            string actualVal = tree.GetNodeVal(2);
            Assert.AreEqual(exceptedVal, actualVal);
        }

        [TestMethod]
        public void RemoveNode_DeleteNodeIsLeftAndHaveRightChildTree_ReturnExceptionVal()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "C", "E", "F", "G", "H", "I" };
            IEnumerable<string> inOrder = new[] { "B", "D", "A", "E", "C", "F", "H", "G", "I" };
            BiTree<string> tree = new(preOrder, inOrder);

            tree.RemoveNode(1);

            int exceptedCount = 8;
            int actualCount = tree.Count;
            Assert.AreEqual(exceptedCount, actualCount);

            string exceptedVal = "D";
            string actualVal = tree.GetNodeVal(1);
            Assert.AreEqual(exceptedVal, actualVal);
        }

        [TestMethod]
        public void RemoveNode_DeleteNodeIsRightAndHaveRightChildTree_ReturnExceptionVal()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "C", "E", "F", "G", "H", "I" };
            IEnumerable<string> inOrder = new[] { "B", "D", "A", "E", "C", "F", "H", "G", "I" };
            BiTree<string> tree = new(preOrder, inOrder);

            tree.RemoveNode(5);

            int exceptedCount = 8;
            int actualCount = tree.Count;
            Assert.AreEqual(exceptedCount, actualCount);

            string exceptedVal = "G";
            string actualVal = tree.GetNodeVal(5);
            Assert.AreEqual(exceptedVal, actualVal);
        }

        [TestMethod]
        public void RemoveNode_DeleteNodeIsLeftButNotRightChildTree_ReturnExceptionVal()
        {
            IEnumerable<string> preOrder = new[] { "A", "B", "D", "H", "I", "J", "E", "C", "F","G" };
            IEnumerable<string> inOrder = new[] { "I", "H", "J", "D", "B", "E", "A", "F", "C","G" };
            BiTree<string> tree = new(preOrder, inOrder);

            tree.RemoveNode(1);

            int exceptedCount = 9;
            int actualCount = tree.Count;
            Assert.AreEqual(exceptedCount, actualCount);

            string exceptedVal = "D";
            string actualVal = tree.GetNodeVal(1);
            Assert.AreEqual(exceptedVal, actualVal);
        }

        [TestMethod]
        public void RemoveNode_DeleteNodeIsRightButNotRightChildTree_ReturnExceptionVal()
        {
            IEnumerable<string> preOrder = new[] { "A","B","D","H","I", "J", "E", "C", "F", "G" };
            IEnumerable<string> inOrder = new[] { "I", "H", "J", "D", "B", "E", "A", "F", "C", "G" };
            BiTree<string> tree = new(preOrder, inOrder);

            tree.RemoveNode(2);

            int exceptedCount = 9;
            int actualCount = tree.Count;
            Assert.AreEqual(exceptedCount, actualCount);

            string exceptedVal = "F";
            string actualVal = tree.GetNodeVal(2);
            Assert.AreEqual(exceptedVal, actualVal);
        }

        [TestMethod]
        public void RemoveNode_DeleteNodeIsLeftAndHaveLeftAndRightChildTree_ReturnExceptionVal()
        {
            IEnumerable<string> preOrder = new[]
                { "A", "B", "D", "H", "I", "K", "L", "M", "E", "C", "F", "J", "G" };
            IEnumerable<string> inOrder = new[] 
                { "H", "D", "K", "I", "M", "L", "B", "E", "A", "F", "J", "C", "G" };
            BiTree<string> tree = new(preOrder, inOrder);

            tree.RemoveNode(1);

            int exceptedCount = 12;
            int actualCount = tree.Count;
            Assert.AreEqual(exceptedCount, actualCount);

            string exceptedVal = "L";
            string actualVal = tree.GetNodeVal(1);
            Assert.AreEqual(exceptedVal, actualVal);
        }

        [TestMethod]
        public void RemoveNode_DeleteNodeIsRightAndHaveLeftAndRightChildTree_ReturnExceptionVal()
        {
            IEnumerable<string> preOrder = new[]
                { "A", "B", "D", "H", "I", "K", "L", "M", "E", "C", "F", "J", "G" };
            IEnumerable<string> inOrder = new[]
                { "H", "D", "K", "I", "M", "L", "B", "E", "A", "F", "J", "C", "G" };
            BiTree<string> tree = new(preOrder, inOrder);

            tree.RemoveNode(2);

            int exceptedCount = 12;
            int actualCount = tree.Count;
            Assert.AreEqual(exceptedCount, actualCount);

            string exceptedVal = "J";
            string actualVal = tree.GetNodeVal(2);
            Assert.AreEqual(exceptedVal, actualVal);
        }

        [TestMethod]
        public void Contains_ReturnFalse()
        {
            IEnumerable<string> preOrder = new[]
                { "A", "B", "D", "H", "I", "K", "L", "M", "E", "C", "F", "J", "G" };
            IEnumerable<string> inOrder = new[]
                { "H", "D", "K", "I", "M", "L", "B", "E", "A", "F", "J", "C", "G" };
            BiTree<string> tree = new(preOrder, inOrder);

            bool actualBool = tree.Contains("9");
            Assert.IsFalse(actualBool);
        }

        [TestMethod]
        public void Contains_ReturnTrue()
        {
            IEnumerable<string> preOrder = new[]
                { "A", "B", "D", "H", "I", "K", "L", "M", "E", "C", "F", "J", "G" };
            IEnumerable<string> inOrder = new[]
                { "H", "D", "K", "I", "M", "L", "B", "E", "A", "F", "J", "C", "G" };
            BiTree<string> tree = new(preOrder, inOrder);

            bool actualBool = tree.Contains("L");
            Assert.IsTrue(actualBool);
        }

    }
}
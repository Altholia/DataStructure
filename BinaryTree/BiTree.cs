using System.Xml.Linq;

namespace BinaryTree;

public class BiTree<T> : IBiTree<T>
{
    private Node<T> _headNode;

    public BiTree()
    {
        Count = 0;
        _headNode = null!;
    }

    public int Count { get; private set; }

    /// <summary>
    /// 构造函数，根据前序遍历和中序遍历结果构造一棵二叉树
    /// </summary>
    /// <param name="preOrder">前序遍历结果</param=
    /// <param name="inOrder">中序遍历结果</param>
    /// <exception cref="ArgumentNullException">如果前序遍历或中序遍历结果为空，则抛出异常</exception>
    /// <exception cref="InvalidOperationException">如果前序遍历和中序遍历的数据元素个数不一致则抛出异常</exception>
    public BiTree(IEnumerable<T> preOrder, IEnumerable<T> inOrder)
    {
        if(preOrder is null)
            throw new ArgumentNullException(nameof(preOrder),"preOrder is NULL");

        if (inOrder is null)
            throw new ArgumentNullException(nameof(inOrder), "inOrder is NULL");

        List<T> preList = preOrder.ToList();
        List<T> inList = inOrder.ToList();

        if (preList.Count==0)
            throw new InvalidOperationException("preOrder doesn't have any data");

        if (inList.Count==0)
            throw new InvalidOperationException("inOrder doesn't have any data");

        if (preList.Count != inList.Count)
            throw new InvalidOperationException("The array length is inconsistent");

        Count = preList.Count;
        _headNode = BuilderBiTree(preList, inList);
    }

    /// <summary>
    /// 根据前序遍历结果和中序遍历结果构造一棵二叉树
    /// </summary>
    /// <param name="preOrder">前序遍历</param>
    /// <param name="inOrder">中序遍历</param>
    /// <param name="parNode">结点</param>
    /// <returns>返回根结点</returns>
    private static Node<T> BuilderBiTree(List<T>? preOrder, List<T>? inOrder, Node<T>? parNode = null)
    {
        if (preOrder is null || inOrder is null || preOrder.Count == 0 || inOrder.Count == 0)
            return null!;

        T rootVal = preOrder[0];//获取根节点的值
        int rootIndex = inOrder.IndexOf(rootVal);//获取根节点在中序遍历中的位置

        List<T> leftInOrder = inOrder[..rootIndex];//获取中序遍历的左子树
        List<T> rightInOrder = inOrder[(rootIndex + 1)..];//获取中序遍历的右子树

        List<T> leftPreOrder = preOrder[1..(leftInOrder.Count + 1)];
        List<T> rightPreOrder = preOrder[(leftInOrder.Count + 1)..];

        Node<T> node = new()
        {
            Data = rootVal,
            ParentNode = parNode
        };

        node.LeftNode = BuilderBiTree(leftPreOrder, leftInOrder, node);
        node.RightNode = BuilderBiTree(rightPreOrder, rightInOrder, node);

        return node;
    }

    /// <summary>
    /// 根据层序遍历的结果获取指定索引的结点的值
    /// </summary>
    /// <param name="index">指定索引</param>
    /// <returns>返回结点的值</returns>
    /// <exception cref="IndexOutOfRangeException">如果指定的索引超出范围则抛出异常</exception>
    public T GetNodeVal(int index)
    {
        if(index < 0 || index >= Count)
            throw new IndexOutOfRangeException(nameof(index));

        return LevelOrderTraversal().ElementAt(index);
    }

#if DEBUG
    public Node<T> GetNode(int index)
    {
        if (index < 0 || index >= Count)
            throw new IndexOutOfRangeException(nameof(index));

        return LevelOrderTraversalNodes()?[index]
               ?? throw new InvalidOperationException("internal error,please contact admin");
    }
#endif

    /// <summary>
    /// 统计叶结点的个数
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">如果二叉树为空则抛出异常</exception>
    /// <exception cref="InvalidOperationException">如果通过层序遍历返回的结果是NULL则抛出异常</exception>
    public int GetLeafCount()
    {
        if (IsEmpty())
            throw new ArgumentNullException(nameof(_headNode), "The tree is empty");

        IReadOnlyList<Node<T>> nodes = LevelOrderTraversalNodes() ?? throw new InvalidOperationException("internal error,please contact admin");

        return nodes.Count(node => node.LeftNode is null && node.RightNode is null);
    }

    /// <summary>
    /// 计算二叉树的深度
    /// </summary>
    /// <returns>返回二叉树的深度</returns>
    public int GetDepth() => GetDepth(_headNode);

    /// <summary>
    /// 私有方法，用于计算二叉树的深度
    /// </summary>
    /// <param name="rootNode">根结点</param>
    /// <returns>返回树的最大深度</returns>
    private int GetDepth(Node<T>? rootNode)
    {
        if (rootNode is null || Count == 0)
            return 0;

        int leftDepth = GetDepth(rootNode.LeftNode);
        int rightDepth = GetDepth(rootNode.RightNode);

        return Math.Max(leftDepth, rightDepth) + 1;
    }

    /// <summary>
    /// 为指定索引处的结点添加子结点
    /// </summary>
    /// <param name="val">子结点的值</param>
    /// <param name="index">父结点的索引</param>
    /// <param name="rightOrRight">是添加为左结点还是添加为右结点</param>
    /// <exception cref="ArgumentNullException">如果添加的值为空则抛出异常</exception>
    /// <exception cref="IndexOutOfRangeException">如果索引超出范围则抛出异常</exception>
    /// <exception cref="InvalidOperationException">如果返回的父结点为空则抛出异常</exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void AddNode(T val,int index,LefOrRigEnum rightOrRight)
    {
        if (val is null)
            throw new ArgumentNullException(nameof(val), "val is NULL");
        if (index < 0 || index >= Count)
            throw new IndexOutOfRangeException(nameof(index));

        Node<T> parentNode = LevelOrderTraversalNodes()?[index] ?? throw new InvalidOperationException("internal error,please contact admin");

        switch (rightOrRight)
        {
            case LefOrRigEnum.Left:
                if (parentNode.LeftNode is not null)
                    throw new InvalidOperationException("tree have left node");
                parentNode.LeftNode = new Node<T>
                {
                    Data = val,
                    ParentNode = parentNode
                };
                Count++;
                break;
            case LefOrRigEnum.Right:
                if (parentNode.RightNode is not null)
                    throw new InvalidOperationException("tree have right node");
                parentNode.RightNode = new Node<T>
                {
                    Data = val,
                    ParentNode = parentNode
                };
                Count++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(rightOrRight));
        }
    }

    /// <summary>
    /// 删除二叉树中的指定索引处的结点（注意，这里没有添加删除根结点的有关操作）
    /// </summary>
    /// <param name="index">需要删除的结点</param>
    /// <exception cref="IndexOutOfRangeException">如果索引超出范围则抛出异常</exception>
    /// <exception cref="InvalidOperationException">如果没有查找到需要删除的结点则抛出异常</exception>
    public void RemoveNode(int index)
    {
        if (index < 0 || index >= Count)
            throw new IndexOutOfRangeException(nameof(index));

        Node<T> deleteNode = LevelOrderTraversalNodes()?[index]
                       ?? throw new InvalidOperationException("internal error,please contact admin");

        //如果删除的结点是叶结点
        if (deleteNode.LeftNode is null && deleteNode.RightNode is null)
        {
            //先判断被删除的结点是父结点的左子树还是右子树
            if(deleteNode.ParentNode!.LeftNode==deleteNode)
                deleteNode.ParentNode.LeftNode = null;//如果是左子树，则将左子树置空
            else
                deleteNode.ParentNode.RightNode = null;//如果是右子树，则将右子树置空

            Count--;
            return;
        }

        //如果删除的结点只有左子树
        if (deleteNode.LeftNode is not null && deleteNode.RightNode is null)
        {
            //判断删除的结点是父结点的左子树还是右子树
            if (deleteNode.ParentNode!.LeftNode == deleteNode)
            {
                //如果是左子树，则将删除结点的左子树作为父结点的左子树，并将删除结点的左子树的父结点指向父结点
                deleteNode.ParentNode.LeftNode = deleteNode.LeftNode;
                deleteNode.LeftNode.ParentNode = deleteNode.ParentNode;
            }
            else
            {
                //如果是右子树，则将删除结点的左子树作为父结点的右子树，并将删除结点的左子树的父结点指向父结点
                deleteNode.ParentNode.RightNode = deleteNode.LeftNode;
                deleteNode.LeftNode.ParentNode = deleteNode.ParentNode;
            }

            Count--;
            return;
        }

        //如果删除的结点只有右子树
        if (deleteNode.RightNode is not null && deleteNode.LeftNode is null)
        {
            //如果删除的结点是父结点的左子树
            if (deleteNode.ParentNode!.LeftNode == deleteNode)
            {
                deleteNode.ParentNode.LeftNode = deleteNode.RightNode;
                deleteNode.RightNode.ParentNode = deleteNode.ParentNode;
            }
            else
            {
                //如果删除的结点是父结点的右子树
                deleteNode.ParentNode.RightNode = deleteNode.RightNode;
                deleteNode.RightNode.ParentNode = deleteNode.ParentNode;
            }

            Count--;
            return;
        }

        //如果删除的结点既有左子树又有右子树
        if (deleteNode.LeftNode is not null && deleteNode.RightNode is not null)
        {
            //如果删除的结点的左子树没有右子树
            if (deleteNode.LeftNode.RightNode is null)
            {
                deleteNode.Data = deleteNode.LeftNode.Data;
                deleteNode.LeftNode = deleteNode.LeftNode.LeftNode;
                if(deleteNode.LeftNode is not null)
                    deleteNode.LeftNode.ParentNode = deleteNode;

                Count--;
                return;
            }

            //如果删除的结点的左子树有右子树
            if (deleteNode.LeftNode!.RightNode is not null)
            {
                Node<T> rightNode = deleteNode.LeftNode.RightNode;
                while (rightNode.RightNode is not null)
                    rightNode = rightNode.RightNode;

                deleteNode.Data = rightNode.Data;

                Node<T> rightNodeParent = rightNode.ParentNode!;
                rightNode = rightNode.LeftNode!;
                if (rightNode is not null)
                {
                    rightNodeParent.RightNode = rightNode;
                    rightNode.ParentNode = rightNodeParent;
                }
                else
                    rightNodeParent.RightNode = null;

                Count--;
            }
        }
    }

    /// <summary>
    /// 查看二叉树中是否有指定的值
    /// </summary>
    /// <param name="val">需要确认是否存在的值</param>
    /// <returns>如果有，则返回true;否则返回false</returns>
    public bool Contains(T val) => LevelOrderTraversal().Contains(val);

    /// <summary>
    /// 层序遍历二叉树
    /// </summary>
    /// <returns>返回结点值的集合</returns>
    /// <exception cref="InvalidOperationException">如果二叉树为空则抛出异常</exception>
    public IEnumerable<T> LevelOrderTraversal()
    {
        if(IsEmpty())
            throw new InvalidOperationException("The tree is empty");

        return LevelOrderTraversalNodes()?.Select(node => node.Data)
               ?? throw new InvalidOperationException("internal error,please contact admin");
    }

    /// <summary>
    /// 内部方法，用于层序遍历二叉树
    /// </summary>
    /// <returns>返回结点集</returns>
    private IReadOnlyList<Node<T>>? LevelOrderTraversalNodes()
    {
        if (IsEmpty())
            return null;

        Node<T> headNode = _headNode;

        Queue<Node<T>> nodes = new();
        List<Node<T>> nodeList = new();

        nodes.Enqueue(headNode);
        while (nodes.Count != 0)
        {
            Node<T> node = nodes.Dequeue();
            nodeList.Add(node);

            if(node.LeftNode is not null)
                nodes.Enqueue(node.LeftNode);
            if(node.RightNode is not null)
                nodes.Enqueue(node.RightNode);
        }

        return nodeList;
    }

    /// <summary>
    /// 获取二叉树的前序遍历结果（仅包含数据域）
    /// </summary>
    /// <returns>返回结果集</returns>
    /// <exception cref="InvalidOperationException">如果二叉树为空则抛出异常</exception>
    public IEnumerable<T> PreOrderTraversal()
    {
        if(IsEmpty())
            throw new InvalidOperationException("tree is empty");

        return PreOrderTraversalNodes().Select(r => r.Data);
    }

    /// <summary>
    /// 获取二叉树的前序遍历结果（包括数据域和指针域）
    /// </summary>
    /// <returns>返回结果集</returns>
    /// <exception cref="InvalidOperationException">如果二叉树为空则抛出异常</exception>
    private IReadOnlyList<Node<T>> PreOrderTraversalNodes()
    {
        if (IsEmpty())
            throw new InvalidOperationException("tree is empty");

        Node<T> headNode = _headNode;
        List<Node<T>> nodes = new();

        InternalPreOrderTraversalNode(headNode);
        void InternalPreOrderTraversalNode(Node<T>? node)
        {
            if (node is null)
                return;

            nodes.Add(node);
            InternalPreOrderTraversalNode(node.LeftNode);
            InternalPreOrderTraversalNode(node.RightNode);
        }

        return nodes;
    }

    /// <summary>
    /// 获取二叉树的中序遍历结果（仅包含数据域）
    /// </summary>
    /// <returns>返回结果集</returns>
    /// <exception cref="InvalidOperationException">如果二叉树为空则抛出异常</exception>
    public IEnumerable<T> OrderTraversal()
    {
        if(IsEmpty())
            throw new InvalidOperationException("tree is empty");

        return OrderTraversalNodes().Select(r => r.Data);
    }

    /// <summary>
    /// 获取二叉树的中序遍历结果（包含数据域和指针域）
    /// </summary>
    /// <returns>返回结果集</returns>
    /// <exception cref="InvalidOperationException">如果二叉树为空则抛出异常</exception>
    private IReadOnlyList<Node<T>> OrderTraversalNodes()
    {
        if(IsEmpty())
            throw new InvalidOperationException("tree is empty");

        Node<T> headNode = _headNode;
        List<Node<T>> nodes = new();

        InternalOrderTraversalNodes(headNode);
        void InternalOrderTraversalNodes(Node<T>? node)
        {
            if (node is null)
                return;

            InternalOrderTraversalNodes(node.LeftNode);
            nodes.Add(node);
            InternalOrderTraversalNodes(node.RightNode);
        }

        return nodes;
    }

    /// <summary>
    /// 获取二叉树的后序遍历结果（包含数据域）
    /// </summary>
    /// <returns>返回结果集</returns>
    /// <exception cref="InvalidOperationException">如果二叉树为空则抛出异常</exception>
    public IEnumerable<T> PostOrderTraversal()
    {
        if(IsEmpty())
            throw new InvalidOperationException("tree is empty");

        return PostOrderTraversalNodes().Select(r => r.Data);
    }

    /// <summary>
    /// 获取二叉树的后序遍历结果（包含数据域和指针域）
    /// </summary>
    /// <returns>返回结果集</returns>
    /// <exception cref="InvalidOperationException">如果二叉树为空则抛出异常</exception>
    private IReadOnlyList<Node<T>> PostOrderTraversalNodes()
    {
        if (IsEmpty())
            throw new InvalidOperationException("tree is empty");

        Node<T> headNode = _headNode;
        List<Node<T>> nodes = new();

        InternalPostOrderTraversalNodes(headNode);
        void InternalPostOrderTraversalNodes(Node<T>? node)
        {
            if (node is null)
                return;

            InternalPostOrderTraversalNodes(node.LeftNode);
            InternalPostOrderTraversalNodes(node.RightNode);
            nodes.Add(node);
        }

        return nodes;
    }

    /// <summary>
    /// 判断二叉树是否为空
    /// </summary>
    /// <returns>空返回true，否则返回false</returns>
    public bool IsEmpty() => Count == 0;

    /// <summary>
    /// 将二叉树清空
    /// </summary>
    public void Clear()
    {
        _headNode = null!;
        Count = 0;
    }

    public void Dispose()
    {
        Clear();
        GC.SuppressFinalize(this);
    }
}
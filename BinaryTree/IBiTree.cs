namespace BinaryTree;

public interface IBiTree<T> : IDisposable
{
    T GetNodeVal(int index);//获取指定索引处的结点值
    int GetLeafCount();//获取叶子结点个数
    int GetDepth();//获取树的深度
    void AddNode(T val,int index,LefOrRigEnum lefOrRig);//添加结点
    void RemoveNode(int index);//删除结点
    bool Contains(T val);//判断是否包含指定值的结点
    bool IsEmpty();
    IEnumerable<T> LevelOrderTraversal();//层序遍历
    IEnumerable<T> PreOrderTraversal();//前序遍历
    IEnumerable<T> OrderTraversal();//中序遍历
    IEnumerable<T> PostOrderTraversal();//后序遍历
    void Clear();//清空树
}
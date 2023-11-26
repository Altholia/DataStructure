namespace BinaryTree;

public class Node<T>
{
#pragma warning disable CS8618
    public T Data { get; set; }
#pragma warning restore CS8618
    public Node<T>? LeftNode { get; set; }
    public Node<T>? RightNode { get; set; }
    public Node<T>? ParentNode { get; set; }
}
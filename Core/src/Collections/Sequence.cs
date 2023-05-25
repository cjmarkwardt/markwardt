namespace Markwardt;

public interface ISequence<T> : IReadOnlyCollection<T>
{
    T First { get; }
    T Last { get; }
}

public interface IShiftableSequence<T> : ISequence<T>
{
    void ShiftFirst(T item);
    void ShiftBefore(T item, T target);

    void ShiftLast(T item);
    void ShiftAfter(T item, T target);
}

public interface IMutableSequence<T> : IShiftableSequence<T>, ICollection<T>
{
    bool Prepend(T item);
    bool Prepend(T item, T target);

    bool Append(T item);
    bool Append(T item, T target);
}

public class Sequence<T> : IMutableSequence<T>
{
    private readonly Dictionary<T, Node> nodes = new();

    private Node? first;
    private Node? last;

    public int Count => nodes.Count;

    public T First => first != null ? first.Item : throw new InvalidOperationException("Set is empty");
    public T Last => last != null ? last.Item : throw new InvalidOperationException("Set is empty");

    bool ICollection<T>.IsReadOnly => false;

    public void ShiftFirst(T item)
        => MoveFirst(nodes[item]);

    public void ShiftBefore(T item, T target)
        => MoveBefore(nodes[item], target);

    public void ShiftLast(T item)
        => MoveLast(nodes[item]);

    public void ShiftAfter(T item, T target)
        => MoveAfter(nodes[item], target);

    public bool Prepend(T item)
        => Insert(item, MoveFirst);

    public bool Prepend(T item, T target)
        => Insert(item, node => MoveBefore(node, target));

    public bool Append(T item)
        => Insert(item, MoveLast);

    public bool Append(T item, T target)
        => Insert(item, node => MoveAfter(node, target));

    public void Clear()
    {
        nodes.Clear();
        first = last = null;
    }

    public bool Contains(T item)
        => nodes.ContainsKey(item);

    public bool Remove(T item)
    {
        if (nodes.TryGetValue(item, out Node? node))
        {
            Cut(node);
            return true;
        }

        return false;
    }

    private bool Insert(T item, Action<Node> insert)
    {
        bool isAdded = false;
        if (!nodes.TryGetValue(item, out Node? node))
        {
            node = new(item);
            isAdded = true;
        }

        insert(node);
        return isAdded;
    }

    private void Cut(Node node)
    {
        if (first == node)
        {
            first = node.Next;
        }
        
        if (last == node)
        {
            last = node.Previous;
        }

        node.Cut();
        nodes.Remove(node.Item);
    }

    private void MoveFirst(Node node)
    {
        Cut(node);
        node.Insert(null, first);
        nodes[node.Item] = node;
    }

    private void MoveBefore(Node node, T target)
    {
        Cut(node);
        node.Insert(nodes[target].Previous, nodes[target]);
        nodes[node.Item] = node;
    }

    private void MoveLast(Node node)
    {
        Cut(node);
        node.Insert(last, null);
        nodes[node.Item] = node;
    }

    private void MoveAfter(Node node, T target)
    {
        Cut(node);
        node.Insert(nodes[target], nodes[target].Next);
        nodes[node.Item] = node;
    }

    public IEnumerator<T> GetEnumerator()
    {
        Node? current = first;
        while (current != null)
        {
            yield return current.Item;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    void ICollection<T>.Add(T item)
        => Append(item);

    public void CopyTo(T[] array, int arrayIndex)
    {
        foreach (T item in this)
        {
            array[arrayIndex++] = item;
        }
    }

    private class Node
    {
        public Node(T item)
        {
            Item = item;
        }

        public T Item { get; }

        public Node? Previous { get; set; }
        public Node? Next { get; set; }

        public void Cut()
        {
            if (Previous != null)
            {
                Previous.Next = Next;
            }

            if (Next != null)
            {
                Next.Previous = Previous;
            }

            Previous = null;
            Next = null;
        }

        public void Insert(Node? left, Node? right)
        {
            Cut();

            if (left != null)
            {
                left.Next = this;
            }

            if (right != null)
            {
                right.Previous = this;
            }

            Previous = left;
            Next = right;
        }
    }
}
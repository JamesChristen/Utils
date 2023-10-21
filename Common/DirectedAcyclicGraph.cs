namespace Common
{
    public class DirectedAcyclicGraph<T>
    {
        public T Item { get; set; }
        public List<DirectedAcyclicGraph<T>> Children { get; set; } = new List<DirectedAcyclicGraph<T>>() { };
        public List<DirectedAcyclicGraph<T>> Parents { get; set; } = new List<DirectedAcyclicGraph<T>>() { };

        public DirectedAcyclicGraph()
        {
        }

        public DirectedAcyclicGraph(T item)
        {
            Item = item;
        }

        public void AddChild(T child)
        {
            DirectedAcyclicGraph<T> childNode = new(child);
            childNode.Parents.Add(this);
            Children.Add(childNode);
        }

        public void AddParent(T parent)
        {
            DirectedAcyclicGraph<T> parentNode = new(parent);
            parentNode.Children.Add(this);
            Parents.Add(parentNode);
        }

        public static IEnumerable<DirectedAcyclicGraph<T>> CreateDAG<K>(
            IEnumerable<T> items,
            Func<T, K> keySelector,
            Func<T, IEnumerable<K>> parentKeySelector)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(keySelector);
            ArgumentNullException.ThrowIfNull(parentKeySelector);

            Dictionary<K, DirectedAcyclicGraph<T>> dict = items.ToDictionary(x => keySelector(x), x => new DirectedAcyclicGraph<T>(x));
            foreach (KeyValuePair<K, DirectedAcyclicGraph<T>> set in dict)
            {
                IEnumerable<K> parentKeys = parentKeySelector(set.Value.Item);
                if (parentKeys != null)
                {
                    foreach (K key in parentKeys)
                    {
                        if (dict.TryGetValue(key, out DirectedAcyclicGraph<T> parent))
                        {
                            parent.Children.Add(set.Value);
                            set.Value.Parents.Add(parent);
                        }
                    }
                }
            }

            return dict.Values.Where(x => x.Parents.Count == 0);
        }
    }
}

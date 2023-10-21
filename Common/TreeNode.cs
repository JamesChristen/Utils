using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class TreeNode<T>
    {
        public T Item { get; set; }
        public List<TreeNode<T>> Children { get; set; } = new List<TreeNode<T>>() { };
        public TreeNode<T> Parent { get; set; }
        public bool IsExpanded { get; set; } = false;

        public bool HasChildren => Children?.Any() ?? false;

        public TreeNode()
        {
        }

        public TreeNode(T item)
        {
            Item = item;
        }

        public void Add(T child)
        {
            TreeNode<T> childNode = new TreeNode<T>(child)
            {
                Parent = this
            };
            Children.Add(childNode);
        }

        public TreeNode<S> Transform<S>(Func<T, S> transformation)
        {
            S transformedItem = transformation(Item);
            TreeNode<S> transformedNode = new TreeNode<S>
            {
                Item = transformedItem
            };

            foreach (TreeNode<T> child in Children)
            {
                TreeNode<S> transformedChild = child.Transform(transformation);
                transformedChild.Parent = transformedNode;
                transformedNode.Children.Add(transformedChild);
            }
            return transformedNode;
        }

        public IEnumerable<TreeNode<T>> Flatten()
        {
            List<TreeNode<T>> items = Children.SelectMany(x => x.Flatten()).ToList();
            items.Add(this);
            return items;
        }

        public override bool Equals(object obj)
        {
            if (obj is TreeNode<T> other)
            {
                if (other.Children.Count != Children.Count)
                {
                    return false;
                }

                foreach (TreeNode<T> child in Children)
                {
                    if (!other.Children.Contains(child))
                    {
                        return false;
                    }
                }

                return other.Item.Equals(Item);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Item, Children);
        }

        public override string ToString()
        {
            return Item?.ToString() ?? "NULL";
        }
    }

    public static class TreeNodeExtensions
    {
        public static IEnumerable<TreeNode<T>> CreateTree<T, K>(
            this IEnumerable<T> items,
            Func<T, K> keySelector,
            Func<T, K> parentKeySelector,
            TreeNode<T> parent,
            K rootValue = default)
        {
            if (items == null)
            {
                yield break;
            }

            foreach (T item in items.Where(c => EqualityComparer<K>.Default.Equals(parentKeySelector(c), rootValue)))
            {
                var treeNode = new TreeNode<T>
                {
                    Item = item,
                    Parent = parent,
                    IsExpanded = false
                };
                treeNode.Children = items.CreateTree(keySelector, parentKeySelector, treeNode, keySelector(item)).ToList();
                yield return treeNode;
            }
        }

        public static IEnumerable<TreeNode<T>> FilterTree<T, K>(
            this IEnumerable<TreeNode<T>> treeNodes,
            K searchObject,
            Func<T, K> searchFunction)
        {
            List<TreeNode<T>> newTreeNodes = new List<TreeNode<T>>() { };
            if (searchObject == null)
            {
                return treeNodes;
            }

            foreach (TreeNode<T> node in treeNodes)
            {
                if (EqualityComparer<K>.Default.Equals(searchFunction(node.Item), searchObject))
                {
                    newTreeNodes.Add(node);
                    continue;
                }
                TreeNode<T> filteredNode = FilterChildrenTree(node, searchObject, searchFunction);
                if (filteredNode != null)
                {
                    newTreeNodes.Add(filteredNode);
                }
            }
            return newTreeNodes;
        }

        private static TreeNode<T> FilterChildrenTree<T, K>(
            this TreeNode<T> treeNode,
            K searchObject,
            Func<T, K> searchFunction)
        {
            List<TreeNode<T>> toRemove = new List<TreeNode<T>>() { };
            foreach (TreeNode<T> child in treeNode.Children)
            {
                if (FilterChildrenTree(child, searchObject, searchFunction) == null)
                {
                    toRemove.Add(child);
                }
            }

            foreach (TreeNode<T> node in toRemove)
            {
                treeNode.Children.Remove(node);
            }

            if ((treeNode.Children == null || treeNode.Children.Count == 0)
                && !EqualityComparer<K>.Default.Equals(searchFunction(treeNode.Item), searchObject))
            {
                return null;
            }

            return treeNode;
        }

        public static void UpdateTree<T>(
            this IEnumerable<TreeNode<T>> tree,
            Func<T, T, bool> compareFunction,
            Func<T, T, bool> parentCompareFunction,
            T updateObject)
        {
            bool updated = false;

            foreach (TreeNode<T> treeNode in tree)
            {
                if (compareFunction(treeNode.Item, updateObject))
                {
                    treeNode.Item = updateObject;
                    updated = true;
                    return;
                }

                if (parentCompareFunction(treeNode.Item, updateObject))
                {
                    TreeNode<T> node = treeNode.Children.FirstOrDefault(x => compareFunction(x.Item, updateObject));

                    if (node != null)
                    {
                        node.Item = updateObject;
                        updated = true;
                        return;
                    }

                    treeNode.Children.Add(new TreeNode<T>(updateObject));
                    updated = true;
                    return;
                }

                treeNode.Children.UpdateTree(compareFunction, parentCompareFunction, updateObject);
            }

            if (!updated)
            {
                List<TreeNode<T>> treeList = tree.ToList();
                treeList.Add(new TreeNode<T>(updateObject));
                tree = treeList;
                updated = true;
            }
        }

        public static TreeNode<T> FindNode<T, K>(this IEnumerable<TreeNode<T>> treeNodes, Func<T, K> predicate, K matchObject)
        {
            foreach (TreeNode<T> node in treeNodes)
            {
                if (EqualityComparer<K>.Default.Equals(predicate(node.Item), matchObject))
                {
                    return node;
                }

                TreeNode<T> result = node.Children.FindNode(predicate, matchObject);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public static void ExpandParent<T>(this TreeNode<T> node)
        {
            while (node != null && node.Parent != null)
            {
                node.IsExpanded = true;
                node.Parent.IsExpanded = true;
                node = node.Parent;
            }
        }

        public static IEnumerable<TreeNode<T>> OrderTree<T, S>(
            this IEnumerable<TreeNode<T>> treeNodes,
            Func<T, S> defaultOrderFunction,
            bool isDescending = false)
        {
            List<TreeNode<T>> orderedNodes = new List<TreeNode<T>>() { };
            if (!treeNodes.Any())
            {
                return treeNodes;
            }

            orderedNodes = isDescending ? 
                treeNodes.OrderByDescending(x => defaultOrderFunction(x.Item)).ToList() : 
                treeNodes.OrderBy(x => defaultOrderFunction(x.Item)).ToList();
            
            for (int i = 0; i < orderedNodes.Count; i++)
            {
                orderedNodes[i].Children = orderedNodes[i].Children.OrderTree(defaultOrderFunction).ToList();
            }
            return orderedNodes;
        }

        public static IEnumerable<TreeNode<T>> OrderTreeByProperty<T, S, S2, S3, S4>(
            this IEnumerable<TreeNode<T>> treeNodes,
            Func<T, bool> filterType2,
            Func<T, bool> filterType3,
            Func<T, bool> filterType4,
            Func<T, S> defaultOrderFunction,
            Func<T, S2> type2OrderFunction,
            Func<T, S3> type3OrderFunction,
            Func<T, S4> type4OrderFunction)
        {
            List<TreeNode<T>> orderedNodes = new List<TreeNode<T>>() { };
            if (!treeNodes.Any())
            {
                return treeNodes;
            }

            if (treeNodes.All(x => filterType2(x.Item)))
            {
                orderedNodes = treeNodes.OrderBy(x => type2OrderFunction(x.Item)).ToList();
            }
            else if (treeNodes.All(x => filterType3(x.Item)))
            {
                orderedNodes = treeNodes.OrderBy(x => type3OrderFunction(x.Item)).ToList();
            }
            else if (treeNodes.All(x => filterType4(x.Item)))
            {
                orderedNodes = treeNodes.OrderBy(x => type4OrderFunction(x.Item)).ToList();
            }
            else
            {
                orderedNodes = treeNodes.OrderBy(x => defaultOrderFunction(x.Item)).ToList();
            }

            for (int i = 0; i < orderedNodes.Count; i++)
            {
                orderedNodes[i].Children = orderedNodes[i].Children.OrderTreeByProperty(
                    filterType2, 
                    filterType3, 
                    filterType4, 
                    defaultOrderFunction, 
                    type2OrderFunction, 
                    type3OrderFunction, 
                    type4OrderFunction).ToList();
            }
            return orderedNodes;
        }
    }
}

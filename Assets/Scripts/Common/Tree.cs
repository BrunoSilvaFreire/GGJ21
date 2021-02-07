using System;
using System.Collections.Generic;
using System.Linq;
namespace Common {
    [Serializable]
    public class Tree<T> {
        public string name;
        public T self;
        public List<Tree<T>> children = new List<Tree<T>>();
        public Tree<T> FindOrCreate(string path) {
            return FindOrCreate(path.Split('/'));
        }
        public Tree<T> FindOrCreate(string[] path) {
            var current = this;
            foreach (var segment in path) {
                if (string.IsNullOrEmpty(segment)) {
                    continue;
                }
                var next = current.children.FirstOrDefault(tree => tree.name == segment);
                if (next == null) {
                    next = new Tree<T> {
                        name = segment
                    };
                    current.children.Add(next);
                }
                current = next;
            }
            return current;
        }
    }
}
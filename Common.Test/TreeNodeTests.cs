using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Test
{
    [TestClass]
    public class TreeNodeTests
    {
        public class MockClass
        {
            public int Id { get; set; }
            public int ParentId { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public bool IsTrue { get; set; }

            public override string ToString()
            {
                return $"ID:{Id}, PID:{ParentId}, {Name}";
            }

            public override bool Equals(object obj)
            {
                return obj is MockClass mc && mc.GetHashCode() == GetHashCode();
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Id, ParentId, Name, Date, IsTrue);
            }
        }

        [TestMethod]
        public void Test_CreateTree_CreatesTreeBasedOnProperty()
        {
            MockClass a1 = new MockClass { Id = 1, Name = "a1" };
            MockClass a1b1 = new MockClass { Id = 2, Name = "a1b1", ParentId = a1.Id };
            MockClass a1b2 = new MockClass { Id = 3, Name = "a1b2", ParentId = a1.Id };
            MockClass a1b2c1 = new MockClass { Id = 4, Name = "a1b2c1", ParentId = a1b2.Id };
            MockClass a2 = new MockClass { Id = 5, Name = "a2" };

            MockClass[] arr = new MockClass[] { a1, a1b1, a1b2, a1b2c1, a2 };

            List<TreeNode<MockClass>> expected = new List<TreeNode<MockClass>>()
            {
                new TreeNode<MockClass>()
                {
                    Item = a1,
                    Children = new List<TreeNode<MockClass>>
                    {
                        new TreeNode<MockClass>()
                        {
                            Item = a1b1,
                            Children = new List<TreeNode<MockClass>>()
                        },
                        new TreeNode<MockClass>()
                        {
                            Item = a1b2,
                            Children = new List<TreeNode<MockClass>>
                            {
                                new TreeNode<MockClass>()
                                {
                                    Item = a1b2c1,
                                    Children = new List<TreeNode<MockClass>>()
                                }
                            }
                        }
                    }
                },
                new TreeNode<MockClass>()
                {
                    Item = a2,
                    Children = new List<TreeNode<MockClass>>()
                }
            };

            IEnumerable<TreeNode<MockClass>> tree = arr.CreateTree(x => x.Id, x => x.ParentId, null);

            int i = 0;
            foreach (TreeNode<MockClass> node in tree)
            {
                Assert.AreEqual(expected[i], node);
                i += 1;
            }
        }

        [TestMethod]
        public void Test_CreateTree_EmptyListShouldReturnEmptyTree()
        {
            IEnumerable<TreeNode<MockClass>> tree =
                new List<MockClass>().CreateTree(x => x.Id, x => x.ParentId, null);

            Assert.AreEqual(tree.Count(), 0);
        }

        [TestMethod]
        public void Test_CreateTree_NullListReturnsEmptyTree()
        {
            IEnumerable<MockClass> list = null;

            try
            {
                IEnumerable<TreeNode<MockClass>> tree = list.CreateTree(x => x.Id, x => x.ParentId, null);
                Assert.AreEqual(tree.Count(), 0);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Test_FilterTree_LeafNodeMatchesIncludesOnlyParents()
        {
            MockClass a1 = new MockClass { Id = 1, Name = "a1" };
            MockClass a1b1 = new MockClass { Id = 2, Name = "a1b1", ParentId = a1.Id };
            MockClass a1b2 = new MockClass { Id = 3, Name = "a1b2", ParentId = a1.Id };
            MockClass a1b2c1 = new MockClass { Id = 4, Name = "a1b2c1", ParentId = a1b2.Id };
            MockClass a2 = new MockClass { Id = 5, Name = "a2" };

            List<TreeNode<MockClass>> tree = new List<TreeNode<MockClass>>()
            {
                new TreeNode<MockClass>()
                {
                    Item = a1,
                    Children = new List<TreeNode<MockClass>>
                    {
                        new TreeNode<MockClass>()
                        {
                            Item = a1b1,
                            Children = new List<TreeNode<MockClass>>()
                        },
                        new TreeNode<MockClass>()
                        {
                            Item = a1b2,
                            Children = new List<TreeNode<MockClass>>
                            {
                                new TreeNode<MockClass>()
                                {
                                    Item = a1b2c1,
                                    Children = new List<TreeNode<MockClass>>()
                                }
                            }
                        }
                    }
                },
                new TreeNode<MockClass>()
                {
                    Item = a2,
                    Children = new List<TreeNode<MockClass>>()
                }
            };

            List<TreeNode<MockClass>> expected = new List<TreeNode<MockClass>>()
            {
                new TreeNode<MockClass>()
                {
                    Item = a1,
                    Children = new List<TreeNode<MockClass>>
                    {
                        new TreeNode<MockClass>()
                        {
                            Item = a1b2,
                            Children = new List<TreeNode<MockClass>>
                            {
                                new TreeNode<MockClass>()
                                {
                                    Item = a1b2c1,
                                    Children = new List<TreeNode<MockClass>>()
                                }
                            }
                        }
                    }
                }
            };

            IEnumerable<TreeNode<MockClass>> filteredTree = tree.FilterTree("a1b2c1", x => x.Name);

            int i = 0;
            foreach (TreeNode<MockClass> node in filteredTree)
            {
                Assert.AreEqual(expected[i], node);
                i += 1;
            }
        }

        [TestMethod]
        public void Test_FilterTree_BranchNodeMatchesIncludesOnlyParents()
        {
            MockClass a1 = new MockClass { Id = 1, Name = "a1" };
            MockClass a1b1 = new MockClass { Id = 2, Name = "a1b1", ParentId = a1.Id };
            MockClass a1b2 = new MockClass { Id = 3, Name = "a1b2", ParentId = a1.Id };
            MockClass a1b2c1 = new MockClass { Id = 4, Name = "a1b2c1", ParentId = a1b2.Id };
            MockClass a2 = new MockClass { Id = 5, Name = "a2" };

            List<TreeNode<MockClass>> tree = new List<TreeNode<MockClass>>()
            {
                new TreeNode<MockClass>()
                {
                    Item = a1,
                    Children = new List<TreeNode<MockClass>>
                    {
                        new TreeNode<MockClass>()
                        {
                            Item = a1b1,
                            Children = new List<TreeNode<MockClass>>()
                        },
                        new TreeNode<MockClass>()
                        {
                            Item = a1b2,
                            Children = new List<TreeNode<MockClass>>
                            {
                                new TreeNode<MockClass>()
                                {
                                    Item = a1b2c1,
                                    Children = new List<TreeNode<MockClass>>()
                                }
                            }
                        }
                    }
                },
                new TreeNode<MockClass>()
                {
                    Item = a2,
                    Children = new List<TreeNode<MockClass>>()
                }
            };

            List<TreeNode<MockClass>> expected = new List<TreeNode<MockClass>>()
            {
                new TreeNode<MockClass>()
                {
                    Item = a1,
                    Children = new List<TreeNode<MockClass>>
                    {
                        new TreeNode<MockClass>()
                        {
                            Item = a1b2,
                            Children = new List<TreeNode<MockClass>>()
                        }
                    }
                }
            };

            IEnumerable<TreeNode<MockClass>> filteredTree = tree.FilterTree("a1b2", x => x.Name);

            int i = 0;
            foreach (TreeNode<MockClass> node in filteredTree)
            {
                Assert.AreEqual(expected[i], node);
                i += 1;
            }
        }

        [TestMethod]
        public void Test_FilterTree_NoMatchesReturnsEmptyList()
        {
            MockClass a1 = new MockClass { Id = 1, Name = "a1" };

            List<TreeNode<MockClass>> tree = new List<TreeNode<MockClass>>()
            {
                new TreeNode<MockClass>()
                {
                    Item = a1,
                    Children = new List<TreeNode<MockClass>>()
                }
            };

            IEnumerable<TreeNode<MockClass>> filteredTree = tree.FilterTree("a1b2", x => x.Name);
            Assert.IsNotNull(filteredTree);
            Assert.AreEqual(0, filteredTree.Count());
        }
    }
}

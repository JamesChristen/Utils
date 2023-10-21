using Ardalis.GuardClauses;
using System.Text.RegularExpressions;

namespace Common.Validation
{
    public class Validate
    {
        private readonly List<Exception> _exceptions;
        private readonly List<TreeNode<ICheck>> _checkTree;

        private List<TreeNode<ICheck>> _currentBranch;
        private TreeNode<ICheck> _currentNode;
        private bool _nextIsSkipped = false;

        private Validate()
        {
            _exceptions = new List<Exception>();
            _checkTree = new List<TreeNode<ICheck>>();
            _currentBranch = _checkTree;
            _currentNode = null;
        }

        public static Validate That
        {
            get
            {
                Validate foo = new Validate();
                return foo;
            }
        }

        private enum BranchState { NewRoot, ContinueBranch, NewBranch }

        /// <summary>Creates a new branch off the current node</summary>
        public Validate Then
        {
            get
            {
                if (_currentBranch.Count == 0)
                {
                    throw new InvalidOperationException($"Invalid setup - cannot '{nameof(Then)}' on an empty branch");
                }
                _currentNode = _currentBranch[^1];
                _currentBranch = _currentNode.Children;
                return this;
            }
        }

        /// <summary>Creates a new root branch</summary>
        public Validate Root
        {
            get
            {
                _currentNode = null;
                _currentBranch = _checkTree;
                return this;
            }
        }

        /// <summary>Goes back to the previous branch</summary>
        public Validate Back
        {
            get
            {
                if (_currentBranch.Count == 0)
                {
                    throw new InvalidOperationException($"Invalid setup - cannot '{nameof(Back)}' from an empty branch");
                }

                _currentNode = _currentNode?.Parent ?? null;
                _currentBranch = _currentNode?.Children ?? _checkTree;
                return this;
            }
        }

        public Validate If(Func<bool> predicate)
        {
            _nextIsSkipped = !predicate();
            return this;
        }

        public Validate If(bool predicate)
        {
            _nextIsSkipped = !predicate;
            return this;
        }

        /// <summary>
        /// Checks the results of the validation parameters and throws a <see cref="ValidationException"/> if any do not pass validation
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        public void Check()
        {
            foreach (TreeNode<ICheck> tree in _checkTree)
            {
                CheckTree(tree);
            }

            if (_exceptions.Any())
            {
                List<Exception> exceptions = new List<Exception>(_exceptions);
                _exceptions.Clear();
                throw new ValidationException(exceptions);
            }
        }

        private void CheckTree(TreeNode<ICheck> tree)
        {
            try
            {
                tree.Item.Check();

                if (tree.HasChildren)
                {
                    foreach (TreeNode<ICheck> child in tree.Children)
                    {
                        CheckTree(child);
                    }
                }
            }
            catch (ValidationException validationEx)
            {
                foreach (Exception inner in validationEx.InnerExceptions)
                {
                    _exceptions.Add(inner);
                }
            }
            catch (Exception ex)
            {
                _exceptions.Add(ex);
            }
        }

        private Validate AddCheck(Func<object> validCheck, string name)
        {
            ICheck check = new Checker(validCheck, name);
            return AddCheck(check);
        }

        private Validate AddCheck(ICheck check)
        {
            if (_nextIsSkipped)
            {
                _nextIsSkipped = false;
                return this;
            }

            if (_currentNode == null) // Root branch
            {
                _currentBranch.Add(new TreeNode<ICheck>(check));
            }
            else // Add to existing branch
            {
                _currentNode.Add(check);
            }

            return this;
        }

        private interface ICheck
        {
            void Check();
        }

        private class Checker : ICheck
        {
            private readonly Func<object> _check;
            private readonly string _name;

            public Checker(Func<object> func, string name)
            {
                _check = func;
                _name = name;
            }

            public void Check()
            {
                _check();
            }

            public override string ToString() => _name;
        }

        private class ConditionalChecker : ICheck
        {
            private readonly ICheck _check;
            private readonly bool _skip;

            public ConditionalChecker(ICheck check, bool skip)
            {
                _check = check;
                _skip = skip;
            }

            public void Check()
            {
                if (!_skip)
                {
                    _check.Check();
                }
            }
        }

        public Validate IsNull<T>(T input, string name)
            => AddCheck(() => Guard.Against.NotNull(input, name), name);

        public Validate IsNotNull<T>(T input, string name)
            => AddCheck(() => Guard.Against.Null(input, name), name);

        public Validate IsNegative(decimal input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i < 0, $"{name} must be < 0"), name);

        public Validate IsNegativeOrZero(decimal input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i <= 0, $"{name} must be <= 0"), name);

        public Validate IsPositive(decimal input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i > 0, $"{name} must be > 0"), name);

        public Validate IsPositiveOrZero(decimal input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i >= 0, $"{name} must be >= 0"), name);

        public Validate IsZero(decimal input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i == 0, $"{name} must be != 0"), name);

        public Validate IsNegative(long input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i < 0, $"{name} must be < 0"), name);

        public Validate IsNegativeOrZero(long input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i <= 0, $"{name} must be <= 0"), name);

        public Validate IsPositive(long input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i > 0, $"{name} must be > 0"), name);

        public Validate IsPositiveOrZero(long input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i >= 0, $"{name} must be >= 0"), name);

        public Validate IsZero(long input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i == 0, $"{name} must be != 0"), name);

        public Validate IsNegative(int input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i < 0, $"{name} must be < 0"), name);

        public Validate IsNegativeOrZero(int input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i <= 0, $"{name} must be <= 0"), name);

        public Validate IsPositive(int input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i > 0, $"{name} must be > 0"), name);

        public Validate IsPositiveOrZero(int input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i >= 0, $"{name} must be >= 0"), name);

        public Validate IsZero(int input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, i => i == 0, $"{name} must be != 0"), name);

        public Validate IsNullOrEmpty(string input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, x => string.IsNullOrEmpty(x), $"{name} must be null or empty"), name);

        public Validate IsNullOrWhiteSpace(string input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, x => string.IsNullOrWhiteSpace(x), $"{name} must be null or whitespace"), name);

        public Validate IsNotNullOrEmpty(string input, string name)
            => AddCheck(() => Guard.Against.NullOrEmpty(input, name), name);

        public Validate IsNotNullOrWhiteSpace(string input, string name)
            => AddCheck(() => Guard.Against.NullOrWhiteSpace(input, name), name);

        public Validate IsNullOrEmpty<T>(IEnumerable<T> input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, x => x == null || !x.Any(), name), name);

        public Validate IsNotNullOrEmpty<T>(IEnumerable<T> input, string name)
            => AddCheck(() => Guard.Against.NullOrEmpty(input, name), name);

        public Validate IsTrue(bool input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, x => x, $"{name} is not true"), name);

        public Validate IsFalse(bool input, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, x => !x, $"{name} is not false"), name);

        public Validate AreEqual<T>(T input, T expected, string name)
        {
            AddCheck(() =>
            {
                if (input == null)
                {
                    if (expected != null)
                    {
                        ArgumentNullException.ThrowIfNull(input);
                    }
                    return true; // Both null is equal
                }
                else if (expected == null)
                {
                    throw new ArgumentNullException(nameof(expected));
                }

                Guard.Against.InvalidInput(input, input => input.Equals(expected), $"{name} not equal to expected: {input} => {expected}");
                return true;
            }, name);
            return this;
        }

        public Validate AreEqual<T, S>(T input, S toCompare, Func<T, S, bool> comparison, string name)
        {
            AddCheck(() =>
            {
                ArgumentNullException.ThrowIfNull(comparison);
                Guard.Against.InvalidInput(input, input => comparison(input, toCompare), $"{name} not equal to expected: {input} => {toCompare}");
                return true;
            }, name);
            return this;
        }

        public Validate IsValidInput<T>(T input, Func<T, bool> validCheck, string message)
            => AddCheck(() => Guard.Against.InvalidInput(input, validCheck, message), message);

        public Validate ContainsParam<T>(DynamicParams<T> @params, T param) where T : struct, Enum
            => AddCheck(() => Guard.Against.InvalidInput(@params, p => p.HasParam(param), $"Params does not contain {param.ToLongString()}"), param.ToString());

        public Validate RegexMatches(string input, string pattern, string name)
            => AddCheck(() => Guard.Against.InvalidInput(input, x => Regex.IsMatch(x, pattern), $"{name} does not match Regex {pattern}"), name);

        public Validate IsValid(IValidatable input)
        {
            AddCheck(() =>
            {
                ArgumentNullException.ThrowIfNull(input);

                try
                {
                    input.CheckValid();
                }
                catch (ValidationException ex)
                {
                    throw new ValidationException(ex.InnerExceptions.Select(x => new Exception($"{input.ValidationIdentifier}: {x.Message}")));
                }
                return true;
            }, input?.ValidationIdentifier ?? "NULL");
            return this;
        }

        public Validate AreValid(IEnumerable<IValidatable> input)
        {
            string name = input == null || !input.Any() ? "NONE" : string.Join(", ", input.Select(x => x.ValidationIdentifier));
            AddCheck(() =>
            {
                if (input == null)
                {
                    return true;
                }

                List<Exception> exceptions = new List<Exception>();
                foreach (IValidatable i in input)
                {
                    try
                    {
                        i.CheckValid();
                    }
                    catch (ValidationException ex)
                    {
                        exceptions.AddRange(ex.InnerExceptions.Select(x => new Exception($"{i.ValidationIdentifier}: {x.Message}")));
                    }
                }
                if (exceptions.Any())
                {
                    throw new ValidationException(exceptions);
                }
                return true;
            }, name);
            return this;
        }

        public Validate AreValid(params IValidatable[] input)
            => AreValid(input?.AsEnumerable() ?? Array.Empty<IValidatable>());

        public Validate ContainsKey<T, S>(Dictionary<T, S> dictionary, T requiredKey, string name)
            => AddCheck(() => Guard.Against.DoesNotContainKey(dictionary, requiredKey, name), name);

        public Validate IsEnumDefined<T>(int id, string name) where T : Enum
            => AddCheck(() => Guard.Against.InvalidInput(id, x => Enum.IsDefined(typeof(T), x), $"({typeof(T).Name}) {name} undefined: {id}"), name);

        public Validate IsEnumDefined<T>(byte id, string name) where T : Enum
            => AddCheck(() => Guard.Against.InvalidInput(id, x => Enum.IsDefined(typeof(T), Convert.ToInt32(x)), $"({typeof(T).Name}) {name} undefined: {id}"), name);
    }
}

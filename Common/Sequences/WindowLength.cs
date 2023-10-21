namespace Common.Sequences
{
    public struct WindowLength
    {
        public bool IsInfinite { get; set; }
        public int Length { get; set; }

        public WindowLength(int length)
            : this(false, length)
        {
        }

        private WindowLength(bool isInfinite, int length)
        {
            if (length <= 0M)
            {
                throw new ArgumentException($"{nameof(WindowLength)} cannot have length <= 0");
            }

            if (isInfinite)
            {
                IsInfinite = true;
                Length = 0;
            }
            else
            {
                IsInfinite = false;
                Length = length;
            }
        }

        public static WindowLength Infinite => new WindowLength()
        {
            IsInfinite = true,
            Length = 0
        };

        public override string ToString()
        {
            return IsInfinite ? "Inf" : Length.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is WindowLength len && len.IsInfinite == IsInfinite && len.Length == Length;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsInfinite, Length);
        }

        public static implicit operator string(WindowLength length)
        {
            return length.ToString();
        }

        public static implicit operator WindowLength(string input)
        {
            if (!CanParse(input, out WindowLength length))
            {
                throw new ArgumentException($"Cannot parse {input} to {nameof(WindowLength)}");
            }
            return length;
        }

        public static bool CanParse(string input, out WindowLength length)
        {
            if (input.Equals("Inf", StringComparison.OrdinalIgnoreCase))
            {
                length = Infinite;
                return true;
            }
            else if (int.TryParse(input, out int len) && len > 0)
            {
                length = new WindowLength(len);
                return true;
            }
            else
            {
                length = new WindowLength()
                {
                    IsInfinite = false,
                    Length = 0
                };
                return false;
            }
        }

        public static WindowLength Parse(string input)
        {
            return input;
        }
    }
}

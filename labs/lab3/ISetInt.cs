namespace lab3
{
    public interface ISetInt
    {
        int GetCount { get; } 
        bool Add(int value);
        bool Remove(int value);
        bool Contains(int value);
        void Clear();
        void CopyTo(int[] array);
        bool Overlaps(ISetInt other);
        void SymmetricExceptWith(ISetInt other);
    }
}
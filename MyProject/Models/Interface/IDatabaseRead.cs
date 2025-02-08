namespace ProjectLayer.Models.Interface
{
    internal interface IDatabaseRead
    {
        object? Get(int value);
        object? Get(string value);
        object? List(object? value);

        bool Any(int value);
        
        bool Any1(string value);
        
        bool Any2(object value1, object value2);

    }
}
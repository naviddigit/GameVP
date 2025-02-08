namespace ProjectLayer.Models.Interface
{
    internal interface IDatabaseReadMore
    {
        object? Sum(object value);
        int? Count(object? value);

    }
}
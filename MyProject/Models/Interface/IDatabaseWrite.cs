namespace ProjectLayer.Models.Interface
{
    internal interface IDatabaseWrite:IDatabaseRead
    {
        object Insert(object items);
    }
}
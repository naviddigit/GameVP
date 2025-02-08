namespace ProjectLayer.Models.Interface
{
    internal interface IDatabase
    {
        void Update(object items);
        void Update(object items1, object items2);
        bool Delete(object items);
    }
}
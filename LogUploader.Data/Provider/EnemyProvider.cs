namespace LogUploader.Data
{
    public interface EnemyProvider<T> where T : Enemy
    {
        T Get(int id);
        T Get(string nameEN);
    }
}
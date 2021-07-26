namespace LogUploader.Data.StaticDataLoader
{
    internal class AddEnemyRegistrator : IAddEnemyRegistrator
    {
        public bool Exists(int id)
        {
            return AddEnemy.ExistsID(id);
        }

        public AbstractAddEnemy Get(int id)
        {
            return AddEnemy.GetByID(id);
        }

        public void Register(Enemy.BasicInfo info, bool intresting)
        {
            new AddEnemy(info, intresting);
        }

        public void Register()
        {
            new AddEnemy();
        }
    }

}

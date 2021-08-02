using System;

using LogUploader.Data.Repositories;

namespace LogUploader.Data.StaticDataLoader
{
    internal class AddEnemyRegistrator : IAddEnemyRegistrator
    {
        private AddEnemyRepository Repo;

        public AddEnemyRegistrator(AddEnemyRepository repo)
        {
            Repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public bool Exists(int id)
        {
            return Repo.Exists(id);
        }

        public void Register((int iD, string nameEN, string nameDE, GameArea gameArea) basicInfo, bool intresting)
        {
            (int iD, string nameEN, string nameDE, GameArea gameArea) = basicInfo;
            AddEnemy add = new AddEnemy(iD, nameEN, nameDE, gameArea, intresting);
            Repo.Add(add);
        }

        public void Register()
        {
            AddEnemy add = new AddEnemy();
            Repo.Add(add);
        }

        AddEnemy Get(int id)
        {
            return Repo.Get(id);
        }

        AddEnemy IAddEnemyRegistrator.Get(int id)
        {
            throw new NotImplementedException();
        }
    }

}

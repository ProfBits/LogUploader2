using System;

namespace LogUploader.Data.Repositories
{
    internal abstract class AreaRepository<T> : AreaProvider<T> where T : GameArea
    {
        protected virtual T AreaData { get; set; }
        
        public string Name { get => AreaData.Name; }
        public string NameEN { get => AreaData.NameEN; }
        public string NameDE { get => AreaData.NameDE; }
        public string ShortName { get => AreaData.ShortName; }
        public string ShortNameEN { get => AreaData.ShortNameEN; }
        public string ShortNameDE { get => AreaData.ShortNameDE; }

        internal AreaRepository(T areaData)
        {
            AreaData = areaData ?? throw new ArgumentNullException(nameof(areaData));
        }

        internal virtual void SetArea(T area)
        {
            AreaData = area ?? throw new ArgumentNullException(nameof(area), "Area cannot be set to null");
        }

        public virtual T Get()
        {
            return AreaData;
        }
    }

    internal class TrainingAreaRepository : AreaRepository<Training>
    {
        internal TrainingAreaRepository() : base(new Training()) { }
    }

    internal class WvWAreaRepository : AreaRepository<WvW>
    {
        internal WvWAreaRepository() : base(new WvW()) { }
    }

    internal class UnkowenAreaRepository : AreaRepository<UnkowenGameArea>
    {
        internal UnkowenAreaRepository() : base(new UnkowenGameArea()) { }
    }
}

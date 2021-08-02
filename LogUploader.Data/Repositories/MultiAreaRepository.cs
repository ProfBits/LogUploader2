using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.Repositories
{
    internal abstract class MultiAreaRepository<T> : MultiAreaProvider<T> where T : MultiGameArea
    {
        protected virtual Dictionary<int, T> Data { get; }

        protected MultiAreaRepository()
        {
            Data = new Dictionary<int, T>();
        }

        public bool Exists(int number)
        {
            return Data.ContainsKey(number);
        }

        public T Get(int id)
        {
            return Data[id];
        }

        internal void Add(T area)
        {
            if (area is null)
                throw new ArgumentNullException(nameof(area), "Cannot add null area.");
            if (Exists(area.ID))
                throw new ArgumentException($"Area with id = {area.ID} cannot be added, alread exixts.", nameof(area));
            Data.Add(area.ID, area);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return Data.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class RaidRepository : MultiAreaRepository<Raid>
    {

    }

    internal class StrikeRepository : MultiAreaRepository<Strike>
    {

    }

    internal class FractalRepository : MultiAreaRepository<Fractal>
    {

    }

    internal class DragonResponseMissionRepository : MultiAreaRepository<DragonResponseMission>
    {

    }
}

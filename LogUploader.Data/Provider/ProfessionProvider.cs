using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data
{
    public interface ProfessionProvider
    {
        Profession Get(eProfession profession);
        Profession Get(string name);
        Profession Get(int raidOrgaPlusID);
        Profession GetByAbbreviation(string abbreviation);
    }
}

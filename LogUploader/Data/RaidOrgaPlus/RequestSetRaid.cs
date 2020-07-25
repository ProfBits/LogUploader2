using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Data.RaidOrgaPlus
{
    /*
     *  {
     *      session: <gültige Session-ID aus Login mit Raidleiter-Rechten für entsprechenden Raid>
     *      body: {
     *          terminId: 123
     *          aufstellungen: [
     *              {
     *                  aufstellungId?: 1234
     *                  bossId?: 12
     *                  isCM?: true / false
     *                  positionen: [
     *                      {
     *                          position: 1
     *                          spielerId: 12
     *                          classId: 20
     *                          roleId: 5
     *                      }
     *                  ]
     *              }
     *          ]}
     *  }
     * 
     * 
     * 
     * 
     * 
     */

    public class RequestSetRaid
    {
        public long session { get; set; }
        public Raid body { get; set; }
    }
}

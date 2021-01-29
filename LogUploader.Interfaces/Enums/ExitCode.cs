using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader
{

    public enum ExitCode : int
    {
        OK = 0,

        #region Errors (1-39)
        WIN32_EXCPTION = 1,
        CLR_EXCPTION = 2,
        #endregion

        #region Development/Test (40-49)
        LANG_XML_CREATION_BUILD = 42,
        DEV_TEST = 41,
        #endregion

        #region Expected Exits (100-199)
        UPDATING = 100,
        #endregion

        #region Startup Errors (200-299)
        STARTUP_FAILED = 200,
        ALREADY_RUNNING = 201,
        INIT_SETUP_FAILED = 202,
        EI_UPDATE_FATAL_ERROR = 203,
        LOAD_SETTINGS_ERROR = 204,
        #endregion
    }
}

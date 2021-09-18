using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogUploader.Test;

using NUnit.Framework;


namespace LogUploader.Test.Configuration
{
    public class BuildConfigurationTest
    {
        [Test]
        public void BuildConfigurationDefineTest()
        {
            var detectedConfigs = ValidateCompileDefines().Where(config => config != default).ToList();
            Assert.Multiple(() =>
            {
                foreach (var config in detectedConfigs)
                {
                    if (config.status != Status.SUCCESS)
                    {
                        Assert.Fail(config.message);
                    }
                }
            });

            Assert.That(detectedConfigs, Has.Count.EqualTo(1));

            TestContext.WriteLine("Build Configuration: " + detectedConfigs[0].message);
        }

        private IEnumerable<(Status status, string message)> ValidateCompileDefines()
        {
#pragma warning disable CS0219 // Ignore unused warning, only one of the lines shoulbe be used anyway
            const Status FAILE = Status.FAILE;
            const Status SUCCESS = Status.SUCCESS;
#pragma warning restore CS0219
            string Message = $"Invalid build define constants. See {typeof(BuildConfigurationTest).FullName} for more info.\nActual config ID is: ";

#if DEBUG && ALPHA && BETA && TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 000");
#elif DEBUG && ALPHA && BETA && TRACE && !CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 001");
#elif DEBUG && ALPHA && BETA && !TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 002");
#elif DEBUG && ALPHA && BETA && !TRACE && !CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 003");
#elif DEBUG && ALPHA && !BETA && TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 004");
#elif DEBUG && ALPHA && !BETA && TRACE && !CREATE_LANGUAGE_XMLS
            yield return (SUCCESS, "Alpha Release");
#elif DEBUG && ALPHA && !BETA && !TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 005");
#elif DEBUG && ALPHA && !BETA && !TRACE && !CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 006");
#elif DEBUG && !ALPHA && BETA && TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 007");
#elif DEBUG && !ALPHA && BETA && TRACE && !CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 008");
#elif DEBUG && !ALPHA && BETA && !TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 009");
#elif DEBUG && !ALPHA && BETA && !TRACE && !CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 010");
#elif DEBUG && !ALPHA && !BETA && TRACE && CREATE_LANGUAGE_XMLS
            yield return (SUCCESS, "CreateLanguageXMLs build");
#elif DEBUG && !ALPHA && !BETA && TRACE && !CREATE_LANGUAGE_XMLS
            yield return (SUCCESS, "Debug Build");
#elif DEBUG && !ALPHA && !BETA && !TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 011");
#elif DEBUG && !ALPHA && !BETA && !TRACE && !CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 012");
#elif !DEBUG && ALPHA && BETA && TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 013");
#elif !DEBUG && ALPHA && BETA && TRACE && !CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 014");
#elif !DEBUG && ALPHA && BETA && !TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 015");
#elif !DEBUG && ALPHA && BETA && !TRACE && !CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 016");
#elif !DEBUG && ALPHA && !BETA && TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 017");
#elif !DEBUG && ALPHA && !BETA && TRACE && !CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 018");
#elif !DEBUG && ALPHA && !BETA && !TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 019");
#elif !DEBUG && ALPHA && !BETA && !TRACE && !CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 020");
#elif !DEBUG && !ALPHA && BETA && TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 021");
#elif !DEBUG && !ALPHA && BETA && TRACE && !CREATE_LANGUAGE_XMLS
            yield return (SUCCESS, "Beta Release");
#elif !DEBUG && !ALPHA && BETA && !TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 022");
#elif !DEBUG && !ALPHA && BETA && !TRACE && !CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 023");
#elif !DEBUG && !ALPHA && !BETA && TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 024");
#elif !DEBUG && !ALPHA && !BETA && TRACE && !CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 025");
#elif !DEBUG && !ALPHA && !BETA && !TRACE && CREATE_LANGUAGE_XMLS
            yield return (FAILE, "Invalid build config 026");
#elif !DEBUG && !ALPHA && !BETA && !TRACE && !CREATE_LANGUAGE_XMLS
            yield return (SUCCESS, "Release Build");
#else
            yield return (FAILE, "Unkown Configuration. A Combination of compile symbols is missing");
#endif

            yield return default;
        }

        private enum Status
        {
            SUCCESS,
            FAILE
        }
    }
}

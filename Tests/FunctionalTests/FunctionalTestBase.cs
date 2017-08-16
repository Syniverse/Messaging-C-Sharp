using Newtonsoft.Json;
using ScgApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class TestSetup
{
    public String url;
    public UInt64 mdnRangeStart = 0;
    public String senderIdCountry;
    public String senderIdSms;
}

public class FunctionalTestBase : IDisposable
{
    public Session Session { get; private set; }
    public AuthInfo Auth { get; private set; }
    public TestSetup Setup { get; private set; }

    protected String GetConfig(String key, String defaultName)
    {
        String v = Environment.GetEnvironmentVariable(key);
        if (!String.IsNullOrEmpty(v))
        {
            return v;
        }

        foreach (var h in new List<String>() { "HOME", "HOMEPATH" })
        {
            v = Environment.GetEnvironmentVariable(h);
            if (!String.IsNullOrEmpty(v))
            {
                return v + "/" + defaultName;
            }
        }
        return defaultName;
    }

    /* Test setup and auth file paths are deduced from:
     *   - Environment variables TEST_AUTH and TEST_SETUP
     *   - %HOME%/auth.json
     *   - %HOMEPATH%/auth.json
     *   - ./auth.json
     *   - %HOME%/test_setup.json
     *   - %HOMEPATH%/test_setup.json
     *   - ./test_setup.json
     */
    public FunctionalTestBase()
    {
        String authPath = GetConfig("TEST_AUTH", "auth.json");
        Auth = AuthInfo.LoadFromFile(authPath);
        String setupPath = GetConfig("TEST_SETUP", "test_setup.json");
        Setup = LoadTestSetup(setupPath);
        Session = new Session(auth: Auth, baseUrl: Setup.url);
    }

    private TestSetup LoadTestSetup(String path)
    {
        using (StreamReader file = File.OpenText(path))
        {
            JsonSerializer serializer = new JsonSerializer();
            return serializer.Deserialize(file, typeof(TestSetup)) as TestSetup;
        }
    }

    protected async Task CleanUpMdn(UInt64 mdn = 0)
    {
        if (mdn == 0)
            mdn = Setup.mdnRangeStart;

        var res = Contact.Resource(Session);
        var filter = new Dictionary<String, String>
            {
                { "primary_mdn", mdn.ToString() }
            };
        foreach (var c in res.List(filter))
        {
            await res.Delete(c.Id);
        }
    }

    protected int GetNumItems<T>(IEnumerable<T> collection, int max = 50)
    {
        int count = 0;
        foreach (var c in collection)
        {
            count++;
            if (max-- == 0)
                break;
        }
        return count;
    }

    public void Dispose()
    {
        if (Session != null)
        {
            Session.Dispose();
            Session = null;
        }
    }
}
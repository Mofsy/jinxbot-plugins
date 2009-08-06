using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Diagnostics;

namespace JinxBot.Plugins.Data.XmlDatabase.Tests
{
    /// <summary>
    /// Summary description for TestXmlDataLoad
    /// </summary>
    [TestClass]
    public class TestXmlDataLoad
    {
        public TestXmlDataLoad()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            XmlDatabase db = new XmlDatabase();
            StringReader sr = new StringReader(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<JinxBotDatabase Provider=""JinxBot.Plugins.Data.Xml.XmlDatabase, JinxBot.Plugins.Data.Xml, Version=1.0.0.0""
                 Version=""1.0"">
    <Roles>
        <Role Name=""O"" Description=""Owner"">
            <Overrides>
                <Override Name=""B"" />
            </Overrides>
        </Role>
        <Role Name=""B"" Description=""Autoban"" />
    </Roles>

    <Users>
        <User Name=""MyndFyre"" Gateway=""USEast"" LastSeen=""Never"">
            <Roles>
                <Add Role=""O"" />
            </Roles>
        </User>
        <User Name=""brew"" Gateway=""USEast"" LastSeen=""Never"">
            <Roles>
                <Add Role=""B"" />
            </Roles>
        </User>
    </Users>

    <Metas>
        <Meta InputString=""*joe*"" Match="".*joe.*"">
            <Roles>
                <Add Role=""B"" />
            </Roles>
        </Meta>
    </Metas>
</JinxBotDatabase>");
            db.Load(sr);
            Assert.IsTrue(db.IsRoleOverridden("B", new string[] { "B", "O" }));
            Assert.IsFalse(db.IsRoleOverridden("O", new string[] { "B", "O" }));

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            db.Save(sw);
            Debug.WriteLine(sb.ToString(), "Database Persisted");
            db.Load(new StringReader(sb.ToString()));
            sb = new StringBuilder();
            sw = new StringWriter(sb);
            db.Save(sw);
            Debug.WriteLine(sb.ToString());
        }
    }
}

using Chat;
using Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTest
{
	[TestClass]
	public class DBTestBase
	{
		public static ChatDBContext db;

		/// <summary>
		/// Executes once before the test run. (Optional)
		/// </summary>
		/// <param name="context"></param>
		[AssemblyInitialize]
		public static void AssemblyInit(TestContext context)
		{
			db = new ChatDBContext().CreateTestContext();
			DeleteDB();
			db.Database.EnsureCreated();
		}

		[ClassInitialize]
		public static void TestFixtureSetup(TestContext context)
		{

		}

		[TestInitialize]
		public void Setup()
		{

		}

		/// <summary>
		/// Runs after each test. (Optional)
		/// </summary>
		[TestCleanup]
		public void TearDown()
		{

		}



		/// <summary>
		/// Runs once after all tests in this class are executed. (Optional)
		/// Not guaranteed that it executes instantly after all tests from the class.
		/// </summary>
		[ClassCleanup]
		public static void TestFixtureTearDown()
		{


		}

		/// <summary>
		/// Executes once after the test run. (Optional)
		/// </summary>
		[AssemblyCleanup]
		public static void AssemblyCleanup()
		{
			DeleteDB();
		}

		public static void DeleteDB()
		{
			db.Database.EnsureDeleted(db.Chats_Participants);
			db.Database.EnsureDeleted(db.ForwardedMessages);
			db.Database.EnsureDeleted(db.ChatMessages);
			db.Database.EnsureDeleted(db.Chats);
		}
	}
}

using Eurofins.Common.DataConcerns.EF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;

namespace MoqBug
{
    [TestClass]
    public class TestWithJustMock
    {
        public class StubDbContext : EurofinsDbContext
        {
            public StubDbContext() : base("stubConnection")
            {

            }
        }

        private readonly InheritedRepository _firstRepo;
        private readonly StandAloneRepository _secondRepo;

        private const string FirstReturn = "Hello from inherited mock";
        private const string SecondReturn = "Hello from inherited mock";

        public TestWithJustMock()
        {
            _firstRepo = Mock.Create<InheritedRepository>(new UnitOfWork(new StubDbContext()), new StubDbContext());
            _secondRepo = Mock.Create<StandAloneRepository>(new UnitOfWork(new StubDbContext()), new StubDbContext());

            Mock.Arrange(() => _firstRepo.GetString()).Returns(FirstReturn);
            Mock.Arrange(() => _secondRepo.GetString()).Returns(SecondReturn);
        }

        /// <summary>
        /// Repository&lt;TEntity&gt; inherits IRepository&lt;TEntity&gt; and the GetById signature contains different type 
        /// parameters - TRelatedEntity V/S TSubEntity. JustMock is not affected by this issue present in MOQ as below:
        /// Ref: https://github.com/Moq/moq4/issues/193
        /// Ref2: https://github.com/castleproject/Core/issues/106
        /// </summary>
        [TestMethod]
        public void TestThatInheritedRepositoryMockInstanceCanBeCreated()
        {
            Assert.AreEqual(FirstReturn, _firstRepo.GetString());
        }

        [TestMethod]
        public void TestThatStandAloneRepositoryMockInstanceCanBeCreated()
        {
            Assert.AreEqual(SecondReturn, _secondRepo.GetString());
        }
    }
}
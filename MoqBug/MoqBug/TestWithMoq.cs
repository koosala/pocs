using Eurofins.Common.DataConcerns.EF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MoqBug
{
    [TestClass]
    public class TestWithMoq
    {
        public class StubDbContext : EurofinsDbContext
        {
            public StubDbContext() : base("stubConnection")
            {

            }
        }

        private readonly Mock<InheritedRepository> _firstRepo;
        private readonly Mock<StandAloneRepository> _secondRepo;

        public TestWithMoq()
        {
            _firstRepo = new Mock<InheritedRepository>(new UnitOfWork(new StubDbContext()), new StubDbContext());
            _secondRepo = new Mock<StandAloneRepository>(new UnitOfWork(new StubDbContext()), new StubDbContext());
        }

        /// <summary>
        /// Issue with MOQ - if the type arguments between interface and derived class are different, an exception
        /// "An exception of type 'System.Collections.Generic.KeyNotFoundException" is thrown by a component used by MOQ
        /// Repository&lt;TEntity&gt; inherits IRepository&lt;TEntity&gt; and the GetById signature contains different type 
        /// parameters - TRelatedEntity V/S TSubEntity
        /// Ref: https://github.com/Moq/moq4/issues/193
        /// Ref2: https://github.com/castleproject/Core/issues/106
        /// </summary>
        [TestMethod]
        public void TestThatInheritedRepositoryMockInstanceCanBeCreated()
        {
            var obj = _firstRepo.Object;
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void TestThatStandAloneRepositoryMockInstanceCanBeCreated()
        {
            var obj = _secondRepo.Object;
            Assert.IsNotNull(obj);
        }
    }
}

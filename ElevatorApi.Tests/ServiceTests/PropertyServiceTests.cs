using ElevatorApi.Services;

namespace ElevatorApi.Tests.ServiceTests
{
    public class PropertyServiceTests : BaseTest
    {
        private readonly PropertyService _sut;

        public PropertyServiceTests()
        {
            _sut = new PropertyService();
        }


        [Theory]
        [MemberData(nameof(GetData))]
        public void Classes_with_valid_properties_should_return_true(string? order, bool expected)
        {
            var actual = _sut.ValidOrderBy<ErrandEntity>(order);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(null, false)]
        [InlineData("null", false)]
        public void Classes_with_invalid_properties_should_return_false(string? order, bool expected)
        {
            var actual = _sut.ValidOrderBy<ErrandEntity>(order);

            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GetData()
        {
            var allData = new List<object[]>
            {
                new object[] { "id", true },
                new object[] { "title", true },
                new object[] { "description", true },
                new object[] { "errandStatus", true},
                new object[] { "errandStatus,asc", true},
                new object[] { "errandStatus", true},
                new object[] { "title,desc", true},
            };
            return allData;
        }
    }
}

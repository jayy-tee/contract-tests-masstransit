using System;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Blah.MessageContracts.UnitTests
{
    [TestFixture]
    public class GizmoAddedTests : MessageContractTestBase
    {
        [Test]
        public void ShouldHaveCorrectName()
        {
            /*
             * WARNING: POTENTIAL BREAKING CHANGE!!
             * If this test fails, the topology and therefore consumers will likely be broken.
             * Consider this a HARD GATE to protect against this scenario.
             */
            
            // Arrange
            var sut = typeof(GizmoAdded);
            const string expectedName = "Blah.MessageContracts.GizmoAdded";
            
            // Assert
            sut.FullName.Should().Be(expectedName, $"the full name of {sut.Name} is used by the RabbitMq topology for pub/sub");
        }
        
        [Test]
        public async Task CanBeDeserializedWithExpectedPropertiesAndTypes()
        {
            /*
             * In this test, we leverage MassTransit's in-memory harness to make assertions on properties
             * of a message published to the bus.
             */

            // Arrange
            var theExpectedId = Guid.NewGuid();
            var theExpectedName = "TheName";
            var theExpectedPrice = decimal.MaxValue;

            var messageData = new
            {
                Name = theExpectedName,
                Id = theExpectedId,
                Price = theExpectedPrice
            };

            // Act
            var jsonString = await PublishAndSerializeMessageViaHarness<GizmoAdded>(messageData);
            
            // Assert
            using var jsonDoc = JsonDocument.Parse(jsonString);
            var sut = jsonDoc.RootElement;
            
            // WARNING: Any test failures generated below reflect a broken consumer. This is a hard gate.
            sut.ShouldContainProperty(withName: "Name");
            sut.ShouldContainProperty(withName: "Id");
            sut.ShouldContainProperty(withName: "Price");

            // Additionally, we assert that we can cast the deserialized property to the appropriate type.
            sut.GetProperty("Id").GetGuid().Should().Be(theExpectedId);
            sut.GetProperty("Price").GetDecimal().Should().Be(theExpectedPrice);
            sut.GetProperty("Name").GetString().Should().Be(theExpectedName);
        }
    }
}
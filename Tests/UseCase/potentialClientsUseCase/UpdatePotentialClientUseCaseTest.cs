using Moq;
using Xunit;
using SagaAserhi.Application.DTO;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.UseCases.PotentialClientsUseCase;

namespace SagaAserhi.Tests.UseCase.PotentialClientsUseCase
{
    public class UpdatePotentialClientUseCaseTest
    {
        private readonly Mock<IPotentialClientRepository> _mockRepository;
        private readonly UpdatePotentialClientUseCase _useCase;
        
        private const string VALID_ID = "valid_id";
        private const string COMPANY_NAME = "Test Company";
        private const string PHONE = "123456789";
        private const string EMAIL = "test@company.com";
        private const string STATUS = "Active";

        public UpdatePotentialClientUseCaseTest()
        {
            _mockRepository = new Mock<IPotentialClientRepository>();
            _useCase = new UpdatePotentialClientUseCase(_mockRepository.Object);
        }

        [Fact]
        public async Task Execute_WithValidData_ShouldUpdateAndReturnDto()
        {
            // Arrange
            PotentialClient existingClient = new()
            {
                Id = VALID_ID,
                CompanyBusinessName = COMPANY_NAME,
                ContactPhone = PHONE,
                ContactEmail = EMAIL,
                Status = STATUS
            };

            UpdatePotentialClientDto updateDto = new()
            {
                CompanyBusinessName = "Updated Company",
                ContactPhone = "987654321",
                ContactEmail = "updated@company.com",
                Status = "Inactive"
            };

            _mockRepository.Setup(repo => repo.GetByIdPotencialClient(VALID_ID))
                .ReturnsAsync(existingClient);

            // Act
            UpdatePotentialClientDto result = await _useCase.Execute(VALID_ID, updateDto);

            // Assert
            Assert.Equal(updateDto.CompanyBusinessName, result.CompanyBusinessName);
            Assert.Equal(updateDto.ContactPhone, result.ContactPhone);
            Assert.Equal(updateDto.ContactEmail, result.ContactEmail);
            Assert.Equal(updateDto.Status, result.Status);
        }

        [Fact]
        public async Task Execute_WithPartialUpdate_ShouldPreserveExistingValues()
        {
            // Arrange
            PotentialClient existingClient = new()
            {
                Id = VALID_ID,
                CompanyBusinessName = COMPANY_NAME,
                ContactPhone = PHONE,
                ContactEmail = EMAIL,
                Status = STATUS
            };

            UpdatePotentialClientDto partialUpdateDto = new()
            {
                CompanyBusinessName = "Updated Company",
                ContactPhone = null!,
                ContactEmail = null!,
                Status = null!
            };

            _mockRepository.Setup(repo => repo.GetByIdPotencialClient(VALID_ID))
                .ReturnsAsync(existingClient);

            // Act
            UpdatePotentialClientDto result = await _useCase.Execute(VALID_ID, partialUpdateDto);

            // Assert
            Assert.Equal(partialUpdateDto.CompanyBusinessName, result.CompanyBusinessName);
            Assert.Equal(PHONE, result.ContactPhone);
            Assert.Equal(EMAIL, result.ContactEmail);
            Assert.Equal(STATUS, result.Status);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task Execute_WithInvalidId_ShouldThrowArgumentException(string invalidId)
        {
            // Arrange
            UpdatePotentialClientDto updateDto = new();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _useCase.Execute(invalidId, updateDto));
        }

        [Fact]
        public async Task Execute_WithNonExistentClient_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            const string NON_EXISTENT_ID = "non_existent_id";
            UpdatePotentialClientDto updateDto = new();

            _mockRepository.Setup(repo => repo.GetByIdPotencialClient(NON_EXISTENT_ID))
                .ReturnsAsync((PotentialClient)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _useCase.Execute(NON_EXISTENT_ID, updateDto));
        }
    }
}
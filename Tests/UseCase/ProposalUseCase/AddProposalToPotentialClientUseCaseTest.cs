// using Moq;
// using Xunit;
// using System;
// using System.Threading.Tasks;
// using SagaAserhi.Application.DTO.ProposalDtos;
// using SagaAserhi.Domain.Entities;
// using SagaAserhi.Application.Interfaces.IRepository;
// using SagaAserhi.Application.UseCases.ProposalsUseCase;

// namespace SagaAserhi.Tests.UseCase.ProposalUseCase
// {
//     public class AddProposalToPotentialClientUseCaseTest
//     {
//         private readonly Mock<IPotentialClientRepository> _mockPotentialClientRepository;
//         private readonly Mock<IProposalRepository> _mockProposalRepository;
//         private readonly AddProposalToPotentialClientUseCase _useCase;
//         private readonly DateTime _testDate;
        
//         private const string VALID_CLIENT_ID = "valid_client_id";

//         public AddProposalToPotentialClientUseCaseTest()
//         {
//             _mockPotentialClientRepository = new Mock<IPotentialClientRepository>();
//             _mockProposalRepository = new Mock<IProposalRepository>();
//             _useCase = new AddProposalToPotentialClientUseCase(
//                 _mockPotentialClientRepository.Object,
//                 _mockProposalRepository.Object
//             );
//             _testDate = DateTime.UtcNow;
//         }

//         [Fact]
//         public async Task Execute_ConDatosValidos_DebeCrearPropuestaExitosamente()
//         {
//             // Arrange
//             var client = new PotentialClient
//             {
//                 Id = VALID_CLIENT_ID,
//                 BusinessInfo = new BusinessInfo { TradeName = "Test Company" }
//             };

//             var proposalDto = new CreateProposalDto
//             {
//                 ClientId = VALID_CLIENT_ID,
//                 Status = new ProposalStatus
//                 {
//                     Proposal = "Pendiente",
//                     Sending = "No enviado",
//                     Review = "Sin revisar"
//                 },
//                 CreatedAt = _testDate
//             };

//             _mockPotentialClientRepository.Setup(repo => 
//                 repo.GetByIdPotencialClient(VALID_CLIENT_ID))
//                 .ReturnsAsync(client);

//             _mockProposalRepository.Setup(repo => 
//                 repo.CreateProposal(It.IsAny<Proposal>()))
//                 .ReturnsAsync(true);

//             // Act
//             var result = await _useCase.Execute(VALID_CLIENT_ID, proposalDto);

//             // Assert
//             Assert.Equal("Propuesta creada exitosamente", result);
//             _mockProposalRepository.Verify(repo => 
//                 repo.CreateProposal(It.Is<Proposal>(p =>
//                     p.ClientId == VALID_CLIENT_ID &&
//                     p.Number.StartsWith("PROP-") &&
//                     p.Status.Proposal == "Pendiente" &&
//                     p.Status.Sending == "No enviado" &&
//                     p.Status.Review == "Sin revisar" &&
//                     p.History.Count == 1 &&
//                     p.History[0].Action == "Creación de propuesta" &&
//                     p.History[0].PotentialClientId == VALID_CLIENT_ID
//                 )), Times.Once);
//         }

//         [Theory]
//         [InlineData("")]
//         [InlineData(null)]
//         [InlineData(" ")]
//         public async Task Execute_ConClienteIdInvalido_DebeLanzarArgumentException(string invalidClientId)
//         {
//             // Arrange
//             var proposalDto = new CreateProposalDto
//             {
//                 ClientId = invalidClientId,
//                 Status = new ProposalStatus()
//             };

//             // Act & Assert
//             await Assert.ThrowsAsync<ArgumentException>(() => 
//                 _useCase.Execute(invalidClientId, proposalDto));
//         }

//         [Fact]
//         public async Task Execute_ConDtoNulo_DebeLanzarArgumentNullException()
//         {
//             // Act & Assert
//             await Assert.ThrowsAsync<ArgumentNullException>(() => 
//                 _useCase.Execute(VALID_CLIENT_ID, null!));
//         }

//         [Fact]
//         public async Task Execute_ConClienteInexistente_DebeLanzarInvalidOperationException()
//         {
//             // Arrange
//             _mockPotentialClientRepository.Setup(repo => 
//                 repo.GetByIdPotencialClient(VALID_CLIENT_ID))
//                 .ReturnsAsync((PotentialClient)null!);

//             var proposalDto = new CreateProposalDto
//             {
//                 ClientId = VALID_CLIENT_ID,
//                 Status = new ProposalStatus()
//             };

//             // Act & Assert
//             await Assert.ThrowsAsync<InvalidOperationException>(() => 
//                 _useCase.Execute(VALID_CLIENT_ID, proposalDto));
//         }

//         [Fact]
//         public async Task Execute_CuandoFallaCreacionPropuesta_DebeLanzarInvalidOperationException()
//         {
//             // Arrange
//             var client = new PotentialClient
//             {
//                 Id = VALID_CLIENT_ID,
//                 BusinessInfo = new BusinessInfo { TradeName = "Test Company" }
//             };

//             var proposalDto = new CreateProposalDto
//             {
//                 ClientId = VALID_CLIENT_ID,
//                 Status = new ProposalStatus()
//             };

//             _mockPotentialClientRepository.Setup(repo => 
//                 repo.GetByIdPotencialClient(VALID_CLIENT_ID))
//                 .ReturnsAsync(client);

//             _mockProposalRepository.Setup(repo => 
//                 repo.CreateProposal(It.IsAny<Proposal>()))
//                 .ReturnsAsync(false);

//             // Act & Assert
//             await Assert.ThrowsAsync<InvalidOperationException>(() => 
//                 _useCase.Execute(VALID_CLIENT_ID, proposalDto));
//         }
//     }
// }
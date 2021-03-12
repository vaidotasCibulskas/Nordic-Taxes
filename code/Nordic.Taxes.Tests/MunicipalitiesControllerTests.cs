using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Nordic.Taxes.Controllers;
using Nordic.Taxes.Domain.Models;
using Nordic.Taxes.Domain.Services;
using Nordic.Taxes.Domain.Services.Communication;
using Nordic.Taxes.Mapping;
using Nordic.Taxes.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Nordic.Taxes.Tests
{
	public class MunicipalitiesControllerTests
	{
		private static IMapper _mapper;

		public MunicipalitiesControllerTests()
		{
			if (_mapper == null)
			{
				var mappingConfig = new MapperConfiguration(mc =>
				{
					mc.AddProfile(new ModelToResourceProfile());
					mc.AddProfile(new ResourceToModelProfile());
				});
				IMapper mapper = mappingConfig.CreateMapper();
				_mapper = mapper;
			}
		}

		[Fact]
		public async Task MunicipalitiesController_GetAllAsync_ReturnsResult()
		{
			// Arrange
			var mockService = new Mock<IMunicipalityService>();
			mockService.Setup(repo => repo.ListAsync())
				.ReturnsAsync(GetMunicipalities());
			var loggerMock = new Mock<ILogger<MunicipalitiesController>>();

			var controllerMock = new MunicipalitiesController(loggerMock.Object, mockService.Object, _mapper);

			// Act
			var result = await controllerMock.GetAllAsync();

			// Assert
			var municipalityResourceList = Assert.IsType<List<MunicipalityResource>>(result);
			var model = Assert.IsAssignableFrom<List<MunicipalityResource>>(
				municipalityResourceList);
			Assert.Equal(2, model.Count);
		}



		private List<Municipality> GetMunicipalities()
		{
			var munList = new List<Municipality>();
			munList.Add(new Municipality()
			{
				Id = 1,
				Name = "Test One"
			});
			munList.Add(new Municipality()
			{
				Id = 2,
				Name = "Test Two"
			});
			return munList;
		}


		[Fact]
		public async Task MunicipalitiesController_Post_ReturnsBadRequestResult_WhenModelStateIsInvalid()
		{
			// Arrange
			var mockService = new Mock<IMunicipalityService>();
			mockService.Setup(repo => repo.ListAsync())
				.ReturnsAsync(GetMunicipalities());
			var loggerMock = new Mock<ILogger<MunicipalitiesController>>();

			var controllerMock = new MunicipalitiesController(loggerMock.Object, mockService.Object, _mapper);

			controllerMock.ModelState.AddModelError("Name", "Required");
			var municRes = new SaveMunicipalityResource();

			// Act
			var result = await controllerMock.PostAsync(municRes);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			Assert.IsType<System.Collections.Generic.List<string>>(badRequestResult.Value);
		}

		private Task<MunicipalityResponse> GetMunicipalityResp(int id)
		{
			return Task.Run(() =>
				new MunicipalityResponse(GetMunicipalities().Where(x => x.Id == id).FirstOrDefault())
			); ;
		}


		[Fact]
		public async Task MunicipalitiesController_Post_ReturnsOKRequestResult_WhenModelStateIsValid()
		{
			// Arrange
			var mockService = new Mock<IMunicipalityService>();
			var municResToSave = new SaveMunicipalityResource();
			var loggerMock = new Mock<ILogger<MunicipalitiesController>>();
			mockService.Setup(repo => repo.SaveAsync(It.IsAny<Municipality>()))
				.Returns(GetMunicipalityResp(1))
				.Verifiable();


			var controllerMock = new MunicipalitiesController(loggerMock.Object, mockService.Object, _mapper);
			var municRes = new SaveMunicipalityResource()
			{
				Name = "Test One1",
			};


			// Act
			var result = await controllerMock.PostAsync(municRes);

			// Assert

			var postActionResult = Assert.IsType<OkObjectResult>(result);
			var resultValue = Assert.IsType<MunicipalityResource>(postActionResult.Value); //
			Assert.Equal(GetMunicipalities().FirstOrDefault().Name, resultValue.Name);

			mockService.Verify();
		}

		[Fact]
		public async Task MunicipalitiesController_Put_ReturnsOKtResult_WhenIdIsCorrect()
		{
			// Arrange
			int municId = 1;
			var mockService = new Mock<IMunicipalityService>();
			//mockService.Setup(repo => repo.UpdateAsync(municId, GetMunicipalities().FirstOrDefault()))
			mockService.Setup(repo => repo.UpdateAsync(municId, It.IsAny<Municipality>()))
				.Returns(GetMunicipalityResp(municId))
				.Verifiable();
				
			var loggerMock = new Mock<ILogger<MunicipalitiesController>>();
			var controllerMock = new MunicipalitiesController(loggerMock.Object, mockService.Object, _mapper);
			var saveMunicRes = new SaveMunicipalityResource()
			{
				Name = (await GetMunicipalityResp(municId)).Municipality.Name
			};


			// Act
			var result = await controllerMock.PutAsync(municId, saveMunicRes);

			// Assert
			var postActionResult = Assert.IsType<OkObjectResult>(result);
			var resultValue = Assert.IsType<MunicipalityResource>(postActionResult.Value); //
			Assert.Equal((await GetMunicipalityResp(municId)).Municipality.Id, resultValue.Id);
			mockService.Verify();
		}

	}
}


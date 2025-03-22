using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class ExampleManagementService(
    IExampleRepository _exampleRepository,
    IExampleQueryService _exampleQueryService,
    ILogger<ExampleManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor
) : IExampleManagementService
{
    public async Task<Example> Create(CreateExampleDto createExampleDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested CreateAsync for ExampleName {TargetExampleName}",
            authenticatedUserId, createExampleDto.ExampleName);
        
        // Check unique camps
        if (await _exampleQueryService.ExistByName(createExampleDto.ExampleName)){
            _logger.LogError(
                "User with Id {authenticatedUserId} requested CreateAsync for ExampleName {TargetExampleName} but the examplename already exists",
                authenticatedUserId, createExampleDto.ExampleName);
            throw new UniqueConstraintViolationException("Example with this name already exists", Fields.Examples.ExampleName);
        }

        // Create example
        Example example = new()
        {
            ExampleName = createExampleDto.ExampleName
        };

        _exampleRepository.Add(example);

        await _exampleRepository.SaveChanges();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested CreateAsync for ExampleName {TargetExampleName} and the example was created",
            authenticatedUserId, createExampleDto.ExampleName);
        
        return example;
    }
    
    public async Task<Example> Update(UpdateExampleDto updateExampleDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Example example = await _exampleQueryService.GetByIdThrowsNotFoundAsync(updateExampleDto.ExampleId);
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested UpdateAsync for ExampleId {TargetExampleId}",
            authenticatedUserId, updateExampleDto.ExampleId);
        
        bool hasChanges = ModelsHelper.UpdateModel(example, updateExampleDto, async (field, value) =>
        {
            switch (field)
            {
                case nameof(updateExampleDto.ExampleName):
                    return await _exampleQueryService.ExistByName((string)value);
                
                default:
                    return false;
            }
        });
        
        if (hasChanges)
        {
            await _exampleRepository.SaveChanges();
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateAsync for ExampleId {TargetExampleId} and the example was updated",
                authenticatedUserId, updateExampleDto.ExampleId);
        }
        else
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateAsync for ExampleId {TargetExampleId} and the example was not updated",
                authenticatedUserId, updateExampleDto.ExampleId);
        }
        
        return example;
    }
}
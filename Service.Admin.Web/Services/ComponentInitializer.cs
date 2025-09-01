using System.Data;

namespace Service.Admin.Web.Services;

public class ComponentInitializer(
    IEnumerable<IInitializeAsync> initializeComponents) : IComponentInitializer
{
    private readonly List<InitializationType> _order =
    [
        InitializationType.Controller,
        InitializationType.StateService
    ];

    private Dictionary<InitializationType, List<IInitializeAsync>> LoadingStrategy =>
        initializeComponents
            .GroupBy(c => c.Type)
            .ToDictionary(
                group => group.Key,
                group => group.ToList()
            );

    public async Task InitializeServicesAsync()
    {
        try
        {
            ValidateInitializationComponents();

            foreach (var type in _order) await InitializeComponentsOfType(type);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to initialize services", ex);
        }
    }

    private void ValidateInitializationComponents()
    {
        var missingTypes = _order.Except(LoadingStrategy.Keys).ToList();
        if (missingTypes.Count != 0)
            throw new ConstraintException(
                $"Missing initialization components for types: {string.Join(", ", missingTypes)}");

        var unexpectedTypes = LoadingStrategy.Keys.Except(_order).ToList();
        if (unexpectedTypes.Count != 0)
            throw new ConstraintException(
                $"Unexpected initialization component types found: {string.Join(", ", unexpectedTypes)}");
    }

    private async Task InitializeComponentsOfType(InitializationType type)
    {
        try
        {
            await Task.WhenAll(
                LoadingStrategy[type].Select(c => c.InitializeComponents())
            );
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to initialize components of type {type}", ex);
        }
    }
}
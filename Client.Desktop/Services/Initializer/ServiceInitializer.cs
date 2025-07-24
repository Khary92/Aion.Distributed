using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Desktop.Services.Initializer;

public class ServiceInitializer(
    IEnumerable<IInitializeAsync> initializeComponents,
    IEnumerable<IRegisterMessenger> messengerComponents) : IServiceInitializer
{
    private readonly List<InitializationType> _order =
    [
        InitializationType.Model,
        InitializationType.Service
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
            RegisterMessengers();
            ValidateInitializationComponents();

            foreach (var type in _order)
            {
                await InitializeComponentsOfType(type);
            }
        }
        catch (Exception ex)
        {
            throw new InitializationException("Failed to initialize services", ex);
        }
    }

    private void RegisterMessengers()
    {
        foreach (var component in messengerComponents)
        {
            try
            {
                component.RegisterMessenger();
            }
            catch (Exception ex)
            {
                throw new InitializationException($"Failed to register messenger for {component.GetType().Name}", ex);
            }
        }
    }

    private void ValidateInitializationComponents()
    {
        var missingTypes = _order.Except(LoadingStrategy.Keys).ToList();
        if (missingTypes.Any())
        {
            throw new ConstraintException(
                $"Missing initialization components for types: {string.Join(", ", missingTypes)}");
        }

        var unexpectedTypes = LoadingStrategy.Keys.Except(_order).ToList();
        if (unexpectedTypes.Any())
        {
            throw new ConstraintException(
                $"Unexpected initialization component types found: {string.Join(", ", unexpectedTypes)}");
        }
    }

    private async Task InitializeComponentsOfType(InitializationType type)
    {
        try
        {
            await Task.WhenAll(
                LoadingStrategy[type].Select(c => c.InitializeAsync())
            );
        }
        catch (Exception ex)
        {
            throw new InitializationException($"Failed to initialize components of type {type}", ex);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Lifecycle.Startup.Scheduler;
using Client.Desktop.Services.Mock;

namespace Client.Desktop.Lifecycle.Startup.Tasks.Initialize;

public class AsyncInitializeTask(
    IEnumerable<IInitializeAsync> initializeComponents,
    IMockSettingsService mockSettingsService) : IStartupTask
{
    private readonly List<InitializationType> _mockedOrder =
    [
        InitializationType.MockServices,
        InitializationType.MockModels,
        InitializationType.Model,
        InitializationType.Service,
        InitializationType.ViewModel
    ];

    private readonly List<InitializationType> _order =
    [
        InitializationType.Model,
        InitializationType.Service,
        InitializationType.ViewModel
    ];

    private Dictionary<InitializationType, List<IInitializeAsync>> LoadingStrategy =>
        initializeComponents
            .GroupBy(c => c.Type)
            .ToDictionary(
                group => group.Key,
                group => group.ToList()
            );

    public StartupTask StartupTask => StartupTask.AsyncInitialize;

    private List<InitializationType> GetOrder()
    {
        return mockSettingsService.IsMockingModeActive ? _mockedOrder : _order;
    }

    public async Task Execute()
    {
        ValidateInitializationComponents();

        foreach (var type in GetOrder())
        {
            await InitializeComponentsOfType(type);
        }
    }

    private void ValidateInitializationComponents()
    {
        var missingTypes = GetOrder().Except(LoadingStrategy.Keys).ToList();

        if (missingTypes.Any())
            throw new ConstraintException(
                $"Missing initialization components for types: {string.Join(", ", missingTypes)}");

        var unexpectedTypes = LoadingStrategy.Keys.Except(GetOrder()).ToList();
        if (unexpectedTypes.Any())
            throw new ConstraintException(
                $"Unexpected initialization component types found: {string.Join(", ", unexpectedTypes)}");
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
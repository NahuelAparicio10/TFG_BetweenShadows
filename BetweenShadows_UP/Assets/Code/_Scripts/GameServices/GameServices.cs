using System;
using System.Collections.Generic;

public static class GameServices
{
    private static readonly Dictionary<Type, IGameServices> _services = new();
    
    // -- Registers a service
    public static void Register<T>(T service) where T : IGameServices
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
        {
            UnityEngine.Debug.LogWarning($"Service {type} already registered. Overwriting.");
        }

        _services[type] = service;
    }
    
    //-- Gets a service from active services
    public static T Get<T>() where T : IGameServices
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out var service))
            return (T)service;
        
        throw new Exception($"Service {type} not found. Register the service before getting it!");
    }
    
    //-- Tries to get the service if exists returns true
    public static bool TryGet<T>(out T service) where T : IGameServices
    {
        service = default;
        if (GameServices.Get<T>() is not T s) return false;
        
        service = s;
        return true;
    }
    
    //-- Deletes a service from dictionary
    public static void Unregister<T>() where T : IGameServices
    {
        _services.Remove(typeof(T));
    }
    
    //-- Clears all the services
    public static void ClearAll()
    {
        _services.Clear();
    }
}

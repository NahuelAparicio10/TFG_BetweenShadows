using System.Collections.Generic;

public sealed class InputBuffer
{
    private readonly struct Entry
    {
        public readonly ICommand command;
        public readonly double time;

        public Entry(ICommand cmd, double t)
        {
            command = cmd;
            time = t;
        }
    }

    private readonly Queue<Entry> _queue = new();
    private readonly double _maxHoldSeconds;

    public InputBuffer(double maxHoldSeconds = 0.2f) => _maxHoldSeconds = maxHoldSeconds;

    public void Enqueue(ICommand cmd)
    {
        _queue.Enqueue(new Entry(cmd, UnityEngine.Time.timeAsDouble));
    }

    public bool TryConsume<T>(out T cmd) where T : struct, ICommand
    {
        if (_queue.Count == 0)
        {
            cmd = default;
            return false;
        }
        
        // First of all we consume the first valid command by type and expiration time.
        double timeNow = UnityEngine.Time.timeAsDouble;
        T? best = null;
        int bestPriority = int.MinValue;
        int count = _queue.Count;
        
        var temporal = new List<Entry>(count);
        
        // If expiration time is not passed we add the command entry to temporal list
        while (_queue.Count > 0)
        {
            var entry = _queue.Dequeue();
            if (timeNow - entry.time <= _maxHoldSeconds)
            {
                temporal.Add(entry);
            }
        }
        // We check for the best priority command on the list
        foreach (var entry in temporal)
        {
            if (entry.command is T t && t.Priority > bestPriority)
            {
                best = t;
                bestPriority = t.Priority;
            }
        }

        bool consumed = best.HasValue;
        
        // We add all again except the consumed one
        foreach (var entry in temporal)
        {
            if (consumed && entry.command is T t && t.Priority == bestPriority)
            {
                consumed = false;
                continue;
            }
            _queue.Enqueue(entry);
        }

        if (best.HasValue)
        {
            cmd = best.Value; 
            return true;
        }
        
        cmd = default; 
        return false;
    }

    public void Clear() => _queue.Clear();
}

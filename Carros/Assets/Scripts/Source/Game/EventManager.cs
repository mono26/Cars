// Copyright (c) What a Box Creative Studio. All rights reserved.

using System;
using System.Collections.Generic;

public class GameEvent { }

public static class EventManager
{
    private static Dictionary<Type, List<EventHandlerBase>> events;

    static EventManager()
    {
        events = new Dictionary<Type, List<EventHandlerBase>>();
        return;
    }

    public static void AddListener<T>(EventHandler<T> listener) where T : GameEvent
    {
        if (!ExistsEventType<T>()) {
            events[typeof(T)] = new List<EventHandlerBase>();
        }
        if (!SubscriptionExists(listener)) {
            events[typeof(T)].Add(listener);
        }
        return;
    }

    private static bool ExistsEventType<T>() where T : GameEvent
    {
        bool eventTypeExists = true;
        if (!events.ContainsKey(typeof(T))) {
            eventTypeExists = false;
        }
        return eventTypeExists;
    }

    private static bool SubscriptionExists<T>(EventHandler<T> receiver) where T : GameEvent
    {
        bool existsSubscription = false;
        if (ExistsEventType<T>())
        {
            List<EventHandlerBase> receivers;
            events.TryGetValue(typeof(T), out receivers);
            for (int i = 0; i < receivers.Count; i++)
            {
                if (receivers[i] == receiver)
                {
                    existsSubscription = true;
                    break;
                }
            }
        }
        return existsSubscription;
    }

    public static void RemoveListener<T>(EventHandler<T> listener) where T : GameEvent
    {
        Type eventType = typeof(T);
        try
        {
            if (ExistsEventType<T>() && SubscriptionExists<T>(listener))
            {
                List<EventHandlerBase> subscriberList = events[typeof(T)];
                for (int i = 0; i < subscriberList.Count; i++)
                {
                    if (subscriberList[i] == listener)
                    {
                        subscriberList.Remove(subscriberList[i]);
                        if (subscriberList.Count == 0)
                        {
                            events.Remove(eventType);
                        }
                    }
                }
            }
            else {
                throw new EventException(typeof(T), string.Format(listener + "not subscribed to the event"));
            }
        }
        catch (EventException eventException) {
            eventException.DisplayException();
        }
        return;
    }

    public static void TriggerEvent<T>(T newEvent) where T : GameEvent
    {
        try
        {
            if (ExistsEventType<T>())
            {
                List<EventHandlerBase> subscribersList;
                events.TryGetValue(typeof(T), out subscribersList);
                for (int i = 0; i < subscribersList.Count; i++)
                {
                    (subscribersList[i] as EventHandler<T>).OnGameEvent(newEvent);
                }
            }
            else{
                throw new EventException(typeof(T), "Has no listeners.");
            }
        }
        catch (EventException eventException) {
            eventException.DisplayException();
        }
        return;
    }
}

// TODO check if refactorization is possible
public interface EventHandlerBase { }

public interface EventHandler<T> : EventHandlerBase
{
    void OnGameEvent(T eventType);
}
